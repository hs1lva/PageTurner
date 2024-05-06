using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Controllers;
using backend.Models;
using backend.Services;
using backend.Interfaces;
using System.Collections.Generic;
using NSubstitute;


namespace PageTurnerTests.Controllers
{
    [TestFixture]
    public class ComentarioLivroControllerTests
    {
        private PageTurnerContext _context;
        private ComentarioService _comentarioService;
        private ComentarioLivroController _controller;
        private Utilizador _utilizador;
        private Livro _livro;

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


            // Adicionar estados de conta
            var estadoAtivoConta = new EstadoConta { estadoContaId = 1, descricaoEstadoConta = "Ativo" };
            var estadoInativoConta = new EstadoConta { estadoContaId = 2, descricaoEstadoConta = "Inativo" };
            var estadoBanidoConta = new EstadoConta { estadoContaId = 3, descricaoEstadoConta = "Banido" };
            _context.EstadoConta.AddRange(estadoAtivoConta, estadoInativoConta, estadoBanidoConta);


            // Adicionar estados de comentário
            var estadoAtivo = new EstadoComentario { estadoComentarioId = 1, descricaoEstadoComentario = "Ativo" };
            var estadoRemovido = new EstadoComentario { estadoComentarioId = 2, descricaoEstadoComentario = "Removido" };
            var estadoPendente = new EstadoComentario { estadoComentarioId = 3, descricaoEstadoComentario = "Pendente" };
            _context.EstadoComentario.AddRange(estadoAtivo, estadoRemovido, estadoPendente);

            // Adicionar conteúdo ofensivo de exemplo
            var conteudoOfensivo1 = new ConteudoOfensivo { conteudoOfensivoId = 1, especificacaoConteudoOfensivo = "ofensivo" };
            _context.ConteudoOfensivo.Add(conteudoOfensivo1);

            // Adicionar comentários de exemplo
            _context.ComentarioLivro.AddRange(
                new ComentarioLivro { comentarioId = 1, comentario = "Ótimo livro!", estadoComentario = estadoAtivo },
                new ComentarioLivro { comentarioId = 2, comentario = "Não gostei deste livro.", estadoComentario = estadoAtivo }
            );

            // Criando objetos de modelo usando os métodos da classe MockData
            var autorLivro = MockData.CreateAutorLivro(1, "Autor Teste");
            var generoLivro = MockData.CreateGeneroLivro(1, "Gênero Teste");
            _utilizador = MockData.CreateUtilizador(1, "Nome", "Apelido", DateTime.Now, "username", "password", "email@example.com", "foto", DateTime.Now, DateTime.Now, false, false, false, 1, estadoAtivoConta, 1, 1);
            _livro = MockData.CreateLivro(1, "Título do Livro", 2022, "Português", autorLivro, generoLivro);
            _context.Utilizador.Add(_utilizador);
            _context.Livro.Add(_livro);
            _context.AutorLivro.Add(autorLivro);
            _context.GeneroLivro.Add(generoLivro);
            // Salvar mudanças no contexto do banco de dados
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
            var idsOfensivos = await comentario.IdentificarConteudoOfensivo(comentario.comentario);
            await comentario.VerificarEProcessarConteudoOfensivo(estadoAtivo, estadoRemovido);

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

        [Test]
        public async Task PostComentarioLivro_BansUserAfterThreeOffensiveComments()
        {

            var user = await _context.Utilizador.FirstOrDefaultAsync(u => u.utilizadorID == _utilizador.utilizadorID);
            var livro = await _context.Livro.FirstOrDefaultAsync(l => l.livroId == _livro.livroId);
            var estadoPendente = await _context.EstadoComentario.FirstOrDefaultAsync(e => e.descricaoEstadoComentario == "Pendente");
            var estadoRemovido = await _context.EstadoComentario.FirstOrDefaultAsync(e => e.descricaoEstadoComentario == "Removido");

            // Verificar se algum dos itens não foi encontrado
            if (user == null) Console.WriteLine("user e null");

            if (livro == null) Console.WriteLine("livro e null");
            if (estadoPendente == null) Console.WriteLine("estadopendente e null");
            if (estadoRemovido == null) Console.WriteLine("estadoremovido e null");





            // Adicione três comentários ofensivos para o novo usuário
            for (int i = 0; i < 3; i++)
            {
                // var estadoPendente = await _context.EstadoComentario.FirstOrDefaultAsync(e => e.descricaoEstadoComentario == "Pendente");
                // var estadoRemovido = await _context.EstadoComentario.FirstOrDefaultAsync(e => e.descricaoEstadoComentario == "Removido");

                var offensiveComment = new ComentarioLivro
                {
                    comentarioId = i + 1000, // Use diferentes IDs para cada novo comentário ofensivo
                    comentario = " ofensivo Offensive Comment",
                    dataComentario = DateTime.Now,
                    utilizadorId = _utilizador.utilizadorID,
                    livroId = _livro.livroId,
                    estadoComentario = estadoPendente
                };

                // Adicione a nova instância ao contexto
                _context.ComentarioLivro.Add(offensiveComment);

                // Verificar e atualizar o comentário (incluindo a lógica de banimento do usuário)
                await offensiveComment.VerificarEAtualizarComentario();

                // Ensure changes are saved successfully after each iteration
                await _context.SaveChangesAsync();
                if (_context.ChangeTracker.HasChanges())
                {
                    Assert.Fail("Failed to save changes to the database.");
                }
            }

            // Console log para verificar se o usuário está sendo recuperado corretamente
            Console.WriteLine("Before retrieving updated user from the database...");

            // Retrieve the updated user from the database after comments are processed
            var updatedUser = await _context.Utilizador.FirstOrDefaultAsync(u => u.utilizadorID == _utilizador.utilizadorID);

            // Console log para verificar o estado do usuário
            Console.WriteLine($"User state after comment processing: {updatedUser?.estadoContaId}");


            // Assert that the updated user is not null and banned
            Assert.That(updatedUser, Is.Not.Null);
            Assert.That(updatedUser.estadoContaId, Is.EqualTo(3));
        }



        [TearDown]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted(); // Remove o banco de dados após cada teste
            _context.Dispose(); // Descarta o contexto
        }
    }
}