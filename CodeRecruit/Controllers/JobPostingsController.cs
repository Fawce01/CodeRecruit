using CodeRecruit.Constants;
using CodeRecruit.Models;
using CodeRecruit.Repositories;
using CodeRecruit.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodeRecruit.Controllers
{
    [Authorize] // can only used if the user is logged in
    public class JobPostingsController : Controller
    {

        private readonly IRepository<JobPosting> _repository;
        private readonly UserManager<IdentityUser> _userManager;

        public JobPostingsController(IRepository<JobPosting> repository, UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var jobPostings = await _repository.GetAllAsync();

            if (User.IsInRole(Roles.Employer))
            {
                // Get the current user ID
                var userId = _userManager.GetUserId(User);
                // Get all postings where the posting useer id is equal to the user id
                var filteredJobPostings = jobPostings.Where(jp => jp.UserId == userId);
                return View(filteredJobPostings);
            }

            return View(jobPostings);
        }

        [Authorize(Roles = "Admin, Employer")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Employer")]
        public async Task<IActionResult> Create(JobPostingViewModel jobPostingVm)
        {

            if (ModelState.IsValid)
            {
                var jobPosting = new JobPosting
                {
                    Title = jobPostingVm.Title,
                    Description = jobPostingVm.Description,
                    Company = jobPostingVm.Company,
                    Location = jobPostingVm.Location,
                    UserId = _userManager.GetUserId(User),
                };

                await _repository.AddAsync(jobPosting);
                // redirect user to the index page
                return RedirectToAction(nameof(Index));
            }

            return View(jobPostingVm);

        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Employer")]
        public async Task<IActionResult> Delete(int id)
        {
            var jobPosting = await _repository.GetByIdAllAsync(id);
            if (jobPosting == null)
            {
                // couldn't find job posting
                return NotFound();
            }

            // Get current user ID
            var userId = _userManager.GetUserId(User);

            if (!User.IsInRole(Roles.Admin) && jobPosting.UserId != userId)
            {
                // user is not an admin and the jobposting to be removed was not created by the current user
                // if the user is a jobseeker - they do not have access to the delete button on the front end
                return Forbid();
            }

            await _repository.DeleteAsync(id);

            return Ok();
        }
    }
}
