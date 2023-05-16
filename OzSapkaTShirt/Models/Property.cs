using System.ComponentModel.DataAnnotations.Schema;

namespace OzSapkaTShirt.Models
{
    public class Property
    {
        public long Id { get; set; }
        public string PropertyName { get; set; }
        public long CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

    }
}
