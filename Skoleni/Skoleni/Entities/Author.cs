using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace Skoleni.Entities
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }

    public class AuthorDbConfiguration : EntityTypeConfiguration<Author>
    {
        public AuthorDbConfiguration()
        {
            ToTable("Authors");
            HasKey(x => x.AuthorId);
            Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            Property(x => x.LastName).IsRequired().HasMaxLength(100);

            HasMany(x => x.Books).WithMany(x => x.Authors);

            HasMany(x => x.Comments)
                .WithRequired(x => x.Author)
                .HasForeignKey(x => x.AuthorId)
                .WillCascadeOnDelete(true);
        }
    }
}