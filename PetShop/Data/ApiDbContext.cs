using Core;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace PetShop.Data
{
    public class ApiDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Adoption> Adoptions { get; set; }

        public ApiDbContext() : base() { }
        
        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<User>(_user =>
            {
                _user.HasMany(d => d.Adoptions).WithOne(d => d.Adopter).HasForeignKey(d => d.AdopterId);
                _user.HasMany(d => d.Pets).WithOne(d => d.Owner).HasForeignKey(d => d.OwnerId);
            });

            SetGlobalQueryFilters(mb);
            SetCreatedAtAndModifiedAtProperty(mb);

            base.OnModelCreating(mb);
        }


        #region Global Query Filters

        private void SetGlobalQueryFilters(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var entityClrType = entityType.ClrType;

                if (typeof(IDeletable).IsAssignableFrom(entityClrType) && !entityClrType.IsGenericType)
                {
                    var method = SetGlobalQueryForSoftDeleteMethodInfo.MakeGenericMethod(entityClrType);
                    method.Invoke(this, new object[] { modelBuilder });
                }
            }
        }

        public void SetGlobalQueryForSoftDelete<T>(ModelBuilder builder) where T : class, IEntity
        {
            builder.Entity<T>().HasQueryFilter(item => !EF.Property<DateTimeOffset?>(item, nameof(IDeletable.DeletedAt)).HasValue);
        }

        private static readonly MethodInfo SetGlobalQueryForSoftDeleteMethodInfo = typeof(ApiDbContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == nameof(SetGlobalQueryForSoftDelete));


        #endregion

        #region Set CreatedAt and ModifiedAt Property

        private void SetCreatedAtAndModifiedAtProperty(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var entityClrType = entityType.ClrType;

                if (typeof(IEntity).IsAssignableFrom(entityClrType))
                {
                    var method = SetCreatedAtAndModifiedAtPropertyOnAddMethodInfo.MakeGenericMethod(entityClrType);
                    method.Invoke(this, new object[] { modelBuilder });
                }
            }
        }

        public void SetCreatedAtAndModifiedAtPropertyOnAdd<T>(ModelBuilder builder) where T : class, IEntity
        {
            builder.Entity<T>().Property(d => d.CreatedAt).ValueGeneratedOnAdd();
            builder.Entity<T>().Property(d => d.ModifiedAt).ValueGeneratedOnAddOrUpdate();
        }

        private static readonly MethodInfo SetCreatedAtAndModifiedAtPropertyOnAddMethodInfo = typeof(ApiDbContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == nameof(SetCreatedAtAndModifiedAtPropertyOnAdd));

        #endregion
    }
}
