using WazeCredit.Models;

namespace WazeCredit.Services
{
    public interface ICreditApproved
    {
        double GetCreditApproved(CreditApplication creditApplication);
    }
}
