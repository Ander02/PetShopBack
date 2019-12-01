using Core.Extensions;
using FluentValidation.Validators;
using System;
using System.Linq;

namespace Api.Infrastructure.Validator
{
    public class CpfValidator : PropertyValidator
    {
        public CpfValidator() : base("{PropertyValue} is a invalid Cpf") { }

        public bool IsCpfValid(string cpf)
        {
            if (cpf is null) return false;

            cpf = cpf.Clear("-", ".");
            if (cpf.Length != 11) return false;

            var cpfArr = cpf.Select(c => Convert.ToInt32(c.ToString())).ToArray();

            for (int i = 0; i < 10; i++)
                if (cpfArr.All(d => d == i)) return false;

            if (Mod11(cpfArr.Take(9).ToArray()) != cpfArr[9]) return false;

            return Mod11(cpfArr.Take(10).ToArray()) == cpfArr[10];
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (!(context.PropertyValue is string cpf)) return false;
            return IsCpfValid(cpf);
        }

        private int Mod11(int[] arr)
        {
            int sum = 0;
            for (int i = arr.Length - 1; i >= 0; i--) sum += (arr.Length - i + 1) * arr[i];
            sum %= 11;
            return sum < 2 ? 0 : 11 - sum;
        }
    }
}
