using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/* 合理的A*寻路关键在于H函数的确定,假设当前点到终点的最小实际代价为R,A*寻路有如下特点:
 * 1.如果H>R,则A*结果可能不是最短路径
 * 2.如果能始终保持H<=R,则A*可以必然寻得最短路径,
 * 3.H<R的程度越严重,遍历的节点就越多
 * 
 * 根据上面3点,所以看似比较合理的结论是H=R
 * 这样设计,很多路线的F是完全相同的(最短路线本身就可能不止1条,如上设计会导致所有的最短路线点F值相同),
 * 此时由于堆排序的不稳定性,我们会遍历很多多余的节点,而且路线变得不可控(你也无法知道A*究竟选择了什么样的最短路线)
 * 更合理的方法是我们在不导致寻出绕远路线的情况下的提高H值,将提升部分称为E(F=G+H+E),E按照下面两点来做:
 * 1.在我们希望的路线上节点E要小于其他节点,这样便使得路线可控,而且会减少遍历节点数(我们希望的路线上的F值(E比其他的小)是最小的,尽管也有其他最短路线,但是他们F值要大)
 * 2.E不能太大,只要不超过一个相邻节点的代价则不会出现因为此改动而绕远的情况
 * 
 * 
 * 
 * 举个简单的例子:
 * 当我们希望在保证最短路线的情况下,按照邻节点添加的顺序来走,假设直走一个格子代价为1,斜走代价为1.5,
 * 比如每个节点的邻节点,从正上方开始顺时针添加,目标在起点(0,0),终点(5,-2),我们希望的路线是:(0,0),(1,0),(2,0),(3,0),(4,0),(5,-1)
 * 那么我们寻路遍历自己节点的时候,让E的初值为0,遍历一次加0.01*(父节点的H)即可,这里如果不*(父节点的H)的话,依然无法避免多个最短路径上出现相同的F值情况,
 * 因为最短路线虽然可能有很多,但是他们必然都是由完全相同的元素(以某个方向移动一个格子)组成的,只是顺序不同,如果E值仅仅与邻节点添加顺序相关,
 * 则(4,-1)点的F值与(-1,-1)相同,都是5.53(它俩想都在父节点右下边,E都为0.03),依然无法避免多余遍历和路线不可控,
 * 但是当我们将其与(父节点H)挂钩后,随着距离终点变近(父节点H变小),虽然是同一个方向,但是E值就会变小.
 * 这里没有用自身的H是由于使用自身H,倒数第二步的选择有时会不正常(在(3,0)点选择后面路径时),(4,0)点的H值为3,E为3*0.02=0.06.(4,-1)点的H值为2,E为2*0.03=0.06,
 * 两者相等了,所以,为了保证我们始终选择想要的方向,邻节点都以自己的系数*父节点H值即可
 * 
 * 
 * 但是,就上述情况,我们也可以不添加E,通过控制节点的比较换位策略,实现尽可能的少遍历节点,并且使得寻路尽可能按照我们的添加顺序来行走.
 * 比较的时候总是用新插入的节点和已经在树中的节点比较,当相等情况出现时设定为新节点小,如此设计对于一次邻节点遍历,当多个邻节点F值相同时,
 * 最后遍历添加的会被放在堆顶,取出堆顶节点,遍历此节点邻节点时,如是重复,我们就获得了这样的路径:优先走添加较晚的临街点的最优路径.
 * 那么我么只要反向遍历邻节点,或者干脆添加邻节点的时候就按照期望结果的反向添加,
 * 注意:如果设定为新节点大,只能保证第一轮添加堆顶为先添加的邻节点,取出此节点后堆顶节点无法控制,依然会导致多余的遍历,不可行,
 * 
 * 
 */
/// <summary>
/// 常规A*,带有F,G,H
/// 带有开启列表,关闭列表,开启列表为一个根据F值排序的小根堆
/// 直走一个格子代价为2,斜走代价为3
/// </summary>
public class NormalAStar
{
    //public static float PathExtraCost = 0.3f;

    Dictionary<int, bool> closeNode = new Dictionary<int, bool>();

    Dictionary<int, float> node2G = new Dictionary<int, float>();

    static int straightCost = 1;
    int maxCheckMulH = 10;
    HeapSort heap = new HeapSort(1000);


