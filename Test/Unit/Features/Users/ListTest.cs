using Api.Features.Users;
using FluentAssertions;
using Api;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Test.Unit.Features.Users
{
    public class ListTest : BaseTestClass
    {
        public ListTest(ServerTestFixture<Startup> fixture) : base(fixture) { }

        [Fact]
        public async Task ListarUsuariosSemFiltroRetornaTodosOsUsuarios()
        {
            var result = await mediator.Send(new List.Query { });

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task ListarUsuariosComFiltroFiltroRetornaUsuariosFiltrados()
        {
            var result = await mediator.Send(new List.Query
            {
                Search = "Admin"
            });

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task ListarUsuariosComFiltroFiltroRetornaUsuariosFiltrados2()
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
