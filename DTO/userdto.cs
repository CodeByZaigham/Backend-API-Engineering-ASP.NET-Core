using System;
using shopforge_backend.models;

namespace shopforge_backend.DTO
{
     public class userdto
     {
          public Guid Id { get; set; }
          public String? Name { get; set; }
          required public String Email { get; set; }
          required public String Password {get; set;}
          public String? Phone { get; set; }
          public String? Gender { get; set; }
          public Role Role { get; set; }
     }
}
