using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 对流层改正计算
{
    class Algorithm
    {
        /// <summary>
        /// 根据系数和高度角，计算对流层湿延迟分量
        /// </summary>
        /// <param name="se"></param>
        /// <param name="aw"></param>
        /// <param name="bw"></param>
        /// <param name="cw"></param>
        /// <returns></returns>
        public double CalMW(double se, double aw, double bw, double cw)
        {
            double up = 1 / (1 + aw / (1 + bw / (1 + cw)));
            double down = 1 / (se + aw / (se + bw / (se + cw)));

            //return up / down;
            return down / up;
        }

        /// <summary>
        /// 根据系数和高度角，计算对流层干延迟分量
        /// </summary>
        /// <param name="se"></param>
        /// <param name="H"></param>
        /// <param name="ad"></param>
        /// <param name="bd"></param>
        /// <param name="cd"></param>
        /// <param name="ah"></param>
        /// <param name="bh"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
        public double CalMD(double se, double H, double ad, double bd, double cd)
        {
            double ah = 2.53e-5, bh = 5.49e-3, ch = 1.14e-3;
            double left, right, left_up, left_down, right_down, right_up;

            left_up = 1 / (1 + ad / (1 + bd / (1 + cd)));
            left_down = 1 / (se + ad / (se + bd / (se + cd)));
            //left = left_up / left_down;
            left = left_down / left_up;


            right_up = 1 / (1 + ah / (1 + bh / (1 + ch)));
            right_down = 1 / (se + ah / (se + bh / (se + ch)));
            //right = (1 / se - right_up / right_down) * H / 1000;
            right = (1 / se - right_down / right_up) * H / 1000;

            return left + right;
        }

        /// <summary>
        /// 干延迟投影映射系数插值
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="B"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public double InterpolationW(double a1, double a2, double B, int n)
        {
            return a1 + (a2 - a1) * (B - n * 15) / 15;
        }

        /// <summary>
        /// 湿延迟投影映射系数插值
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="B"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public double InterpolationD(double avg1, double avg2, double amp1, double amp2, double B, int n, int t, int t0)
        {
            return avg1 + (avg2 - avg1) * (B - n * 15) / 15
               + amp1 + (amp2 - amp1) * (B - n * 15) / 15 * Math.Cos(2 * Math.PI * (t - t0) / 365.25);
        }


        /// <summary>
        /// 计算对流层延迟
        /// </summary>
        /// <param name="dataCenter"></param>
        public void CalTropDely(DataCenter dataCenter)
        {
            foreach (var station in dataCenter.stations)
            {
                double se = Math.Sin(station.E / 180.0 * Math.PI);
                double B = Math.Abs(station.B);     // 纬度要取绝对值
                double L = station.L;
                double H = station.H;
                double aw, bw, cw;                  // 插值后的湿延迟投影映射系数
                double ad, bd, cd;                  // 插值后的干延迟投影映射系数


                // 计算年积日
                int year = (int)(station.Time / 10000);
                int month = (int)((station.Time - year * 10000) / 100);
                int day = (int)station.Time - year * 10000 - month * 100;

                DateTime date = new DateTime(year, month, day);
                int t = date.DayOfYear;

                
                int n = (int)(B / 15);      // 根据纬度插值

                if(n == 0)
                {
                    aw = dataCenter.aw[0];
                    bw = dataCenter.bw[0];
                    cw = dataCenter.cw[0];

                    ad = dataCenter.ah1[0] + dataCenter.ah1[0] * Math.Cos(2 * Math.PI * (t - dataCenter.t0) / 365.25);
                    bd = dataCenter.bh1[0] + dataCenter.bh1[0] * Math.Cos(2 * Math.PI * (t - dataCenter.t0) / 365.25);
                    cd = dataCenter.ch1[0] + dataCenter.ch1[0] * Math.Cos(2 * Math.PI * (t - dataCenter.t0) / 365.25);


                }
                else if(n == 5)
                {
                    aw = dataCenter.aw[4];
                    bw = dataCenter.bw[4];
                    cw = dataCenter.cw[4];

                    ad = dataCenter.ah1[4] + dataCenter.ah1[4] * Math.Cos(2 * Math.PI * (t - dataCenter.t0) / 365.25);
                    bd = dataCenter.bh1[4] + dataCenter.bh1[4] * Math.Cos(2 * Math.PI * (t - dataCenter.t0) / 365.25);
                    cd = dataCenter.ch1[4] + dataCenter.ch1[4] * Math.Cos(2 * Math.PI * (t - dataCenter.t0) / 365.25);

                }
                else
                {
                    aw = InterpolationW(dataCenter.aw[n - 1], dataCenter.aw[n], B, n);
                    bw = InterpolationW(dataCenter.bw[n - 1], dataCenter.bw[n], B, n);
                    cw = InterpolationW(dataCenter.cw[n - 1], dataCenter.cw[n], B, n);

                    ad = InterpolationD(dataCenter.ah1[n - 1], dataCenter.ah1[n], dataCenter.ah2[n - 1], dataCenter.ah2[n], B, n, t, dataCenter.t0);
                    bd = InterpolationD(dataCenter.bh1[n - 1], dataCenter.bh1[n], dataCenter.bh2[n - 1], dataCenter.bh2[n], B, n, t, dataCenter.t0);
                    cd = InterpolationD(dataCenter.ch1[n - 1], dataCenter.ch1[n], dataCenter.ch2[n - 1], dataCenter.ch2[n], B, n, t, dataCenter.t0);
                }


                // 延迟改正量计算
                double ZWD = 0.1;                                             
                double ZHD = 2.29951 * Math.Pow(Math.E, -0.000116 * H);
                station.mw = CalMW(se, aw, bw, cw);
                station.md = CalMD(se, H, ad, bd, cd);
                station.TropDely = ZHD * station.md + ZWD * station.mw;
            }

        }



    }
}
