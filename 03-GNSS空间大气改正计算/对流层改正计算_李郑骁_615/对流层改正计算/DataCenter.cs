using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 对流层改正计算
{

    class Station
    {
        public string Name;     // 测站名1 
        public double Time;     // 时间
        public double B;        // 纬度
        public double L;        // 经度
        public double H;        // 高程
        public double E;        // 高度角
        public double md;       // 对流层干延迟
        public double mw;       // 对流层湿延迟
        public double TropDely; // 对流层延迟

        public Station()
        {

        }


        public Station(string name,double time,double l,double b,double h,double e)
        /*
         * 用于创建已初始化的 Station 对象。

         * 
         */
        {
            this.Name = name;
            this.Time = time;
            this.B = b;
            this.L = l;
            this.H = h;
            this.E = e;
        }
    }

    class DataCenter
    {
        
        public List<Station> stations = new List<Station>();

        public int t0 = 28;     // 参考时刻年积日

        // 湿分量投影映射函数表
        public List<double> aw = new List<double>();
        public List<double> bw = new List<double>();
        public List<double> cw = new List<double>();
        
        // 干分量投影映射函数表
        public List<double> ah1 = new List<double>();
        public List<double> bh1 = new List<double>();
        public List<double> ch1 = new List<double>();
        public List<double> ah2 = new List<double>();
        public List<double> bh2 = new List<double>();
        public List<double> ch2 = new List<double>();


        public DataCenter()
        {
            aw.Add(0.00058021897); bw.Add(0.0014275268); cw.Add(0.043472961);   // 15°
            aw.Add(0.00056794847); bw.Add(0.0015138625); cw.Add(0.046729510);   // 30°
            aw.Add(0.00058118019); bw.Add(0.0014572752); cw.Add(0.043908931);   // 45°
            aw.Add(0.00059727542); bw.Add(0.0015007428); cw.Add(0.044626982);   // 60°
            aw.Add(0.00061641693); bw.Add(0.0017599082); cw.Add(0.054736038);   // 75°

            ah1.Add(0.0012769934); bh1.Add(0.0029153695); ch1.Add(0.062610505);   // 15°
            ah1.Add(0.0012683230); bh1.Add(0.0029152299); ch1.Add(0.062837393);   // 30°
            ah1.Add(0.0012465397); bh1.Add(0.0029288445); ch1.Add(0.063721774);   // 45°
            ah1.Add(0.0012196049); bh1.Add(0.0029022565); ch1.Add(0.063824265);   // 60°
            ah1.Add(0.0012045996); bh1.Add(0.0029024912); ch1.Add(0.064258455);   // 75°

            ah2.Add(0.0); bh2.Add(0.0); ch2.Add(0.0);
            ah2.Add(0.000012709626); bh2.Add(0.000021414979); ch2.Add(0.000090128400);   // 30°
            ah2.Add(0.000026523662); bh2.Add(0.000030160779); ch2.Add(0.000043497037);   // 45°
            ah2.Add(0.000034000452); bh2.Add(0.000072562722); ch2.Add(0.00084795348);    // 60°
            ah2.Add(0.000041202191); bh2.Add(0.00011723375); ch2.Add(0.0017037206);      // 75°
        }


    }
}
