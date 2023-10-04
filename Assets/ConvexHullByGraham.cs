using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ConvexHullByGraham : MonoBehaviour {
    
    static PointD[] points = new PointD[]
    {   
        new PointD(0, 0),
        new PointD(0, 10),
        new PointD(10, 10),
        new PointD(10, 0),
        new PointD(1, 0),

        new PointD(4, 3),
        new PointD(5, 2),
        new PointD(6, 5),
        new PointD(4, 9),
        new PointD(4, 2),

        new PointD(5, 1),
        new PointD(6, 5),
        new PointD(1, 3),
        new PointD(7, 2),
        new PointD(8, 2),

        new PointD(6, 7),
        new PointD(8, 5),
        new PointD(9, 3),
        new PointD(7, 8),
        new PointD(8, 9),
    };

        public void GetConvexHullByGraham()
        {
            Console.WriteLine("Graham 算法");
            PrintPoints(GetConvexHullByGraham(points));
        }

        private void PrintPoints(PointD[] points)
        {
            // Console.WriteLine(points.Select(p => $"  ({p.X}, {p.Y})").Join("\r\n"));
        }

    
        /// <summary>
        /// 通过Graham算法获取围绕所有点的凸多边形的轮廓点<br/>
        /// </summary>
        /// <param name="points">点数组</param>
        /// <returns>轮廓点数组</returns>
        public static PointD[] GetConvexHullByGraham(PointD[] points)
        {
            if (points.Length < 3)
            {
                throw new ArgumentException("凸包算法需要至少3个点");
            }

            List<PointD> pointList = new List<PointD>(points);

            // 找到最下面的点
            PointD lowestPoint = pointList[0];
            for (int i = 1; i < pointList.Count; i++)
            {
                if (pointList[i].Y < lowestPoint.Y || (pointList[i].Y == lowestPoint.Y && pointList[i].X < lowestPoint.X))
                {
                    lowestPoint = pointList[i];
                }
            }

            // 将最下面的点作为起点，并按照极角排序其他点
            pointList.Remove(lowestPoint);
            pointList.Sort((p1, p2) =>
            {
                double angle1 = Math.Atan2(p1.Y - lowestPoint.Y, p1.X - lowestPoint.X);
                double angle2 = Math.Atan2(p2.Y - lowestPoint.Y, p2.X - lowestPoint.X);
                if (angle1 < angle2)
                {
                    return -1;
                }
                else if (angle1 > angle2)
                {
                    return 1;
                }
                else
                {
                    double distance1 = Math.Sqrt(Math.Pow(p1.X - lowestPoint.X, 2) + Math.Pow(p1.Y - lowestPoint.Y, 2));
                    double distance2 = Math.Sqrt(Math.Pow(p2.X - lowestPoint.X, 2) + Math.Pow(p2.Y - lowestPoint.Y, 2));
                    if (distance1 < distance2)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            });

            // 使用栈来存储凸包的点
            Stack<PointD> hull = new Stack<PointD>();
            hull.Push(lowestPoint);
            hull.Push(pointList[0]);
            for (int i = 1; i < pointList.Count; i++)
            {
                PointD top = hull.Pop();
                while (hull.Any() && Cross(hull.Peek(), top, pointList[i]) <= 0)
                {
                    top = hull.Pop();
                }
                hull.Push(top);
                hull.Push(pointList[i]);
            }

            return hull.ToArray();
        }

        /// <summary>
        /// 计算从 a 到 b 再到 c 的叉积
        /// </summary>
        /// <returns>叉积值</returns>
        private static double Cross(PointD a, PointD b, PointD c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
        }

        public struct PointD 
        {
            public PointD(double x, double y) 
            {
                X = x;
                Y = y;
            }

            public double X { get; set; }
            public double Y { get; set; }

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                PointD other = (PointD)obj;
                return X.Equals(other.X) && Y.Equals(other.Y);
            }
        }

}
 