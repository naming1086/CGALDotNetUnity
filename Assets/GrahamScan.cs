using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class GrahamScan : MonoBehaviour {
    //离散的点
    public Transform[] Points; //这是用于存储一组点的数组，这些点将被用于计算凸包。
    //组成边的顶点集合
    public List<Transform> ePoint = new List<Transform>(); //这是一个列表，用于存储组成凸包的顶点。
 
    private bool IsFinsh = false;
    void Start()
    {
        Aritmetic();
    }
 
    //算法
    void Aritmetic()
    {
        Transform bp = BasePoint(); //获取基准点 bp，通常是具有最低y坐标（在代码中使用z坐标代替y坐标）和最低x坐标的点。
        bp.name = "P0"; 
        ePoint.Add(bp); //将基准点添加到 ePoint 列表中。   
        Transform[] polarSort = PolarSort(bp);
        for (int i = 0; i < polarSort.Length; i++)
        {
            if (polarSort[i] == bp) continue;
            polarSort[i].name = "P" + (i + 1);
        }
        //通过排序，而可以得知 第一个点肯定在凸边上
        ePoint.Add(polarSort[0]);
        StartCoroutine(Ia(polarSort));
    }
 
    IEnumerator Ia(Transform[] polarSort)
    {
        for (int i = 1; i < polarSort.Length; i++)
        {
            yield return new WaitForSeconds(1);
            ePoint.Add(polarSort[i]);
            //这是一个协程，用于执行Graham扫描的主要过程。它遍历 polarSort 中的点，依次添加它们到 ePoint 中，并检查是否需要移除前一点以保持凸包性质。
            float d = PointDir(ePoint[ePoint.Count-1].position, ePoint[ePoint.Count - 2].position, ePoint[ePoint.Count - 3].position);
            if (d <= 0)
                continue;
            while (true)
            {
                yield return new WaitForSeconds(1);
                ePoint.RemoveAt(ePoint.Count-2);
                d = PointDir(ePoint[ePoint.Count - 1].position, ePoint[ePoint.Count - 2].position, ePoint[ePoint.Count - 3].position);
                if (d < 0)
                    break;
            }
        }
        IsFinsh = true;
    }
 
    //这个方法用于在Unity场景视图中绘制凸包的边缘。如果算法尚未完成，它会绘制正在构建的凸包边缘；如果完成，它将绘制最终的凸包。
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (!IsFinsh)
        {
            for (int i = 0; i < ePoint.Count - 1; i++)
                Gizmos.DrawLine(ePoint[i].position, ePoint[i + 1].position);
            return;
        }
 
        for (int i = 0; i < ePoint.Count; i++)
        {
            if (i == ePoint.Count - 1)
            {
                Gizmos.DrawLine(ePoint[i].position,ePoint[0].position);
                continue;
            }
            Gizmos.DrawLine(ePoint[i].position,ePoint[i+1].position);
        }
 
    }
 
    /// <summary>
    /// 获取基点P0
    /// </summary>
    /// <returns></returns>
    Transform BasePoint()
    {
        //取y值最小 如果多个y值相等，去x最小(这里y 我们取z)
        Transform minPoint = Points[0];
        for (int i = 1; i < Points.Length; i++)
        {
            if (Points[i].position.z < minPoint.position.z)
                minPoint = Points[i];
            else if (Points[i].position.z < minPoint.position.z)
                minPoint = Points[i].position.x < minPoint.position.x ? Points[i] : minPoint;
        }
        return minPoint;
    }
    /// <summary>
    /// 获取点的方向 =0 在线上 <0在左侧 >0在右侧
    /// </summary>
    /// <param name="p"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    float PointDir(Vector3 p, Vector3 p1, Vector3 p2)
    {
        Vector3 v1 = p1 - p;
        Vector3 v2 = p2 - p1;
        float f = v1.x * v2.z - v2.x * v1.z;
        return f;
    }
 
    /// <summary>
    /// 极角排序
    /// </summary>
    /// <param name="bp"></param>
    /// <returns></returns>
    Transform[] PolarSort(Transform bp)
    {
        List<Transform> p = new List<Transform>();
        for (int i = 0; i < Points.Length; i++)
        {
            //如果是自己，则跳过
            if (Points[i] == bp) continue;
            Vector3 v = Vector3.zero;
            float e = GetProlar(Points[i], bp,out v);
            int index = -1;
            for (int j = 0; j < p.Count; j++)
            {
                Vector3 v1 = Vector3.zero;
                float e1 = GetProlar(p[j],bp,out v1);
                if (e1 > e)
                {
                    index = j;
                    break;
                }
                if (e1 == e && v.magnitude < v1.magnitude)
                {
                    index = j;
                    break;
                }
            }
            if (index == -1)
            {
                p.Add(Points[i]);
                continue;
            }
            p.Insert(index,Points[i]);
        }
        return p.ToArray();
    }
    /// <summary>
    /// 获取极角
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    /// <returns></returns>
    float GetProlar(Transform pos1, Transform pos2,out Vector3 v)
    {
        v = pos1.position - pos2.position;
        return Mathf.Atan2(v.z,v.x);
    }
 
}
 