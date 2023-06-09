﻿namespace CarCare_Companion.Infrastructure.Data.Models.Identity;

using System;

using CarCare_Companion.Infrastructure.Data.Models.Contracts;
using Microsoft.AspNetCore.Identity;

public class ApplicationRole : IdentityRole<Guid>, IAuditInfo, IDeletableEntity
{
    public ApplicationRole()
        : this(null)
    {
    }

    public ApplicationRole(string name)
        : base(name)
    {
        this.Id = Guid.NewGuid();
        this.CreatedOn = DateTime.UtcNow;
    }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}
