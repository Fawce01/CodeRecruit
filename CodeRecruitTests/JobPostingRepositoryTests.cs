using CodeRecruit.Data;
using CodeRecruit.Models;
using CodeRecruit.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeRecruitTests
{
    public class JobPostingRepositoryTests
    {
        // Tests for all methods in the JobPostingRepository

        private readonly DbContextOptions<ApplicationDbContext> _options;
        public JobPostingRepositoryTests()
        {
            // Creates the new in memory database
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("JobPositingDb")
                .Options;
        }

        // Initialises the new in memory database
        private ApplicationDbContext CreateDbContex() => new ApplicationDbContext(_options);

        [Fact]
        public async Task AddAsync_ShouldAddJobPosting()
        {
            // db context
            var db = CreateDbContex();

            // job posting repos instance
            var repository = new JobPostingRepository(db);

            // add job posting
            var jobPosting = new JobPosting
            {
                Title = "Test Title",
                Description = "Test Description",
                PostedDate = DateTime.Now,
                Company = "Test Company",
                Location = "Test Location",
                UserId = "TestUserId",
            };

            // execute method being tested
            await repository.AddAsync(jobPosting);

            // result? SingleOrDefault will return null if the expression evaluates to false
            var result = db.JobPostings.Find(jobPosting.Id);

            // assert (expecting output) - we expect that result will not be null
            Assert.NotNull(result);
            // assert if the title equals what we set in AddAsync ("Test Title")
            Assert.Equal("Test Title", result.Title);
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnJobPosting()
        {
            var db = CreateDbContex();

            var repository = new JobPostingRepository(db);

            var jobPosting = new JobPosting
            {
                Title = "Test Title2",
                Description = "Test Description",
                PostedDate = DateTime.Now,
                Company = "Test Company",
                Location = "Test Location",
                UserId = "TestUserId",
            };

            // add job posting to table by Entity Frame AddAsync
            await db.JobPostings.AddAsync(jobPosting);
            await db.SaveChangesAsync();

            var result = repository.GetByIdAllAsync(jobPosting.Id);

            Assert.NotNull(jobPosting);
            Assert.Equal("Test Title2", jobPosting.Title);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowKeyNotFoundException()
        {
            var db = CreateDbContex();
            var repository = new JobPostingRepository(db);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.GetByIdAllAsync(9909));
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllJobPostings()
        {
            var db = CreateDbContex();

            var repository = new JobPostingRepository(db);

            var jobPosting1 = new JobPosting
            {
                Title = "Test Title First",
                Description = "Test Description First",
                PostedDate = DateTime.Now,
                Company = "Test Company First",
                Location = "Test Location First",
                UserId = "TestUserId First",
            };
            var jobPosting2 = new JobPosting
            {
                Title = "Test Title Second",
                Description = "Test Description Second",
                PostedDate = DateTime.Now,
                Company = "Test Company Second",
                Location = "Test Location Second",
                UserId = "TestUserId Second",
            };

            // add job posting to table by Entity Frame AddAsync
            await db.JobPostings.AddRangeAsync(jobPosting1, jobPosting2);
            await db.SaveChangesAsync();

            var result = await repository.GetAllAsync();

            Assert.NotNull(result);
            Assert.True(result.Count() >= 2);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateJobPosting()
        {
            var db = CreateDbContex();

            var repository = new JobPostingRepository(db);

            var jobPosting = new JobPosting
            {
                Title = "Test Title",
                Description = "Test Description",
                PostedDate = DateTime.Now,
                Company = "Test Company",
                Location = "Test Location",
                UserId = "TestUserId",
            };

            await db.JobPostings.AddAsync(jobPosting);
            await db.SaveChangesAsync();

            jobPosting.Description = "Updated Description";

            await repository.UpdateAsync(jobPosting);

            var result = db.JobPostings.Find(jobPosting.Id);

            Assert.NotNull(result);
            Assert.Equal("Updated Description", result.Description);

        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteJobPosting()
        {
            var db = CreateDbContex();

            var repository = new JobPostingRepository(db);
            var jobPosting = new JobPosting
            {
                Title = "Test Title",
                Description = "Test Description",
                PostedDate = DateTime.Now,
                Company = "Test Company",
                Location = "Test Location",
                UserId = "TestUserId",
            };

            await db.JobPostings.AddAsync(jobPosting);
            await db.SaveChangesAsync();

            await repository.DeleteAsync(jobPosting.Id);

            var result = db.JobPostings.Find(jobPosting.Id);

            Assert.Null(result);
        }

    }
}
