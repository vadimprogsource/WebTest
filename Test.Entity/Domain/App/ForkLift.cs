using Test.Api.Domain.App;
using Test.Api.Domain.Security;
using Test.Entity.Domain.Security;

namespace Test.Entity.Domain.App
{
    public class ForkLift : EntityBase, IForkLift
    {
        public string Brand { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public decimal Capacity { get; set; } // up to 3 digits after decimal
        public bool IsActive { get; set; } = false;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public User ModifiedBy { get; set; } = User.Empty;

        public Guid ModifiedByGuid { get; set; }

        public List<ForkFault> Faults { get; set; } = [];

        IReadOnlyCollection<IForkFault> IForkLift.Faults => Faults;

        IUser IForkLift.ModifiedBy => ModifiedBy;


        public ForkLift()
        { }

        public ForkLift(IForkLift source) : base(source)
        {
            Update(source);

            ModifiedAt = source.ModifiedAt;

            if (source.ModifiedBy != null)
            {
                ModifiedByGuid = source.ModifiedBy.Guid;
            }


        }

        public ForkLift Update(IForkLift source)
        {
            Brand = source.Brand;
            Number = source.Number;
            Capacity = source.Capacity;
            IsActive = source.IsActive;
            return this;
        }

    }
}

