namespace Test.Api
{
    public interface IIdentity
    {
        Guid Guid { get; }
        bool IsValid { get; }
    }
}

