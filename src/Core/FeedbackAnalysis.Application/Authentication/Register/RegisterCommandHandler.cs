using FeedbackAnalysis.Application.Core.Abstractions.Authentication;
using FeedbackAnalysis.Contracts.Authentication;
using FeedbackAnalysis.Domain.Entities;
using FeedbackAnalysis.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FeedbackAnalysis.Application.Authentication.Register
{
    internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, JwtTokenResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtProvider _jwtProvider;

        public RegisterCommandHandler(
            UserManager<User> userManager,
            IJwtProvider jwtProvider)
        {
            _userManager = userManager;
            _jwtProvider = jwtProvider;
        }
        public async Task<JwtTokenResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                throw new ExecutingException($"User with email {request.Email} is already exists.", System.Net.HttpStatusCode.Conflict);
            }

            var newUser = new User { Email = request.Email, UserName = request.Email };

            var result = await _userManager.CreateAsync(newUser, request.Password);
            if (!result.Succeeded)
            {
                throw new ExecutingException($"{result?.Errors?.FirstOrDefault()?.Description}", System.Net.HttpStatusCode.BadRequest);
            }

            var token = await _jwtProvider.GenerateJWTTokenAsync(newUser, null);
            return token;
        }
    }
}
