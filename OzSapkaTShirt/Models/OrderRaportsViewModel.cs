using System.ComponentModel;

namespace OzSapkaTShirt.Models
{
    public class OrderRaportsViewModel
    {
        [DisplayName("Kullanıcı")]
        public string UserId { get; set; }
        [DisplayName("Ürün")]

        public long ProductId { get; set; }
        [DisplayName("Başlangıç Tarihi")]
        public DateTime start { get; set; }
        [DisplayName("Bitiş Tarihi")]
        public DateTime end { get; set; }
    }
}
