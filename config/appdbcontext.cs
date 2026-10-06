using System;
using Microsoft.EntityFrameworkCore;

namespace shopforge_backend.config;

public class appdbcontext:DbContext
{
     public appdbcontext(DbContextOptions options):base(options)
     {
          
     }

}

// we need to create models and then add them in appdbcontext then
// initialize migrations and update db