namespace WazeCredit.Services.LifetimeExample
{
    public class TransientService
    {
        private readonly Guid _guid;
        public TransientService()
        {
            _guid = Guid.NewGuid();
        }

        public string GetGuid() => _guid.ToString();
    }
}
