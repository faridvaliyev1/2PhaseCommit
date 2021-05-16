using System;
using Two_Phase_Commit.Models;

namespace Two_Phase_Commit.Services
{
    class ProcessService : IProcessService
    {
        public Process Arbitrary_Failure(Process process, int interval)
        {
            process.Failure_Interval = interval;
            process.Is_Arbitrary_Failure = true;
            process.Is_Time_Failure = false;
            process.Failure_Start = DateTime.Now;

            return process;
        }
        public Process Time_Failure(Process process, int interval)
        {
            process.Is_Time_Failure = true;
            process.Is_Arbitrary_Failure = false;
            process.Failure_Start = DateTime.Now;
            process.Failure_Interval = interval;

            return process;
        }
    }
}
