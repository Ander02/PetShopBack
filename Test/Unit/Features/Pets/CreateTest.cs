using Api;
using Api.Data.Seeder;
using Api.Features.Pets;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Test.Extensions;
using Xunit;

namespace Test.Unit.Features.Pets
{
    public class CreateTest : BaseTestClass
    {
        public CreateTest(ServerTestFixture<Startup> fixture) : base(fixture) { }

        [Fact]
        public async Task CriarPetSemInformarDadosRetornaErros()
        {
            var result = await mediator.Send(new Create.Command { });

            notificationContext.EnsureHasNotifications();
            result.Should().BeNull();
        }

        [Fact]
        public async Task CriarPetSemInformarNomeRetornaErros()
        {
            var result = await mediator.Send(new Create.Command
            {
                Breed = "Pet Breed",
                OwnerId = UserIds.User2Id
            });

            notificationContext.EnsureHasNotifications();
            result.Should().BeNull();
        }

        [Fact]
        public async Task CriarPetSemInformarRacaRetornaErros()
        {
            var result = await mediator.Send(new Create.Command
            {
                Name = "Pet Name",
                OwnerId = UserIds.User2Id
            });

            notificationContext.EnsureHasNotifications();
            result.Should().BeNull();
        }

        [Fact]
        public async Task CriarPetInformandoDonoInvalidoRetornaErros()
        {
            var result = await mediator.Send(new Create.Command
            {
                Name = "Pet Name",
                Breed = "Pet Breed",
                OwnerId = Guid.NewGuid()
            });

            notificationContext.EnsureHasNotifications();
            result.Should().BeNull();
        }

        [Fact]
        public async Task CriarPetComInformacoesCorretasRetornaErros()
        {
            Create.Command command = new Create.Command
            {
                Name = "Pet Name",
                Breed = "Pet Breed",
                OwnerId = UserIds.AdminId,
            };
            var result = await mediator.Send(command);

            notificationContext.EnsureHasNoNotifications();
            result.Should().NotBeNull();

            var pet = await db.Pets.FirstOrDefaultAsync(d => d.Id == result.Id);
            pet.Should().NotBeNull();

            pet.Name.Should().Be(result.Name).And.Be(command.Name);
            pet.Breed.Should().Be(result.Breed).And.Be(command.Breed);
            pet.OwnerId.Should().Be(result.OwnerId).And.Be(command.OwnerId);
        }
    }
}
