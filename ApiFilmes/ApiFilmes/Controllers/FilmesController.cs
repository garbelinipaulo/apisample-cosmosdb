using Data.Redis.Interfaces;
using Data.Redis.Services;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiFilmes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmesController : ControllerBase
    {
        private readonly ILogger<FilmesController> _logger;
        private FilmesServices _filmesServices = new FilmesServices();
        public FilmesController(ILogger<FilmesController> logger)
        {
            _logger = logger;
        }
         

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var _filmes = await _filmesServices.repositorioFilmes.GetAll();
           
            if(_filmes?.Count() == 0)
            { 
                _filmesServices.repositorioFilmes.SaveCollection(new FilmesModel().RetornarListaParaTesteBanco().ToDictionary(t => t.idFilme.ToString(), t => t));
                _filmes = await _filmesServices.repositorioFilmes.GetAll();
            }

            return Ok(_filmes);
        }

        [HttpGet("Get/{Key}")] 
        public async Task<IActionResult> Get(string Key)
        {
            return Ok(await _filmesServices.repositorioFilmes.Get(Key));
        }

        [HttpPost("Post")]
        public async Task<IActionResult> Post(FilmesModel obj)
        { 
            return Ok(await _filmesServices.repositorioFilmes.Save(obj, obj.idFilme.ToString()));
        }
    }
}
