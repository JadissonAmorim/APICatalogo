using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext contexto) : base(contexto)
        {
        }

        public async Task<IEnumerable<Categoria>> GetProdutosCategoria()
        {
            return await Get().Include(produto => produto.Produtos).ToListAsync();
        }
    }
}
