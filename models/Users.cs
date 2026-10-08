using System.ComponentModel.DataAnnotations;
using System.Data;

namespace shopforge_backend.models
{
    public class Users
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public String? Name { get; set; }
        required public String Email { get; set; }
        required public String Password {get; set;}
        public String? Phone { get; set; }
        public String? Gender { get; set; }
        public Role Role { get; set; }
    }
    public enum Role
    {
        Admin,
        User
    }
}
