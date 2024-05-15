using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using backend.Controllers;
using backend.Models;

namespace PageTurnerTests.Controllers
{
    public class TrocaControllerTests
    {
        // [Test]
        // public async Task CheckMatch_ReturnsOkWithMatchingEstantes()
        // {
        //     using (var context = new PageTurnerContext(_options))
        //     {
        //         // Arrange
        //         var controller = new TrocaController(context);

        //         // Act
        //         var actionResult = await controller.CheckMatch(1);

        //         // Assert
        //         Assert.That(actionResult, Is.InstanceOf<OkObjectResult>());
        //     }
        // }

        // [Test]
        // public async Task CheckMatch_ReturnsOkWithMatchingEstantes()
        // {
        //     using (var context = new PageTurnerContext(_options))
        //     {
        //         // Arrange
        //         var controller = new TrocaController(context);

        //         // Act
        //         var actionResult = await controller.CheckMatch(1);

        //         // Assert
        //         var okResult = actionResult as OkObjectResult;
        //         Assert.That(okResult, Is.Not.Null);

        //         var matchingEstantes = okResult.Value as List<Estante>;
        //         Assert.That(matchingEstantes, Is.Not.Null);

        //         if (matchingEstantes.Count > 0)
        //         {
        //             var match = matchingEstantes.First();
        //             Assert.That(match, Is.Not.Null);
        //             Assert.That(match.tipoEstante.descricaoTipoEstante, Is.EqualTo("Estante Troca"));

        //             var livro = match.livro;
        //             Assert.That(livro, Is.Not.Null);
        //             Console.WriteLine($"Livro com match: {livro.tituloLivro}");

        //             var utilizador = match.utilizador;
        //             Assert.That(utilizador, Is.Not.Null);
        //             Console.WriteLine($"Usuário da outra estante: {utilizador.nome}");
        //         }
        //         else
        //         {
        //             Console.WriteLine("Nenhum match encontrado.");
        //         }
        //     }
        // }

        [Test]
        public async Task ProcuraMatch_ReturnsMatchingComEstanteTroca()
        {
            var options = new DbContextOptionsBuilder<PageTurnerContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new PageTurnerContext(options))
            {
                var estadoContaTest = MockData.CreateEstadoConta(1, "Ativo");
                var paisTest = MockData.CreatePais(1, "Portugal");
                var cidadeTest = MockData.CreateCidade(1, "Braga", paisTest.paisId);
                var utilizadorTest1 = MockData.CreateUtilizador(1, "UtilizadorTest", "Teste", DateTime.Now, "usernametest1", "passwordtest1", "emailtestunit99@ipca.pt", "foto1.jpg", DateTime.Now, DateTime.Now, true, true, true, 1, estadoContaTest, cidadeTest.cidadeId);
                var utilizadorTest2 = MockData.CreateUtilizador(2, "UtilizadorTest2", "Teste2", DateTime.Now, "usernametest2", "passwordtest2", "emailtestunit98@ipca.pt", "foto2.jpg", DateTime.Now, DateTime.Now, true, true, true, 1, estadoContaTest, cidadeTest.cidadeId);
                var autorLivroTest1 = MockData.CreateAutorLivro(1, "Autor Teste 1");
                var autorLivroTest2 = MockData.CreateAutorLivro(2, "Autor Teste 2");
                var autorLivroTest3 = MockData.CreateAutorLivro(3, "Autor Teste 3");
                var generoLivroTest1 = MockData.CreateGeneroLivro(1, "Género Teste 1");
                var generoLivroTest2 = MockData.CreateGeneroLivro(2, "Género Teste 2");
                var generoLivroTest3 = MockData.CreateGeneroLivro(3, "Género Teste 3");
                var livroTest1 = MockData.CreateLivro(1, "Livro Teste 1", 2020, "Português", autorLivroTest1, generoLivroTest1);
                var livroTest2 = MockData.CreateLivro(2, "Livro Teste 2", 2018, "Inglês", autorLivroTest2, generoLivroTest2);
                var livroTest3 = MockData.CreateLivro(3, "Livro Teste 3", 2019, "Francês", autorLivroTest3, generoLivroTest3);
                var tipoEstanteTest1 = MockData.CreateTipoEstante(1, "Estante Troca");
                var tipoEstanteTest2 = MockData.CreateTipoEstante(2, "Estante Desejos");
                var estanteTest1 = MockData.CreateEstante(1, utilizadorTest1, livroTest1, tipoEstanteTest1);
                var estanteTest2 = MockData.CreateEstante(2, utilizadorTest1, livroTest2, tipoEstanteTest2);
                var estanteTest3 = MockData.CreateEstante(3, utilizadorTest2, livroTest2, tipoEstanteTest1);
                var estanteTest4 = MockData.CreateEstante(4, utilizadorTest2, livroTest1, tipoEstanteTest2);
                var estanteTest5 = MockData.CreateEstante(5, utilizadorTest2, livroTest3, tipoEstanteTest2);


                context.AddRange(utilizadorTest1, utilizadorTest2, autorLivroTest1, autorLivroTest2, generoLivroTest1, generoLivroTest2, livroTest1, livroTest2, tipoEstanteTest1, tipoEstanteTest2, estanteTest1, estanteTest2, estanteTest3, estanteTest4);
                context.SaveChanges();
            }

