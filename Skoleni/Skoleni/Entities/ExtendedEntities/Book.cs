using System;
using System.Linq.Expressions;
using LinqKit;

namespace Skoleni.Entities
{
    public partial class Book
    {
        public static Expression<Func<Book, bool>> IsFree()
        {
            return b => b.Price.BasePrice == 0;
        }

        public static Expression<Func<Book, bool>> ContainsWord(params string[] keywords)
        {
            ExpressionStarter<Book> predicate = PredicateBuilder.New<Book>(false);
            foreach (string keyword in keywords)
            {
                string temp = keyword;
                predicate = predicate.Or(p => p.Description.Contains(temp) || p.Title.Contains(temp));
            }
            return predicate;
        }
    }
}