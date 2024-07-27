using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 纵横断面
{
    class Algorithm
    {
        /// <summary>
        /// 计算方位角
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double CalAzimuth(Point p1,Point p2)
        {
            double angle = 0;
            double sheta = 0;
            double dx = 0;
            double dy = 0;

            dx = p2.X - p1.X;
            dy = p2.Y - p1.Y;
            sheta = Math.Atan(dy / dx);

            if (dx != 0)
            {
                if (dy > 0 && dx > 0) angle = sheta;
                else if (dy > 0 && dx < 0) angle = Math.PI + sheta;
                else if (dy < 0 && dx < 0) angle = Math.PI + sheta;
                else if (dy < 0 && dx > 0) angle = 2 * Math.PI + sheta;
            }
            else
            {
                if (dy > 0)
                {
                    angle = Math.PI / 2;
                }
                else
                {
                    angle = 3 * Math.PI / 2;
                }
            }
            return angle;

        }
        /// <summary>
        /// 弧度转角度
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public static double Rad2Dms(double rad)
        {
            //背
            double deg, min, sec;

            rad = rad / Math.PI * 180;
            deg = (int)(rad);
            min = (int)((rad - deg) * 60);
            sec = (int)((rad - deg - min / 60) * 3600 * 100);
            return deg + min / 100.0 + sec / 1000000.0;
        }
        /// <summary>
        /// 计算内插点高程
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="allPoints"></param>
        /// <param name="n"></param>
        /// <param name="nearPoints"></param>
        /// <returns></returns>
        public static double CalInhertH(Point p1,List<Point>allPoints, int n,out List<Point> nearPoints)
        {
            //out nearPoints,错误,注意还有全部点
            nearPoints = new List<Point>();
            List<Point> tempPoints = new List<Point>();
            double temp1 = 0;
            double temp2 = 0;
            foreach (var o in allPoints)
            {
                tempPoints.Add(o);
            }
            for(int i=0; i<tempPoints.Count;i++)
            {
                tempPoints[i].Distance = CalDistance(p1, tempPoints[i]);
                if(p1.Name==tempPoints[i].Name)
                {
                    tempPoints.Remove(tempPoints[i]);
                    i--;
                }
            }
            tempPoints = tempPoints.OrderBy(o => o.Distance).ToList();
            
            for(int i=0;i<n;i++)
            {
                temp1 += tempPoints[i].H / tempPoints[i].Distance;
                temp2 += 1 / tempPoints[i].Distance;
                nearPoints.Add(tempPoints[i]);
            }
            return temp1 / temp2;
        }
        /// <summary>
        /// 计算两点间的距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double CalDistance(Point p1,Point p2)
        {
            double distance = 0;

            distance = Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) +(p2.Y - p1.Y) * (p2.Y - p1.Y));
            return distance;
        }
        /// <summary>
        /// 计算单个横截面积
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public static double CalProFile(Point p1,Point p2,double h)
        {
            double detaL = CalDistance(p1, p2);
            double area = (p1.H + p2.H - h * 2) *detaL/ 2;
            return area;
        }
        /// <summary>
        /// 计算纵断面长度
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double CalVerticalSectionLength(Point p1,Point p2)
        {
            double distance = 0;
            distance = CalDistance(p1, p2);
            return distance;
        }
        /// <summary>
        /// 计算纵断面的点
        /// </summary>
        /// <param name="keyPoints"></param>
        /// <param name="allPoints"></param>
        /// <param name="n"></param>
        /// <param name="RHD"></param>
        /// <param name="deta"></param>
        /// <param name="inhertPoints"></param>
        /// <returns></returns>
        public static PrioFile GetVerticalSectionInhertPoints(List<Point>keyPoints,List<Point>allPoints,int n,double RHD,
            double deta,out List<Point>inhertPoints)
        {
            PrioFile vertica =new PrioFile();
            List<Point> totalPoints = new List<Point>();
            Point start = new Point();
            Point end = new Point();
            inhertPoints = new List<Point>();
            start = keyPoints[0];
            totalPoints.Add(start);
            Point tempPoints = new Point();
            double sumD = 0;
            double sumS = 0;

            int num = 1;
            for(int i=0;i<keyPoints.Count-1;i++)
            {
                Point nowPoint = keyPoints[i];
                Point nextPoint = keyPoints[i+1];

                double Azimuth = CalAzimuth(nowPoint, nextPoint);
                double Distance = CalDistance(nowPoint, nextPoint);
                sumD += Distance;

                nextPoint.Distance = sumD;
                double D0 = CalDistance(keyPoints[i], keyPoints[0]);

                while((deta-D0)<Distance)
                {
                    double Li = deta - D0;
                    tempPoints.Name = "Z" + num.ToString();
                    tempPoints.X = keyPoints[i].X + Li * Math.Cos(Azimuth);
                    tempPoints.Y=keyPoints[i].Y + Li * Math.Sin(Azimuth);
                    tempPoints.H = CalInhertH(tempPoints, allPoints, n, out inhertPoints);
                    totalPoints.Add(tempPoints);
                    deta += deta;
                    num++;
                }
                totalPoints.Add(nextPoint);
 
            }
            for (int i = 0; i < totalPoints.Count - 1; i++)
            {
                double S = CalProFile(totalPoints[i], totalPoints[i + 1], RHD);
                sumS += S;
            }
            vertica.totallPoints = totalPoints;
            vertica.sumD = sumD;
            vertica.sumS = sumS;

            
            return vertica;
        }

    }
}
