using System;

namespace shopforge_backend.generics;

public class response_model<T>
{
     public T? Data {get;set;}
     public String? Message {get;set;} 
     public bool Status {get;set;}

     public response_model<T> success(T? data , String message)
     {
          return new response_model<T>{
               Data=data,
               Message=message,
               Status=true
          };
     }
     public response_model<T> fail(T? data , String message)
     {
          return new response_model<T>{
               Data=data,
               Message=message,
               Status=false
          };
     }
}
