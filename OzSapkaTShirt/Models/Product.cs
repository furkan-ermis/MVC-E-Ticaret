using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OzSapkaTShirt.Models
{
    public class Product
    {
        public long Id { get; set; }

        [Column(TypeName = "nchar(30)")]
        [DisplayName("İsim")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "En fazla 30, en az 2 karakter")]
        public string Name { get; set; } = default!;

        [Column(TypeName = "nchar(200)")]
        [DisplayName("Açıklama")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "En fazla 200, en az 10 karakter")]
        public string Description { get; set; } = default!;


        public bool HatSize { get; set; }
        public bool SmallSize { get; set; }
        public bool MediumSize { get; set; }
        public bool LargeSize { get; set; }
        public bool XLargeSize { get; set; }

        [DisplayName("Fiyat")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [Range(10, 10000, ErrorMessage = "₺10 - ₺10.000 arası")]
        public float Price { get; set; }

        [Column(TypeName = "nchar(20)")]
        [DisplayName("Kumaş")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "En fazla 20, en az 3 karakter")]
        public string Fabric { get; set; } = default!;
        [Column(TypeName = "nchar(20)")]
        [DisplayName("Renk")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "En fazla 20, en az 2 karakter")]
        public string Color { get; set; } = default!;

        [NotMapped]
        [DisplayName("Resim")]
        public IFormFile? Image { get; set; }

        [Column(TypeName = "image")]
        [DisplayName("Resim")]
        public byte[]? DBImage { get; set; }

        [Column(TypeName = "image")]
        [DisplayName("Resim")]
        public byte[]? ThumbNail { get; set; }

        [Column(TypeName = "image")]
        public byte[]? DetailImg { get; set; }

        public long CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        public string? PropertyName { get; set; }


    }
}
