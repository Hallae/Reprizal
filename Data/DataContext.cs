



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

        ///<summary>
        //parameterless dataContext for unit testing
        ///</summary>
        public DataContext() : base(new DbContextOptions<DataContext>())
        {
        }
    }
  


}
