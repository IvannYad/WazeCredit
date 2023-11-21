using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WazeCredit.Data.Repository.IRepository;
using WazeCredit.Models;

namespace WazeCredit.Data.Repository
{
    public class CreditApplicationRepository : Repository<CreditApplication>, ICreditApplicationRepository
    {
        private readonly ApplicationDbContext _context;
        public CreditApplicationRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }
        public void Update(CreditApplication creditApplication)
        {
            _context.CreditApplications.Update(creditApplication);
        }
    }
}
