using FeedbackAnalysis.Application.FeedbackAnalyze.CreateFeedback;
using FeedbackAnalysis.Application.FeedbackAnalyze.GetFeedback;
using FeedbackAnalysis.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackAnalysis.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;

        public FeedbackController(IMediator mediator,
            UserManager<User> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> PostFeedbackInput([FromBody]string feedback)
        {
            var email = (await _userManager.GetUserAsync(HttpContext.User)).Email;

            var createFeedbackCommand = new CreateFeedbackCommand(feedback, email);
            await _mediator.Send(createFeedbackCommand);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetFeedback()
        {
            var getFeedbackCommand = new GetFeedbackCommand();
            return Ok(await _mediator.Send(getFeedbackCommand));
        }
    }
}
