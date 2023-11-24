
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewspaperSellerModels;
using NewspaperSellerTesting;
namespace NewspaperSellerSimulation
{
  
    public partial class Form2 : Form 
    {
        private DataGridView dataGridView1;
        private Button button1;
        private Label label1;
        private SimulationSystem simulationSystem;


        public Form2(SimulationSystem obj)
        {
            InitializeComponent();
            dataGridView1.ColumnCount = 9;
            dataGridView1.Columns[0].Name = "Day";
            dataGridView1.Columns[1].Name = "Random Number For Day Type";
            dataGridView1.Columns[2].Name = "Type Of Newsday";
            dataGridView1.Columns[3].Name = "Random Digit For Demand";
            dataGridView1.Columns[4].Name = "Demand";
            dataGridView1.Columns[5].Name = "Sales";
            dataGridView1.Columns[6].Name = "Lost profit from excess Demand";
            dataGridView1.Columns[7].Name = "Salvage From Sale Of Scrap";
            dataGridView1.Columns[8].Name = "Daily profit";

            simulationSystem=obj;
            simulationSystem.system_output();
            System.Threading.Thread.Sleep(100); 
            for (int i = 0; i < simulationSystem.NumOfRecords; i++)
            {
                this.dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = simulationSystem.SimulationTable[i].DayNo.ToString();
                dataGridView1.Rows[i].Cells[1].Value = simulationSystem.SimulationTable[i].RandomNewsDayType.ToString();
                dataGridView1.Rows[i].Cells[2].Value = simulationSystem.SimulationTable[i].NewsDayType.ToString();
                dataGridView1.Rows[i].Cells[3].Value = simulationSystem.SimulationTable[i].RandomDemand.ToString();
                dataGridView1.Rows[i].Cells[4].Value = simulationSystem.SimulationTable[i].Demand.ToString();
                dataGridView1.Rows[i].Cells[5].Value = simulationSystem.SimulationTable[i].SalesProfit.ToString();
                dataGridView1.Rows[i].Cells[6].Value = simulationSystem.SimulationTable[i].LostProfit.ToString();
                dataGridView1.Rows[i].Cells[7].Value = simulationSystem.SimulationTable[i].ScrapProfit.ToString();
                dataGridView1.Rows[i].Cells[8].Value = simulationSystem.SimulationTable[i].DailyNetProfit.ToString();
                

            }
        }

        private async void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(226, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Simulation Table";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 58);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(626, 481);
            this.dataGridView1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(231, 557);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(169, 39);
            this.button1.TabIndex = 2;
            this.button1.Text = "Performance";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(650, 620);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Name = "Form2";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.dataGridView1, 0);
            this.Controls.SetChildIndex(this.button1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(simulationSystem);
            form3.Show();
            this.Hide();    
        }
    }
}