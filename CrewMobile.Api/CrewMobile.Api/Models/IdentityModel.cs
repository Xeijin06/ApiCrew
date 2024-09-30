using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using CrewMobile.Domain.Models;
using CrewMobile.Common.Models;

namespace CrewMobile.Api.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = new ClaimsIdentity(await manager.GetClaimsAsync(this), authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //DbSets
        public DbSet<FlightCrew> FlightCrews { get; set; }
        //TODO: Validar si el cambio a CMParameter afecta a la aplicación
        public DbSet<CMParameter> Parameters { get; set; }
        public DbSet<Descriptor> Descriptors { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Passanger> Passangers { get; set; }
        public DbSet<SecuritySSR> SecuritySSRs { get; set; }
        public DbSet<ProccessFileLog> ProccessFileLogs { get; set; }
        public DbSet<FlighStatusMock> FlighStatusMocks { get; set; }
    }
}