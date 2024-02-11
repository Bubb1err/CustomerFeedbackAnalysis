using FeedbackAnalysis.Contracts.Authentication;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace FeedbackAnalysis.Application.Authentication.Login
{
    public sealed class LoginCommand : IRequest<JwtTokenResponse>
    {
        public LoginCommand(
            string email,
            string password)
        {
            Email = email;
            Password = password;
        }

        [Required]
        [EmailAddress]
        public string Email { get; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; }
    }
}
