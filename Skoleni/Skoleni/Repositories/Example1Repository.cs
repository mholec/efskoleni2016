using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using Skoleni.Controllers;
using Skoleni.Entities;

namespace Skoleni.Repositories
{
    /// <summary>
    /// Ukázky na načítání dat z databáze
    /// </summary>
    public class Example1Repository
    {
        private readonly AppContext _db;

        public Example1Repository(AppContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Práce s rozhraním IQueryable
        /// </summary>
        public List<Book> GetAllBooks(BooksFilter filter = null)
        {
            IQueryable<Book> books = _db.Books.AsQueryable();

            if (!string.IsNullOrEmpty(filter?.Keyword))
            {
                books = books.Where(b => b.Title.Contains(filter.Keyword));
            }

            if (filter?.PriceFrom != null)
            {
                books = books.Where(b => b.Price.BasePrice >= filter.PriceFrom.Value);
            }

            if (filter?.PriceTo != null)
            {
                books = books.Where(b => b.Price.BasePrice <= filter.PriceTo.Value);
            }
            
            return books.ToList();
        }

        /// <summary>
        /// Řazení záznamů a omezení počtu
        /// </summary>
        public List<Book> GetBooksWithOrder()
        {
            // seřazení dle ceny sestupně a při shodě dále dle titulku vzestupně
            List<Book> expensiveBooks = _db.Books.OrderByDescending(b => b.Price.BasePrice).ThenBy(b => b.Title).ToList();

            // 5 nejlevnějších knih
            List<Book> cheapBooks = _db.Books.OrderBy(b => b.Price).Take(5).ToList();

            return cheapBooks;
        }

        /// <summary>
        /// Vrácení jednoho záznamu, porovnání přístupů
        /// </summary>
        public Book GetOneBook()
        {
            // sql načte jeden záznam       vrátí první záznam, který vyhovuje       vrátí výjimku, když nenajde žádný záznam
            Category category = _db.Categories.OrderBy(c => c.Books.Count).First();

            // sql načte jeden záznam       vrátí první záznam, který vyhovuje       vrátí NULL, když nenajde žádný záznam
            Comment comment = _db.Comments.FirstOrDefault(b => b.Text.Contains("líbí"));

            // sql načte dva záznamy        vrátí jediný záznam, který vyhovuje      vrátí výjimku, když nenajde nic nebo více než 1 záznam
            User user = _db.Users.Single(u => u.Username == "mholec");

            // sql načte dva záznamy        vrátí jediný záznam, který vyhovuje      vrátí výjimku, když najde více než jeden záznam
            Book book = _db.Books.SingleOrDefault(u => u.BookId == Guid.Empty);

            // zkusí najít v contextu a pokud nenajde, volá FirstOrDefault()
            Book cachedBook = _db.Books.Find(Guid.Empty);

            return book;
        }

        /// <summary>
        /// Načtení množiny dat na základě pole Id
        /// Př.: Uživatel má oblíbené kategorie a dle těchto kategorií načteme všechny knihy
        /// </summary>
        public List<Book> GetBooksByFavoriteCategories(string username)
        {
            User user = _db.Users.Single(x => x.Username == username);
            int[] favoriteCategories = user.GetFavoriteCategories();

            List<Book> books = _db.Books.Where(b => favoriteCategories.Contains(b.CategoryId)).ToList();

            return books;
        }

        /// <summary>
        /// Vrácení konkrétního typu v rámci dědičnosti
        /// Př.: Chceme pouze papírové knihy
        /// </summary>
        public List<Paperback> GetAllPaperbacks()
        {
            // Nepoužívat, selekce typu probíhá nad materializovanými daty
            // var paperbacks = GetAllBooks().OfType<Paperback>().ToList();

            List<Paperback> paperbacks = _db.Books.OfType<Paperback>().ToList();

            return paperbacks;
        }

        /// <summary>
        /// Seskupování dat (GroupBy)
        /// Př.: Chceme vždy ID kategorie a všechny knihy k ní
        /// </summary>
        public Dictionary<int, List<Book>> GetAllBooksGroupedByCategory()
        {
            Dictionary<int, List<Book>> books = _db.Books.GroupBy(b => b.CategoryId).Select(group => new
            {
                CategoryId = group.Key,
                Books = group.ToList()
            }).ToDictionary(x=> x.CategoryId, x=> x.Books);

            return books;
        }

        /// <summary>
        /// Distinct
        /// Př.: Vrátit všechny jména autorů ale unikátně
        /// </summary>
        public List<string> GetAllAuthorsNames()
        {
            List<string> authors = _db.Authors.Select(x=> x.FirstName + " " + x.LastName).Distinct().ToList();

            return authors;
        }

        /// <summary>
        /// Union
        /// Př.: Spojení dvou zdrojů
        /// </summary>
        public List<Book> GetBooksFromTwoSources()
        {
            var all = _db.Books.OrderBy(b => b.BookId).Take(1).Union(_db.Books.OrderBy(b => b.BookId).Skip(1).Take(1)).ToList();

            return all;
        }

        /// <summary>
        /// Intersect (průnik)
        /// Př.: Chci knihy od Rollinse, které jsou v mých oblíbených žánrech
        /// </summary>
        public List<Book> GetBooksFromRollinsInMyFavoriteCategories(string username)
        {
            var favoriteCategories = _db.Users.FirstOrDefault(x => x.Username == username).GetFavoriteCategories();

            // Funkční ale neoptimální (spíše pro ukázku intersect)
            var all = _db.Books.Where(b => b.Authors.Any(a => a.LastName == "Rollins"))
                .Intersect(_db.Books.Where(b => favoriteCategories.Contains(b.CategoryId))).ToList();

            // Doporučená verze (jedna podmínka)
            var faster =
                _db.Books.Where(
                    b => b.Authors.Any(a => a.LastName == "Rollins") && favoriteCategories.Contains(b.CategoryId)).ToList();

            return faster;
        }


        /// <summary>
        /// Př.: Chci knihy od Rollinse, které nejsou Thriller
        /// </summary>
        public List<Book> GetBooksFromRollinsNotInThrillers()
        {
            // Funkční ale neoptimální (spíše pro ukázku except)
            var all = _db.Books.Where(b => b.Authors.Any(a => a.LastName == "Rollins"))
                .Except(_db.Books.Where(b => b.Title != "Thrillery")).ToList();

            // Doporučená verze (jedna podmínka)
            var faster =
                _db.Books.Where(
                    b => b.Authors.Any(a => a.LastName == "Rollins") && b.Title != "Thrillery").ToList();

            // Výkonnostně nejlepší řešení
            var thrillerId = _db.Categories.FirstOrDefault(x => x.Title == "Thrillery").CategoryId;
            var authorId = _db.Authors.FirstOrDefault(x => x.LastName == "Rollins").AuthorId;

            var books = _db.Books.Where(b => b.Authors.Any(a => a.AuthorId == authorId) && b.CategoryId != thrillerId).ToList();

            return books;
        }

        /// <summary>
        /// Subdotazy
        /// Př.: Vrátit knihy, které jsou v kategorii, jejíž titulek začíná slovem "Nové" 
        /// </summary>
        public List<Book> GetNewBooks()
        {
            List<Book> books = _db.Books.Where(b => _db.Categories.Any(c => c.CategoryId == b.CategoryId && c.Title.StartsWith("Nové"))).ToList();

            return books;
        }

        /// <summary>
        /// Načtení do dictionary
        /// Př.: Chci sestavit slovník pro dropdown
        /// </summary>
        public Dictionary<int, string> GetCategories()
        {
            // Nepoužívat, ToDictionary si veškerá data materializuje
            // return _db.Categories.ToDictionary(x => x.CategoryId, x => x.Title);

            return _db.Categories.Select(c => new {c.CategoryId, c.Title}).ToDictionary(x => x.CategoryId, x => x.Title);
        }

        /// <summary>
        /// Načtení závislé kolekce formou eager loadingu
        /// Př.: Chci načíst kategorie se všemi knihami a jejich autory
        /// </summary>
        public List<Category> GetCategoriesWithBooksAndAuthors()
        {
            List<Category> categories = _db.Categories.Include(c => c.Books.Select(b => b.Authors)).ToList();

            return categories;
        }

        /// <summary>
        /// Načtení bázové kolekce (ukázka LazyLoadingu)
        /// Př.: Chci načíst pouze kategorie, protože knihy budu potřebovat načítat jen v některých případech
        /// </summary>
        public List<Category> GetPlainCategories()
        {
            List<Category> categories = _db.Categories.ToList();

            // ukázka lazy loadingu
            ICollection<Book> books = categories.FirstOrDefault().Books; // dodatečný dotaz

            // problém by bylo použití v cyklech
            foreach (var category in categories)
            {
                List<Book> book = category.Books.ToList(); // dodatečný dotaz a každým průchodem cyklem
            }

            return categories;
        }

        /// <summary>
        /// Explicit loading
        /// Př.: Chci načíst kategorii a k ní specifické knihy
        /// </summary>
        public Category GetCategoriesWithBooks(int categoryId)
        {
            Category category = _db.Categories.FirstOrDefault(x => x.CategoryId == categoryId);

            _db.Entry(category).Reference(c => c.ParentCategory).Load(); // pokud je ParentCategory null, nic ani nebude načítat
            _db.Entry(category).Collection(c => c.Books).Query().Where(x=> x.Title == "Amazonie").Load(); // podmíněný explicit loading
            _db.Entry(category).Collection(c => c.Books).Load(); // kompletní lazy loading

            // !!! Je třeba myslet na to, že pracuji s nekompletní kolekcí během dalšího zpracování

            return category;
        }

        /// <summary>
        /// Asynchronní načtení kolekce dat
        /// Př.: Chci načíst asynchronně všechny knihy
        /// </summary>
        public async Task<List<Book>> GetBooksAsync()
        {
            List<Book> books = await _db.Books.ToListAsync();

            return books;
        }

        /// <summary>
        /// Asynchronní načtení záznamu s podmnožinou
        /// Př.: Chci načíst asynchronně kategorii se všemi knihami
        /// </summary>
        public async Task<Category> GetCategoryWithBooksAsync(int categoryId)
        {
            Category category = await _db.Categories.Include(c => c.Books).FirstOrDefaultAsync(c => c.CategoryId == categoryId);

            return category;
        }
    }
}