--16 tabelas 

-- estado da conta
INSERT INTO [dbo].[EstadoConta] ([descricaoEstadoConta]) VALUES ('Entrada 1');
INSERT INTO [dbo].[EstadoConta] ([descricaoEstadoConta]) VALUES ('Entrada 2');
INSERT INTO [dbo].[EstadoConta] ([descricaoEstadoConta]) VALUES ('Entrada 3');

--TipoUtilizador
INSERT INTO [dbo].[TipoUtilizador] ([descricaoTipoUti]) VALUES ('Administrador');
INSERT INTO [dbo].[TipoUtilizador] ([descricaoTipoUti]) VALUES ('Usuário Regular');
INSERT INTO [dbo].[TipoUtilizador] ([descricaoTipoUti]) VALUES ('Convidado');

--Pais 
INSERT INTO [dbo].[Pais] ([nomePais]) VALUES ('Brasil');
INSERT INTO [dbo].[Pais] ([nomePais]) VALUES ('Estados Unidos');
INSERT INTO [dbo].[Pais] ([nomePais]) VALUES ('Reino Unido');

--EstadoComentario
INSERT INTO [dbo].[EstadoComentario] ([descricaoEstadoComentario]) 
VALUES 
('Ativo'),
('Inativo'),
('Removido');


--EstadoTroca
INSERT INTO [dbo].[EstadoTroca] ([descricaoEstadoTroca]) 
VALUES 
('Pendente'),
('Aceita'),
('Recusada');

--Cidade
INSERT INTO [dbo].[Cidade] ([nomeCidade], [paisCidadepaisId]) VALUES ('Rio de Janeiro', 1);
INSERT INTO [dbo].[Cidade] ([nomeCidade], [paisCidadepaisId]) VALUES ('Nova Iorque', 2);
INSERT INTO [dbo].[Cidade] ([nomeCidade], [paisCidadepaisId]) VALUES ('Londres', 3);


-- Tipo Estante
INSERT INTO [dbo].[TipoEstante] ([descricaoTipoEstante]) VALUES ('Tipo 1');
INSERT INTO [dbo].[TipoEstante] ([descricaoTipoEstante]) VALUES ('Tipo 2');
INSERT INTO [dbo].[TipoEstante] ([descricaoTipoEstante]) VALUES ('Tipo 3');

-- GeneroLivro
INSERT INTO [dbo].[GeneroLivro] ([descricaoGenero]) VALUES ('Ficção Científica');
INSERT INTO [dbo].[GeneroLivro] ([descricaoGenero]) VALUES ('Fantasia');
INSERT INTO [dbo].[GeneroLivro] ([descricaoGenero]) VALUES ('Romance');
-- Autor
INSERT INTO [dbo].[AutorLivro] ([nomeAutorNome]) VALUES ('J.K. Rowling');
INSERT INTO [dbo].[AutorLivro] ([nomeAutorNome]) VALUES ('George Orwell');
INSERT INTO [dbo].[AutorLivro] ([nomeAutorNome]) VALUES ('Jane Austen');

-- ConteudoOfensivo
INSERT INTO [dbo].[ConteudoOfensivo] ([especificacaoConteudoOfensivo]) VALUES ('Discurso de ódio');
INSERT INTO [dbo].[ConteudoOfensivo] ([especificacaoConteudoOfensivo]) VALUES ('Violência explícita');
INSERT INTO [dbo].[ConteudoOfensivo] ([especificacaoConteudoOfensivo]) VALUES ('Conteúdo sexual explícito');

-- Utilizador
INSERT INTO [dbo].[Utilizador] ([nome], [apelido], [dataNascimento], [username], [password], [email], [fotoPerfil], [dataRegisto], [ultimologin], [notficacaoPedidoTroca], [notficacaoAceiteTroca], [notficacaoCorrespondencia], [tipoUtilizadortipoUtilId], [estadoContaId], [cidadeId]) 
VALUES 
('João', 'Silva', '1990-05-15', 'joaosilva', 'senha123', 'joao@example.com', 'perfil1.jpg', '2024-03-21 09:00:00', '2024-03-21 09:00:00', 1, 1, 1, 1, 1, 1),
('Maria', 'Santos', '1985-08-20', 'msantos', 'segredo456', 'maria@example.com', 'perfil2.jpg', '2024-03-20 10:30:00', '2024-03-21 08:30:00', 1, 1, 0, 2, 1, 2),
('Pedro', 'Oliveira', '1995-02-10', 'poliveira', '123456', 'pedro@example.com', 'perfil3.jpg', '2024-03-19 11:45:00', '2024-03-21 07:45:00', 0, 1, 1, 3, 2, 3);

-- Livro
INSERT INTO [dbo].[Livro] ([tituloLivro], [anoPrimeiraPublicacao], [idiomaOriginalLivro], [autorLivroId], [generoLivrogeneroId]) 
VALUES 
('Dom Casmurro', 1899, 1, 1, 1),
('Orgulho e Preconceito', 1813, 1, 2, 2),
('Cem Anos de Solidão', 1967, 2, 3, 3);


-- Comentario
INSERT INTO [dbo].[CommentLivro] ([comentario], [dataComentario], [utilizadorID], [livroId], [estadoComentarioId]) 
VALUES 
('Este livro é incrível!', '2024-03-21 08:00:00', 1, 1, 1),
('Não recomendo este livro.', '2024-03-20 14:30:00', 2, 2, 2),
('Estou ansioso para ler este livro!', '2024-03-19 17:45:00', 3, 3, 1);


-- ComentarioOfensivos
INSERT INTO [dbo].[ComentarioLivroConteudoOfensivo] ([comentarioscomentarioId], [conteudoOfensivoId]) VALUES (1, 1);
INSERT INTO [dbo].[ComentarioLivroConteudoOfensivo] ([comentarioscomentarioId], [conteudoOfensivoId]) VALUES (2, 2);
INSERT INTO [dbo].[ComentarioLivroConteudoOfensivo] ([comentarioscomentarioId], [conteudoOfensivoId]) VALUES (3, 3);

-- Estante
INSERT INTO [dbo].[Estante] ([ultimaAtualizacao],[tipoEstanteId],[utilizadorID],[livroId]) VALUES ('2024-03-21 10:00:00',1,1,1);
INSERT INTO [dbo].[Estante] ([ultimaAtualizacao],[tipoEstanteId],[utilizadorID],[livroId]) VALUES ('2024-03-21 11:00:00',2,2,2);
INSERT INTO [dbo].[Estante] ([ultimaAtualizacao],[tipoEstanteId],[utilizadorID],[livroId]) VALUES ('2024-03-21 12:00:00',3,3,3);

até aqui já foi falta esta

-- Inserção de dados na tabela Troca
INSERT INTO [dbo].[Troca] ([dataPedidoTroca], [dataAceiteTroca], [estanteId], [estadoTrocaId])
VALUES 
    ('2024-03-21 08:00:00', NULL, 1, 1),  -- Exemplo de valores fictícios
    ('2024-03-20 09:30:00', '2024-03-20 09:45:00', 2, 2),  -- Substitua os valores fictícios pelos dados reais
    ('2024-03-19 10:45:00', NULL, 3, 1);  -- que você deseja inserir
