using System;
using System.Collections.Generic;
using static NewspaperSellerModels.Enums;

namespace NewspaperSellerModels
{
    public class SimulationSystem
    {
        public SimulationSystem()
        {
            DayTypeDistributions = new List<DayTypeDistribution>();
            DemandDistributions = new List<DemandDistribution>();
            SimulationTable = new List<SimulationCase>();
            PerformanceMeasures = new PerformanceMeasures();
        }
        ///////////// INPUTS /////////////
        public int NumOfNewspapers { get; set; }
        public int NumOfRecords { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal ScrapPrice { get; set; }
        public decimal UnitProfit { get; set; }
        public List<DayTypeDistribution> DayTypeDistributions { get; set; }
        public List<DemandDistribution> DemandDistributions { get; set; }

        ///////////// OUTPUTS /////////////
        public List<SimulationCase> SimulationTable { get; set; }
        public PerformanceMeasures PerformanceMeasures { get; set; }

        //calculate
        public void Calculate1_CummProbability_RandomDigitAssigmint(List<DayType> dayType, List<Decimal> prob)
        {
            DayTypeDistributions = new List<DayTypeDistribution>();
            for (int i = 0; i < 3; i++)
            {

                if (i == 0)
                {
                    DayTypeDistributions.Add(new DayTypeDistribution());
                    DayTypeDistributions[i].DayType = dayType[i];
                    DayTypeDistributions[i].Probability = prob[i];
                    DayTypeDistributions[i].CummProbability = prob[i];
                    DayTypeDistributions[i].MinRange = 1;
                    DayTypeDistributions[i].MaxRange = ((int)(DayTypeDistributions[i].CummProbability * 100));
                }
                else
                {
                    DayTypeDistributions.Add(new DayTypeDistribution());
                    DayTypeDistributions[i].DayType = dayType[i];
                    DayTypeDistributions[i].Probability = prob[i];
                    DayTypeDistributions[i].CummProbability = prob[i] + DayTypeDistributions[i - 1].CummProbability;
                    DayTypeDistributions[i].MinRange = ((int)(DayTypeDistributions[i - 1].CummProbability * 100)) + 1;
                    DayTypeDistributions[i].MaxRange = ((int)(DayTypeDistributions[i].CummProbability * 100));
                }
            }
        }

        //calculate demand cumalitive and ranges      done
        public void Calculate2_CummProbability_RandomDigitAssigmint(List<DayType> dayType, List<Decimal> prob, List<int> Demand)
        {

            DemandDistributions = new List<DemandDistribution>();

            int combin_count = 0;
            for (int i = 0; i < Demand.Count; i++)
            {

                DemandDistribution demand_new = new DemandDistribution();
                demand_new.DayTypeDistributions = new List<DayTypeDistribution>();
                demand_new.Demand = Demand[i];
                for (int j = 0; j < 3; j++)
                {
                    demand_new.DayTypeDistributions.Add(new DayTypeDistribution());
                    if (i == 0)
                    {
                        demand_new.DayTypeDistributions[j].DayType = dayType[combin_count];
                        demand_new.DayTypeDistributions[j].Probability = prob[combin_count];
                        demand_new.DayTypeDistributions[j].CummProbability = prob[combin_count];
                        demand_new.DayTypeDistributions[j].MinRange = 1;
                        demand_new.DayTypeDistributions[j].MaxRange = (int)(demand_new.DayTypeDistributions[j].CummProbability * 100);                       
                    }
                    else
                    {
                        demand_new.DayTypeDistributions[j].DayType = dayType[combin_count];
                        demand_new.DayTypeDistributions[j].Probability = prob[combin_count];
                        demand_new.DayTypeDistributions[j].CummProbability = prob[combin_count] + DemandDistributions[i - 1].DayTypeDistributions[j].CummProbability;
                        int last_max = (int)(DemandDistributions[i - 1].DayTypeDistributions[j].CummProbability * 100);

                        //demand_new.DayTypeDistributions[j].CummProbability = prob[combin_count] + demand_new.DayTypeDistributions[combin_count - 1].CummProbability;
                        //demand_new.DayTypeDistributions[j].MinRange = ((int)demand_new.DayTypeDistributions[combin_count - 1].CummProbability * 100) + 1;
                        //demand_new.DayTypeDistributions[j].MaxRange = ((int)demand_new.DayTypeDistributions[combin_count].CummProbability * 100);
                        if (last_max >= 100)
                        {
                            demand_new.DayTypeDistributions[j].MinRange = 0;
                            demand_new.DayTypeDistributions[j].MaxRange = 0;
                        }
                        else
                        {
                            demand_new.DayTypeDistributions[j].MinRange = (int)(DemandDistributions[i - 1].DayTypeDistributions[j].CummProbability * 100) + 1;
                            demand_new.DayTypeDistributions[j].MaxRange = (int)(demand_new.DayTypeDistributions[j].CummProbability * 100);
                        }                       
                    }                   
                    combin_count++;
                }
                DemandDistributions.Add(demand_new);
            }
        }


        //get value of demand from distribution table  still not
        int get_distribution_demand(Enums.DayType dayType, int rand_value, List<DemandDistribution> list_distribution)
        {
            int j;
            if (dayType == DayType.Good)
            {
                j = 0;
            }
            else if (dayType == DayType.Fair)
            {
                j = 1;
            }
            else if (dayType == DayType.Poor)
            {
                j = 2;
            }
            else
            {
                j = 0;
            }
            for (int i = 0; i < list_distribution.Count; i++)
            {
                if (rand_value >= list_distribution[i].DayTypeDistributions[j].MinRange && rand_value <= list_distribution[i].DayTypeDistributions[j].MaxRange)
                {
                    return list_distribution[i].Demand;
                }
            }

            return 0;
        }
        //get daytype from distribution table done
        Enums.DayType get_distribution_daytype(int rand_value, List<DayTypeDistribution> list_distribution)
        {
            for (int i = 0; i < list_distribution.Count; i++)
            {
                if (rand_value >= list_distribution[i].MinRange && rand_value <= list_distribution[i].MaxRange)
                {
                    return list_distribution[i].DayType;
                }
            }
            return 0;
        }

        //daily profot = revenue - (newspaper*)
        void calculate_profit_parameters(ref SimulationCase current)
        {
            decimal cost_of_newpapers = NumOfNewspapers * PurchasePrice;
            if (current.Demand > NumOfNewspapers)
            {
                //if d = 90  n = 70  done
                current.LostProfit = (current.Demand - NumOfNewspapers) * (SellingPrice - PurchasePrice);
                current.ScrapProfit = 0;
                current.SalesProfit = NumOfNewspapers * SellingPrice;
                current.DailyCost = cost_of_newpapers;
                current.DailyNetProfit = current.SalesProfit - cost_of_newpapers - current.LostProfit;
            }
            else if (current.Demand < NumOfNewspapers)
            {
                current.LostProfit = 0;
                current.ScrapProfit = (NumOfNewspapers - current.Demand) * ScrapPrice;
                current.SalesProfit = current.Demand * SellingPrice;
                current.DailyCost = cost_of_newpapers;
                current.DailyNetProfit = current.SalesProfit - cost_of_newpapers + current.ScrapProfit;
            }
            else if (current.Demand == NumOfNewspapers)
            {
                //done
                current.LostProfit = 0;
                current.ScrapProfit = 0;
                current.SalesProfit = current.Demand * SellingPrice;
                current.DailyNetProfit = current.SalesProfit - cost_of_newpapers;
                current.DailyCost = cost_of_newpapers;
            }

        }


        //fill simulation table && performance calculations
        public void system_output()
        {
            SimulationCase sim_case;
            SimulationTable = new List<SimulationCase>();
            Random rand = new Random();
            //Random rand2 = new Random();

            int num_of_scraps = 0;
            int num_excess = 0;
            decimal lost_profit_excess = 0;
            decimal total_sales_revenue = 0;
            decimal total_scrap = 0;
            decimal total_cost = 0;
            decimal total_net_profit = 0;
            for (int i = 1; i <= NumOfRecords; ++i)
            {
                int rand_demand = rand.Next(1, 100);
                int rand_day = rand.Next(1, 100);
                sim_case = new SimulationCase();
                sim_case.DayNo = i;
                sim_case.RandomDemand = rand_demand;
                sim_case.RandomNewsDayType = rand_day;

                Enums.DayType day = get_distribution_daytype(rand_day, DayTypeDistributions);
                if (day == (Enums.DayType)0)
                {
                    sim_case.NewsDayType = DayType.Good;
                }
                else if (day == (Enums.DayType)1)
                {
                    sim_case.NewsDayType = DayType.Fair;
                }
                else
                {
                    sim_case.NewsDayType = DayType.Poor;
                }
                sim_case.Demand = get_distribution_demand(sim_case.NewsDayType, sim_case.RandomDemand, DemandDistributions);
                calculate_profit_parameters(ref sim_case);
                SimulationTable.Add(sim_case);
                if (sim_case.ScrapProfit != 0)
                {
                    num_of_scraps++;
                }
                if (sim_case.LostProfit != 0)
                {
                    num_excess++;
                }
                //Total Lost Profit from Excess Demand
                lost_profit_excess += sim_case.LostProfit;
                //Total Sales Revenue
                total_sales_revenue += sim_case.SalesProfit;
                //Total Salvage from sale of Scrap papers
                total_scrap += sim_case.ScrapProfit;
                //Total Cost of Newspapers
                //Net Profit


            }

            PerformanceMeasures.DaysWithUnsoldPapers = num_of_scraps;
            PerformanceMeasures.DaysWithMoreDemand = num_excess;
            PerformanceMeasures.TotalLostProfit = lost_profit_excess;
            PerformanceMeasures.TotalSalesProfit = total_sales_revenue;
            PerformanceMeasures.TotalScrapProfit = total_scrap;
            PerformanceMeasures.TotalCost = NumOfNewspapers * PurchasePrice * NumOfRecords;
            PerformanceMeasures.TotalNetProfit = total_sales_revenue - PerformanceMeasures.TotalCost - lost_profit_excess + total_scrap;
        }

        //generate random nums
    }
}