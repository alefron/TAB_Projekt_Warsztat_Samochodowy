using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }

    }
}
