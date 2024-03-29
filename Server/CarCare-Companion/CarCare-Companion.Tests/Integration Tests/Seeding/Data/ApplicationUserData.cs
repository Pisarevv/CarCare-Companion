﻿namespace CarCare_Companion.Tests.Integration_Tests.Seeding.Data;

using System;

using CarCare_Companion.Infrastructure.Data.Models.Identity;

using static Integration_Tests.Common.TestDataConstants;

public static class ApplicationUserData
{
    public static List<ApplicationUser> Users = new List<ApplicationUser>()
    {
        new ApplicationUser()
        {
            Id = Guid.Parse(User1Id),  
            FirstName = "Harold",
            LastName = "Peterson",
            UserName = "HaroldPeterson@mail.com",
            Email = "HaroldPeterson@mail.com",
            SecurityStamp = Guid.NewGuid().ToString(),
            CreatedOn = DateTime.UtcNow,
        },
         new ApplicationUser()
        {
            Id = Guid.Parse(User2Id),
            FirstName = "Emma",
            LastName = "Smith",
            UserName = "EmmaSmith@mail.com",
            Email =  "EmmaSmith@mail.com",
            SecurityStamp = Guid.NewGuid().ToString(),
            CreatedOn = DateTime.UtcNow,
        },
          new ApplicationUser()
        {
            Id = Guid.Parse(User3Id),
            FirstName = "Oliver",
            LastName = "Johnson",
            UserName = "OliverJohnson@mail.com",
            Email = "OliverJohnson@mail.com",
            SecurityStamp = Guid.NewGuid().ToString(),
            CreatedOn = DateTime.UtcNow,
        },
           new ApplicationUser()
        {
            Id = Guid.Parse(User4Id),
            FirstName = "Ava",
            LastName = "Brown",
            UserName = "AvaBrown@mail.com",
            Email = "AvaBrown@mail.com",
            SecurityStamp = Guid.NewGuid().ToString(),
            CreatedOn = DateTime.UtcNow,
        }

    };
}
