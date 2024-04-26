using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;

namespace PageTurnerTests.Controllers
{
    public class MockData
    {
        // Criar um autor de livro para testes
        public static AutorLivro CreateAutorLivro(int id, string nome)
        {
            return new AutorLivro
            {
                autorLivroId = id,
                nomeAutorNome = nome
            };
        }

        // Criar um género de livro para testes
        public static GeneroLivro CreateGeneroLivro(int id, string descricao)
        {
            return new GeneroLivro
            {
                generoId = id,
                descricaoGenero = descricao
            };
        }

        // Criar um tipo de utilizador para testes
        public static TipoUtilizador CreateTipoUtilizador(int id, string descricao)
        {
            return new TipoUtilizador
            {
                tipoUtilId = id,
                descricaoTipoUti = descricao
            };
        }

        // Criar um utilizador para testes
        public static Utilizador CreateUtilizador(int id, string nome, string apelido, DateTime dataNascimento, string username, string password, string email, string fotoPerfil, DateTime dataRegisto, DateTime ultimologin, bool notficacaoPedidoTroca, bool notficacaoAceiteTroca, bool notficacaoCorrespondencia, int tipoUtilizadorId, EstadoConta estadoContas, int cidadeIdtest)
        {
            return new Utilizador
            {
                utilizadorID = id,
                nome = nome,
                apelido = apelido,
                dataNascimento = dataNascimento,
                username = username,
                password = password,
                email = email,
                fotoPerfil = fotoPerfil,
                dataRegisto = dataRegisto,
                ultimologin = ultimologin,
                notficacaoPedidoTroca = notficacaoPedidoTroca,
                notficacaoAceiteTroca = notficacaoAceiteTroca,
                notficacaoCorrespondencia = notficacaoCorrespondencia,
                tipoUtilizadorId = tipoUtilizadorId,
                estadoConta = estadoContas,
                cidadeId = cidadeIdtest,
                Avaliacoes = new List<AvaliacaoLivro>(),
                Comentarios = new List<ComentarioLivro>()
            };
        }

        // Criar um livro para testes
        public static Livro CreateLivro(int id, string titulo, int anoPrimeiraPublicacao, string idiomaOriginal, AutorLivro autorLivro, GeneroLivro generoLivro)
        {
            return new Livro
            {
                livroId = id,
                tituloLivro = titulo,
                anoPrimeiraPublicacao = anoPrimeiraPublicacao,
                idiomaOriginalLivro = idiomaOriginal,
                autorLivro = autorLivro,
                generoLivro = generoLivro,
                Comentarios = new List<ComentarioLivro>(),
                Avaliacoes = new List<AvaliacaoLivro>()
            };
        }

        // Criar um tipo de estante para testes
        public static TipoEstante CreateTipoEstante(int id, string descricao)
        {
            return new TipoEstante
            {
                tipoEstanteId = id,
                descricaoTipoEstante = descricao
            };
        }

        // Criar uma estante para testes
        public static Estante CreateEstante(int id, Utilizador utilizador, Livro livros, TipoEstante tipoEstantes)
        {
            return new Estante
            {
                estanteId = id,
                ultimaAtualizacao = DateTime.Now,
                utilizador = utilizador,
                livro = livros,
                livroNaEstante = true,
                tipoEstante = tipoEstantes
            };
        }

        // Criar um estado de conta do utilizador para testes
        public static EstadoConta CreateEstadoConta(int id, string descricao)
        {
            return new EstadoConta
            {
                estadoContaId = id,
                descricaoEstadoConta = descricao
            };
        }

        // Criar uma avaliação de livro para testes
        public static AvaliacaoLivro CreateAvaliacaoLivro(int id, int classificacao, int utilizadorId, int livroId)
        {
            return new AvaliacaoLivro
            {
                AvaliacaoId = id,
                Nota = classificacao,
                DataAvaliacao = DateTime.Now,
                UtilizadorId = utilizadorId,
                LivroId = livroId
            };
        }

        // Criar uma cidade para testes
        public static Cidade CreateCidade(int id, string nome, int paisId)
        {
            return new Cidade
            {
                cidadeId = id,
                nomeCidade = nome,
                paisId = paisId
            };
        }

        // Criar um país para testes
        public static Pais CreatePais(int id, string nome)
        {
            return new Pais
            {
                paisId = id,
                nomePais = nome
            };
        }
    }
}