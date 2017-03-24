using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LineHelper : MonoBehaviour
{
    public static Color ForeverColor = Color.black;  //永久线条的颜色

    static int _curId = 0;

    static LineHelper _instance;

    public static LineHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("LineHelper");
                _instance = go.AddComponent<LineHelper>();
            }
            return _instance;
        }
    }

    List<Line> normalLines = new List<Line>();


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < normalLines.Count; )
        {
            if (normalLines[i].Life < 0)
            {
                normalLines.RemoveAt(i);
            }
            else
            {
                normalLines[i].DebugDraw();
                normalLines[i].Life -= Time.deltaTime;
                i++;
            }
        }
    }
    public void ClearAll()
    {
        normalLines.Clear();
    }

    public Line CreateLine(Vector3 p1, Vector3 p2, Color color, float time)
    {
        Line line = new Line(p1, p2, color, time);
        normalLines.Add(line);
        return line;
    }

    public Line CreateLine(Color color, float time)
    {
        Line line = new Line(color, time);
        normalLines.Add(line);
        return line;
    }


    public void AddLine(Vector3 p1, Vector3 p2, Color color, float time)
    {
        CreateLine(p1, p2, color, time);
    }

    public void AddLine(int id, Vector3 p1, Vector3 p2, Color color, float time)
    {
        Line line = GetLine(id);
        if (line == null)
        {
            line = CreateLine(p1, p2, color, time);
            line.Id = id;
        }
        else
        {
            line.Points[0] = p1;
            line.Points[1] = p2;
            line.Life = time;
            line.Color = color;
        }
    }

    public Line GetLine(int id)
    {
        return normalLines.Find(t => t.Id == id);
    }



    public static void DrawSector(Vector3 center, float radius, Color color, float duration, Vector3 forward, float angle)
    {
        if (angle > 360)
        {
            angle = 360;
        }
        forward.Normalize();
        int count = (int)(angle * 0.1f);

        Vector3 startVec = Quaternion.Euler(0, -0.5f * angle, 0) * forward * radius;
        Vector3 endVec = Quaternion.Euler(0, 0.5f * angle, 0) * forward * radius;


        Vector3 curPos = center + startVec;
        for (int i = 1; i < count; i++)
        {
            Vector3 nextPos = center + Quaternion.Euler(0, angle * i / (count - 1), 0) * startVec;
            Debug.DrawLine(curPos, nextPos, color, duration, false);
            curPos = nextPos;
        }

        if (angle < 360)
        {//角度小于360,则表示是扇形,需要画两条边
            Debug.DrawLine(center, center + startVec, color, duration, false);
            Debug.DrawLine(center, center + endVec, color, duration, false);
        }

    }

    public void DeleteLine(int id)
    {
        normalLines.RemoveAll(t => t.Id == id);
    }

    public void DeleteLine(Line line)
    {
        normalLines.Remove(line);
    }

    /// <summary>
    /// 删除所有普通线
    /// </summary>
    public void ClearNormalLine()
    {
        normalLines.Clear();
    }
    /// <summary>
    /// 删除所有线
    /// </summary>
    public void Clear()
    {
        ClearNormalLine();
    }




}

public class Line
{
    public int Id = -1;
    public List<Vector3> Points { get; set; }
    public Color Color { get; set; }
    public float Life;


    public Line(Color color, float life)
    {
        this.Color = color;
        this.Life = life < 0 ? 1000000 : life;
        this.Points = new List<Vector3>();
    }


    public Line(Vector3 p1, Vector3 p2, Color color, float life)
        : this(color, life)
    {

        this.Points.Add(p1);
        this.Points.Add(p2);
    }

    public void DebugDraw()
    {
        if (this.Points.Count < 2)
        {
            return;
        }

        for (int i = 0; i < this.Points.Count - 1; i++)
        {
            Debug.DrawLine(this.Points[i], this.Points[i + 1], this.Color, 0.01f, false);
        }

        if (this.Points.Count > 2)
        {
            Debug.DrawLine(this.Points[0], this.Points[this.Points.Count - 1], this.Color, 0.01f, false);
        }
    }



}