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
    public class UtilizadorControllerTest
    {
        private UtilizadorController _controller;
        private PageTurnerContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<PageTurnerContext>()
                .UseInMemoryDatabase(databaseName: "PageTurner")
                .Options;

            _context = new PageTurnerContext(options);
            _controller = new UtilizadorController(_context);
        }

        /// <summary>
        /// Teste para criar um novo utilizador com sucesso
        /// </summary>
        [Test]
        public void PostUtilizador_DeveCriarNovoUtilizador()
        {
            // Arrange
            var utilizadorDTO = new UtilizadorCreateDTO
            {
                nome = "Novo",
                apelido = "Utilizador",
                dataNascimento = DateTime.Now.AddYears(-25),
                username = "novoutilizador",
                password = "novasenha",
                email = "novoutilizador@example.com",
                fotoPerfil = "perfil.jpg",
                ultimologin = DateTime.Now,
                notficacaoPedidoTroca = true,
                notficacaoAceiteTroca = true,
                notficacaoCorrespondencia = true,
                tipoUtilizadorId = 1, // tem que existir um tipo de utilizador
                estadoContaId = 1, // tem que existir um estado de conta
                cidadeId = 1
            };

            // Act
            var result = _controller.PostUtilizador(utilizadorDTO);

            // Assert
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.That(createdAtActionResult, Is.Not.Null);

            // Verificar se o utilizador foi criado
            Assert.That(utilizadorDTO, Is.Not.Null);

            // Mostrar mensagem de sucesso de teste na criacao do utilizador no terminal
            Console.WriteLine("Utilizador criado com sucesso! Email: " + utilizadorDTO.email);
        }

        /// <summary>
        /// Teste para criar um novo utilizador com falha porque o email já existe
        /// </summary>
        [Test]
        public async Task PostUtilizador_DeveRetornarBadRequestPorEmailExistente()
        {
            // Arrange
            var utilizadorDTO = new UtilizadorCreateDTO
            {
                nome = "Novo2",
                apelido = "Utilizador2",
                dataNascimento = DateTime.Now.AddYears(-25),
                username = "novoutilizador2",
                password = "novasenha",
                email = "novoutilizador@example.com", // Usar o mesmo email do utilizador existente
                fotoPerfil = "perfil2.jpg",
                ultimologin = DateTime.Now,
                notficacaoPedidoTroca = true,
                notficacaoAceiteTroca = true,
                notficacaoCorrespondencia = true,
                tipoUtilizadorId = 1, // tem que existir um tipo de utilizador
                estadoContaId = 1, // tem que existir um estado de conta
                cidadeId = 1
            };

            // Act
            var result = _controller.PostUtilizador(utilizadorDTO);

            // Assert
            var conflictResult = result as ConflictObjectResult; // 409 no post do utilizador (conflict)
            Assert.That(conflictResult, Is.Not.Null);
            Assert.That(conflictResult.StatusCode, Is.EqualTo(409));

            // Mostrar mensagem de erro de teste na criacao do utilizador no terminal
            Console.WriteLine("Erro ao criar utilizador! Email: " + utilizadorDTO.email + " já existe.");
        }


        /// <summary>
        /// Teste para criar um novo utilizador com falha porque o username já existe
        /// </summary>
        [Test]
        public async Task PostUtilizador_DeveRetornarBadRequestPorUsernameExistente()
        {
            // Arrange
            var utilizadorDTO = new UtilizadorCreateDTO
            {
                nome = "Novo2",
                apelido = "Utilizador2",
                dataNascimento = DateTime.Now.AddYears(-25),
                username = "novoutilizador", // Utilizar um username que já existe
                password = "novasenha",
                email = "novoutilizador33@example.com",
                fotoPerfil = "perfil2.jpg",
                ultimologin = DateTime.Now,
                notficacaoPedidoTroca = true,
                notficacaoAceiteTroca = true,
                notficacaoCorrespondencia = true,
                tipoUtilizadorId = 1, // Tem que existir um tipo de utilizador
                estadoContaId = 1, // Tem que existir um estado de conta
                cidadeId = 1
            };

            // Act
            var result = _controller.PostUtilizador(utilizadorDTO);

            // Assert
            var conflictResult = result as ConflictObjectResult; // 409 no post do utilizador (conflict)
            Assert.That(conflictResult, Is.Not.Null);
            Assert.That(conflictResult.StatusCode, Is.EqualTo(409));

            // Mostrar mensagem de erro de teste na criação do utilizador no terminal
            Console.WriteLine("Erro ao criar utilizador! Username: " + utilizadorDTO.username + " já existe.");
            Console.WriteLine($"-------------------------------------------------");
        }
    }
}