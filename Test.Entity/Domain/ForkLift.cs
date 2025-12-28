using Test.Api.Domain;

namespace Test.Entity.Domain
{
    public class ForkLift : EntityBase, IForkLift
    {
        public string Brand { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public decimal Capacity { get; set; } // up to 3 digits after decimal
        public bool IsActive { get; set; } = false;
        public DateTime ModifiedAt { get; set; } = DateTime.Now;
        public User ModifiedBy { get; set; } = new User();

        public Guid ModifiedByGuid { get; set; }

        public List<ForkFault> Faults { get; set; } = new();

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

