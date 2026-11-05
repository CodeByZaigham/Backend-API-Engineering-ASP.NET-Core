using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using shopforge_backend.Application.DTO;
using shopforge_backend.Application.iservices;
using shopforge_backend.Domain.models;
using shopforge_backend.config;

namespace shopforge_backend.services
{
     public class authservice(appdbcontext _context , IConfiguration configuration):iauthservice
     {
          private string GetJWTToken(Users user)
          {
               var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    configuration["JWT:Key"] ?? throw new InvalidOperationException("JWT secret is missing.")));

               var issuer = configuration["JWT:Issuer"]!;
               var audience = configuration["JWT:Audience"]!;

               var claims = new List<Claim>
               {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
               };

               var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

               var expires = DateTime.UtcNow.AddMinutes(
                    double.Parse(configuration["JWT:AccessTokenMinutes"] ?? "15"));

               var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: expires,
                    signingCredentials: credentials
               );

               return new JwtSecurityTokenHandler().WriteToken(token);
          }

          // Result codes: 0 = bad input, 1 = unknown email, 2 = ok, 3 = ok (hash upgraded), 4 = wrong password
          public async Task<Tuple<int,tokendto>> LoginUser(logindto dto)
          {
               var result = new tokendto();

               if (dto == null)
               {
                    result.token = "";
                    result.message = "please fill all the details";
                    return new Tuple<int, tokendto>(0, result);
               }

               var email = dto.Email.Trim();
               var existinguser = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
               if (existinguser == null)
               {
                    result.token = "";
                    result.message = "invalid email or password";
                    return new Tuple<int, tokendto>(1, result);
               }

               var hasher = new PasswordHasher<string>();
               var check = hasher.VerifyHashedPassword(existinguser.Email, existinguser.Password, dto.Password);

               if (check == PasswordVerificationResult.Success)
               {
                    result.token = GetJWTToken(existinguser);
                    result.message = "login successful";
                    return new Tuple<int, tokendto>(2, result);
               }

               if (check == PasswordVerificationResult.SuccessRehashNeeded)
               {
                    existinguser.Password = hasher.HashPassword(existinguser.Email, dto.Password);
                    await _context.SaveChangesAsync();
                    result.token = GetJWTToken(existinguser);
                    result.message = "login successful";
                    return new Tuple<int, tokendto>(3, result);
               }

               result.token = "";
               result.message = "invalid email or password";
               return new Tuple<int, tokendto>(4, result);
          }

          public async Task<Tuple<int,string>> RegisterUser(registerdto dto)
          {
               var email = dto.Email.Trim().ToLowerInvariant();

               var exists = await _context.Users.AnyAsync(x => x.Email == email);
               if (exists)
               {
                    return new Tuple<int, string>(0, "user already exists!");
               }

               var hasher = new PasswordHasher<string>();

               _context.Users.Add(new Users
               {
                    Id = Guid.NewGuid(),
                    Name = $"{dto.FirstName} {dto.LastName}".Trim(),
                    Email = email,
                    Password = hasher.HashPassword(email, dto.Password),
                    Phone = dto.Phone,
                    Gender = dto.Gender,
                    Role = Role.User     // nobody can register themselves as admin
               });
               await _context.SaveChangesAsync();
               return new Tuple<int, string>(1, "user created successfully!");
          }

          public async Task<userdto?> GetUserById(Guid id)
          {
               var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
               if (user == null)
                    return null;

               return new userdto
               {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    Gender = user.Gender,
                    Role = user.Role
               };
          }
     }
}
