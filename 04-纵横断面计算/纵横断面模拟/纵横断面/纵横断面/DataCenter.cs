using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 纵横断面
{
    class DataCenter
    {
        public List<Point> KeyPoints=new List<Point>();
        public List<Point> AllPoints = new List<Point>();
        public List<Point> TestPoints = new List<Point>();
        public List<Point> ScatPoints = new List<Point>();
        public PrioFile vertical = new PrioFile();
        public double RHD;
    }
    class Point
    {
        public string Name;
        public double X;
        public double Y;
        public double H;
        public double Distance;
        public bool IsKeyPoints;
        public Point()
        {
            IsKeyPoints = false;
        }
        public Point(string name,double x,double y)
        {
            this.Name = name;
            this.X = x;
            this.Y = y; 
        }
        public Point(string name,double x,double y,double h)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
            this.H = h;
        }

    }
    class PrioFile
    {
        public  Point start;
        public  Point end;
        public  List<Point> totallPoints;//全部点集
        public Point allPoints;//全部点
        public double sumS;//断面面积
        public double sumD;//断面长度
        public PrioFile()
        {
            start = new Point();
            end = new Point();
            totallPoints = new List<Point>();
            allPoints = new Point();
        }
    }

}
