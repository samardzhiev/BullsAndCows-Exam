namespace BullsAndCows.Data
{
    using System;
    using System.Linq;

    using BullsAndCows.Data.Repositories;
    using BullsAndCows.Models;

    public interface IBullsAndCowsData
    {
        IRepository<User> Users { get; }

        IRepository<Game> Games { get; }

        int SaveChanges();
    }
}
