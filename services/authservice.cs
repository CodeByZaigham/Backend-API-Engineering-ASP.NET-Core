using System;
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
                    
                    
               }
               catch (System.Exception)
               {
                    
                    throw;
               }
          }

     } 
}


