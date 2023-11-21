using WazeCredit.Data.Repository.IRepository;

namespace WazeCredit.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            CreditApplication = new CreditApplicationRepository(_context);
        }

        public ICreditApplicationRepository CreditApplication { get; private set; }

        public void Dispose()
        {
            // Do some cutom disposing.
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
