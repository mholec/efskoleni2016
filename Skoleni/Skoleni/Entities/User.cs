using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;

namespace Skoleni.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FavoriteCategories { get; set; }

        public int[] GetFavoriteCategories()
        {
            if (string.IsNullOrEmpty(FavoriteCategories))
                return new int[0];

            return FavoriteCategories.Split(',').Select(x => int.Parse(x.Trim())).ToArray();
        }

        public class UserDbConfiguration : EntityTypeConfiguration<User>
        {
            public UserDbConfiguration()
            {
                ToTable("Users");

                HasKey(x => x.UserId);
                Property(x => x.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

                Property(x => x.Username).IsRequired().HasMaxLength(100);
            }
        }
    }
}