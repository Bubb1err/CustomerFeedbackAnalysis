using FeedbackAnalysis.Application.Core.Abstractions.Authentication;
using FeedbackAnalysis.Contracts.Authentication;
using FeedbackAnalysis.Domain.Entities;
using FeedbackAnalysis.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FeedbackAnalysis.Application.Authentication.Login
{
    internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, JwtTokenResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtProvider _jwtProvider;

        public LoginCommandHandler(
            UserManager<User> userManager,
            IJwtProvider jwtProvider)
        {
            _userManager = userManager;
            _jwtProvider = jwtProvider;
        }

        public async Task<JwtTokenResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var userExist = await _userManager.FindByEmailAsync(request.Email);

            if (userExist == null)
            {
                throw new ExecutingException($"User does not exists.", System.Net.HttpStatusCode.Unauthorized);
            }
            if (!(await _userManager.CheckPasswordAsync(userExist, request.Password)))
            {
                throw new ExecutingException($"Incorrect password.", System.Net.HttpStatusCode.BadRequest);
            }

            var token = await _jwtProvider.GenerateJWTTokenAsync(userExist, null);
            return token;
        }
    }
}
