using Api.Data;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Api.Infrastructure.Validator
{
    public class UserExistenceValidator : AsyncValidatorBase
    {
        private readonly ApiDbContext db;

        public UserExistenceValidator(ApiDbContext db) : base("User with id {PropertyValue} doesn't exists")
        {
            this.db = db;
        }

        protected override async Task<bool> IsValidAsync(PropertyValidatorContext context, CancellationToken cancellationToken)
        {
            if (!(context.PropertyValue is Guid id)) return false;

            return await db.Users.AsNoTracking().AnyAsync(d => d.Id == id, cancellationToken);
        }
    }
}
