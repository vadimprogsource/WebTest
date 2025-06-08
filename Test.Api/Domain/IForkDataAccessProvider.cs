using System;
namespace Test.Api.Domain
{
    public interface IForkDataAccessProvider
    {
        Task<IForkLift> UpdateIsActiveAsync(int id, bool active);
    }
}

