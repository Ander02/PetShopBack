using Core;
using Nudes.SeedMaster.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Data.Seeder
{
    public class PetSeeder : BaseSeed<ApiDbContext>
    {
        public override Task Seed(ApiDbContext context)
        {
            var pets = new List<Pet>
            {
                new Pet
                {
                    Id = PetIds.AdminPetId,
                    Name = "Luna",
                    OwnerId = UserIds.AdminId,
                    InDonation = false,
                    Breed = "Vira Lata Caramelo",
                },
                new Pet
                {
                    Id = PetIds.User2PetId,
                    Name = "Bob",
                    OwnerId = UserIds.User2Id,
                    InDonation = false,
                    Breed = "Pooddle",
                }
            };

            context.Pets.AddRange(pets);

            return Task.CompletedTask;
        }
    }

    public static class PetIds
    {
        public static Guid AdminPetId => Guid.Parse("4702023d-86b0-4f2b-9cab-7c46771765f0");
        public static Guid User2PetId => Guid.Parse("010a710c-ba50-42e1-8bb1-370ccf5dda89");
    }
}
