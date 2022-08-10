using ApiCatalogo.Pagination;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext contexto) : base(contexto)
        {
        }

        public async Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParamers)
        {
            return await PagedList<Produto>.ToPagedList(Get().OrderBy(on => on.ProdutoId),
            produtosParamers.PageNumber, produtosParamers.PageSize);
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorPreco()
        {
            return await Get().OrderBy(produto => produto.Price).ToListAsync();
        }
    }
}
