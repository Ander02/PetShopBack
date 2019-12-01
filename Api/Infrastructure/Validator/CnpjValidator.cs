using Core.Extensions;
using FluentValidation.Validators;
using System;
using System.Linq;

namespace Api.Infrastructure.Validator
{
    public class CnpjValidator : PropertyValidator
    {
        public CnpjValidator() : base("{PropertyValue} is a invalid Cnpj") { }

        public bool IsCnpjValid(string cnpj)
        {
            if (cnpj is null) return false;

            cnpj = cnpj.Clear("-", ".", "/");

            if (cnpj.Length != 14) return false;

            var cnpjArray = cnpj.Select(c => Convert.ToInt32(c.ToString())).ToArray();

            for (int i = 0; i < 10; i++)
                if (cnpjArray.All(d => d == i)) return false;

            if (Mod11(cnpjArray.Take(12).ToArray()) != cnpjArray[12]) return false;

            return Mod11(cnpjArray.Take(13).ToArray()) == cnpjArray[13];
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (!(context.PropertyValue is string cpf)) return false;
            return IsCnpjValid(cpf);
        }

        private int Mod11(int[] array)
        {
            int sum = 0;
            for (int i = 1; i < array.Length - 7; i++) sum += (array.Length - 6 - i) * array[i - 1];
            for (int i = 1; i < array.Length - (array.Length % 10) - 1; i++) sum += (array.Length - (array.Length % 10) - i) * array[i + (array.Length % 10) + 1];
            sum %= 11;
            return sum < 2 ? 0 : 11 - sum;
        }
    }
}
