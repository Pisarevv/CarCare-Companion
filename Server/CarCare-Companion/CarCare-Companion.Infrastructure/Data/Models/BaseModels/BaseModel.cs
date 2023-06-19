namespace CarCare_Companion.Infrastructure.Data.Models.BaseModels
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.EntityFrameworkCore;

    using CarCare_Companion.Infrastructure.Data.Models.Contracts;


    public abstract class BaseModel<TKey> : IAuditInfo
    {
        [Comment("Identifier")]
        [Key]
        public Guid Id { get; set; }

        [Comment("Date of creation")]
        public DateTime CreatedOn { get; set; }

        [Comment("Last date of modification")]
        public DateTime? ModifiedOn { get; set; }
    }
}
