using System.Data.Entity.ModelConfiguration;

namespace Skoleni.Entities
{
    public class Price
    {
        public decimal BasePrice { get; set; }
        public decimal VatRate { get; set; }
        public decimal PriceWithVat => BasePrice + BasePrice * VatRate;
        public decimal Vat => PriceWithVat - BasePrice;
    }

    public class PriceDbConfiguration : ComplexTypeConfiguration<Price>
    {
        public PriceDbConfiguration()
        {
            Property(x => x.BasePrice).IsRequired().HasColumnName("Price");
            Property(x => x.VatRate).IsRequired().HasColumnName("VatRate");
        }
    }
}