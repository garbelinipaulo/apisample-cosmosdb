# apisample-cosmosdb



O projeto é uma simples API em .net core com algumas librarys para visualização dos patterns e implementação de alguns componentes.

Existem duas librarys de data em que se utiliza Cosmos em uma e Redis na outra.


O cosmosDB foi implementado de forma básica afim de mostrar a rotina de inicialização.
O Redis é o utilizado pela api no momento pois não necessita configurar nada apenas rodar no docker.
O rabbitMQ também é utilizado e foi rodado com docker.

A ideia da api é apenas de consultar uma lista de filmes de forma genérica, e caso você busque algum filme com um id específico, ele vai utilizar o Rabbit para gerar um queue e este ser consumido por um serviço de consumer rodando em background na api.


