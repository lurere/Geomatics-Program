using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace 纵横断面
{
    class FileHelper
    {
        /// <summary>
        /// 文件打开函数
        /// </summary>
        /// <param name="dataCenter"></param>
        public void LoadData(DataCenter dataCenter)//不会写了
        {
             OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "文本文件|*.txt";
            opf.Title = "请选择要打开的文件";
             
            if (opf.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(opf.FileName);
                string[] line1 = sr.ReadLine().Split(',');
                dataCenter.RHD = double.Parse(line1[1]);
                sr.ReadLine();
                string[] line3 = sr.ReadLine().Split(',');
                Point testPoint1 = new Point(line3[0], double.Parse(line3[1]), double.Parse(line3[2]));
                dataCenter.TestPoints.Add(testPoint1);
                string[] line4 = sr.ReadLine().Split(',');
                Point testPoint2 = new Point(line4[0], double.Parse(line4[1]), double.Parse(line4[2]));
                dataCenter.TestPoints.Add(testPoint2);
                sr.ReadLine();

                //if(!sr.EndOfStream)不是if
                while(!sr.EndOfStream)
                {
                    string[] lines = sr.ReadLine().Split(',');
                    Point point = new Point(lines[0], double.Parse(lines[1]), double.Parse(lines[2]),double.Parse(lines[3]));
                    dataCenter.AllPoints.Add(point);
                    if (lines[0].StartsWith("K"))
                    {
                        dataCenter.KeyPoints.Add(point);
                    }
                    else
                    {
                        dataCenter.ScatPoints.Add(point);
                    }
                }
                sr.Close();
            }
        }
        public void SaveFile(string report)
        {
            try
            {
                SaveFileDialog svf = new SaveFileDialog();
                svf.Filter = "文本文件|*.txt";
                svf.Title = "请输入要保存的文件：";
                if (svf.ShowDialog() == DialogResult.OK)
                {
                    StreamWriter sw = new StreamWriter(svf.FileName);
                    sw.Write(report);
                    sw.Close();

                }
            }
            catch
            {
                MessageBox.Show("输出错误");
                return;
            }
            
        }
    }
}