            using (var context = new PageTurnerContext(options))
            {
                // Arrange
                var trocaService = new Troca();
                var minhaEstanteId = 1; // Estante de troca do utilizador 1

                // Act
                var actionResult = await trocaService.ProcuraMatch(minhaEstanteId, context);

                // Assert
                Assert.That(actionResult, Is.InstanceOf<ActionResult<MatchDTO>>());

                var matchDTO = actionResult.Value as MatchDTO;
                Assert.That(matchDTO, Is.Not.Null, "matchDTO não deve ser null");

                Assert.That(matchDTO.ListEstantesDosUsersQuerLivroETemLivrosQueMeInteressam, Is.Not.Null, "Lista de estantes com livros que eu quero não deve ser nula");
                Assert.That(matchDTO.ListEstantesDosUsersQuerLivroETemLivrosQueMeInteressam.Count, Is.GreaterThan(0), "Nenhuma estante encontrada com livros que eu quero");

                foreach (var estante in matchDTO.ListEstantesDosUsersQuerLivroETemLivrosQueMeInteressam)
                {
                    Assert.That(estante, Is.Not.Null);
                    Assert.That(estante.tipoEstante.descricaoTipoEstante, Is.EqualTo("Estante Troca"));

                    var livro = estante.livro;
                    var utilizador = estante.utilizador;

                    Assert.That(livro, Is.Not.Null);
                    Assert.That(utilizador, Is.Not.Null);

                    Console.WriteLine($"Livro com match: {livro.tituloLivro}");
                    Console.WriteLine($"Tipo de estante: {estante.tipoEstante.descricaoTipoEstante}");
                    Console.WriteLine($"Usuário da outra estante: {utilizador.nome}");
                }
            }

            // Limpa a memória após o término do primeiro teste
            using (var context = new PageTurnerContext(options))
            {
                context.Database.EnsureDeleted();
            }
        }

