using System;
using System.Dynamic;
using Test.Api;

namespace TestWebApi.Models;

public  record ErrorModel
{
    public string Code   { get; }
    public string Reason { get; }
    public ErrorModel? Inner { get; private set; }

    public ErrorModel(IError error)
    {
        Code = error.Code;
        Reason = error.Reason;
    }

    public ErrorModel(IEnumerable<IError> errors) 
    {
        IError first = errors.First();

        Code = first.Code;
        Reason = first.Reason;
        ErrorModel head = this;

       foreach (IError err in errors.Skip(1))
       {
            head = head.Inner  = new(err);
      }

    }

    private static string ToCamelCase(string name) => char.ToLowerInvariant(name[0]) + name[1..];

    public static object Create(IEnumerable<IError> errors)=> errors.GroupBy(x => x.Source).ToDictionary(x => ToCamelCase(x.Key), x => new ErrorModel(x));
    

 }



