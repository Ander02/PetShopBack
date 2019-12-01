using Core.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Api;
using Api.Features.Users;
using System.Threading.Tasks;
using Test.Extensions;
using Xunit;

namespace Test.Unit.Features.Users
{
    public class CreateTest : BaseTestClass
    {
        private readonly string _validCpf = "11122233396";
        private readonly string _validCnpj = "11222333000181";

        public CreateTest(ServerTestFixture<Startup> fixture) : base(fixture) { }

        [Fact]
        public async Task CriarUsuarioSemInformarDadosRetornaErros()
        {
            var result = await mediator.Send(new Create.Command { });

            notificationContext.EnsureHasNotifications();
            result.Should().BeNull();

        }

        [Fact]
        public async Task CriarUsuarioSemInformarNomeRetornaErros()
        {
            var result = await mediator.Send(new Create.Command
            {
                Name = string.Empty,
                Email = "email@email.com",
                LegalDocument = _validCpf,
                Password = "senha123",
                PersonType = PersonType.Physical,
                PhoneNumber = "1112341234",
            });

            notificationContext.EnsureHasNotifications();
            result.Should().BeNull();
        }

        [Fact]
        public async Task CriarUsuarioSemInformarEmailRetornaErros()
        {
            var result = await mediator.Send(new Create.Command
            {
                Name = "Nome Teste",
                LegalDocument = _validCpf,
                Password = "senha123",
                PersonType = PersonType.Physical,
                PhoneNumber = "1112341234",
            });

            notificationContext.EnsureHasNotifications();
            result.Should().BeNull();
        }

        [Fact]
        public async Task CriarUsuarioInformandoEmailInvalidoRetornaErros()
        {
            var result = await mediator.Send(new Create.Command
            {
                Name = "Nome Teste",
                Email = "emailinvalido",
                LegalDocument = _validCpf,
                Password = "senha123",
                PersonType = PersonType.Physical,
                PhoneNumber = "1112341234",
            });

            notificationContext.EnsureHasNotifications();
            result.Should().BeNull();
        }

        [Fact]
        public async Task CriarUsuarioInformandoCpfInvalidoRetornaErros()
        {
            var result = await mediator.Send(new Create.Command
            {
                Name = "Nome Teste",
                Email = "email@email.com",
                LegalDocument = _validCnpj,
                Password = "senha123",
                PersonType = PersonType.Physical,
                PhoneNumber = "1112341234",
            });

            notificationContext.EnsureHasNotifications();
            result.Should().BeNull();
        }

        [Fact]
        public async Task CriarUsuarioInformandoCnpjInvalidoRetornaErros()
        {
            var result = await mediator.Send(new Create.Command
            {
                Name = "Nome Teste",
                Email = "email@email.com",
                LegalDocument = _validCpf,
                Password = "senha123",
                PersonType = PersonType.Legal,
                PhoneNumber = "1112341234",
            });

            notificationContext.EnsureHasNotifications();
            result.Should().BeNull();
        }

        [Fact]
        public async Task CriarUsuarioInformandoSenhaInvalidaRetornaErros()
        {
            var result = await mediator.Send(new Create.Command
            {
                Name = "Nome Teste",
                Email = "email@email.com",
                LegalDocument = _validCpf,
                Password = "s123",
                PersonType = PersonType.Physical,
                PhoneNumber = "1112341234",
            });

            notificationContext.EnsureHasNotifications();
            result.Should().BeNull();
        }

        [Fact]
        public async Task CriarUsuarioInformandoTipoDeUsuarioInvalidoRetornaErros()
        {
            var result = await mediator.Send(new Create.Command
            {
                Name = "Nome Teste",
                Email = "email@email.com",
                LegalDocument = _validCpf,
                Password = "s123",
                PersonType = (PersonType)999,
                PhoneNumber = "1112341234",
            });

            notificationContext.EnsureHasNotifications();
            result.Should().BeNull();
        }

        [Fact]
        public async Task CriarUsuarioSemInformarTelefoneRetornaErros()
        {
            var result = await mediator.Send(new Create.Command
            {
                Name = "Nome Teste",
                Email = "email@email.com",
                LegalDocument = _validCpf,
                Password = "s123",
                PersonType = PersonType.Legal,
            });

            notificationContext.EnsureHasNotifications();
            result.Should().BeNull();
        }

        [Fact]
        public async Task CriarUsuarioComDadosCorretosCriaRegistroNoBancoDeDados()
        {
            var request = new Create.Command
            {
                Name = "Nome Teste",
                Email = "email@email.com",
                LegalDocument = _validCpf,
                Password = "senha123",
                PersonType = PersonType.Physical,
                PhoneNumber = "1112341234"
            };
            var result = await mediator.Send(request);

            notificationContext.EnsureHasNoNotifications();
            result.Should().NotBeNull();

            var user = await db.Users.FirstOrDefaultAsync(d => d.Id == result.Id);
            user.Should().NotBeNull();

            user.Name.Should().Be(result.Name).And.Be(request.Name);
            user.Email.Should().Be(result.Email).And.Be(request.Email);
            user.LegalDocument.Should().Be(result.LegalDocument).And.Be(request.LegalDocument);
            user.PersonType.Should().Be(result.PersonType).And.Be(request.PersonType);
            user.PhoneNumber.Should().Be(result.PhoneNumber).And.Be(request.PhoneNumber);

            user.IsPasswordEqualsTo(request.Password).Should().BeTrue();
        }
    }
}
