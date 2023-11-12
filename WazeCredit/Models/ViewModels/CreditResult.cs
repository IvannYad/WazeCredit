namespace WazeCredit.Models.ViewModels
{
    public class CreditResult
    {
        public bool Success { get; set; }
        public IEnumerable<string>? ErrorList { get; set; }
        public int CreditID { get; set; }
        public double CreditApproved { get; set; }
    }
}
