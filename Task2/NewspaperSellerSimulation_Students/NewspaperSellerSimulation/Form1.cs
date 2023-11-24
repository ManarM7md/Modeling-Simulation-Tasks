using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewspaperSellerModels;
using NewspaperSellerTesting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using static NewspaperSellerModels.Enums;
using static NewspaperSellerTesting.Constants;

namespace NewspaperSellerSimulation
{
    
    public partial class Form1 : Form
    {
       private SimulationSystem obj;


        public Form1()
        {

           
            InitializeComponent();
            obj = new SimulationSystem();
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "Good";
            dataGridView1.Columns[1].Name = "Fair";
            dataGridView1.Columns[2].Name = "Poor";

            dataGridView2.ColumnCount = 4;
            dataGridView2.Columns[0].Name = "Demand";
            dataGridView2.Columns[1].Name = "Good";
            dataGridView2.Columns[2].Name = "Fair";
            dataGridView2.Columns[3].Name = "Poor";
        }
       
        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        //load data form file
        String data,row, filename;
        public int NumOfNewspapers, NumOfRecords;
        decimal PurchasePrice, ScrapPrice, SellingPrice;
        String[] lst1,lst2;

        List<Decimal> prob;
        List<Enums.DayType> dayType;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        List<Int32> demond;

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;
                var sr = new StreamReader(filename);
                while (!sr.EndOfStream)
                {
                    data = sr.ReadLine();

                    if (data == "") continue;
                    if (data == "NumOfNewspapers")
                    {
                        data = sr.ReadLine();

                        textBox1.Text = Convert.ToString(data);
                        continue;
                    }
                    if (data == "NumOfRecords")
                    {
                        data = sr.ReadLine();

                        textBox3.Text = Convert.ToString(data);
                        continue;
                    }
                    if (data == "PurchasePrice")
                    {
                        data = sr.ReadLine();

                        textBox2.Text = Convert.ToString(data);
                        continue;
                    }
                    if (data == "ScrapPrice")
                    {
                        data = sr.ReadLine();

                        textBox4.Text = Convert.ToString(data);
                        continue;
                    }
                    if (data == "SellingPrice")
                    {
                        data = sr.ReadLine();

                        textBox5.Text = Convert.ToString(data);
                        continue;
                    }
                    if (data == "DayTypeDistributions")
                    {
                        int x = 0;

                        while (true)
                        {
                            data = sr.ReadLine();

                            if (data == "") { break; }
                            if (data == null) { break; }
                            // MessageBox.Show(row);
                            lst1 = data.Split(',');


                            dataGridView1.Rows.Add(lst1);
                            x++;
                        }

                        continue;
                    }
                    if (data == "DemandDistributions")
                    {
                        int y = 0;

                        while (true)
                        {
                            data = sr.ReadLine();
                            if (data == "") { break; }
                            if (data == null) { break; }
                            lst2 = data.Split(',');

                            dataGridView2.Rows.Add(lst2);
                            y++;
                        }
                        continue;
                    }

                }
               
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            prob = new List<Decimal>();
            dayType = new List<Enums.DayType>();
            demond = new List<Int32>();

            obj.NumOfNewspapers = Int32.Parse(textBox1.Text);
            obj.NumOfRecords = Int32.Parse(textBox3.Text);
            obj.PurchasePrice = Decimal.Parse(textBox2.Text);
            obj.ScrapPrice = Decimal.Parse(textBox4.Text);
            obj.SellingPrice = Decimal.Parse(textBox5.Text);

            for (int x = 0; x < dataGridView1.Rows.Count; x++)
            {
                if (dataGridView1.Rows[x].Cells[0].Value != null)
                {
                    dayType.Add((Enums.DayType)0);
                    prob.Add(Decimal.Parse(dataGridView1.Rows[x].Cells[0].Value.ToString()));

                }
                // Handle parsing error for probability value
                if (dataGridView1.Rows[x].Cells[1].Value != null)
                {
                    dayType.Add((Enums.DayType)1);
                    prob.Add(Decimal.Parse(dataGridView1.Rows[x].Cells[1].Value.ToString()));


                }
                if (dataGridView1.Rows[x].Cells[2].Value != null)
                {
                    dayType.Add((Enums.DayType)2);
                    prob.Add(Decimal.Parse(dataGridView1.Rows[x].Cells[2].Value.ToString()));
                }

            }
            obj.Calculate1_CummProbability_RandomDigitAssigmint(dayType, prob);
            dayType.Clear();
            prob.Clear();
            for (int x = 0; x < dataGridView2.Rows.Count; x++)
            {
                if (dataGridView2.Rows[x].Cells[0].Value != null)
                {
                    demond.Add(Int32.Parse(dataGridView2.Rows[x].Cells[0].Value.ToString()));
                }
                if (dataGridView2.Rows[x].Cells[1].Value != null)
                {
                    dayType.Add((Enums.DayType)0);
                    prob.Add(Decimal.Parse(dataGridView2.Rows[x].Cells[1].Value.ToString()));
                }
                if (dataGridView2.Rows[x].Cells[2].Value != null)
                {
                    dayType.Add((Enums.DayType)1);
                    prob.Add(Decimal.Parse(dataGridView2.Rows[x].Cells[2].Value.ToString()));
                }
                if (dataGridView2.Rows[x].Cells[3].Value != null)
                {
                    dayType.Add((Enums.DayType)2);
                    prob.Add(Decimal.Parse(dataGridView2.Rows[x].Cells[3].Value.ToString()));
                }
            }
            obj.Calculate2_CummProbability_RandomDigitAssigmint(dayType, prob,demond);
            
            Form2 form2 = new Form2(obj);
            form2.Show();
         this.Hide();
            //SimulationSystem system = new SimulationSystem();
            string FileName = "";
            if (filename == "E:\\FCIS\\FCIS_4_1\\Modeling & Simulation\\Lab 3_Task 2\\NewspaperSellerSimulation_Students\\NewspaperSellerSimulation\\TestCases\\TestCase1.txt")
            { FileName = Constants.FileNames.TestCase1; }
            else if (filename =="E:\\FCIS\\FCIS_4_1\\Modeling & Simulation\\Lab 3_Task 2\\NewspaperSellerSimulation_Students\\NewspaperSellerSimulation\\TestCases\\TestCase2.txt") { FileName = Constants.FileNames.TestCase2; }
            else if (filename =="E:\\FCIS\\FCIS_4_1\\Modeling & Simulation\\Lab 3_Task 2\\NewspaperSellerSimulation_Students\\NewspaperSellerSimulation\\TestCases\\TestCase3.txt") { FileName = Constants.FileNames.TestCase3; }
            string result = TestingManager.Test(obj, FileName);
            MessageBox.Show(result);
        }
        
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        int x, y;
        int index = 1;
         
        
    

        
    }
}
