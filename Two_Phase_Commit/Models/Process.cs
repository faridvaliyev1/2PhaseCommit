using System;

namespace Two_Phase_Commit.Models
{
    public class Process
    {
      public string Name { get; set; }
      public bool Is_Processor { get; set;}
      public bool Is_Time_Failure { get; set; }
      public bool Is_Arbitrary_Failure { get; set; }
      public DateTime? Failure_Start { get; set; }
      public int Failure_Interval { get; set; } 
    }
}
