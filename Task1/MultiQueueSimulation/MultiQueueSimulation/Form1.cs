using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MultiQueueModels;
using MultiQueueTesting;

namespace MultiQueueSimulation
{

    public partial class Form1 : Form
    {
        

        public Form1()
        {
            
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            input_panel.BringToFront();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)

        {
            if (!char.IsNumber(e.KeyChar)&& (!char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel2.BringToFront();
        }
        int server_graphID = 1;
        List<int> z;
        private void button3_Click(object sender, EventArgs e)
        {
            
            panel3.BringToFront();
          z=new List<int>();
            z = system.Service_time(server_graphID);
            chart1.Series["Busy Time"].SetCustomProperty("PointWidth", "1");
            for (int i = 0; i < system.TimeOfEndSimulation; i++) {

                chart1.Series["Busy Time"].Points.AddXY(i+1, z[i]);

            }
           // chart1.ChartAreas[0].AxisX.IsStartedFromZero = true;
            //chart1.ChartAreas[0].AxisX.Minimum = 0;

        }
       String Data;
        int numofservers;
        int StoppingNum;
        int serverMethod;
        int stopCerteria;
        String row;
        int x, y;
        String[] tmp;
        String[] tmp2;
        int current_server = 1;
        String filename;

        SimulationSystem system =new SimulationSystem();
        //load data form file
        private void button6_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {

                filename = openFileDialog1.FileName;
                var sr = new StreamReader(filename);
                while (!sr.EndOfStream)
                {
                    Data = sr.ReadLine();

                    if (Data == "") continue;
                    //MessageBox.Show(Data);
                    if (Data== "NumberOfServers")
                    {
                        Data = sr.ReadLine();
                        
                        textBox1.Text = Convert.ToString(Data);
                        continue;
                    }
                    if(Data== "StoppingNumber")
                    {
                        Data = sr.ReadLine();
                        StoppingNum = Convert.ToInt32(Data);
                        textBox2.Text = Convert.ToString(Data);
                        continue;

                    }
                    if (Data == "StoppingCriteria")
                    {
                        Data = sr.ReadLine();
                        stopCerteria = Convert.ToInt32(Data);
                        // zero base 
                        comboBox2.SelectedIndex = stopCerteria - 1;
                        continue;

                    }
                    if (Data == "SelectionMethod") {

                        Data = sr.ReadLine();
                        serverMethod = Convert.ToInt32(Data);
                        comboBox1.SelectedIndex= serverMethod - 1;

                        continue;

                    }
                      

                    //load data form server
                    if (Data == $"ServiceDistribution_Server{current_server}")
                    {
                        int x = 0;

                        while (true)
                        {
                            Data = sr.ReadLine();

                            if (Data == "") { break; }
                            if (Data == null) { break; }
                            // MessageBox.Show(row);
                            tmp2 = Data.Split(',');

                            dataGridView2.Rows.Add();
                            dataGridView2.Rows[x].Cells[0].Value = Convert.ToString(tmp2[0]);
                            dataGridView2.Rows[x].Cells[1].Value = Convert.ToString(tmp2[1]);

                            x++;
                        }

                        continue;
                    }





                }//end of whi
            }

            // load data from file to input panel 



        }
        
        //********************************************
        //add different server 
        List<int> server_time;
        List<decimal> server_prob;
        int thecurrent_server = 0;
        private void button4_Click(object sender, EventArgs e)
        {
            //fill data of servers
            server_time=new List<int>();
            server_prob=new List<decimal>();

           
            numofservers = Int16.Parse(textBox1.Text);
            system.NumberOfServers = numofservers;

            if (thecurrent_server <= numofservers)
            {

                for (int x = 0; x < dataGridView2.Rows.Count; x++)
                {



                    // Handle parsing error for time value
                    if (dataGridView2.Rows[x].Cells[0].Value != null)
                    {

                        server_time.Add(Int16.Parse(dataGridView2.Rows[x].Cells[0].Value.ToString()));

                    }

                    // Handle parsing error for probability value
                    if (dataGridView2.Rows[x].Cells[1].Value != null)
                    {

                        server_prob.Add(Decimal.Parse(dataGridView2.Rows[x].Cells[1].Value.ToString()));


                    }

                }

                system.Fill_Service_Table(thecurrent_server, server_time, server_prob,numofservers);

                thecurrent_server++;

                dataGridView2.Rows.Clear();
                server_time.Clear();
                server_prob.Clear();
            }
            else {

                MessageBox.Show("You entered the correct number of server ");
            
            
            
            }
        }


            //************************************************************************
        List<int> time;
        List<Decimal> prob;
        Enums Enums =new Enums();
        //assign the data & calculate 
        private void button7_Click(object sender, EventArgs e)
        {

            StoppingNum=Int16.Parse(textBox2.Text);
           

            // object of simulation system
           system.StoppingNumber = StoppingNum;
            //selection method
            switch(comboBox1.SelectedIndex)
            {

                    case 0:
                    system.SelectionMethod = Enums.SelectionMethod.HighestPriority;
                    break;
                    case 1:
                    system.SelectionMethod = Enums.SelectionMethod.Random;
                    break;

                    case 2:
                    system.SelectionMethod = Enums.SelectionMethod.LeastUtilization;
                    break;


            }
            //Stopping StoppingCriteria
            switch (comboBox2.SelectedIndex)
            {

                case 0:
                    system.StoppingCriteria = Enums.StoppingCriteria.NumberOfCustomers;
                    break;
                case 1:
                    system.StoppingCriteria= Enums.StoppingCriteria.SimulationEndTime;
                    break;

            }
            //**********************************************************************************

            //fill data in disribuation time table 
            time = new List<int>();
            prob = new List<Decimal>();
            for (int x = 0; x < dataGridView1.Rows.Count; x++)
                {


                
                     // Handle parsing error for time value
                    if (dataGridView1.Rows[x].Cells[0].Value != null)
                    {
                      
                            time.Add( Int32.Parse(dataGridView1.Rows[x].Cells[0].Value.ToString()));
                 
                    }

                // Handle parsing error for probability value
                       if (dataGridView1.Rows[x].Cells[1].Value != null)
                       {

                        prob.Add( Decimal.Parse(dataGridView1.Rows[x].Cells[1].Value.ToString()));
                

                        }
                
                }

             system.Fill_Customer_Table(time, prob);

            system.fill_ID();
            // ************************************
            // fill simulation table
            system.Fill_Simulation_Table();

            //MessageBox.Show("done");

            //
           


            //display the output table
            for (int i = 0; i < system.StoppingNumber; i++)
            {
                 this.dataGridView3.Rows.Add();
                dataGridView3.Rows[i].Cells[0].Value=i+1;

                dataGridView3.Rows[i].Cells[1].Value = system.SimulationTable[i].RandomInterArrival;
                dataGridView3.Rows[i].Cells[2].Value = system.SimulationTable[i].InterArrival;
                dataGridView3.Rows[i].Cells[3].Value = system.SimulationTable[i].ArrivalTime;
                dataGridView3.Rows[i].Cells[4].Value = system.SimulationTable[i].RandomService;


                dataGridView3.Rows[i].Cells[5].Value = system.SimulationTable[i].ServiceTime;

                dataGridView3.Rows[i].Cells[6].Value = system.SimulationTable[i].StartTime;
                dataGridView3.Rows[i].Cells[7].Value = system.SimulationTable[i].EndTime;
                dataGridView3.Rows[i].Cells[8].Value = system.SimulationTable[i].AssignedServer.ID;
                dataGridView3.Rows[i].Cells[9].Value = system.SimulationTable[i].TimeInQueue;


           }

            //performace

            system.PerformanceMeasures_for_system();
            textBox3.Text = system.PerformanceMeasures.AverageWaitingTime.ToString();
            textBox4.Text = system.PerformanceMeasures.MaxQueueLength.ToString();
            textBox5.Text = system.PerformanceMeasures.WaitingProbability.ToString();


            system.PerformanceMeasures_for_each_server();
            for (int i = 0; i < system.NumberOfServers; i++)
            {
                this.dataGridView4.Rows.Add();

                dataGridView4.Rows[i].Cells[0].Value = i + 1;
                dataGridView4.Rows[i].Cells[1].Value = system.Servers[i].AverageServiceTime;
                dataGridView4.Rows[i].Cells[2].Value = system.Servers[i].Utilization;

                dataGridView4.Rows[i].Cells[3].Value = system.Servers[i].IdleProbability;



            }

            //test 
            string tmp = TestingManager.Test(system, Constants.FileNames.TestCase2);
            MessageBox.Show(tmp);


        }

        private void button8_Click(object sender, EventArgs e)
        {
            performance.BringToFront();
         


        }
        //load next server 
        private void button5_Click(object sender, EventArgs e)
        {
            var sr = new StreamReader(filename);
            current_server++;
            while (!sr.EndOfStream)
            {
                Data = sr.ReadLine();

                if (Data == "") continue;
                if (Data != $"ServiceDistribution_Server{current_server}") continue;
                //load data form server
                if (current_server <= numofservers)
                {
                    if (Data == $"ServiceDistribution_Server{current_server}")
                    {
                        int x = 0;

                        while (true)
                        {
                            Data = sr.ReadLine();

                            if (Data ==null) { break; }
                            if (Data == "") { break; }

                            // MessageBox.Show(row);
                            tmp2 = Data.Split(',');

                            dataGridView2.Rows.Add();
                            dataGridView2.Rows[x].Cells[0].Value = Convert.ToString(tmp2[0]);
                            dataGridView2.Rows[x].Cells[1].Value = Convert.ToString(tmp2[1]);

                            x++;
                        }

                        continue;
                    }
                }
                //MessageBox.Show(Data);
            }
         }

        private void button9_Click(object sender, EventArgs e)
        {
            server_graphID++;
            if (server_graphID <= system.NumberOfServers)
            {
                label14.Text = $"Server{server_graphID}";

                z = new List<int>();
                z = system.Service_time(server_graphID);
              chart1.Series["Busy Time"].Points.Clear();  
                chart1.Series["Busy Time"].SetCustomProperty("PointWidth", "1");
                for (int i = 0; i < system.TimeOfEndSimulation; i++)
                {

                    chart1.Series["Busy Time"].Points.AddXY(i + 1, z[i]);

                }
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
           

        }

        private void input_panel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            server_graphID--;
            if (server_graphID <= system.NumberOfServers)
            {
                label14.Text = $"Server{server_graphID}";

                z = new List<int>();
                z = system.Service_time(server_graphID);
                chart1.Series["Busy Time"].Points.Clear();
                chart1.Series["Busy Time"].SetCustomProperty("PointWidth", "1");
                for (int i = 0; i < system.TimeOfEndSimulation; i++)
                {

                    chart1.Series["Busy Time"].Points.AddXY(i + 1, z[i]);

                }
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void bar_panel_Paint(object sender, PaintEventArgs e)
        {

        }

        // for accept only numbers  
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && (!char.IsControl(e.KeyChar)))
            {
                e.Handled = true;
            }

        }
      
    }
}
