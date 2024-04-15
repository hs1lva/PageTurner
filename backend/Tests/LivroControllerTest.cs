using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Controllers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
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
            int utilizadorId = 1;
            var mockLivro = new Mock<Livro>();
            var mockContext = new Mock<PageTurnerContext>();

            var livrosSugeridosDto = new List<LivroDTO>
            {
                new LivroDTO { LivroId = 1, TituloLivro = "Livro 1" },
                new LivroDTO { LivroId = 2, TituloLivro = "Livro 2" }
            };

            mockLivro.Setup(l => l.SugerirLivros(It.IsAny<int>(), mockContext.Object))
                     .ReturnsAsync(livrosSugeridosDto);

            var controller = new LivroController(mockContext.Object);

            // Act
            var result = await controller.SugerirLivros(utilizadorId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var okObjectResult = result.Result as OkObjectResult;
            Assert.That(okObjectResult.Value, Is.InstanceOf<List<LivroDTO>>());
            var livroDtoList = okObjectResult.Value as List<LivroDTO>;
            Assert.That(livroDtoList.Count, Is.EqualTo(2)); // Verifica se foram retornados dois livros, como esperado
        }
    }
}

/* 
Arrange: Nesta seção, preparamos o ambiente de teste. 
Isso inclui criar instâncias mockadas necessárias e configurar seus comportamentos esperados.
As etapas incluem:
- Criar um ID de usuário fictício (utilizadorId) para usar no teste.
- Criar um mock para a classe Livro.
- Criar um mock para o contexto do base de dados PageTurnerContext.
- Definir um conjunto de objetos LivroDTO que representam os resultados esperados da operação de sugestão de livros.
- Configurar o mock do Livro para retornar os objetos LivroDTO quando o método SugerirLivros é chamado com qualquer ID de usuário e contexto de base de dados.

Act: Nesta seção, executamos o método que queremos testar. 
Aqui, chamamos o método SugerirLivros do LivroController criado na etapa de configuração (Arrange).

Assert: Nesta seção, verificamos se o resultado da execução do método corresponde ao comportamento esperado. 
Aqui, estamos a verificar os seguintes pontos:
- Se o resultado retornado não é nulo.
- Se o resultado é um OkObjectResult, indicando que a operação foi bem-sucedida.
- Se o valor contido no OkObjectResult é uma lista de LivroDTO.
- Se a lista de LivroDTO contém o número esperado de livros sugeridos.
*/