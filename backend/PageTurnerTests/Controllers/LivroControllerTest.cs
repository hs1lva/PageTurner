using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Controllers;
using backend.Models;

namespace PageTurnerTests.Controllers
{
    public class LivroControllerTest
    {
        /// <summary>
        /// Teste para sugerir livros
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task SugerirLivros_DeveRetornarListaDeLivrosDTO() // Este teste é para funcionar o return lista de livros
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PageTurnerContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new PageTurnerContext(options))
            {
                var autorLivroTest = MockData.CreateAutorLivro(1, "Autor Teste");
                var autorLivroTest2 = MockData.CreateAutorLivro(2, "Autor Teste 2");
                var generoLivroTest = MockData.CreateGeneroLivro(1, "Género Teste");
                var generoLivroTest2 = MockData.CreateGeneroLivro(2, "Género Teste 2");
                var tipoUtilizadorTest = MockData.CreateTipoUtilizador(1, "Administrador");
                var estadoContaTest = MockData.CreateEstadoConta(1, "Ativo");
                var paisTest = MockData.CreatePais(1, "Portugal");
                var cidadeTest = MockData.CreateCidade(1, "Braga", paisTest.paisId);
                var utilizadorTest = MockData.CreateUtilizador(1, "UtilizadorTest", "Teste", DateTime.Now, "usernametest", "passwordtest", "emailtestunit@ipca.pt", "foto.jpg", DateTime.Now, DateTime.Now, true, true, true, 1, estadoContaTest, cidadeTest.cidadeId);
                var livro1 = MockData.CreateLivro(1, "Livro Teste 1", 2020, "Português", autorLivroTest, generoLivroTest);
                var livro2 = MockData.CreateLivro(2, "Livro Teste 2", 2018, "Inglês", autorLivroTest, generoLivroTest);
                var tipoEstanteTest = MockData.CreateTipoEstante(1, "Pessoal");
                var tipoEstanteTest2 = MockData.CreateTipoEstante(2, "Trocas");
                var tipoEstanteTest3 = MockData.CreateTipoEstante(3, "Desejos");
                var estante = MockData.CreateEstante(1, utilizadorTest, livro1, tipoEstanteTest);
                var estante2 = MockData.CreateEstante(2, utilizadorTest, livro2, tipoEstanteTest2);

                context.AddRange(livro1, livro2, autorLivroTest, generoLivroTest, tipoUtilizadorTest, estadoContaTest, utilizadorTest, tipoEstanteTest, estante);
                context.SaveChanges();
            }

            using (var context = new PageTurnerContext(options))
            {
                // Execute a lógica para sugerir livros
                var controller = new LivroController(context);

                // Act
                var actionResult = await controller.SugerirLivros(1);

                // Assert
                Assert.That(actionResult.Value, Is.InstanceOf<List<LivroDTO>>());
                var result = actionResult.Value as List<LivroDTO>;
                Assert.That(result, Is.Not.Null);
                // Verifique se a lista de livros sugeridos não está vazia
                Assert.That(result, Is.Not.Empty);

                // Mostra a lista de livros sugeridos no terminal
                Console.WriteLine("Lista de Livros Sugeridos:");
                foreach (var livro in result)
                {
                    Console.WriteLine($"Livro ID: {livro.LivroId}, Título: {livro.TituloLivro}, Autor: {livro.AutorLivro.nomeAutorNome}, Género: {livro.GeneroLivro.descricaoGenero}");
                }
            }

            // Limpa a memória após o término do primeiro teste
            using (var context = new PageTurnerContext(options))
            {
                context.Database.EnsureDeleted();
            }
        }

        /// <summary>
        /// Teste para falhar propositadamente a sugestão de livros
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task SugerirLivros_FalharPropositadamenteLivrosDTO()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PageTurnerContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new PageTurnerContext(options))
            {
                var autorLivroTest = MockData.CreateAutorLivro(1, "Autor Teste");
                var autorLivroTest2 = MockData.CreateAutorLivro(2, "Autor Teste 2");
                var generoLivroTest = MockData.CreateGeneroLivro(1, "Género Teste");
                var generoLivroTest2 = MockData.CreateGeneroLivro(2, "Género Teste 2");
                var tipoUtilizadorTest = MockData.CreateTipoUtilizador(1, "Administrador");
                var estadoContaTest = MockData.CreateEstadoConta(1, "Ativo");
                var paisTest = MockData.CreatePais(1, "Portugal");
                var cidadeTest = MockData.CreateCidade(1, "Braga", paisTest.paisId);
                var utilizadorTest = MockData.CreateUtilizador(1, "UtilizadorTest", "Teste", DateTime.Now, "usernametest", "passwordtest", "emailtestunit@ipca.pt", "foto.jpg", DateTime.Now, DateTime.Now, true, true, true, 1, estadoContaTest, cidadeTest.cidadeId);
                var livro1 = MockData.CreateLivro(1, "Livro Teste 1", 2020, "Português", autorLivroTest, generoLivroTest);
                var livro2 = MockData.CreateLivro(2, "Livro Teste 2", 2018, "Inglês", autorLivroTest2, generoLivroTest2); // Livro com autor e género diferentes da referência (livro1)
                var tipoEstanteTest = MockData.CreateTipoEstante(1, "Pessoal");
                var estante = MockData.CreateEstante(1, utilizadorTest, livro1, tipoEstanteTest);

                context.AddRange(livro1, livro2, autorLivroTest, autorLivroTest2, generoLivroTest, generoLivroTest2, tipoUtilizadorTest, estadoContaTest, utilizadorTest, tipoEstanteTest, estante);
                context.SaveChanges();
            }

            using (var context = new PageTurnerContext(options))
            {
                // Execute a lógica para sugerir livros
                var controller = new LivroController(context);

                // Act
                var actionResult = await controller.SugerirLivros(1);

                // Assert
                Assert.That(actionResult.Value, Is.InstanceOf<List<LivroDTO>>());

                var result = actionResult.Value as List<LivroDTO>;

                Assert.That(result, Is.Empty); // Verifique se a lista de livros sugeridos está vazia

                if (result.Count == 0)
                {
                    Console.WriteLine("A lista de livros sugeridos está vazia porque o livro 2 não pode ser recomendado pelo livro 1 devido a diferenças no autor e genero.");
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
