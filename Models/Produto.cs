using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models
{
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }

        [Required]
        [StringLength(80)]
        public string? Name { get; set; }

        [Required]
        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(300)]
        public string? ImagemUrl { get; set; }

        public float Quantity { get; set; }
        public DateTime DateCadastre { get; set; }
        public int CategoriaId { get; set; }
        [JsonIgnore]
        public Categoria? Categoria { get; set; }
    }
}
