using Two_Phase_Commit.Models;

namespace Two_Phase_Commit.Services
{
    public interface IProcessService
    {
        Process Time_Failure(Process process,int interval);
        Process Arbitrary_Failure(Process process,int interval);
    }
}
