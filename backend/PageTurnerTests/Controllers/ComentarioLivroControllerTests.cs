using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Controllers;
using backend.Models;
using backend.Services;
using backend.Interfaces;
using System.Collections.Generic;
using NSubstitute;

[TestFixture]
public class ComentarioLivroControllerTests
{
    private PageTurnerContext _context; 
    private ComentarioService _comentarioService;
    private ComentarioLivroController _controller;

    [SetUp]
    public void Setup()
    {
        // Configurar o contexto para usar banco de dados em memória
        var options = new DbContextOptionsBuilder<PageTurnerContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new PageTurnerContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        // Adicionar estados de comentário
        var estadoAtivo = new EstadoComentario { estadoComentarioId = 1, descricaoEstadoComentario = "Ativo" };
        var estadoRemovido = new EstadoComentario { estadoComentarioId = 2, descricaoEstadoComentario = "Removido" };
        var estadoPendente = new EstadoComentario { estadoComentarioId = 3, descricaoEstadoComentario = "Pendente"};
        _context.EstadoComentario.AddRange(estadoAtivo, estadoRemovido, estadoPendente);

         // Adicionar conteúdo ofensivo de exemplo
        var conteudoOfensivo1 = new ConteudoOfensivo { conteudoOfensivoId = 1, especificacaoConteudoOfensivo = "ofensivo" };
        _context.ConteudoOfensivo.Add(conteudoOfensivo1);

        // Adicionar comentários de exemplo
        _context.ComentarioLivro.AddRange(
            new ComentarioLivro { comentarioId = 1, comentario = "Ótimo livro!", estadoComentario = estadoAtivo },
            new ComentarioLivro { comentarioId = 2, comentario = "Não gostei deste livro.", estadoComentario = estadoAtivo }
        );

        _context.SaveChanges();

        _comentarioService = new ComentarioService(_context);
        _controller = new ComentarioLivroController(_context, _comentarioService);
    }

    [Test]
    public void ComentarioLivroExists_ReturnsCorrectly()
    {
        // Act
        var commentExists = _controller.ComentarioLivroExists(1); // Deve retornar true
        var commentDoesNotExist = _controller.ComentarioLivroExists(3); // deve retornar false

        // Assert
        Assert.That(commentExists, Is.True);
        Assert.That(commentDoesNotExist, Is.False);
    }

    [Test]
    public async Task ObterEstadoComentarioAsync_WithValidDescricao_ReturnsCorrectEstado()
    {
        // Act
        var result = await _controller.ObterEstadoComentarioAsync("Ativo");

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.descricaoEstadoComentario, Is.EqualTo("Ativo"));
    }

    [Test]
    public async Task ObterEstadoComentarioAsync_WithInvalidDescricao_ReturnsNull()
    {
        // Act
        var result = await _controller.ObterEstadoComentarioAsync("Inexistente");

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task VerificarEProcessarConteudoOfensivoAsync_DeveAssociarConteudoOfensivo()
    {
        var comentario = await _context.ComentarioLivro.FindAsync(2);
        var estadoAtivo = await _context.EstadoComentario.FindAsync(1);
        var estadoRemovido = await _context.EstadoComentario.FindAsync(2);

        // Simular a identificação de conteúdo ofensivo
        var idsOfensivos = new List<int> { 1 }; // Conteúdo ofensivo encontrado

        // Act
        await _comentarioService.VerificarEProcessarConteudoOfensivoAsync(comentario, estadoAtivo, estadoRemovido);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(comentario.estadoComentario, Is.EqualTo(estadoAtivo), "O estado do comentário deve permanecer 'Ativo'");
            // Verifique se o método AdicionarRelacoesConteudoOfensivo não foi chamado.
            Assert.That(!_context.ComentarioLivroConteudoOfensivo.Any(co => co.comentarioId == 3), "Não deve adicionar nenhuma relação conteúdo ofensivo");
        });
    }


    [Test]
    public async Task IdentificarConteudoOfensivoAsync_QuandoContemPalavraOfensiva_DeveAtualizarEstadoParaRemovido()
    {
        // Usar o conteúdo ofensivo já existente no banco de dados
        var conteudoOfensivo = await _context.ConteudoOfensivo.FirstOrDefaultAsync(co => co.conteudoOfensivoId == 1);

        // Criar um novo comentário que inclui a palavra ofensiva
        var estadoAtivo = await _context.EstadoComentario.FirstOrDefaultAsync(e => e.descricaoEstadoComentario == "Ativo");
        var estadoRemovido = await _context.EstadoComentario.FirstOrDefaultAsync(e => e.descricaoEstadoComentario == "Removido");
        var estadoPendente = await _context.EstadoComentario.FirstOrDefaultAsync(e => e.descricaoEstadoComentario == "Pendente");
        var comentario = new ComentarioLivro 
        { 
            comentarioId = 3, 
            comentario = "Este livro é realmente ofensivo !", // palavra "ofensivo" tem de passar para estado removido 
            estadoComentario = estadoPendente
        };
        _context.ComentarioLivro.Add(comentario);
        _context.SaveChanges();

        // Act
        var idsOfensivos = await _comentarioService.IdentificarConteudoOfensivoAsync(comentario.comentario);
        await _comentarioService.VerificarEProcessarConteudoOfensivoAsync(comentario, estadoAtivo, estadoRemovido);

        // Assert
        Assert.Multiple(() =>
        {
            // Verifica se a lista de identificadores de conteúdos ofensivos não está vazia
            Assert.That(idsOfensivos, Is.Not.Empty, "Deve identificar conteúdo ofensivo.");

            // Verifica se o estado do comentário foi atualizado para 'Removido'
            Assert.That(comentario.estadoComentario, Is.EqualTo(estadoRemovido), "O estado do comentário deve ser atualizado para 'Removido'");

            // Verifica se a relação entre o comentário e o conteúdo ofensivo foi corretamente registrada na tabela pivot
            Assert.That(_context.ComentarioLivroConteudoOfensivo.Any(co => co.comentarioId == comentario.comentarioId && co.conteudoOfensivoId == conteudoOfensivo.conteudoOfensivoId),
                        Is.True, "Deve adicionar a relação conteúdo ofensivo corretamente");
        });
    }


    [TearDown]
    public void Cleanup()
    {
        _context.Database.EnsureDeleted(); // Remove o banco de dados após cada teste
        _context.Dispose(); // Descarta o contexto
    }
}
