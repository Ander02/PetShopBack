using Core;
using Core.Enums;
using Nudes.SeedMaster.Interfaces;
using PetShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Data.Seeder
{
    public class UserSeeder : BaseSeed<ApiDbContext>
    {
        public override Task Seed(ApiDbContext context)
        {
            var users = new List<User>
            {
                new User
                {
                    Name = "Admin",
                    Email = "email@email.com",
                    LegalDocument = "11122233396",
                    PersonType = PersonType.Legal,
                    PhoneNumber = "1112341234"
                }
            };

            users.ForEach(d => d.SetPassword("senha123"));

            context.Users.AddRange(users);

            return Task.CompletedTask;
        }
    }
}
