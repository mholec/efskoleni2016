using System.Data.Entity;
using Skoleni.Entities;
using Skoleni.Migrations;

namespace Skoleni.Initializers
{
    /// <summary>
    /// Vytvoří databázi jen pokud neexistuje, jinak se řídí migracemi
    /// </summary>
    public class CreateIfNotExistsInit : CreateDatabaseIfNotExists<AppContext>
    {
        protected override void Seed(AppContext context)
        {
            base.Seed(context);
        }
    }

    /// <summary>
    /// Smaže při každé kompilaci databázi a vytvoří ji znovu
    /// </summary>
    public class DropCreateAlwaysInit : DropCreateDatabaseAlways<AppContext>
    {
        protected override void Seed(AppContext context)
        {
            base.Seed(context);
        }
    }

    /// <summary>
    /// Smaže databázi, pokud se cokoliv v modelu změnilo
    /// </summary>
    public class DropIfChangesInit : DropCreateDatabaseIfModelChanges<AppContext>
    {
        protected override void Seed(AppContext context)
        {
            base.Seed(context);
        }
    }

    /// <summary>
    /// Aplikuje migrace, pokud existují a aktualizuje tak databázi
    /// </summary>
    public class UpdateToLatestInit : MigrateDatabaseToLatestVersion<AppContext, Configuration>
    {
    }
}