    public IEnumerator SeekPathLog(Node start, Node target, Dictionary<NodeType, float> costMap)
    {
        Stack<Node> result = new Stack<Node>();

        heap.Clear();
        closeNode.Clear();

        SeekNode realTarget = AddNodeToHeap(start, target, SeekNode.StartParent, costMap, 0);
        realTarget.SelfNode.MarkSelf(Color.yellow, 99999);

        int safeFlag = 0;

        while (heap.Length > 0 && safeFlag++ < 10000)
        {
            SeekNode curNode = heap.GetMin();
            closeNode[curNode.GetKey()] = true;
            List<Node> neighbours = curNode.SelfNode.Neighbours;
            for (int i = 0; i < neighbours.Count; i++)
            {
                Node sonNode = neighbours[i];
                if (closeNode.ContainsKey(sonNode.GetKey()))
                {
                    continue;
                }
                SeekNode seekNode = AddNodeToHeap(sonNode, target, curNode, costMap, i);
                if (seekNode != null)
                {
                    seekNode.SelfNode.MarkSelf(Color.yellow, 99999);
                    yield return new WaitForSeconds(0.1f);
                }
                if (seekNode != null)
                {
                    if (seekNode.H < realTarget.H)
                    {
                        realTarget = seekNode;
                    }
                }
                if (sonNode == target)
                {
                    heap.Clear();
                    break;
                }
            }
        }

        if (safeFlag >= 10000)
        {
            Debug.LogError("逻辑错误");
        }
    }

    #region 堆排序,同一个节点可能在堆里有多个

    public Stack<Node> SeekPath(Node start, Node target, Dictionary<NodeType, float> costMap)
    {
        Stack<Node> result = new Stack<Node>();

        heap.Clear();
        closeNode.Clear();
        node2G.Clear();

        SeekNode realTarget = AddNodeToHeap(start, target, SeekNode.StartParent, costMap, 0);

        int safeFlag = 0;

        while (heap.Length > 0 && safeFlag++ < 10000)
        {
            SeekNode curNode = heap.GetMin();
            closeNode[curNode.GetKey()] = true;
            List<Node> neighbours = curNode.SelfNode.Neighbours;
            for (int i = 0; i < neighbours.Count; i++)
            {
                Node sonNode = neighbours[i];
                if (closeNode.ContainsKey(sonNode.GetKey()))
                {
                    continue;
                }
                SeekNode seekNode = AddNodeToHeap(sonNode, target, curNode, costMap, i);
                if (seekNode != null)
                {
                    if (seekNode.H < realTarget.H)
                    {
                        realTarget = seekNode;
                    }
                }
                if (sonNode == target)
                {
                    heap.Clear();
                    break;
                }
            }
        }

        if (safeFlag >= 10000)
        {
            Debug.LogError("逻辑错误");
        }

        SeekNode temp = realTarget;
        while (temp.H != -1)
        {
            result.Push(temp.SelfNode);
            temp.PutBack();
            temp = temp.Parent;
        }

        return result;
    }

    SeekNode AddNodeToHeap(Node selfNode, Node target, SeekNode parent, Dictionary<NodeType, float> costMap, int index)
    {

        SeekNode node = SeekNode.GetNode(selfNode);

        node.H = Mathf.Abs(target.X - selfNode.X) + Mathf.Abs(target.Z - selfNode.Z);
        float e;
        node.G = costMap[selfNode.Type];
        if (parent != null)
        {
            node.G += parent.G;
            e = parent.H * index * 0.01f;
        }
        else
        {
            e = 0;
        }
        //e = 0;
        node.F = node.G + node.H + e;
        float oldG;
        int selfKey = selfNode.GetKey();
        if (node2G.TryGetValue(selfKey, out oldG))
        {
            if (oldG <= node.G)
            {
                return null;
            }
        }
        node2G[selfKey] = node.G;

        if (node.F > 100)
        {
            node.PutBack();
            return null;
        }
        else
        {
            node.Parent = parent;
            heap.AddOne(node);
            return node;
        }
    }
    #endregion

    #region 插入排序,一个节点在开启列表中只有一个

