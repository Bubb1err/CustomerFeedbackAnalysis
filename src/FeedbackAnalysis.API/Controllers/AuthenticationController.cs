using FeedbackAnalysis.Application.Authentication.Login;
using FeedbackAnalysis.Application.Authentication.RefreshTokenHandle;
using FeedbackAnalysis.Application.Authentication.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackAnalysis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand registerCommand)
        {
            return Ok(await _mediator.Send(registerCommand));
        }

        [HttpPost("signin")]
        public async Task<IActionResult> Login([FromBody] LoginCommand loginCommand)
        {
            return Ok(await _mediator.Send(loginCommand));
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var refreshtokenCommand = new RefreshTokenCommand(refreshToken);
            return Ok(await _mediator.Send(refreshtokenCommand));
        }
    }
}
