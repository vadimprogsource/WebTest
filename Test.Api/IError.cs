using System;
namespace Test.Api;

public interface IError
{
    string Source { get; }
    string Code   { get; }
    string Reason { get; }
}

