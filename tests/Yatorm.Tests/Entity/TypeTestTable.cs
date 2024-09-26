namespace Yatorm.Tests.Entity
{
    public class TypeTestTable
    {
        public Guid Id { get; set; }

        public string TestString { get; set; }

        public int? TestNullInt { get; set; }

        public long? TestNullBigInt { get; set; }

        public int TestInt { get; set; }

        public long TestBigInt { get; set; }

        public DateTime? TestNullDate { get; set; }

        public DateTime TestDate { get; set; }
    }
}
