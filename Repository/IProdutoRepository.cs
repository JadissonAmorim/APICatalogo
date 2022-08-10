using ApiCatalogo.Pagination;
using APICatalogo.Models;

namespace APICatalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParamers);
        Task<IEnumerable<Produto>> GetProdutosPorPreco(); 
    }
}
