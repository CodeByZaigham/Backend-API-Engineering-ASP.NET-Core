using System;
using shopforge_backend.Application.DTO;

namespace shopforge_backend.Application.iservices;

public interface iauthservice
{
     Task<Tuple<int,tokendto>> LoginUser(userdto dto);
     Task<Tuple<int,string>> RegisterUser(userdto dto);
     // isko parhna he ke ye kyu add hua yahan pe
}
