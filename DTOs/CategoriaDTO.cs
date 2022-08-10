namespace APICatalogo.DTOs
{
    public class CategoriaDTO
    {
        public int CategoriaId { get; set; }
        public string? Name { get; set; }
        public string? ImagemUrl { get; set; }

        public ICollection<CategoriaDTO>? Produtos { get; set; }
    }
}
