namespace Domain.Common
{
    public class BaseEntity<TId>
    {
        public TId Id { get; set; }

        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
