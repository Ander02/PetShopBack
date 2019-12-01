using Api;
using FluentAssertions;
using System.Threading.Tasks;
using Test.Extensions;
using Xunit;

namespace Test.Unit.Features.Auth
{
    public class AuthTest : BaseTestClass
    {
        public AuthTest(ServerTestFixture<Startup> fixture) : base(fixture) { }

        [Fact]
        public async Task AcessarComEmailInexistenteRetornaErros()
        {
            var result = await mediator.Send(new Api.Features.Auth.Auth.Command
            {
                Email = "invalid@email.com",
                Password = "senha123"
            });

            notificationContext.EnsureHasNotifications();
            result.Should().BeNull();
        }

        [Fact]
        public async Task AcessarComSenhaInvalidaRetornaErros()
        {
            var result = await mediator.Send(new Api.Features.Auth.Auth.Command
            {
                Email = "admin@email.com",
                Password = "senha"
            });

            notificationContext.EnsureHasNotifications();
            result.Should().BeNull();
        }

        [Fact]
        public async Task AcessarComEmailESenhaValidosRetornaTokenDeAcesso()
        {
            var result = await mediator.Send(new Api.Features.Auth.Auth.Command
            {
                Email = "admin@email.com",
                Password = "senha123"
            });

            notificationContext.EnsureHasNoNotifications();
            result.Should().NotBeNull();

            result.Token.Should().NotBeEmpty();
        }
    }
}
