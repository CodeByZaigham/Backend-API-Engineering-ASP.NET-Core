using System;
using shopforge_backend.DTO;

namespace shopforge_backend.iservices;

public interface iauthservice
{
     Task<Tuple<int,string>> LoginUser(userdto dto);
     // isko parhna he ke ye kyu add hua yahan pe
}
