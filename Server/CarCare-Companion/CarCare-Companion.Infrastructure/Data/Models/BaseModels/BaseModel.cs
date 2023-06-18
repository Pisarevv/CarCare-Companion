namespace CarCare_Companion.Infrastructure.Data.Models.BaseModels
{
    using System.ComponentModel.DataAnnotations;
    using CarCare_Companion.Infrastructure.Data.Models.Contracts;

    public abstract class BaseModel<TKey> : IAuditInfo
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
