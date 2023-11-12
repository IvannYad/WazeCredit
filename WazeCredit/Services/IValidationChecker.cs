using WazeCredit.Models;

namespace WazeCredit.Services
{
    public interface IValidationChecker
    {
        bool ValidateLogic(CreditApplication model);

        public string ErrorMessage { get; }
    }
}
