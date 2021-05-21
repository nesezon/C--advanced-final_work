using System;
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
    }

    public class StoreDbInitializer : DropCreateDatabaseAlways<StoreDB> {
      protected override void Seed(StoreDB context) {

        var roles = new List<Role> {
          new Role { name = "customer" },
          new Role { name = "manager" }
        };
        var users = new List<User> {
          new User { user_id = 1, role_id = 1, login = "ivan", password = "123", full_name = "Иван Иванов", deleted = false },
          new User { user_id = 2, role_id = 1, login = "andrey", password = "231", full_name = "Андрей Андреев", deleted = true },
          new User { user_id = 3, role_id = 2, login = "petr", password = "312", full_name = "Петр Петров", deleted = false }
        };
        var orders = new List<Order> {
          new Order { order_id = 1, user_id = 1, order_time = new DateTime(2021, 1, 15, 12, 11, 10) },
          new Order { order_id = 2, user_id = 2, order_time = new DateTime(2021, 1, 30, 9, 20, 0) },
          new Order { order_id = 3, user_id = 3, order_time = new DateTime(2021, 2, 11, 1, 40, 1) }
        };
        var products = new List<Product> {
          new Product { product_id = 1, name = "Ручка", price = 4.5M, deleted = false },
          new Product { product_id = 2, name = "Тетрадь", price = 2M, deleted = false },
          new Product { product_id = 3, name = "Циркуль", price = 7M, deleted = false }
        };
        var order_items = new List<Order_item> {
          new Order_item { order_id = 1, product_id = 2, quantity = 11 },
          new Order_item { order_id = 1, product_id = 3, quantity = 1 },
          new Order_item { order_id = 2, product_id = 1, quantity = 3 },
          new Order_item { order_id = 2, product_id = 2, quantity = 7 },
          new Order_item { order_id = 3, product_id = 3, quantity = 1 }
        };
        roles.ForEach(s => context.Roles.Add(s));
        users.ForEach(s => context.Users.Add(s));
        orders.ForEach(s => context.Orders.Add(s));
        products.ForEach(s => context.Products.Add(s));
        order_items.ForEach(s => context.Order_items.Add(s));
        context.SaveChanges();

      }
    }
  }
}