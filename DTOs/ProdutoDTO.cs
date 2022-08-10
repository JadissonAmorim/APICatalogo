namespace APICatalogo.DTOs
{
    public class ProdutoDTO
    {

        public int ProdutoId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImagemUrl { get; set; }
        public int CategoriaId { get; set; }
    }
}
