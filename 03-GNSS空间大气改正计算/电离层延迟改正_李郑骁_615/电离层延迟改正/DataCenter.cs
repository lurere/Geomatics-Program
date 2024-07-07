using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 电离层延迟改正
{
    class Satellite
    {
        //与输出有关吗
        /// <summary>
        /// 
        /// </summary>
        public string Prn;      // 卫星 PRN
        /// <summary>
        /// 
        /// </summary>
        public double X;        // X 坐标
        public double Y;        // Y 坐标
        public double Z;        // Z 坐标

        public double A;        // 方位角
        public double E;        // 高度角
        public double IonDely;  //电离层延迟

        /// <summary>
        /// 
        /// </summary>
        public Satellite()//和类名一样默认构造函数：Satellite()，不接收参数，用于创建空的卫星对象。
        {

        }

        public Satellite(string prn, double x, double y, double z)//接收卫星 PRN 号码和 XYZ 坐标，并初始化相应的属性。
        {
            this.Prn = prn;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

    }
    
    /// <summary>
    /// 记住这种定义方法
    /// </summary>
    class DataCenter
    {
        //这个是输出，有一部分在上面
        public double Bp = 31 / 180.0 * Math.PI; 
        public double Lp = 114 / 180.0 * Math.PI;                                // 测站经纬度
        public double Station_x = -2225669.7744, Station_y = 4998936.1598, Station_z = 3265908.9678;        // 测站 XYZ
        public double t_year = 2016, t_mon = 8, t_day = 16, t_hour = 10, t_min = 45, t_sec = 0;     //直接定义  
        public List<Satellite> satellites = new List<Satellite>();

    }
}
