using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace shopforge_backend.generics;

public class response_model<T>
{
     public T? Data {get;set;}
     public String? Message {get;set;}
     public bool Status {get;set;}

     // Not sent to the client. Used by ToActionResult() to pick the HTTP status.
     [JsonIgnore]
     public int HttpCode {get;set;} = 200;

     public static response_model<T> success(T? data , String message)
     {
          return new response_model<T>{
               Data=data,
               Message=message,
               Status=true,
               HttpCode=200
          };
     }

     // httpCode is optional, so existing calls like fail(null,"msg") still compile
     public static response_model<T> fail(T? data , String message , int httpCode = 400)
     {
          return new response_model<T>{
               Data=data,
               Message=message,
               Status=false,
               HttpCode=httpCode
          };
     }

     public IActionResult ToActionResult()
     {
          return new ObjectResult(this){ StatusCode = HttpCode };
     }
}
