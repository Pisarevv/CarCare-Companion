namespace CarCare_Companion.Infrastructure.Data.Models.Contracts
{
    public interface IAuditInfo
    {
        DateTime CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }
    }
}
