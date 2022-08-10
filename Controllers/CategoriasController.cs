using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace APICatalogo.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
            private readonly IUnitOfWork _context;
            private readonly IMapper _mapper;
            public CategoriasController(IUnitOfWork contexto, IMapper mapper)
            {
                _context = contexto;
                _mapper = mapper;
            }

            [HttpGet("produtos")]
            public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
            {
            var categorias = await _context.CategoriaRepository.GetProdutosCategoria();
                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
                return categoriasDto;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
            {
                var categorias = await _context.CategoriaRepository.Get().ToListAsync();
                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
                return categoriasDto;
            }

            [HttpGet("{id}", Name = "ObterCategoria")]
            public async Task<ActionResult<CategoriaDTO>> Get(int? id)
            {
                var categoria = await _context.CategoriaRepository.GetById(p => p.CategoriaId == id);

                if (categoria == null)
                {
                    return NotFound();
                }
                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
                return categoriaDto;
            }

            [HttpPost]
            public async Task<ActionResult> Post([FromBody] CategoriaDTO categoriaDto)
            {
                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _context.CategoriaRepository.Add(categoria);
                await _context.Commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoriaDTO);
            }

            [HttpPut("{id}")]
            public async Task<ActionResult> Put(int id, [FromBody] CategoriaDTO categoriaDto)
            {
                if (id != categoriaDto.CategoriaId)
                {
                    return BadRequest();
                }

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _context.CategoriaRepository.Update(categoria);
                await _context.Commit();
                return Ok();
            }

            [HttpDelete("{id}")]
            public async Task<ActionResult<CategoriaDTO>> Delete(int? id)
            {
                var categoria = await _context.CategoriaRepository.GetById(p => p.CategoriaId == id);

                if (categoria == null)
                {
                    return NotFound();
                }
                _context.CategoriaRepository.Delete(categoria);
                await _context.Commit();

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return categoriaDto;
            }
        }
    
}
