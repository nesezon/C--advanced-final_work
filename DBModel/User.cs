using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.DBModel {
  public class User {
    public User() {
      Orders = new HashSet<Order>();
    }

    [Key]
    public int user_id { get; set; }

    public short role_id { get; set; }

    [Required]
    [StringLength(30)]
    public string login { get; set; }

    [Required]
    [StringLength(30)]
    public string password { get; set; }

    [Required]
    [StringLength(80)]
    public string full_name { get; set; }

    public bool deleted { get; set; } = false;

    public virtual ICollection<Order> Orders { get; set; }

    public virtual Role Roles { get; set; }
  }

  public class UsersForEdit {
    public int user_id { get; set; }
    public string full_name { get; set; }
    public string login { get; set; }
    public string password { get; set; }
    public short role_id { get; set; }
  }

  public class RolesForCombo {
    public short role_id { get; set; }
    public string name { get; set; }
  }
}
