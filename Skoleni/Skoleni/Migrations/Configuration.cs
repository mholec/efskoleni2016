using System;
using System.Collections.Generic;
using Skoleni.Entities;

namespace Skoleni.Migrations
{
    using System.Data.Entity.Migrations;

    public sealed class Configuration : DbMigrationsConfiguration<Entities.AppContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(AppContext context)
        {
            // users
            context.Users.AddOrUpdate(x => x.Username, new User
            {
                Username = "mholec",
                FavoriteCategories = "1, 2, 3, 4, 5, 6, 7, 8, 9"
            });

            // categories
            Category categoryThriller = new Category { Title = "Thrillery" };
            context.Categories.AddOrUpdate(x => x.Title, categoryThriller);

            // authors
            Author authorRollins = new Author { FirstName = "James", LastName = "Rollins" };
            context.Authors.AddOrUpdate(x => new {x.FirstName, x.LastName}, authorRollins);

            // books
            context.Books.AddOrUpdate(x => x.Title, new Paperback
            {
                Title = "Amazonie",
                Perex = "Kniha z amazonie",
                BookId = Guid.NewGuid(),
                CategoryId = categoryThriller.CategoryId,
                Size = PaperbackSize.A4,
                Price = new Price {BasePrice = 390, VatRate = 0.15M},
                Authors = new List<Author>
                {
                    authorRollins
                }
            });
        }
    }
}
