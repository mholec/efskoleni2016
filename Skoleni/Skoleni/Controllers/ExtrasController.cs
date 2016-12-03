using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Text;
using System.Web.Mvc;
using Skoleni.Entities;
using Skoleni.Extras;

namespace Skoleni.Controllers
{
    public class ExtrasController : Controller
    {
        private readonly AppContext _db;

        public ExtrasController(AppContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Aktualizace object graph
        /// Př.: Chci vložit kategorii i s nadřazenou kategorií a knihami
        /// </summary>
        public ActionResult UpdateObjectGraph()
        {
            // vytvoření kompletního stromu
            Category category = new Category()
            {
                Title = "Povídky",
                ParentCategory = new Category()
                {
                    Title = "Zlevněné knihy"
                },
                Books = new List<Book>()
                {
                    new Paperback() {Title = "Povídka o černém notebooku", Description = "Nová knížka", Size = PaperbackSize.A4, Price = new Price() {BasePrice = 0, VatRate = 0} },
                    new Paperback() {Title = "Humoreska", Description = "Trochu jiná kniha", Size = PaperbackSize.A4, Price = new Price() {BasePrice = 50, VatRate = 0.15M } }
                }
            };
            _db.Categories.Add(category);
            _db.SaveChanges();

            return Content("OK");
        }

        //////////////////////////////////////////////////////////
        ///                                                    ///
        ///   C H A N G E   T R A C K I N G                    ///
        ///                                                    ///
        //////////////////////////////////////////////////////////

        /// <summary>
        /// Change Tracker API
        /// Př.: Chci ci auditovat změny na nějaké kolekci, třeba kategorie
        /// </summary>
        public ActionResult ChangeTrackingForAudit()
        {
            Category category = _db.Categories.Include(x => x.Books).FirstOrDefault(b => b.CategoryId == 4);
            category.Title = "Naučte se C# za 21 hodin";
            category.Books.FirstOrDefault().Title = "Super kniha";
            category.ParentCategory = null;

            List<AuditItem> result = new Auditor(_db.ChangeTracker).AuditChanges(true);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ChangeTracker API
        /// Př.: Chci knihy read only ale jednu později upravit
        /// </summary>
        public ActionResult ChangeTrackingForReadOnlyQueries()
        {
            List<Book> books = _db.Books.AsNoTracking().ToList();

            Book book = books.FirstOrDefault();
            book.Title = book.Title + " TRACKED X";
            _db.SaveChanges();

            // musím změnit stav na Modified
            _db.Entry(book).State = EntityState.Modified;
            _db.SaveChanges();

            return Content("OK");
        }

        /// <summary>
        /// ChangeTracker API
        /// Př.: Mám globálně vypnutý change tracking ale chci jej na chvíli zapnout
        /// </summary>
        public ActionResult ChangeTrackingGlobalSettings()
        {
            _db.Configuration.AutoDetectChangesEnabled = false;

            List<Book> books = _db.Books.ToList();

            Book book = books.FirstOrDefault();
            book.Title = book.Title + " TRACKED X";
            _db.SaveChanges();

            _db.ChangeTracker.DetectChanges();
            _db.SaveChanges();

            return Content("OK");
        }

        //////////////////////////////////////////////////////////
        ///                                                    ///
        ///   V A L I D A T I O N     A P I                    ///
        ///                                                    ///
        //////////////////////////////////////////////////////////

        /// <summary>
        /// Validation API
        /// Př.: Chci vědět jaké konkrétní chyby vznikly při zakládání objektu
        /// </summary>
        public ActionResult Validation()
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                _db.Books.Add(new Paperback() {Title = null, Price = new Price()});
                _db.Books.Add(new Paperback() {Title = "Kniha 2", Price = new Price()});

                _db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult result in ex.EntityValidationErrors)
                {
                    foreach (DbValidationError error in result.ValidationErrors)
                    {
                        sb.AppendLine($"Error Property Name {error.PropertyName} : Error Message: {error.ErrorMessage}");
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                sb.AppendLine(ex.Message);
            }
            catch (Exception ex)
            {
                // not handled
            }

            return Content(sb.ToString());
        }

        //////////////////////////////////////////////////////////
        ///                                                    ///
        ///   A D O   N E T   Q U E R I E S                    ///
        ///                                                    ///
        //////////////////////////////////////////////////////////

        /// <summary>
        /// ADO.NET (underlayer)
        /// Př.: Chci si napsat vlastní SQL dotaz a nepřijít o ChangeTracking
        /// </summary>
        public ActionResult AdoNetExamples()
        {
            DbSqlQuery<Category> query = _db.Categories.SqlQuery(
                "SELECT * FROM Categories WHERE CategoryId > @categoryId",
                new SqlParameter("categoryId", 2));

            List<Category> categories = query.ToList();
            categories.FirstOrDefault().Title = "Změna";
            _db.SaveChanges();

            return Content("OK");
        }
    }
}