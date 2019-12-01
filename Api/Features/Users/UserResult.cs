using Core;
using Core.Enums;
using System;

namespace Api.Features.Users
{
    public class UserResult
    {
        public UserResult() { }

        public UserResult(User user)
        {
            Id = user.Id;
            Email = user.Email;
            LegalDocument = user.LegalDocument;
            Name = user.Name;
            PersonType = user.PersonType;
            PhoneNumber = user.PhoneNumber;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public PersonType PersonType { get; set; }
        public string LegalDocument { get; set; }
        public string PhoneNumber { get; set; }
    }
}
