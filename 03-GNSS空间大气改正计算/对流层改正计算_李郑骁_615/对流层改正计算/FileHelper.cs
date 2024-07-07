using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 对流层改正计算
{
    class FileHelper
    {
        public void LoadData(DataCenter dataCenter)
        {
            try
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "文本文件|*.txt";
                opf.Title = "请选择要导入的数据文件";
                if (opf.ShowDialog() == DialogResult.OK)
                {
                    StreamReader sr = new StreamReader(opf.FileName);
                    string line1 =  sr.ReadLine();
                    dataCenter.stations = new List<Station>();
                    Station station = new Station();
                    
                    while (!sr.EndOfStream)
                    {
                        string[] lines = sr.ReadLine().Trim().Split(',');
                        station = new Station(lines[0], double.Parse(lines[1]), double.Parse(lines[2]),
                            double.Parse(lines[3]), double.Parse(lines[4]), double.Parse(lines[5]));
                        dataCenter.stations.Add(station);
                    }

                }

            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("导入数据文件失败，请重新选择数据文件！");
                throw;
            }


        }

        public void SaveReport(string report)
        {
            try
            {
                SaveFileDialog svf = new SaveFileDialog();
                svf.Filter = "文本文件|*.txt";
                svf.ShowDialog();
                StreamWriter sw = new StreamWriter(svf.FileName);
                sw.Write(report);
                sw.Flush();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
