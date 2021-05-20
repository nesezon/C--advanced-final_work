using System.Collections.Generic;
using System.Data.Entity;

namespace e_commerce.DBModel {
  public class StoreDB : DbContext {
    public StoreDB() : base("name=Model2") { }

    public virtual DbSet<Order_item> Order_items { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
      // избавляюсь от ON DELETE CASCADE для внешних ключей
      modelBuilder.Entity<Order>().HasMany(e => e.Order_items).WithRequired(e => e.Orders).WillCascadeOnDelete(false);
      modelBuilder.Entity<Product>().HasMany(e => e.Order_items).WithRequired(e => e.Products).WillCascadeOnDelete(false);
      modelBuilder.Entity<Role>().HasMany(e => e.Users).WithRequired(e => e.Roles).WillCascadeOnDelete(false);
      modelBuilder.Entity<User>().HasMany(e => e.Orders).WithRequired(e => e.Users).WillCascadeOnDelete(false);
      // TODO: потом проверить без него
      modelBuilder.Entity<Product>().Property(e => e.price).HasPrecision(19, 4);
    }

    public class StoreDbInitializer : DropCreateDatabaseAlways<StoreDB> {

      public override void InitializeDatabase(StoreDB context) {
        // исправляю ошибку, когда при повторном запуске "Cannot drop database because it is currently in use"
        context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction
            , string.Format("ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", context.Database.Connection.Database));
        base.InitializeDatabase(context);
      }

      protected override void Seed(StoreDB context) {

        var roles = new List<Role> {
          new Role { name = "customer" },
          new Role { name = "manager" }
        };
        var users = new List<User> {
          new User { role_id = 1, login = "ivan", password = "123", full_name = "Иван Иванов", deleted = false },
          new User { role_id = 1, login = "andrey", password = "231", full_name = "Андрей Андреев", deleted = true },
          new User { role_id = 2, login = "petr", password = "312", full_name = "Петр Петров", deleted = false }
        };
        roles.ForEach(s => context.Roles.Add(s));
        users.ForEach(s => context.Users.Add(s));
        context.SaveChanges();
      }

    }

  }
}