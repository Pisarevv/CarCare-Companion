namespace CarCare_Companion.Infrastructure.Data.Models.Contracts
{
    using System.ComponentModel.DataAnnotations;
    public abstract class BaseModel<TKey> : IAuditInfo
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
