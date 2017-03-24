using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SimpleMap : MonoBehaviour
{
    public int BodySize = 1;
    public bool ShowProgress;
    public bool UseHeap;

    int width = 10;
    int height = 10;
    int unit = 50;
    bool hasInitMap = false;
    bool hasInitStone = false;
    bool setStart = true;
    NormalAStar star;
    Node start;
    int[,] map;
    string unitStr = "20";
    string xLength = "40";
    string zLength = "26";
    string msg = "";

    Node clickStartNode;
    void Start()
    {
        star = new NormalAStar();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            hasInitStone = true;
        }

        if (hasInitStone)
        {
            #region 寻路
            if (Input.GetMouseButtonDown(0))
            {
                Node current = GetClickNode();
                if (current == null)
                {
                    return;
                }

                if (setStart)
                {
                    msg = "等待设置终点";
                    start = current;
                    setStart = false;
                }
                else
                {
                    if (current != start)
                    {
                        Debug.Log("寻路开始,起点:" + start + "______" + current);
                        setStart = true;
                        float startTime = Time.realtimeSinceStartup;
                        Dictionary<NodeType, float> temp = new Dictionary<NodeType, float>();
                        temp.Add(NodeType.Empty, 1);
                        temp.Add(NodeType.Grass, 1.5f);
                        if (ShowProgress)
                        {
                            StartCoroutine(star.SeekPathLog(start, current, temp));
                            return;
                        }
                        Stack<Node> result;
                        if (UseHeap)
                        {
                            result = star.SeekPath(start, current, temp);
                        }
                        else
                        {
                            result = star.SeekPath2(start, current, temp);
                        }
                        Debug.Log("寻路历时____" + (Time.realtimeSinceStartup - startTime));
                        if (result.Count > 1)
                        {
                            Node first = result.Pop();
                            Node second;
                            while (result.Count > 0)
                            {
                                second = result.Pop();
                                LineHelper.Instance.AddLine(first.WorldPos, second.WorldPos, Color.red, 5);
                                first = second;
                            }
                            msg = "寻路成功";
                        }
                        else
                        {
                            msg = "寻路失败";
                        }
                    }
                }
            }
            #endregion
        }
        else if (hasInitMap)
        {
            if (Input.GetMouseButtonDown(0))
            {
                clickStartNode = GetClickNode();
            }
            if (Input.GetMouseButtonUp(0))
            {
                Node endNode = GetClickNode();
                if (clickStartNode == null || endNode == null)
                {
                    return;
                }

                int xMin = clickStartNode.X < endNode.X ? clickStartNode.X : endNode.X;
                int xMax = clickStartNode.X > endNode.X ? clickStartNode.X : endNode.X;
                int zMin = clickStartNode.Z < endNode.Z ? clickStartNode.Z : endNode.Z;
                int zMax = clickStartNode.Z > endNode.Z ? clickStartNode.Z : endNode.Z;
                NodeType nt = clickStartNode.Type == NodeType.Empty ? NodeType.Grass : NodeType.Empty;
                for (int x = xMin; x <= xMax; x++)
                {
                    for (int z = zMin; z <= zMax; z++)
                    {
                        Node.GetNode(x, z).SetNodeType(nt);
                    }
                }
            }
        }
    }

    void OnGUI()
    {
        xLength = GUILayout.TextField(xLength);
        zLength = GUILayout.TextField(zLength);
        unitStr = GUILayout.TextField(unitStr);
        if (GUILayout.Button("定格地图"))
        {
            int.TryParse(xLength, out width);
            int.TryParse(zLength, out height);
            int.TryParse(unitStr, out unit);
            map = new int[width, height];

            float mapWidth = map.GetLength(0) * unit;
            float mapHeight = map.GetLength(1) * unit;

            Vector3 camPos = Camera.main.transform.position;
            camPos.x = mapWidth / 2;
            camPos.z = mapHeight / 2;
            Camera.main.transform.position = camPos;

            Node.Init(map, unit, Vector3.zero);
            LineHelper.Instance.Clear();
            for (int i = 0; i <= map.GetLength(0); i++)
            {
                LineHelper.Instance.AddLine(new Vector3(i * unit, 0, 0), new Vector3(i * unit, 0, mapHeight), Color.blue, 9999);
            }
            for (int i = 0; i <= map.GetLength(1); i++)
            {
                LineHelper.Instance.AddLine(new Vector3(0, 0, i * unit), new Vector3(mapWidth, 0, i * unit), Color.blue, 9999);
            }
            hasInitStone = false;
            hasInitMap = true;
        }
        if (GUILayout.Button("初始化"))
        {
            for (int x = 0; x < Node.Width; x++)
            {
                for (int z = 0; z < Node.Height; z++)
                {
                    Node.GetNode(x, z).SetNodeType(NodeType.Empty);
                }
            }
            hasInitStone = false;
        }
        msg = GUILayout.TextField(msg);
    }


    Node GetClickNode()
    {
        Vector3 pos = Utils.GetFloorPosFromMousePos();
        int x = (int)pos.x / unit;
        int z = (int)pos.z / unit;
        return Node.GetNode(x, z);
    }

}