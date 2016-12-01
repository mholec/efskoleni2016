using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Skoleni.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int AuthorId { get; set; }
        public string Text { get; set; }

        public virtual Author Author { get; set; }
    }

    public class CommentDbConfiguration : EntityTypeConfiguration<Comment>
    {
        public CommentDbConfiguration()
        {
            ToTable("Comments");

            HasKey(x => x.CommentId);
            Property(x => x.CommentId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Text).IsRequired().HasMaxLength(2000);

            HasRequired(x => x.Author)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.AuthorId)
                .WillCascadeOnDelete(true);
        }
    }
}