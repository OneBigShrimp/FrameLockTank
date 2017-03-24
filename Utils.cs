using UnityEngine;
using System.Collections;

public class Utils
{

    /// <summary>
    /// 获取一条射线和一个平面的交点,返回值表示是否有交点
    /// </summary>
    /// <param name="ray">射线</param>
    /// <param name="plane">平面</param>
    /// <param name="point">交点</param>
    /// <returns></returns>
    public static bool GetPointFromLineAndPlane(Ray ray, Plane plane, out Vector3 point)
    {
        float dis;
        bool result = plane.Raycast(ray, out dis);
        if (result)
        {
            point = ray.GetPoint(dis);
        }
        else
        {
            point = Vector3.zero;
        }
        return result;
    }
    /// <summary>
    /// 获取鼠标点击位置映射到标准地板平面的坐标(Y=0的平面)
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetFloorPosFromMousePos()
    {
        Vector3 point;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        GetPointFromLineAndPlane(ray, plane, out point);
        return point;
    }
}
