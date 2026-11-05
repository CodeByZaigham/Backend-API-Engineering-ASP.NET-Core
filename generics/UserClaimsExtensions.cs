using System.Security.Claims;

namespace shopforge_backend.generics;

public static class UserClaimsExtensions
{
     // Reads the logged-in user's Id from the JWT. Returns null if missing.
     public static Guid? GetUserId(this ClaimsPrincipal user)
     {
          var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? user.FindFirst("nameid")?.Value
                   ?? user.FindFirst("sub")?.Value;

          return Guid.TryParse(value, out var id) ? id : null;
     }
}
