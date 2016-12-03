using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Skoleni.Entities
{
    public partial class Book
    {
        public Guid BookId { get; set; }
        public int CategoryId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public Price Price { get; set; }
        public string Perex { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Author> Authors { get; set; }
    }

    public class BookDbConfiguration : EntityTypeConfiguration<Book>
    {
        public BookDbConfiguration()
        {
            ToTable("Books");
            HasKey(x => x.BookId);
            Property(x => x.BookId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).IsRequired().HasMaxLength(200);
            // Property(x => x.Price.BasePrice).HasColumnName("Price");
            // Property(x => x.Price.BasePrice).HasColumnName("VatRate");

            HasMany(x => x.Authors).WithMany(x => x.Books).Map(x => x.MapLeftKey("BookId").MapRightKey("AuthorId"));
            HasRequired(x => x.Category)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.CategoryId)
                .WillCascadeOnDelete(false);
        }
    }
}