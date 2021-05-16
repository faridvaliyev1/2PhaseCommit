using System;
using System.Threading;
using Two_Phase_Commit.Services;

namespace Two_Phase_Commit
{
    public class Program
    {
        static void Main(string[] args)
        {
            IProcessService service = new ProcessService();
            Phase p = new Phase(service);

            p.Load("2PC.txt");

            p.Time_Failure("P2", 5);

            Thread.Sleep(6000);

            p.SetValue("sa");

            Console.ReadKey();
        }
    }
}
