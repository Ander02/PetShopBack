using System;

namespace Core.Interfaces
{
    public interface IDeletable
    {
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
