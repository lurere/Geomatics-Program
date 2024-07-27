using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 纵横断面
{
    public partial class Form1 : Form
    {
        DataCenter dataCenter = new DataCenter();
        FileHelper fileHelper = new FileHelper();
        public string report = "";


        double H1;
        double H2;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        { 
                fileHelper.LoadData(dataCenter);
                dataGridView1.RowCount = dataCenter.AllPoints.Count;
            for (int i = 0; i < dataCenter.AllPoints.Count; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = dataCenter.AllPoints[i].Name;
                dataGridView1.Rows[i].Cells[1].Value = dataCenter.AllPoints[i].X;
                dataGridView1.Rows[i].Cells[2].Value = dataCenter.AllPoints[i].Y;
                dataGridView1.Rows[i].Cells[3].Value = dataCenter.AllPoints[i].H;

            }
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            打开ToolStripMenuItem_Click(sender, e);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
             fileHelper.SaveFile(report);
        }

        private void 保存报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void 坐标方位角ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            double Azimuth= Algorithm.CalAzimuth(dataCenter.TestPoints[0],dataCenter.TestPoints[1]);
            report += "\t\t\t计算报告\t\t\t\n";
            report += "测试点的方位角：" + Azimuth.ToString("F3");
            richTextBox1.Text = report;
        }

        private void 内插点P的高程值ToolStripMenuItem_Click(object sender, EventArgs e)
        {

             H1=Algorithm.CalInhertH(dataCenter.TestPoints[0], dataCenter.AllPoints, 5, out List<Point> aNearPoints);
             H2=Algorithm.CalInhertH(dataCenter.TestPoints[1], dataCenter.AllPoints, 5, out List<Point> bNearPoints);
            report += "\n\t\t\t获得内插点高程\t\t\t\n";
            report += "内差点A高程：\t" + H1.ToString("F3")+"\n离A最近的点\n";
            for(int i=0;i< aNearPoints.Count;i++)
            {
                report += "点号\t" + aNearPoints[i].Name + "\t距离\t" + aNearPoints[i].Distance + "\n";
            }
            report += "内差点B高程：\t" + H1.ToString("F3") + "\n离B最近的点\n";
            for (int i = 0; i < bNearPoints.Count; i++)
            {
                report += "点号\t" + aNearPoints[i].Name + "\t距离\t" + aNearPoints[i].Distance + "\n";
            }
            richTextBox1.Text = report;
        }

        private void 断面面积计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
             
            dataCenter.TestPoints[0].H = H1;
            dataCenter.TestPoints[1].H = H2;
            double area = Algorithm.CalProFile(dataCenter.TestPoints[0], dataCenter.TestPoints[1], dataCenter.RHD);
            report += "A,B为梯形的面积：\r" +
                "\t" + area.ToString("F3")+"\r\n";
            richTextBox1.Text = report;
        }

        private void 中间数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 计算纵断面总长度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 纵断面长度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double distance = 0;
            for(int i=0;i<dataCenter.KeyPoints.Count-1;i++)
            {
                distance = Algorithm.CalVerticalSectionLength(dataCenter.KeyPoints[i], dataCenter.KeyPoints[i + 1]);
                distance += distance;
            }
            report += "纵断面总长度：\t" + distance.ToString("F3")+"\n";
            richTextBox1.Text = report;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            坐标方位角ToolStripMenuItem_Click(sender, e);
            内插点P的高程值ToolStripMenuItem_Click(sender, e);
            断面面积计算ToolStripMenuItem_Click(sender, e);
            纵断面长度ToolStripMenuItem_Click(sender, e);
            内插点的平面坐标ToolStripMenuItem_Click(sender, e);
        }

        private void 内插点的平面坐标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Point> inhertPoints = new List<Point>();
            dataCenter.vertical = Algorithm.GetVerticalSectionInhertPoints(dataCenter.KeyPoints,
                dataCenter.AllPoints, 5, dataCenter.RHD, 10 , out inhertPoints);
            report += "\t\t\t\t纵断面内插点：\r\n ";
            foreach (var item in dataCenter.vertical.totallPoints)
            {
                report += "点名：  " + item.Name + "       X:" + item.X.ToString("F3") + "      Y:" + item.Y.ToString("F3") + "     H:" + item.H.ToString("F3") + "\r\n";
            }
            richTextBox1.Text = report;
        }
    }
}