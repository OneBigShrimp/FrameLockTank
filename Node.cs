using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 只支持占用一个格子和九个格子的单位
/// </summary>
[Serializable]
public class Node
{
    [SerializeField]
    int _x;
    /// <summary>
    /// 列索引(左边为0,向右增加)
    /// </summary>
    public int X
    {
        get
        {
            return _x;
        }
    }
    [SerializeField]
    int _z;
    /// <summary>
    /// 行索引(下面为0,向上增加)
    /// </summary>
    public int Z
    {
        get
        {
            return _z;
        }
    }
    public Vector3 WorldPos { get; set; }


    [SerializeField]
    NodeType _type;
    /// <summary>s
    /// 节点类型
    /// </summary>
    public NodeType Type
    {
        get
        {
            return _type;
        }
        set
        {
            _type = value;
            ////YZYTODO: 测试用的
            //if (MeshCreate.TestDic.ContainsKey(this))
            //{
            //    MeshCreate.TestDic[this].SetNodeType(value);
            //}
        }
    }

    public bool ExistMoveUnit
    {
        get
        {
            return this._moveUnitFlag > 0;
        }
    }

    int _moveUnitFlag = 0;


    /// <summary>
    /// 邻节点
    /// 对于4Dir只有上下左右四个方向的节点,
    /// 对于8Dir有八个邻节点
    /// </summary>
    public List<Node> Neighbours { get; private set; }
    /// <summary>
    /// 所有的节点,外层Key表示X坐标,内层Key表示Y坐标
    /// </summary>
    static Node[,] allNode;
    /// <summary>
    /// 地图左下角节点的世界坐标
    /// </summary>
    public static Vector3 LeftDown { private set; get; }
    /// <summary>
    /// 地图右上角坐标
    /// </summary>
    public static Vector3 RightUp { private set; get; }
    /// <summary>
    /// 相邻节点的间隔
    /// </summary>
    public static int GridBlank { private set; get; }
    /// <summary>
    /// 地图宽度
    /// </summary>
    public static int Width { private set; get; }
    /// <summary>
    /// 地图高度
    /// </summary>
    public static int Height { private set; get; }

    public Node()
    {

    }

