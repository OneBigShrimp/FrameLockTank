
class HeapSort
{
    public int Length { private set; get; }
    private SeekNode[] sortNodes;
    public HeapSort(int maxNumber)
    {
        sortNodes = new SeekNode[maxNumber + 1];
        Length = 0;
    }
    public void AddOne(SeekNode nodeAdded)
    {
        Length++;
        sortNodes[Length] = nodeAdded;
        if (Length != 1)
        {
            int currentIndex = Length;
            while (currentIndex != 1 && nodeAdded.F <= sortNodes[currentIndex >> 1].F)
            {
                sortNodes[currentIndex] = sortNodes[currentIndex >> 1];
                currentIndex >>= 1;
            }
            sortNodes[currentIndex] = nodeAdded;
        }
    }
    /// <summary>
    /// 添加Node
    /// </summary>
    /// <param name="nodesAdded"></param>
    public void Add(params SeekNode[] nodesAdded)
    {
        for (int i = 0; i < nodesAdded.Length; i++)
        {
            AddOne(nodesAdded[i]);
        }
    }
    /// <summary>
    /// 获取堆中Node的F值最小的Node
    /// </summary>
    /// <returns></returns>
    public SeekNode GetMin()
    {
        SeekNode result = sortNodes[1];
        SeekNode lastNode = sortNodes[Length];
        Length--;
        int currentIndex = 1;
        int leftSon = 2, rightSon = 3;
        bool isOver = false;
        sortNodes[1] = lastNode;
        while (leftSon <= Length && !isOver)
        {//当前节点的左子节点小于长度,则表明当前节点不是叶子点
            if (lastNode.F > sortNodes[leftSon].F || lastNode.F > sortNodes[rightSon].F)
            {//如果当前节点元素的F值小于任意一个子节点,则表示需要换位(因为可能发生连续换位,这里不直接换位),应该选择与两个子节点中较小的那个换
                if (sortNodes[leftSon].F < sortNodes[rightSon].F)
                {//如果左子节点小,则应该将左子节点与父节点换位,并更新当前节点索引为此左子节点
                    sortNodes[currentIndex] = sortNodes[leftSon];
                    currentIndex = leftSon;
                }
                else
                {
                    sortNodes[currentIndex] = sortNodes[rightSon];
                    currentIndex = rightSon;
                }
            }
            else
            {//如果当前节点小于它的两个子节点,则表示完成调整
                isOver = true;
            }
            leftSon = currentIndex << 1;
            rightSon = leftSon + 1;
        }
        sortNodes[currentIndex] = lastNode; //补充添加当前节点的值
        return result;
    }


    public void Clear()
    {
        Length = 0;
        for (int i = 0; i < sortNodes.Length; i++)
        {
            sortNodes[i] = null;
        }
    }


    public SeekNode[] GetArray()
    {
        return sortNodes;
    }

}
