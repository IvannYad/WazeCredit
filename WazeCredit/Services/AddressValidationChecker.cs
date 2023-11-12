using WazeCredit.Models;

namespace WazeCredit.Services
{
    public class AddressValidationChecker : IValidationChecker
    {
        public string ErrorMessage => "Location validation failed";

        public bool ValidateLogic(CreditApplication model)
        {
            if (model.PostalCode <= 0 || model.PostalCode > 99_999)
            {
                return false;
            }

            return true;
        }
    }
}
