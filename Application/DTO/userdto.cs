using System;
using shopforge_backend.models;

namespace shopforge_backend.Application.DTO
{
     // Now ONLY used as a response (GET /api/auth/me).
     // Login and signup use logindto / registerdto (see authdto.cs).
     // Password and Role are no longer accepted from the client.
     public class userdto
     {
          public Guid Id { get; set; }
          public String? Name { get; set; }
          required public String Email { get; set; }
          public String? Phone { get; set; }
          public String? Gender { get; set; }
          public Role Role { get; set; }
     }
}
