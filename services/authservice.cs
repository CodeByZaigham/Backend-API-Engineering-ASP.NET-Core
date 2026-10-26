using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using shopforge_backend.config;
using shopforge_backend.DTO;
using shopforge_backend.iservices;
using shopforge_backend.models;

namespace shopforge_backend.services
{
     public class authservice(appdbcontext _context , IConfiguration configuration):iauthservice
     {
          // private readonly appdbcontext _context;
          // public authservice(appdbcontext context)
          // {
          //      _context=context;
          // }

          private string GetJWTToken(Users user)
          {
               var secret =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
               ?? throw new InvalidOperationException("JWT secret is missing.");

               var issuer = configuration["JWT:Issuer"]!;
               var audience = configuration["JWT:Audience"]!;

               var claims = new List<Claim>
               {
                    new Claim(
                         ClaimTypes.NameIdentifier,
                         user.Id.ToString()),

                    new Claim(
                         ClaimTypes.Role,
                         user.Role.ToString())
               };

               var credentials = new SigningCredentials(
                    secret,
                    SecurityAlgorithms.HmacSha256
               );

               var expires = DateTime.UtcNow.AddMinutes(
               double.Parse(
                    configuration["JWT:AccessTokenMinutes"] ?? "15")
               );

               var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: expires,
                    signingCredentials: credentials
               );

               return new JwtSecurityTokenHandler().WriteToken(token);
          }

          public async Task<Tuple<int,tokendto>> LoginUser(userdto dto)
          {
               try
               {
                    var tokendto=new tokendto();
                    if(dto == null)
                    {
                        
                         tokendto.token="";
                         tokendto.message="please fill all the details";
                         
                         return new Tuple<int, tokendto>(0,tokendto);
                    }
                    var existinguser=await _context.Users.FirstOrDefaultAsync(x=>x.Email==dto.Email);
                    if (existinguser == null)
                    {
                         tokendto.token="";
                         tokendto.message="invalid email";
                         return new Tuple<int, tokendto>(1,tokendto);
                    }
                    var hasher=new PasswordHasher<string>();
                    var result=hasher.VerifyHashedPassword(existinguser.Email , existinguser.Password , dto.Password);
                    if(result == PasswordVerificationResult.Success)
                    {
                         var token = GetJWTToken(existinguser);
                         tokendto.token=token;
                         tokendto.message="login successful";
                         return new Tuple<int, tokendto>(2,tokendto);
                    }
                    else if(result == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                         existinguser.Password=hash_password(dto);
                         _context.Users.Update(existinguser);
                         _context.SaveChanges();
                         var token = GetJWTToken(existinguser);
                         tokendto.token=token;
                         tokendto.message="login successful";
                         return new Tuple<int, tokendto>(3,tokendto);

                    }
                    tokendto.token="";
                    tokendto.message="invalid password";
                    return new Tuple<int, tokendto>(4,tokendto);
               }
               catch (System.Exception)
               {
                    throw;
               }
          }

          public async Task<Tuple<int,string>> RegisterUser(userdto dto)
          {
               try
               {
                    var existinguser=await _context.Users.AnyAsync(x=>x.Email==dto.Email);
                    if (existinguser)
                    {
                         return new Tuple<int, string>(0,"user already exists!");
                    }
                    _context.Users.Add(new models.Users
                    {
                         Id=Guid.NewGuid(),
                         Name=dto.Name,
                         Email=dto.Email,
                         Password=hash_password(dto),
                         Phone=dto.Phone,
                         Gender=dto.Gender,
                         Role=models.Role.User
                    });
                    await _context.SaveChangesAsync();
                    return new Tuple<int, string>(1,"user created successfully!");
               }
               catch (System.Exception)
               {
                    throw;
               }
          }

          private String hash_password(userdto dto)
          {
               var hasher=new PasswordHasher<string>();
               var Result=hasher.HashPassword(dto.Email,dto.Password);
               return Result;
          }
     } 
}


