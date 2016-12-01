using System.Data.Entity;

namespace Skoleni.Entities
{
    public class AppContext : DbContext
    {
        public const string DefaultConnectionStringName = "AppContext";

        public AppContext() : base(DefaultConnectionStringName)
        {
            // Při volání metody SaveChanges se nejprve zkontroluje model
            Configuration.ValidateOnSaveEnabled = true;

            // Při použití SaveChanges se volá DetectChanges metoda, která prochází model a kontroluje celý graph
            Configuration.AutoDetectChangesEnabled = true;

            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
            Configuration.UseDatabaseNullSemantics = false;

            // Timeout pro každý příkaz (nikoliv connection - connectionstring)
            Database.CommandTimeout = 10;
        }

        public AppContext(string connectionName) : base (connectionName)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AuthorDbConfiguration());
            modelBuilder.Configurations.Add(new BookDbConfiguration());
            modelBuilder.Configurations.Add(new CategoryDbConfiguration());
            modelBuilder.Configurations.Add(new CommentDbConfiguration());
            modelBuilder.Configurations.Add(new PriceDbConfiguration());
            modelBuilder.Configurations.Add(new User.UserDbConfiguration());
        }
    }
}