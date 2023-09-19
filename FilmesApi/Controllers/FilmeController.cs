using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{

    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)] //ANOTAÇÃO SWAGGER
    public IActionResult NewFilme([FromBody] CreateFilmeDTO filmeDto)
    {

        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetFindById), new { id = filme.Id }, filme);
        
    }

    [HttpGet]
    public IEnumerable<ReadFilmeDTO> GetAllFilmes([FromQuery]int skip = 0, [FromQuery] int take = 50)
    {
        return _mapper.Map<List<ReadFilmeDTO>>(_context.Filmes.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public IActionResult GetFindById(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();

        var filmeDto = _mapper.Map<ReadFilmeDTO>(filme);
        return Ok(filmeDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateFilme(int id, [FromBody] UpdateFilmeDTO filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault( filme => filme.Id == id);
        if(filme == null) return NotFound();
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult PatchFilme(int id, JsonPatchDocument<UpdateFilmeDTO> patch)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();

        var updateFilme = _mapper.Map<UpdateFilmeDTO>(filme);

        patch.ApplyTo(updateFilme, ModelState);

        if (!TryValidateModel(updateFilme))
        {
            return ValidationProblem(ModelState);
        }

        _mapper.Map(updateFilme, filme);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();

        _context.Remove(filme);
        _context.SaveChanges();

        return NoContent();
    }

}
