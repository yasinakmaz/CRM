namespace CRM.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ServiceFormHeader> SERVICEFORMHEADER { get; set; }
        public DbSet<ServiceFormExpenseDetail> SERVICEFORMEXPENSEDETAIL { get; set; }
        public DbSet<Business> BUSINESS { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ServiceFormHeader>()
            .Property(p => p.IND)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<ServiceFormExpenseDetail>()
            .Property(p => p.IND)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<Business>()
            .Property(p => p.IND)
            .ValueGeneratedOnAdd();
        }
    }
}
