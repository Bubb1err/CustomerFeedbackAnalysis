using FeedbackAnalysis.Contracts.Authentication;
using MediatR;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FeedbackAnalysis.Application.Authentication.Register
{
    public sealed class RegisterCommand : IRequest<JwtTokenResponse>
    {
        public RegisterCommand(
             string email,
             string password,
             string confirmPassword)
        {
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
        }

        [Required]
        [EmailAddress]
        public string Email { get; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; }
    }
}
