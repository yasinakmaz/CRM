namespace CRM.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ServiceFormHeader> SERVICEFORMHEADER { get; set; }
        public DbSet<ServiceFormExpenseDetail> SERVICEFORMEXPENSEDETAIL { get; set; }
        public DbSet<Business> BUSINESS { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer($"Data Source={PublicSettings.MSSQLSERVER};Initial Catalog={PublicSettings.MSSQLDATABASE};Persist Security Info=False;User ID={PublicSettings.MSSQLUSERNAME};Password={PublicSettings.MSSQLPASSWORD};Encrypt=True;Trust Server Certificate=True;Connection Timeout=5;Command Timeout=300;Pooling=True;Application Name=CRM-{AppInfo.VersionString};Language=Turkish;");
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
