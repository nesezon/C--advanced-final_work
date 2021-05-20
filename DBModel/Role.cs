using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.DBModel {
  public class Role {
    public Role() {
      Users = new HashSet<User>();
    }

    [Key]
    public short role_id { get; set; }

    [Required]
    [StringLength(30)]
    public string name { get; set; }
    public virtual ICollection<User> Users { get; set; }
  }
}
