namespace CarCare_Companion.Infrastructure.Data.Models.Contracts
{
    using System.ComponentModel.DataAnnotations;
    public abstract class BaseDeletableModel<TKey> : IAuditInfo, IDeletableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool isDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
