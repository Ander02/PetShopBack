using Core.Enums;
using Core.Extensions;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Core
{
    public class User : IEntity, IDeletable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public PersonType PersonType { get; set; }
        public string LegalDocument { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; private set; }
        public string PasswordSalt { get; private set; }

        public virtual IList<Pet> Pets { get; set; }
        public virtual IList<Adoption> Adoptions { get; set; }

        public DateTimeOffset DeletedAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

        public bool IsPasswordEqualsTo(string password) => Encrypt(password, this.PasswordSalt) == this.PasswordHash;

        public void SetPassword(string password)
        {
            var salt = GenerateSalt();
            this.PasswordSalt = salt.ToBase64();
            this.PasswordHash = Encrypt(password, PasswordSalt);
        }

        protected static string Encrypt(string password, string salt)
        {
            byte[] toEncrypt = salt.FromBase64().Concat(Encoding.UTF8.GetBytes(password)).ToArray();
            using var sha = SHA512.Create();
            return sha.ComputeHash(toEncrypt).ToBase64();
        }

        protected static byte[] GenerateSalt()
        {
            byte[] salt = new byte[64];
            using (var generator = RandomNumberGenerator.Create()) generator.GetBytes(salt);
            return salt;
        }
    }
}
