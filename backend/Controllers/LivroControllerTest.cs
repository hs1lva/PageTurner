using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Controllers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace backend.Tests.Controllers
{
    [TestFixture]
    public class LivroControllerTests
    {
        [Test]
        public async Task SugerirLivros_Returns_Expected_Result()
        {
            // Arrange
            var mockContext = new Mock<PageTurnerContext>();
            var mockLivro = new Mock<Livro>();
            var mockLivroDto = new Mock<LivroDTO>();
            var mockLivrosDaEstante = new List<Livro>
            {
                new Livro { livroId = 1, tituloLivro = "Livro 1", autorLivro = new AutorLivro(), generoLivro = new GeneroLivro() },
                new Livro { livroId = 2, tituloLivro = "Livro 2", autorLivro = new AutorLivro(), generoLivro = new GeneroLivro() }
            };

            var mockAutoresEstante = new List<AutorLivro> { new AutorLivro() };
            var mockGenerosEstante = new List<GeneroLivro> { new GeneroLivro() };

            mockContext.Setup(x => x.Estante).Returns(ReturnsDbSet(new List<Estante>()));
            mockContext.Setup(x => x.Livro).Returns(ReturnsDbSet(mockLivrosDaEstante));

            mockLivro.Setup(l => l.SugerirLivros(It.IsAny<int>(), mockContext.Object))
                     .ReturnsAsync(new List<LivroDTO> { mockLivroDto.Object });

            var controller = new LivroController(mockContext.Object);

            // Act
            var result = await controller.SugerirLivros(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okObjectResult = result.Result as OkObjectResult;
            Assert.That(okObjectResult.Value, Is.InstanceOf<List<LivroDTO>>());
            var livroDtoList = okObjectResult.Value as List<LivroDTO>;
            Assert.That(livroDtoList.Count, Is.EqualTo(1));

        }

        private static DbSet<T> ReturnsDbSet<T>(List<T> data) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.AsQueryable().Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.AsQueryable().Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.AsQueryable().ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet.Object;
        }
    }
}
