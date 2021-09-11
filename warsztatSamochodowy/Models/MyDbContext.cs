using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using warsztatSamochodowy.Models;
using warsztatSamochodowy.Forms;

namespace warsztatSamochodowy.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) :base(options)
        {

        }

        public DbSet<Action> Actions { get; set; }
        public DbSet<ActionType> ActionTypes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Personel> Personel { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Proposal>().HasOne(prop => prop.Vehicle).WithMany(veh => veh.Proposals).HasForeignKey(prop => prop.VehicleId).OnDelete(DeleteBehavior.ClientCascade);
            modelBuilder.Entity<Action>().HasOne(act => act.Proposal).WithMany(prop => prop.Actions).HasForeignKey(act => act.ProposalId).OnDelete(DeleteBehavior.ClientCascade);
        }

    }
}
