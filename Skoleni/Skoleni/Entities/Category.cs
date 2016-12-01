using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Skoleni.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public int? ParentCategoryId { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Book> Books { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> Children { get; set; }
    }

    public class CategoryDbConfiguration : EntityTypeConfiguration<Category>
    {
        public CategoryDbConfiguration()
        {
            ToTable("Categories");

            HasKey(x => x.CategoryId);
            Property(x => x.CategoryId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Title).IsRequired().HasMaxLength(100);

            HasMany(x => x.Books)
                .WithRequired(x => x.Category)
                .HasForeignKey(x => x.CategoryId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.ParentCategory)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentCategoryId)
                .WillCascadeOnDelete(false);
        }
    }
}