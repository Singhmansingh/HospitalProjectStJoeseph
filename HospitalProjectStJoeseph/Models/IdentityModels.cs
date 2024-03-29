using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using HospitalProjectStJoeseph.Models;

namespace HospitalProjectStJoeseph.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        // new entity to the system
        public DbSet<Requisition> Requisitions { get; set; }

        public DbSet<Test> Tests { get; set; }

            public DbSet<Clinic> Clinics { get; set; }
            public DbSet<Service> Services { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Physician> Physicians { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<UserPatient> UserPatients { get; set; }
        public DbSet<BestWish> BestWishes { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
