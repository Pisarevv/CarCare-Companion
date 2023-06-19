namespace CarCare_Companion.Infrastructure.Data.Models.BaseModels
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;
   
    using CarCare_Companion.Infrastructure.Data.Models.Contracts;

    public abstract class BaseDeletableModel<TKey> : IAuditInfo, IDeletableEntity
    {
        [Key]
        [Comment("Identifier")]
        public Guid Id { get; set; }

        [Comment("Date of creation")]
        public DateTime CreatedOn { get; set; }

        [Comment("Last date of modification")]
        public DateTime? ModifiedOn { get; set; }

        [Comment("Deleted status")]
        public bool isDeleted { get; set; }

        [Comment("Date of deleting")]
        public DateTime? DeletedOn { get; set; }
    }
}
