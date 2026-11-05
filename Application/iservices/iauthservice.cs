using System;
using shopforge_backend.Application.DTO;

namespace shopforge_backend.Application.iservices;

public interface iauthservice
{
     Task<Tuple<int,tokendto>> LoginUser(logindto dto);
     Task<Tuple<int,string>> RegisterUser(registerdto dto);
     Task<userdto?> GetUserById(Guid id);
}
