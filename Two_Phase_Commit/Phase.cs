using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Two_Phase_Commit.Models;
using Two_Phase_Commit.Services;

namespace Two_Phase_Commit
{
    public class Phase
    {
        private readonly IProcessService _processService;

        public Phase(IProcessService processService)
        {
            _processService = processService;
        }

        public List<Process> Processes = new List<Process>();

        public Stack<string> States = new Stack<string>();

        public void Load(string path)
        {
            string[] lines = File.ReadAllLines(path);

            for (int i = 1; i < lines.Length; i++)
            {
                if (lines[i] != "#State")
                {
                    Process p = new Process()
                    {
                        Is_Arbitrary_Failure = false,
                        Is_Processor = lines[i].Contains("Coordinator") ? true : false,
                        Is_Time_Failure = false,
                        Name = lines[i].Contains("Coordinator") ? lines[i].Split(";")[0] : lines[i],
                        Failure_Interval = 0,
                        Failure_Start = null
                    };

                    Processes.Add(p);
                }
                else
                {
                    break;
                }
            }

            string[] states = lines[lines.Length - 1].Trim().Split(";")[1].Trim().Split(",");

            foreach (var state in states)
            {
                States.Push(state);
            }

        }

        public void RollBack(int N)
        {
            try
            {
                if (N > States.Count)
                {
                    Console.WriteLine("This number is greater than States length");
                }
                else
                {
                    for(int i = 0; i < N; i++)
                    {
                        States.Pop();
                    }
                }
            }
            catch
            {
                Console.WriteLine("Please provide correct value for the rollback");
            }
        }

        public void Add(string Name)
        {
            Process p = new Process()
            {
                Is_Arbitrary_Failure = false,
                Is_Processor = false,
                Is_Time_Failure = false,
                Name =Name,
                Failure_Interval = 0,
                Failure_Start = null
            };

            Processes.Add(p);
        }

        public void Remove(string Name)
        {
            var process = Processes.Where(p => p.Name == Name).FirstOrDefault();

            if (process == null)
            {
                Console.WriteLine("There is no process like that in the system");
            }
            else
                Processes.Remove(process);
        }

        public void Time_Failure(string Name,int interval)
        {
            try
            {
                var process = Processes.Where(p => p.Name == Name).FirstOrDefault();

                if (process == null)
                    Console.WriteLine("There is no such processor in the system");
                else
                {
                    process = _processService.Time_Failure(process, interval);

                    
                }
            }
            catch
            {
                Console.WriteLine("Please correct your input");
            }
        }

        public void Arbitrary_Failure(string Name,int interval)
        {
            try
            {
                var process = Processes.Where(p => p.Name == Name).FirstOrDefault();

                if (process == null)
                    Console.WriteLine("There is no such processor in the system");
                else
                {
                    process = _processService.Arbitrary_Failure(process, interval);
                }
            }
            catch
            {
                Console.WriteLine("Please correct your input");
            }
        }

        public bool SetValue(string state)
        {
            var time_failures = Processes.Where(p => p.Is_Time_Failure == true);

            foreach(var failure in time_failures)
            {
                var total_seconds = DateTime.Now.Subtract(Convert.ToDateTime(failure.Failure_Start)).TotalSeconds;

                if (total_seconds <= failure.Failure_Interval)
                    return false;
                else
                {
                    failure.Failure_Interval = 0;
                    failure.Failure_Start = null;
                    failure.Is_Time_Failure = false;
                }
            }

            var arbitrary_failures = Processes.Where(p => p.Is_Arbitrary_Failure == true);

            foreach (var failure in arbitrary_failures)
            {
                var total_seconds = DateTime.Now.Subtract(Convert.ToDateTime(failure.Failure_Start)).TotalSeconds;

                if (total_seconds <= failure.Failure_Interval)
                    return false;
                else
                {
                    failure.Failure_Interval = 0;
                    failure.Failure_Start = null;
                    failure.Is_Arbitrary_Failure = false;
                }
            }

            States.Push(state);

            return true;

        }

    }
}
