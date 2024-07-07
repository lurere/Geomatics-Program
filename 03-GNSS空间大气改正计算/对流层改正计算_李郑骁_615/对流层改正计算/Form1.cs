using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 对流层改正计算
{
    public partial class Form1 : Form
    {
        FileHelper fileHelper = new FileHelper();
        DataCenter dataCenter = new DataCenter();
        Algorithm algorithm = new Algorithm();
        string Report = "";

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 导入计算数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            fileHelper.LoadData(dataCenter);
            if (dataCenter.stations.Count > 0)
            {
                dataGridView1.RowCount = dataCenter.stations.Count;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = dataCenter.stations[i].Name;
                    dataGridView1.Rows[i].Cells[1].Value = dataCenter.stations[i].B.ToString();
                    dataGridView1.Rows[i].Cells[2].Value = dataCenter.stations[i].L.ToString();
                    dataGridView1.Rows[i].Cells[3].Value = dataCenter.stations[i].H.ToString();
                    dataGridView1.Rows[i].Cells[4].Value = dataCenter.stations[i].E.ToString();

                }
                toolStripStatusLabel2.Text = "成功导入数据";
            }

        }

        /// <summary>
        /// 对流层延迟计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            algorithm.CalTropDely(dataCenter);
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Cells[5].Value = dataCenter.stations[i].md.ToString();
                dataGridView1.Rows[i].Cells[6].Value = dataCenter.stations[i].mw.ToString();
                dataGridView1.Rows[i].Cells[7].Value = dataCenter.stations[i].TropDely.ToString();
            }

            toolStripStatusLabel6.Text = "对流层参数计算完成";

            Report = "";
            for (int i = 0; i < dataCenter.stations.Count; i++)
            {
                Report += dataCenter.stations[i].Name + "\t" + dataCenter.stations[i].md.ToString("F6") + "\t" + dataCenter.stations[i].mw.ToString("F6") + "\t" + dataCenter.stations[i].TropDely.ToString("F6") + "\r\n";
            }

            textBox1.Text = Report;

        }

        /// <summary>
        /// 保存计算报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            fileHelper.SaveReport(Report);
        }
    }
}
