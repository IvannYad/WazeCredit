using WazeCredit.Models;

namespace WazeCredit.Services
{
    public class CreditApprovedHigh : ICreditApproved
    {
        public double GetCreditApproved(CreditApplication creditApplication)
        {
            // Here we have a logic to calculate approval limit.
            // For now we will hardcode to 30% of salary.
            return creditApplication.Salary * 0.3;
        }
    }
}
