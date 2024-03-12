# Configuração do MySQL Docker

Este directório contém toda a configuração necessária para inicializar uma instância MySQL dentro de um contentor Docker. Isto inclui a criação automática de bases de dados e tabelas necessárias para o projeto.

## Configuração Inicial

Dentro deste directório, encontrará os seguintes componentes:

- `docker-compose.yml`: Ficheiro de configuração Docker Compose que define o serviço MySQL.
- `mysql-init/`: Diretório que contém scripts SQL que são executados automaticamente para configurar a base de dados quando o container é iniciado.

## Como Utilizar

Para iniciar o serviço MySQL, executa o seguinte comando: 

```bash
docker-compose up -d
```

## Aceder à Base de Dados MySQL

Uma vez que o contentor está a funcionar, pode aceder à base de dados MySQL usando as seguintes informações:
```
Endereço IP para ligação Local: 127.0.0.1 (ou localhost)
Porta Padrão: 3306
Nome da Base de Dados: page_turner
Utilizador: admin
Senha: admin
```


## Parar o Serviço MySQL

Para parar o contentor MySQL, utilize:

```bash
docker-compose down
```

Isto irá parar e remover o container MySQL, mas os dados permanecerão persistentes no volume db_data.

## Reinicialização e Limpeza

Se precisar de reiniciar o serviço e começar com uma nova instância da base de dados:
```
Parar o serviço com docker-compose down.
Remover o volume de dados com docker volume rm mysql-docker_db_data.
Reiniciar o serviço com docker-compose up -d.
```

A remoção do volume de dados irá eliminar todos os dados existentes, por isso faz isto apenas se precisares de começar de novo.


## Personalização

Se precisas de personalizar os scripts SQL de inicialização, podes adicionar ou modificar os ficheiros dentro do directório **mysql-init/**. Estes scripts serão executados por ordem alfabética quando o container for iniciado pela primeira vez.
