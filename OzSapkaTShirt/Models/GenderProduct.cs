using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OzSapkaTShirt.Models
{
    public class GenderProduct
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public byte GenderId { get; set; }

        [Column(TypeName = "nchar(15)")]
        public string Name { get; set; } = default!;
    }
}