        [Test]
        public async Task ProcuraMatch_ReturnsMatchingComEstanteDesejos()
        {
            var options = new DbContextOptionsBuilder<PageTurnerContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new PageTurnerContext(options))
            {
                var estadoContaTest = MockData.CreateEstadoConta(1, "Ativo");
                var paisTest = MockData.CreatePais(1, "Portugal");
                var cidadeTest = MockData.CreateCidade(1, "Braga", paisTest.paisId);
                var utilizadorTest1 = MockData.CreateUtilizador(1, "UtilizadorTest", "Teste", DateTime.Now, "usernametest1", "passwordtest1", "emailtestunit99@ipca.pt", "foto1.jpg", DateTime.Now, DateTime.Now, true, true, true, 1, estadoContaTest, cidadeTest.cidadeId);
                var utilizadorTest2 = MockData.CreateUtilizador(2, "UtilizadorTest2", "Teste2", DateTime.Now, "usernametest2", "passwordtest2", "emailtestunit98@ipca.pt", "foto2.jpg", DateTime.Now, DateTime.Now, true, true, true, 1, estadoContaTest, cidadeTest.cidadeId);
                var autorLivroTest1 = MockData.CreateAutorLivro(1, "Autor Teste 1");
                var autorLivroTest2 = MockData.CreateAutorLivro(2, "Autor Teste 2");
                var autorLivroTest3 = MockData.CreateAutorLivro(3, "Autor Teste 3");
                var generoLivroTest1 = MockData.CreateGeneroLivro(1, "Género Teste 1");
                var generoLivroTest2 = MockData.CreateGeneroLivro(2, "Género Teste 2");
                var generoLivroTest3 = MockData.CreateGeneroLivro(3, "Género Teste 3");
                var livroTest1 = MockData.CreateLivro(1, "Livro Teste 1", 2020, "Português", autorLivroTest1, generoLivroTest1);
                var livroTest2 = MockData.CreateLivro(2, "Livro Teste 2", 2018, "Inglês", autorLivroTest2, generoLivroTest2);
                var livroTest3 = MockData.CreateLivro(3, "Livro Teste 3", 2019, "Francês", autorLivroTest3, generoLivroTest3);
                var tipoEstanteTest1 = MockData.CreateTipoEstante(1, "Estante Troca");
                var tipoEstanteTest2 = MockData.CreateTipoEstante(2, "Estante Desejos");
                var estanteTest1 = MockData.CreateEstante(1, utilizadorTest1, livroTest1, tipoEstanteTest1);
                var estanteTest2 = MockData.CreateEstante(2, utilizadorTest1, livroTest2, tipoEstanteTest2);
                var estanteTest3 = MockData.CreateEstante(3, utilizadorTest2, livroTest2, tipoEstanteTest1);
                var estanteTest4 = MockData.CreateEstante(4, utilizadorTest2, livroTest1, tipoEstanteTest2);
                var estanteTest5 = MockData.CreateEstante(5, utilizadorTest2, livroTest3, tipoEstanteTest2);


                context.AddRange(utilizadorTest1, utilizadorTest2, autorLivroTest1, autorLivroTest2, generoLivroTest1, generoLivroTest2, livroTest1, livroTest2, tipoEstanteTest1, tipoEstanteTest2, estanteTest1, estanteTest2, estanteTest3, estanteTest4);
                context.SaveChanges();
            }

            using (var context = new PageTurnerContext(options))
            {
                // Arrange
                var trocaService = new Troca();
                var minhaEstanteId = 4; // Estante de desejos do utilizador 2

                // Act
                var actionResult = await trocaService.ProcuraMatch(minhaEstanteId, context);

                // Assert
                Assert.That(actionResult, Is.InstanceOf<ActionResult<MatchDTO>>());

                var matchDTO = actionResult.Value as MatchDTO;
                Assert.That(matchDTO, Is.Not.Null, "matchDTO não deve ser null");

                Assert.That(matchDTO.ListEstantesDosUsersQuerLivroETemLivrosQueMeInteressam, Is.Not.Null, "Lista de estantes com livros que eu quero não deve ser nula");
                Assert.That(matchDTO.ListEstantesDosUsersQuerLivroETemLivrosQueMeInteressam.Count, Is.GreaterThan(0), "Nenhuma estante encontrada com livros que eu quero");

                foreach (var estante in matchDTO.ListEstantesDosUsersQuerLivroETemLivrosQueMeInteressam)
                {
                    Assert.That(estante, Is.Not.Null);
                    Assert.That(estante.tipoEstante.descricaoTipoEstante, Is.EqualTo("Estante Desejos"));

                    var livro = estante.livro;
                    var utilizador = estante.utilizador;

                    Assert.That(livro, Is.Not.Null);
                    Assert.That(utilizador, Is.Not.Null);

                    Console.WriteLine($"Livro com match: {livro.tituloLivro}");
                    Console.WriteLine($"Tipo de estante: {estante.tipoEstante.descricaoTipoEstante}");
                    Console.WriteLine($"Usuário da outra estante: {utilizador.nome}");
                }
            }

            // Limpa a memória após o término do primeiro teste
            using (var context = new PageTurnerContext(options))
            {
                context.Database.EnsureDeleted();
            }
        }
    }
}