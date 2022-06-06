using Data.Redis.Interfaces;
using Data.Redis.Services;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client; 
using System.Text;

namespace ApiFilmes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmesController : ControllerBase
    {
#if Release
        const string configHost = "connRemota";
#else
        const string configHost = "172.17.0.4"; //docker ip local
#endif 


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

            return Accepted(_filmes);
        }

        [HttpGet("Get/{Key}")] 
        public async Task<IActionResult> Get(string Key)
        {
            var _retornoBusca = await _filmesServices.repositorioFilmes.Get(Key); 


            if(_retornoBusca != null)
            { 
                //gerando info de busca de filmes para ser utilizado como consumo no método GetKeysConsumidas
                await GerarQueue(nameof(FilmesController), $"{DateTime.Now} -> Key {_retornoBusca.idFilme} - Filme {_retornoBusca.xTitulo}");
            }

            return Accepted(_retornoBusca);
        }
         
        [HttpGet("GetFilmesBuscados")]
        public async Task<IActionResult> GetFilmesBuscados()
        {
            return Accepted(await _filmesServices.repositorioFilmes.BuscaLogsGravados());
        }


        [HttpPost("Post")]
        public async Task<IActionResult> Post(FilmesModel obj)
        { 
            return Accepted(await _filmesServices.repositorioFilmes.Save(obj, obj.idFilme.ToString()));
        }

        [HttpDelete("Delete/{Key}")]
        public async Task<IActionResult> Delete(string Key)
        {
            return Accepted(await _filmesServices.repositorioFilmes.Excluir(Key));
        }


        [HttpDelete("LimparBanco")]
        public async Task<IActionResult> LimparBanco()
        {
            return Accepted(await _filmesServices.repositorioFilmes.LimparBanco());
        }


        async Task GerarQueue(string xTipo, string Key)
        {
            var factory = new ConnectionFactory() { HostName = configHost };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: xTipo,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(Key);

                channel.BasicPublish(exchange: "",
                                     routingKey: xTipo,
                                     basicProperties: null,
                                     body: body);



                await Task.CompletedTask;
            }

        }
    }
}
