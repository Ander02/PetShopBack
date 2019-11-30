using Core.Enums;
using Core.Interfaces;
using System;

namespace Core
{
    public class Adoption : IEntity, IDeletable
    {
        public Guid Id { get; set; }
        public Guid PetId { get; set; }
        public virtual Pet Pet { get; set; }

        public Guid AdopterId { get; set; }
        public virtual User Adopter { get; set; }

        public DonationStatus Status { get; set; }

        public DateTimeOffset DeletedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}