    Node(int x, int z, NodeType type)
    {
        this._x = x;
        this._z = z;
        this._type = type;
        this.Neighbours = new List<Node>();
        this.WorldPos = new Vector3(LeftDown.x + (this._x + 0.5f) * GridBlank, 0, LeftDown.z + (this._z + 0.5f) * GridBlank);
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="rowNum">行数</param>
    /// <param name="colNum">列数</param>
    /// <param name="is4Dir">是否为四方向的邻节点,否的时候为八方邻节点</param>
    public static void Init(int[,] map, int gridBlank, Vector3 leftDown)
    {
        LeftDown = leftDown;
        GridBlank = gridBlank;
        RightUp = leftDown + gridBlank * new Vector3(Width, 0, Height);
        Width = map.GetLength(0);
        Height = map.GetLength(1);
        allNode = new Node[Width, Height];
        CreateAllNode(map);
        AddNeighbour();
    }
    /// <summary>
    /// 根据横纵坐标(虚拟的整数坐标也就是列索引和行索引)获取节点
    /// </summary>
    /// <param name="x">行索引</param>
    /// <param name="z">行索引</param>
    /// <returns></returns>
    public static Node GetNode(int x, int z)
    {
        if (x < 0 || x >= Width || z < 0 || z >= Height)
        {
            return null;
        }
        return allNode[x, z];
    }

    /// <summary>
    /// 检测一个点是否超过地图边缘
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public static bool CheckPosOutMap(Vector3 worldPos)
    {
        return worldPos.x < LeftDown.x || worldPos.z < LeftDown.z || worldPos.x > RightUp.x || worldPos.z > RightUp.z;
    }


    public static Node GetNodeByPos(Vector3 pos)
    {
        return Node.GetNode((int)(pos.x - LeftDown.x) / GridBlank, (int)(pos.z - LeftDown.z) / GridBlank);
    }

    /// <summary>
    /// 创建所有节点
    /// </summary>
    /// <param name="map"></param>
    static void CreateAllNode(int[,] map)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Height; z++)
            {
                allNode[x, z] = new Node(x, z, (NodeType)map[x, z]);
            }
        }
    }
    /// <summary>
    /// 添加邻节点
    /// </summary>
    /// <param name="is4Dir">是否是4方向的</param>
    static void AddNeighbour()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Height; z++)
            {
                if (z + 1 < Height)
                {//上
                    allNode[x, z].Neighbours.Add(allNode[x, z + 1]);
                }
                if (x + 1 < Width)
                {//右
                    allNode[x, z].Neighbours.Add(allNode[x + 1, z]);
                }
                if (z - 1 >= 0)
                {//下
                    allNode[x, z].Neighbours.Add(allNode[x, z - 1]);
                }
                if (x - 1 >= 0)
                {//左
                    allNode[x, z].Neighbours.Add(allNode[x - 1, z]);
                }

            }
        }
    }

    public int GetKey()
    {
        return this.X * 1000 + this.Z;
    }


    public void SetNodeType(NodeType type)
    {
        this.Type = type;
        switch (type)
        {
            case NodeType.Empty:
                MarkSelf(Color.gray, 0);
                break;
            case NodeType.Grass:
                MarkSelf(Color.cyan, 99999);
                break;
            case NodeType.Brick:
                break;
            case NodeType.Iron:
                break;
            case NodeType.Water:
                break;
            default:
                break;
        }
    }


    public void MarkSelf(Color color, float time)
    {
        int lineId1 = this._x * 100 + this._z + 1000000;
        if (color == Color.yellow)
        {
            Line line = LineHelper.Instance.GetLine(lineId1);
            if (line != null)
            {
                if (line.Color == Color.yellow)
                {
                    color = Color.green;
                }
                else if (line.Color == Color.green)
                {
                    color = Color.blue;
                }
                else if (line.Color == Color.blue)
                {
                    color = Color.black;
                }
                else if (line.Color == Color.black)
                {
                    color = Color.red;
                }
            }
        }

        LineHelper.Instance.AddLine(lineId1, WorldPos - 0.5f * GridBlank * new Vector3(1, 0, 1), WorldPos + 0.5f * GridBlank * new Vector3(1, 0, 1), color, time);
        int lineId2 = this._x * 100 + this._z + 2000000;
        LineHelper.Instance.AddLine(lineId2, WorldPos - 0.5f * GridBlank * new Vector3(1, 0, -1), WorldPos + 0.5f * GridBlank * new Vector3(1, 0, -1), color, time);
    }

    public static NodeOffset operator -(Node one, Node other)
    {
        return new NodeOffset(one.X - other.X, one.Z - other.Z);
    }
    public static Node operator +(Node one, NodeOffset offset)
    {
        if (one.X + offset.X >= 0 && one.X + offset.X < Width && one.Z + offset.Z >= 0 && one.Z + offset.Z < Height)
        {
            return allNode[one.X + offset.X, one.Z + offset.Z];
        }
        else
        {
            return null;
        }
    }

    public override string ToString()
    {
        return string.Format("({0},{1})", X, Z);
    }
}

public struct NodeOffset
{
    public int X;
    public int Z;

    public NodeOffset(int x, int z)
    {
        this.X = x;
        this.Z = z;
    }



}


public enum NodeType
{
    /// <summary>
    /// 空节点
    /// </summary>
    Empty = 0,
    /// <summary>
    /// 草地
    /// </summary>
    Grass = 1,
    /// <summary>
    /// 砖头
    /// </summary>
    Brick = 2,
    /// <summary>
    /// 铁
    /// </summary>
    Iron = 3,
    /// <summary>
    /// 水
    /// </summary>
    Water = 4,
}
public enum Direction
{
    None = 0,
    Up = 1,
    Down = 2,
    Left = 3,
    Right = 4,
}