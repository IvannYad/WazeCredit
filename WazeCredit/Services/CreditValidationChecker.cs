using WazeCredit.Models;

namespace WazeCredit.Services
{
    public class CreditValidationChecker : IValidationChecker
    {
        public string ErrorMessage => "You did not meet Age/Salary/Credit requirements.";

        public bool ValidateLogic(CreditApplication model)
        {
            if (DateTime.Now.AddYears(-18) < model.DOB)
            {
                return false;
            }

            if (model.Salary < 10_000)
            {
                return false;
            }

            return true;
        }
    }
}
