using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class FilmesModel
    {
        public Guid idFilme { get; private set; }

        /// <summary>
        /// Título do filme
        /// </summary>
        public string xTitulo { get; set; }

        /// <summary>
        /// Média de avaliações
        /// </summary>
        public decimal vMedia { get; set; }

        /// <summary>
        /// Categoria do filme
        /// </summary>
        public string xCategoria { get; set; }

        public FilmesModel()
        {
            idFilme = new Guid();
        }

        /// <summary>
        /// MockData
        /// </summary>
        /// <returns></returns>
        public List<FilmesModel> RetornarListaParaTesteBanco()
        {
            return new List<FilmesModel>
            {
                new FilmesModel
                {
                    idFilme = Guid.NewGuid(),
                    vMedia = 4.5M,
                    xCategoria = "Ficção",
                    xTitulo = "Aliens"
                },
                new FilmesModel
                {
                    idFilme = Guid.NewGuid(),
                    vMedia = 5.0M,
                    xCategoria = "Ação",
                    xTitulo = "Rambo"
                },
                new FilmesModel
                {
                    idFilme = Guid.NewGuid(),
                    vMedia = 4.0M,
                    xCategoria = "Aventura",
                    xTitulo = "Goonies"
                },
                new FilmesModel
                {
                    idFilme = Guid.NewGuid(),
                    vMedia = 4.5M,
                    xCategoria = "Terror",
                    xTitulo = "O Chamado"
                }
            };
        }
    }
}