    List<SeekNode> openList = new List<SeekNode>();
    public Stack<Node> SeekPath2(Node start, Node target, Dictionary<NodeType, float> costMap)
    {
        Stack<Node> result = new Stack<Node>();

        closeNode.Clear();
        openList.Clear();

        SeekNode realTarget = AddNodeToList(start, target, SeekNode.StartParent, costMap, 0);

        int safeFlag = 0;

        while (openList.Count > 0 && safeFlag++ < 10000)
        {
            SeekNode curNode = openList[0];
            openList.RemoveAt(0);
            closeNode[curNode.GetKey()] = true;

            List<Node> neighbours = curNode.SelfNode.Neighbours;
            for (int i = 0; i < neighbours.Count; i++)
            {
                Node sonNode = neighbours[i];
                if (closeNode.ContainsKey(sonNode.GetKey()))
                {
                    continue;
                }
                SeekNode seekNode = AddNodeToList(sonNode, target, curNode, costMap, i);
                if (seekNode != null)
                {
                    if (seekNode.H < realTarget.H)
                    {
                        realTarget = seekNode;
                    }
                }
                if (sonNode == target)
                {
                    heap.Clear();
                    break;
                }
            }
        }

        if (safeFlag >= 10000)
        {
            Debug.LogError("逻辑错误");
        }

        SeekNode temp = realTarget;
        while (temp.H != -1)
        {
            result.Push(temp.SelfNode);
            temp.PutBack();
            temp = temp.Parent;
        }

        return result;
    }


    SeekNode AddNodeToList(Node selfNode, Node target, SeekNode parent, Dictionary<NodeType, float> costMap, int index)
    {
        int selfKey = selfNode.GetKey();
        int oldIndex = openList.FindIndex(t => t.GetKey() == selfKey);
        float curG = costMap[selfNode.Type] + parent.G;
        if (oldIndex >= 0)
        {//节点原本就在开启列表,则检查更新
            SeekNode oldNode = openList[oldIndex];
            if (oldNode.G > curG)
            {//新的路径G值小,则使用新的更新数据
                oldNode.F -= oldNode.G - curG;
                oldNode.G = curG;
                openList.RemoveAt(oldIndex);
                oldNode.Parent = parent;
                InsertSort(oldNode);
            }
            return null;
        }

        SeekNode node = SeekNode.GetNode(selfNode);
        node.H = Mathf.Abs(target.X - selfNode.X) + Mathf.Abs(target.Z - selfNode.Z);
        node.G = costMap[selfNode.Type] + parent.G;
        node.F = node.G + node.H + parent.H * index * 0.01f;
        if (node.F > 100)
        {
            node.PutBack();
            return null;
        }
        else
        {
            node.Parent = parent;
            InsertSort(node);
            //selfNode.MarkSelf(Color.yellow, 99999);
            return node;
        }
    }



    void InsertSort(SeekNode node)
    {
        int index = 0;
        for (; index < openList.Count; index++)
        {
            if (openList[index].G > node.G)
            {
                break;
            }
        }
        openList.Insert(index, node);
    }
    #endregion


}


public class SeekNode
{
    public Node SelfNode;
    /// <summary>
    /// A*中的F,
    /// </summary>
    public float F;
    /// <summary>
    /// 起点到当前点消耗的代价
    /// </summary>
    public float G;
    /// <summary>
    /// 当前点到终点的期望代价
    /// </summary>
    public float H;
    /// <summary>
    /// 父节点
    /// </summary>
    public SeekNode Parent;

    static SeekNode _startParent = new SeekNode() { H = -1 };
    public static SeekNode StartParent
    {
        get
        {
            return _startParent;
        }
    }

    static object lockFlag = new object();

    static Queue<SeekNode> pool = new Queue<SeekNode>();

    public static SeekNode GetNode(Node selfNode)
    {
        lock (lockFlag)
        {
            if (pool.Count > 0)
            {
                SeekNode result = pool.Dequeue();
                result.SelfNode = selfNode;
                return result;
            }
        }
        return new SeekNode() { SelfNode = selfNode };
    }

    public int GetKey()
    {
        return this.SelfNode.GetKey();
    }

    public void PutBack()
    {
        if (pool.Count > 40)
        {
            return;
        }
        lock (lockFlag)
        {
            pool.Enqueue(this);
        }
    }

}
