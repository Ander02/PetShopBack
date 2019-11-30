using Core.Interfaces;
using System;

namespace Core
{
    public class Pet : IEntity, IDeletable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Breed { get; set; }
        public bool InDonation { get; set; }

        public Guid OwnerId { get; set; }
        public virtual User Owner { get; set; }

        public DateTimeOffset DeletedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}
