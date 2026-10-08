using System;
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
                    var existinguser=await _context.Users.FirstOrDefaultAsync(x=>x.Email==dto.Email);
                    if (existinguser == null)
                    {
                         return new Tuple<int, string>(0,"invalid email");
                    }
                    else if(existinguser.Password != dto.Password)
                    {
                         return new Tuple<int, string>(1,"invalid password");
                    }
                    else
                    {
                         return new Tuple<int, string>(2,$"{existinguser.Role}");
                    }
                    
               }
               catch (System.Exception)
               {
                    return new Tuple<int, string>(3,"something went wrong!");
               }
          }
     } 
}


