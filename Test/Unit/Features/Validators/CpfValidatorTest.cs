using FluentAssertions;
using Api;
using Api.Infrastructure.Validator;
using Xunit;

namespace Test.Unit.Features.Validators
{
    public class CpfValidatorTest : BaseTestClass
    {
        private readonly CpfValidator cpfValidator;

        public CpfValidatorTest(ServerTestFixture<Startup> fixture) : base(fixture)
        {
            cpfValidator = new CpfValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData("123")]
        [InlineData("11111111111")]
        [InlineData("22222222222")]
        [InlineData("33333333333")]
        [InlineData("44444444444")]
        [InlineData("55555555555")]
        [InlineData("66666666666")]
        [InlineData("77777777777")]
        [InlineData("88888888888")]
        [InlineData("99999999999")]
        [InlineData("00000000000")]
        [InlineData("12312312312")]
        [InlineData("98746213545")]
        [InlineData("21548752233")]
        [InlineData("74125896512")]
        [InlineData("36985214567")]
        [InlineData("36543821130")]
        [InlineData("20316374213")]
        [InlineData("10884661228")]
        public void ValidateInvalidCpf(string cpf)
        {
            cpfValidator.IsCpfValid(cpf).Should().BeFalse();
        }

        [Theory]
        [InlineData("11122233396")]
        [InlineData("23821728000")]
        [InlineData("40668508043")]
        [InlineData("89665112015")]
        [InlineData("48225500016")]
        [InlineData("97546820030")]
        [InlineData("90316374083")]
        [InlineData("20884661008")]
        [InlineData("94148407025")]
        [InlineData("74850385087")]
        public void ValidateValidCpf(string cpf)
        {
            cpfValidator.IsCpfValid(cpf).Should().BeTrue();
        }
    }
}
