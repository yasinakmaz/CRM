namespace CRM.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ServiceFormHeader> SERVICEFORMHEADER { get; set; }
        public DbSet<ServiceFormExpenseDetail> SERVICEFORMEXPENSEDETAIL { get; set; }
        public DbSet<Business> BUSINESS { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ServiceFormHeader>(entity =>
            {
                entity.HasKey(e => e.IND);
                entity.Property(e => e.IND).ValueGeneratedOnAdd();
                entity.Property(e => e.ServiceDate).HasColumnType("datetime2(3)");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime2(3)");
                entity.Property(e => e.LASTUPDATEDATE).HasColumnType("datetime2(3)");

                entity.HasIndex(e => e.BUSINESSIND).HasDatabaseName("IX_ServiceFormHeader_BusinessInd");
                entity.HasIndex(e => e.ServiceDate).HasDatabaseName("IX_ServiceFormHeader_ServiceDate");
                entity.HasIndex(e => new { e.BUSINESSIND, e.ServiceDate }).HasDatabaseName("IX_ServiceFormHeader_Business_Date");
            });

            modelBuilder.Entity<ServiceFormExpenseDetail>(entity =>
            {
                entity.HasKey(e => e.IND);
                entity.Property(e => e.IND).ValueGeneratedOnAdd();
                entity.Property(e => e.AMOUNT).HasColumnType("decimal(18,2)");
                entity.Property(e => e.COMMENT).HasMaxLength(500);
                entity.Property(e => e.CREATEDDATE).HasColumnType("datetime2(3)");
                entity.Property(e => e.LASTUPDATEDATE).HasColumnType("datetime2(3)");

                entity.HasIndex(e => e.FORMIND).HasDatabaseName("IX_ServiceFormExpenseDetail_FormInd");
                entity.HasIndex(e => e.CREATEDDATE).HasDatabaseName("IX_ServiceFormExpenseDetail_CreatedDate");
            });

            modelBuilder.Entity<Business>(entity =>
            {
                entity.HasKey(e => e.IND);
                entity.Property(e => e.IND).ValueGeneratedOnAdd();
                entity.Property(e => e.TYPE).HasMaxLength(50);
                entity.Property(e => e.BUSINESSNAME).HasMaxLength(200);
                entity.Property(e => e.TAXNUMBER).HasMaxLength(50);
                entity.Property(e => e.TAXOFFICE).HasMaxLength(100);
                entity.Property(e => e.AUTHNAMEANDSURNAME).HasMaxLength(100);
                entity.Property(e => e.PHONENUMBER).HasMaxLength(20);
                entity.Property(e => e.CREATEDATE).HasColumnType("datetime2(3)");
                entity.Property(e => e.LASTUPDATE).HasColumnType("datetime2(3)");

                entity.HasIndex(e => e.BUSINESSNAME).HasDatabaseName("IX_Business_BusinessName");
                entity.HasIndex(e => e.TAXNUMBER).HasDatabaseName("IX_Business_TaxNumber");
                entity.HasIndex(e => e.CREATEDATE).HasDatabaseName("IX_Business_CreateDate");
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=CRM;Trusted_Connection=true;");
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                ChangeTracker.AutoDetectChangesEnabled = true;
                ChangeTracker.DetectChanges();

                var result = await base.SaveChangesAsync(cancellationToken);

                ChangeTracker.AutoDetectChangesEnabled = false;
                return result;
            }
            catch
            {
                ChangeTracker.AutoDetectChangesEnabled = false;
                throw;
            }
        }

        public async Task<int> BulkInsertAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class
        {
            if (!entities.Any()) return 0;

            try
            {
                ChangeTracker.AutoDetectChangesEnabled = false;
                await Set<T>().AddRangeAsync(entities, cancellationToken);
                return await SaveChangesAsync(cancellationToken);
            }
            finally
            {
                ChangeTracker.AutoDetectChangesEnabled = false;
            }
        }

        public IQueryable<T> GetNoTracking<T>() where T : class
        {
            return Set<T>().AsNoTracking();
        }

        public async Task<List<T>> GetAllAsync<T>(CancellationToken cancellationToken = default) where T : class
        {
            return await Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync<T>(int id, CancellationToken cancellationToken = default) where T : class
        {
            return await Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }
    }
}