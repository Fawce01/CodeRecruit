using CodeRecruit.Data;
using CodeRecruit.Models;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;

namespace CodeRecruit.Repositories
{
    public class JobPostingRepository : IRepository<JobPosting>
    {
        private readonly ApplicationDbContext _context;
        public JobPostingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(JobPosting entity)
        {
            // Adds given job posting to the table
            await _context.JobPostings.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            // Get posting to be removed by id
            var jobPosting = await _context.JobPostings.FindAsync(id);
            if (jobPosting == null)
            {
                throw new KeyNotFoundException();
            }
                // If the job is fetched, remove posting from the table
                _context.JobPostings.Remove(jobPosting);
                await _context.SaveChangesAsync(); 
        }

        public async Task<IEnumerable<JobPosting>> GetAllAsync()
        {
            // Get posting by id
            return await _context.JobPostings.ToListAsync();
        }

        public async Task<JobPosting> GetByIdAllAsync(int id)
        {
            // Get posting to be removed by id
            var jobPosting = await _context.JobPostings.FindAsync(id);
            if (jobPosting == null)
            {
                // If the job is not fetched successfully
                throw new KeyNotFoundException();
            }
            return jobPosting;
        }

        public async Task UpdateAsync(JobPosting entity)
        {
            // update given job posting
            _context.JobPostings.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
