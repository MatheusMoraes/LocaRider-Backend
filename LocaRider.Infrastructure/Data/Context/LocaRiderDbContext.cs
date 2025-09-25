using LocaRider.Domain.Entities.Driver;
using LocaRider.Domain.Entities.Motorcycles;
using LocaRider.Domain.Entities.Rental;
using LocaRider.Domain.Entities.RentalPlan;
using LocaRider.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace LocaRider.Infrastructure.Data.Context
{
    public class LocaRiderDbContext: DbContext
    {
        public LocaRiderDbContext(DbContextOptions<LocaRiderDbContext> options)
          : base(options)
        {}

        public DbSet<User> Users { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Motorcycle> Motorcycles{ get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalPlan> RentalPlans { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RentalPlan>().HasData(
                new RentalPlan { RentalPlanId = "1", RentalPlanPeriodInDays = 7, DailyPrice = 30m },
                new RentalPlan { RentalPlanId = "2", RentalPlanPeriodInDays = 15, DailyPrice = 28m },
                new RentalPlan { RentalPlanId = "3", RentalPlanPeriodInDays = 30, DailyPrice = 22m },
                new RentalPlan { RentalPlanId = "4", RentalPlanPeriodInDays = 45, DailyPrice = 20m },
                new RentalPlan { RentalPlanId = "5", RentalPlanPeriodInDays = 50, DailyPrice = 18m }
            );

            modelBuilder.Entity<Rental>()
               .HasOne(l => l.Driver)
               .WithMany(e => e.Rentals)
               .HasForeignKey(l => l.DriverId);

            // Relacionamento Moto -> Locacao
            modelBuilder.Entity<Rental>()
                .HasOne(l => l.Motorcycle)
                .WithMany(m => m.Rentals)
                .HasForeignKey(l => l.MotorcycleId);
        }
    }
}
