

namespace myApi.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Application> Application { get; set; }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>()
                .HasKey(n => n.id);
        }
    }

    public class ActivityDbContext : DbContext
    {

        public ActivityDbContext(DbContextOptions<ActivityDbContext> options) : base(options) { }
        public DbSet<Activity> Activities { get; set; }



    }
}
