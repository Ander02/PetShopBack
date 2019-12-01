using Core;
using Core.Enums;
using Nudes.SeedMaster.Interfaces;
using System;
using System.Collections.Generic;
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
                    Id = UserIds.AdminId,
                    Name = "Admin",
                    Email = "admin@email.com",
                    LegalDocument = "11122233396",
                    PersonType = PersonType.Legal,
                    PhoneNumber = "1112341234"
                },
                new User
                {
                    Id = UserIds.User2Id,
                    Name = "User 2",
                    Email = "user2@email.com",
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

    public static class UserIds
    {
        public static Guid AdminId => Guid.Parse("6580c04b-a59b-4016-b453-dbd6cddce9e6");
        public static Guid User2Id => Guid.Parse("564c8261-a6cb-4e2c-9380-eb978850e756");
    }
}
