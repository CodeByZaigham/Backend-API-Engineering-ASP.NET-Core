using System;
using shopforge_backend.config;
using shopforge_backend.iservices;

namespace shopforge_backend.services;

public class authservice:iauthservice
{
     private readonly appdbcontext _context;
     public authservice(appdbcontext context)
     {
          _context=context;
     }

     public Task<Tuple<int,string>> loginuser()

}
