namespace BullsAndCows.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    using BullsAndCows.Data.Migrations;

    using BullsAndCows.Models;

    public class BullsAndCowsDbContext : IdentityDbContext<User>
    {
        public BullsAndCowsDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BullsAndCowsDbContext, Configuration>());
        }

        public static BullsAndCowsDbContext Create()
        {
            return new BullsAndCowsDbContext();
        }

        public IDbSet<Game> Games { get; set; }
    }
}
