using Api.Features.Pets;
using FluentAssertions;
using Api;
using System;
using System.Threading.Tasks;
using Xunit;
using Api.Data.Seeder;

namespace Test.Unit.Features.Pets
{
    public class ListTest : BaseTestClass
    {
        public ListTest(ServerTestFixture<Startup> fixture) : base(fixture) { }

        [Fact]
        public async Task ListarPetsSemFiltroRetornaTodosOsPets()
        {
            var result = await mediator.Send(new List.Query { });

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task ListarPetsComFiltroDeNomeRetornaPetsFiltrados()
        {
            var result = await mediator.Send(new List.Query
            {
                Search = "Bob"
            });

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task ListarPetsComFiltroDeUsuarioRetornaPetsFiltrados()
        {
            var result = await mediator.Send(new List.Query
            {
                OwnerId = UserIds.AdminId
            });

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task ListarPetsComFiltroRetornaNenhumPet()
        {
            var result = await mediator.Send(new List.Query
            {
                Search = $"{Guid.NewGuid()}"
            });

            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }
    }
}
