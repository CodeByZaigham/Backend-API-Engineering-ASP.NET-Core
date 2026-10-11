using System;

namespace shopforge_backend.generics;

public class response_model<T>
{
    public T? data {get;set;}
    public String? message {get;set;} 
}
