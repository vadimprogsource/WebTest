namespace Test.Api
{
    public interface IEntity : IIdentity
    {
        DateTime CreatedAt { get; }
    }
}

