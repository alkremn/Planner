namespace Planner.DataAccess
{
    using System.Data.Entity;
    using Planner.Model;

    public partial class PlannerDbContext : DbContext
    {
        public PlannerDbContext()
            : base("name=PlannerDbContext")
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Incrementtype> Incrementtypes { get; set; }
        public virtual DbSet<Reminder> Reminders { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .Property(e => e.Address1)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.Address2)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.PostalCode)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.LastUpdateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Appointment>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Appointment>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Appointment>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<Appointment>()
                .Property(e => e.Contact)
                .IsUnicode(false);

            modelBuilder.Entity<Appointment>()
                .Property(e => e.Url)
                .IsUnicode(false);

            modelBuilder.Entity<Appointment>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Appointment>()
                .Property(e => e.LastUpdateBy)
                .IsUnicode(false);

            modelBuilder.Entity<City>()
                .Property(e => e.City1)
                .IsUnicode(false);

            modelBuilder.Entity<City>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<City>()
                .Property(e => e.LastUpdateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .Property(e => e.Country1)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .Property(e => e.LastUpdateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.CustomerName)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.LastUpdateBy)
                .IsUnicode(false);

            modelBuilder.Entity<Incrementtype>()
                .Property(e => e.IncrementTypeDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Reminder>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Reminder>()
                .Property(e => e.Remindercol)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.CreateBy)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.LastUpdatedBy)
                .IsUnicode(false);
        }
    }
}
