using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using shopforge_backend.config;
using shopforge_backend.DTO;
using shopforge_backend.iservices;

namespace shopforge_backend.services
{
     public class authservice:iauthservice
     {
          private readonly appdbcontext _context;
          public authservice(appdbcontext context)
          {
               _context=context;
          }

          public async Task<Tuple<int,string>> LoginUser(userdto dto)
          {
               try
               {
                    if(dto == null)
                    {
                         return new Tuple<int, string>(0,"please fill all the details");
                    }
                    var existinguser=await _context.Users.FirstOrDefaultAsync(x=>x.Email==dto.Email);
                    if (existinguser == null)
                    {
                         return new Tuple<int, string>(1,"invalid email");
                    }
                    var hasher=new PasswordHasher<string>();
                    var result=hasher.VerifyHashedPassword(existinguser.Email , existinguser.Password , dto.Password);
                    if(result == PasswordVerificationResult.Success)
                    {
                         return new Tuple<int, string>(2,$"{existinguser.Role}");
                    }
                    else if(result == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                         existinguser.Password=hash_password(dto);
                         _context.Users.Update(existinguser);
                         _context.SaveChanges();
                         return new Tuple<int, string>(3,$"{existinguser.Role}");

                    }
                    return new Tuple<int, string>(4,"Invalid password");
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


