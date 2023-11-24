using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MultiQueueModels
{
    public class SimulationSystem
    {
        public SimulationSystem()
        {
            this.Servers = new List<Server>();
            this.InterarrivalDistribution = new List<TimeDistribution>();
            this.PerformanceMeasures = new PerformanceMeasures();
            this.SimulationTable = new List<SimulationCase>();
        }
        Random r = new Random();
        ///////////// INPUTS ///////////// 
        //done 
        public int NumberOfServers { get; set; }
        public int TimeOfEndSimulation{ get; set; }
        public int StoppingNumber { get; set; }
        public List<Server> Servers { get; set; }
        public List<TimeDistribution> InterarrivalDistribution { get; set; }
        public Enums.StoppingCriteria StoppingCriteria { get; set; }
        public Enums.SelectionMethod SelectionMethod { get; set; }

        ///////////// OUTPUTS /////////////
        public List<SimulationCase> SimulationTable { get; set; }
        public PerformanceMeasures PerformanceMeasures { get; set; }



        //fill Time Distribution of Customers 
        //done
        public void Fill_Customer_Table(List<int> time, List<Decimal> prob)
        {
            InterarrivalDistribution = new List<TimeDistribution>();


            for (int i = 0; i < time.Count; i++)
            {
                
                if (i == 0)
                {
                    InterarrivalDistribution.Add(new TimeDistribution());
                    InterarrivalDistribution[0].Time = time[0];
                    InterarrivalDistribution[0].Probability = prob[0];
                    InterarrivalDistribution[0].CummProbability = InterarrivalDistribution[0].Probability;
                    InterarrivalDistribution[0].MinRange = 1;
                    InterarrivalDistribution[0].MaxRange = (int)(InterarrivalDistribution[0].CummProbability * 100) ;

                }

                else
                {
                    InterarrivalDistribution.Add(new TimeDistribution());
                    InterarrivalDistribution[i].Time = time[i];
                    InterarrivalDistribution[i].Probability = prob[i];
                    InterarrivalDistribution[i].CummProbability = InterarrivalDistribution[i].Probability + InterarrivalDistribution[i - 1].CummProbability;
                    InterarrivalDistribution[i].MinRange = ((int)(InterarrivalDistribution[i - 1].CummProbability * 100)) + 1;
                    InterarrivalDistribution[i].MaxRange = ((int)(InterarrivalDistribution[i].CummProbability * 100));
                }
            }
        }

        //fill Service Time 
        // in for loop 
        //done 
        public void Fill_Service_Table(int j, List<int> time, List<Decimal> prob, int numberofservers)
        {         
            if (j == 0)
            {
                Servers = new List<Server>();

                for (int i = 0; i < numberofservers; i++)
                {
                    Servers.Add(new Server());
                }
            }

            
            for (int i = 0; i < time.Count; i++)
            {

                Servers[j].TimeDistribution.Add(new TimeDistribution());
                if (i == 0)
                {
                    Servers[j].TimeDistribution[i].Time = time[0];
                    Servers[j].TimeDistribution[i].Probability = prob[0];
                    Servers[j].TimeDistribution[i].CummProbability = Servers[j].TimeDistribution[i].Probability;
                    Servers[j].TimeDistribution[i].MinRange = 1;
                    Servers[j].TimeDistribution[i].MaxRange = (int)(Servers[j].TimeDistribution[0].CummProbability * 100);

                }

                else
                {
                    Servers[j].TimeDistribution[i].Time = time[i];
                    Servers[j].TimeDistribution[i].Probability = prob[i];
                    Servers[j].TimeDistribution[i].CummProbability = Servers[j].TimeDistribution[i].Probability + Servers[j].TimeDistribution[i - 1].CummProbability;
                    Servers[j].TimeDistribution[i].MinRange = ((int)(Servers[j].TimeDistribution[i - 1].CummProbability * 100)) + 1;
                    Servers[j].TimeDistribution[i].MaxRange = ((int)(Servers[j].TimeDistribution[i].CummProbability * 100));
                }
            }
        }

        //get distribution Time for (Time between arrivals, service time)

        int get_distribution_Time(int rand_value, List<TimeDistribution> list_distribution)
        {
            for (int i = 0; i < list_distribution.Count; i++)
            {
                if (rand_value >= list_distribution[i].MinRange && rand_value <= list_distribution[i].MaxRange)
                {
                    return list_distribution[i].Time;
                }
            }
            return 0;
        }
        // InterArrival ==> Time between arrivals, ArrivalTime ==> Clock Time of Arrival
        // interivall time , clock time 

        void calculate_customer_time(int customer_num, ref SimulationCase next_customer, int rand_val)
        {
            next_customer.CustomerNumber = customer_num;


            if (customer_num == 1)
            {
                next_customer.RandomInterArrival = 1;
                next_customer.InterArrival = 0;
                next_customer.ArrivalTime = 0;
            }
            else
            {
                next_customer.RandomInterArrival = rand_val;

                for (int j = 0; j < InterarrivalDistribution.Count; ++j)
                {
                    if (next_customer.RandomInterArrival >= InterarrivalDistribution[j].MinRange
&& next_customer.RandomInterArrival <= InterarrivalDistribution[j].MaxRange)
                    {
                        next_customer.InterArrival = InterarrivalDistribution[j].Time;
                        break;
                    }
                }

                next_customer.ArrivalTime = next_customer.InterArrival + SimulationTable[customer_num - 2].ArrivalTime;
            }
        }
        //service time, end time
        void calculate_service_time(ref SimulationCase next_customer)
        {
            

            System.Threading.Thread.Sleep(10);
            next_customer.RandomService = r.Next(1, 100);

            System.Console.WriteLine(next_customer.RandomService);

            int index = (next_customer.AssignedServer.ID);
            int l;

            if (index == 0)
            {
                l = Servers[index].TimeDistribution.Count;
            }
            else
            {

                l = Servers[index - 1].TimeDistribution.Count;


            }
            for (int i = 0; i < l; i++)
            {
                
                
                if (next_customer.AssignedServer.ID == 1)
                {
                    if (next_customer.RandomService >= Servers[next_customer.AssignedServer.ID-1].TimeDistribution[i].MinRange &&
                       next_customer.RandomService <= Servers[next_customer.AssignedServer.ID-1].TimeDistribution[i].MaxRange)
                    {
                        next_customer.ServiceTime = Servers[next_customer.AssignedServer.ID-1].TimeDistribution[i].Time;
                        break;
                    }
                }
                else
                {
                    

                    if (next_customer.RandomService >= Servers[next_customer.AssignedServer.ID - 1].TimeDistribution[i].MinRange &&
                       next_customer.RandomService <= Servers[next_customer.AssignedServer.ID - 1].TimeDistribution[i].MaxRange)
                    {
                        next_customer.ServiceTime = Servers[next_customer.AssignedServer.ID - 1].TimeDistribution[i].Time;
                        break;
                    }


                }
            }
            next_customer.EndTime = next_customer.ServiceTime + next_customer.StartTime;
        }
        // before the output table 
        public void Fill_Simulation_Table()
        {
            SimulationCase sim_case;
            SimulationTable = new List<SimulationCase>();
            Random rand = new Random();
            if (StoppingCriteria == Enums.StoppingCriteria.NumberOfCustomers)
            {
                
                for (int i = 1; i <= StoppingNumber; ++i)
                {
                    int rand_num = rand.Next(1, 100);
                    sim_case = new SimulationCase();
                //    SimulationTable.Add(sim_case);
                    calculate_customer_time(i, ref sim_case, rand_num);
                    check_priority(ref sim_case);
                    SimulationTable.Add(sim_case);
                }
            }
            else if (StoppingCriteria == Enums.StoppingCriteria.SimulationEndTime)
            {
                bool finish = false;
                int time = 0;
                int customer_num = 1;
                while (time <= StoppingNumber)
                {
                    sim_case = new SimulationCase();
                    int ran_num = rand.Next(1, 100);
                    calculate_customer_time(customer_num, ref sim_case, ran_num);
                    if (customer_num != 1)
                    {
                        if (sim_case.ArrivalTime > StoppingNumber) { finish = true; }
                    }
                    check_priority(ref sim_case);
                    time = sim_case.ArrivalTime;

                    if ((sim_case.ArrivalTime + sim_case.TimeInQueue + sim_case.ServiceTime) > StoppingNumber)
                    { finish = true; }

                    if (finish) { break; }

                    SimulationTable.Add(sim_case);
                    customer_num++;
                }
            }
            TimeOfEndSimulation = SimulationTable[SimulationTable.Count - 1].EndTime;

        }

        // Methods of choice server
        // id =1 
        void Highest_priority_method(ref SimulationCase next_Customer, List<int> server)
        {
             next_Customer.AssignedServer.ID = server[0];
            calculate_service_time(ref next_Customer);
            Servers[next_Customer.AssignedServer.ID - 1].TotalWorkingTime += next_Customer.ServiceTime;
        }
        void Random_method(ref SimulationCase next_Customer, List<int> server)
        {
            Random r = new Random();
            next_Customer.AssignedServer.ID = server[r.Next(0, server.Count)];
            calculate_service_time(ref next_Customer);
            Servers[next_Customer.AssignedServer.ID - 1].TotalWorkingTime += next_Customer.ServiceTime;
        }

        void Least_Utilization_method(ref SimulationCase next_Customer, List<int> server)
        {
            int min_work = Servers[server[0] - 1].TotalWorkingTime;
            int min_ID = Servers[server[0] - 1].ID;
            for (int i = 1; i < server.Count; ++i)
            {
                if (Servers[server[i] - 1].TotalWorkingTime < min_work)
                {
                    min_work = Servers[server[i] - 1].TotalWorkingTime;
                    min_ID = Servers[server[i] - 1].ID;
                }
            }
            next_Customer.AssignedServer.ID = min_ID;
            calculate_service_time(ref next_Customer);
            Servers[next_Customer.AssignedServer.ID - 1].TotalWorkingTime += next_Customer.ServiceTime;
        }
        // chose the next server
        void priority_methods(ref SimulationCase next_Customer, List<int> server)
        {
            if (SelectionMethod == Enums.SelectionMethod.HighestPriority)
                Highest_priority_method(ref next_Customer, server);
            else if (SelectionMethod == Enums.SelectionMethod.Random)
                Random_method(ref next_Customer, server);
            else if (SelectionMethod == Enums.SelectionMethod.LeastUtilization)
                Least_Utilization_method(ref next_Customer, server);
        }

        void one_server_available(ref SimulationCase next_Customer, List<int> servers_available, int num_servers)
        {
            if (num_servers == 1)
            {
                next_Customer.TimeInQueue = 0;
                next_Customer.StartTime = next_Customer.ArrivalTime;
                next_Customer.AssignedServer.ID = servers_available[0];
            }
            else
            {
                next_Customer.AssignedServer.ID = servers_available[0];
            }
            calculate_service_time(ref next_Customer);
            if (next_Customer.AssignedServer.ID == 0)
            {
                Servers[next_Customer.AssignedServer.ID].TotalWorkingTime += next_Customer.ServiceTime;
            }
            else
            {

                Servers[next_Customer.AssignedServer.ID - 1].TotalWorkingTime += next_Customer.ServiceTime;


            }
        }

        void check_priority(ref SimulationCase next_Customer)
        {
            List<int> servers_available = new List<int>();
            for (int i = 0; i < Servers.Count; ++i)
            {
                bool found_Service = false;
                for (int j = SimulationTable.Count - 1; j >= 0; j--)
                {
                    if (SimulationTable[j].AssignedServer.ID == i + 1)
                    {
                        found_Service = true;
                        if (next_Customer.ArrivalTime >= SimulationTable[j].EndTime)
                            servers_available.Add(i + 1);
                        break;
                    }
                }

                if (!found_Service)
                    servers_available.Add(i + 1); // the server is free
            }
            // only one server available
            if (servers_available.Count == 1)
                one_server_available(ref next_Customer, servers_available, 1);
            // check which server will be available first
            else if (servers_available.Count == 0)
            {
                int min_diff = SimulationTable[SimulationTable.Count - 1].EndTime - next_Customer.ArrivalTime;
                int min_ID = SimulationTable[SimulationTable.Count - 1].AssignedServer.ID;
                List<int> server_will_available = new List<int>();
                for (int k = 0; k < Servers.Count; ++k)
                {
                    for (int i = SimulationTable.Count - 1; i >= 0; i--)
                    {
                        if (SimulationTable[i].AssignedServer.ID == k + 1)
                        {
                            if ((SimulationTable[i].EndTime - next_Customer.ArrivalTime) < min_diff)
                            {
                                min_diff = SimulationTable[i].EndTime - next_Customer.ArrivalTime;
                                min_ID = SimulationTable[i].AssignedServer.ID;
                            }
                            break;
                        }
                    }
                }
                server_will_available.Add(min_ID);
                for (int k = 0; k < Servers.Count; k++)
                {
                    for (int i = SimulationTable.Count - 1; i >= 0; i--)
                    {
                        if ((k + 1) == SimulationTable[i].AssignedServer.ID)
                        {
                            if ((SimulationTable[i].EndTime - next_Customer.ArrivalTime) == min_diff &&
                                SimulationTable[i].AssignedServer.ID != min_ID)
                            {
                                server_will_available.Add(SimulationTable[i].AssignedServer.ID);
                            }
                            break;
                        }
                    }
                }
                server_will_available.Sort();
                // get time in queue and start time of service 
                next_Customer.TimeInQueue = min_diff;
                next_Customer.StartTime = next_Customer.ArrivalTime + next_Customer.TimeInQueue;

                if (server_will_available.Count == 1)
                    one_server_available(ref next_Customer, server_will_available, 0);
                else if (server_will_available.Count > 1)
                    priority_methods(ref next_Customer, server_will_available);
            }
            else if (servers_available.Count > 1)
            {
                // no wait time in queue
                next_Customer.TimeInQueue = 0;
                // start time = arrival time
                next_Customer.StartTime = next_Customer.ArrivalTime;
                priority_methods(ref next_Customer, servers_available);
            }

        }

        //////////////////////////////////////////////////////////////////////////////////
        // Performance Measures
        public Decimal average_waiting_time()
        {
            Decimal time_in_queue = 0;
            for (int i = 0; i < SimulationTable.Count; i++)
            {
                time_in_queue += SimulationTable[i].TimeInQueue;
            }
            return time_in_queue / SimulationTable.Count;
        }

        public int Max_Queue_Length()
        {
            int max_length = 0;
            for (int i = 0; i < SimulationTable.Count; i++)
            {
                if (SimulationTable[i].TimeInQueue != 0)
                {
                    int length_of_queue = 1;
                    for (int j = i + 1; j < SimulationTable.Count; j++)
                    {
                        //arrival < start time ==> waited in queue
                        if (SimulationTable[j].ArrivalTime < SimulationTable[i].StartTime)
                            length_of_queue++;
                    }
                    if (length_of_queue > max_length)
                        max_length = length_of_queue;
                }
            }
            return max_length;
        }

        public  decimal Waiting_Probability()
        {
            decimal waited_customers = 0;
            for (int i = 0; i < SimulationTable.Count; i++)
            {
                if (SimulationTable[i].TimeInQueue != 0)
                    waited_customers++;
            }
            return (waited_customers / SimulationTable.Count);
        }
        //*******************************************************************
        // PerformanceMeasures for system
        public void PerformanceMeasures_for_system()
        {
            //averageWaitingTime = (total time in queue / num of customers)
            PerformanceMeasures.AverageWaitingTime = average_waiting_time();
            PerformanceMeasures.MaxQueueLength = Max_Queue_Length();
            ///WaitingProbability = (num of customers waited / num of customers)
            PerformanceMeasures.WaitingProbability = Waiting_Probability();
        }

        // PerformanceMeasures for each server
        public void PerformanceMeasures_for_each_server()
        {
            //get Simulation Time (max time of work)
            int total_simulation_time = 0;
            for (int i = 0; i < SimulationTable.Count; i++)
            {
                if (SimulationTable[i].EndTime > total_simulation_time)
                    total_simulation_time = SimulationTable[i].EndTime;
            }

            //get acual time of service (the actual time of work)
            for (int i = 0; i < NumberOfServers; i++)
            {
                int total_servic_time = 0;
                int server_customers = 0;
                for (int j = 0; j < SimulationTable.Count; j++)
                {
                    if (SimulationTable[j].AssignedServer.ID == i + 1)
                    {
                        total_servic_time += SimulationTable[j].ServiceTime;
                        server_customers++;
                    }
                }

                int idle_time = total_simulation_time - total_servic_time;

                if (server_customers < 1)
                    server_customers = 1;
                if (idle_time < 0)
                    idle_time = 0;

                // average service time = total service time / num of customers in each server
                Servers[i].AverageServiceTime = (Convert.ToDecimal(total_servic_time) / server_customers);
                // idle probability = idle time / total simulation time
                Servers[i].IdleProbability = (Convert.ToDecimal(idle_time) / total_simulation_time);
                // utilization = total service time / total simulation time
                Servers[i].Utilization = (Convert.ToDecimal(total_servic_time) / total_simulation_time);
            }
        }
        // idle time 
        public List<int> Service_time(int service_num)
        {
            List<int> Busy = new List<int>();

            //int idle_time = 0;
            for (int i = 0; i < TimeOfEndSimulation; i++) {

                Busy.Add(0);
            }
                for (int i = 0; i < SimulationTable.Count; i++)
            {
                if (SimulationTable[i].AssignedServer.ID == service_num)
                {
                    
                    for (int x = SimulationTable[i].StartTime; x < SimulationTable[i].EndTime; x++)
                    {
                          Busy[x] =1;
                       
                    }
                }
            }
            return Busy;
        }
        public void fill_ID()
        {

            for (int i = 1; i <= NumberOfServers; i++) {

                Servers[i-1].ID = i;
            }

        }

        //
    }
}

