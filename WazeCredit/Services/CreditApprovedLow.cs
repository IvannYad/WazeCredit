using WazeCredit.Models;

namespace WazeCredit.Services
{
    public class CreditApprovedLow : ICreditApproved
    {
        public double GetCreditApproved(CreditApplication creditApplication)
        {
            // Here we have a logic to calculate approval limit.
            // For now we will hardcode to 50% of salary.
            return creditApplication.Salary * .5;
        }
    }
}
