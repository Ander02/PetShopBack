using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Api.Data;
using Api.Infrastructure.Notifications;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Features.Auth
{
    public class Auth
    {
        public class Command : IRequest<Result>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(d => d.Email).NotEmpty().EmailAddress();
                RuleFor(d => d.Password).NotEmpty().MinimumLength(6);
            }
        }

        public class Result
        {
            public string Token { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result>
        {
            private readonly ApiDbContext db;
            private readonly NotificationContext notificationContext;
            private readonly IConfiguration configuration;

            public Handler(ApiDbContext db, NotificationContext notificationContext, IConfiguration configuration)
            {
                this.db = db;
                this.notificationContext = notificationContext;
                this.configuration = configuration;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await db.Users.FirstOrDefaultAsync(d => d.Email == request.Email, cancellationToken);

                if (user == null)
                {
                    notificationContext.AddNotification("Usuário ou senha inválidos", NotificationType.UnauthenticatedAccess);
                    return null;
                }

                if (!user.IsPasswordEqualsTo(request.Password))
                {
                    notificationContext.AddNotification("Usuário ou senha inválidos", NotificationType.UnauthenticatedAccess);
                    return null;
                }

                var claims = new[]
                 {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.GivenName, user.Name)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Auth:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(configuration["Auth:Issuer"],
                                                 configuration["Auth:Audience"],
                                                  claims,
                                                  expires: DateTime.Now.AddDays(1),
                                                  signingCredentials: creds);
                return new Result
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }
        }
    }
}
