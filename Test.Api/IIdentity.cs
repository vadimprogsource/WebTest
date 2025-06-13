using System;
namespace Test.Api
{
    public interface IIdentity
    {
        int Id { get; set; }
        bool IsValid { get; }
    }
}

