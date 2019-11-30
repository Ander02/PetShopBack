using System;

namespace Core.Interfaces
{
    public interface IEntity
    {
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}
