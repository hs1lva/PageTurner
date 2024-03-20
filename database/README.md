## Instruções para Uso do SQL Server com Docker

Este guia abrange a configuração e uso de uma instância do SQL Server num container Docker.

## Configuração do Docker

1. Certifique-se de que o Docker está instalado e a correr

2. Clonar o repositório 

3. Abrir o o diretório /Database/ e correr 

    ```bash
    docker-compose up -d
    ```

4. Para verificar se o contêiner está a rodar corretamente, usa, ou verifica no Docker Desktop:

    ```bash
    docker ps
    ```

## Configuração da String de Conexão

Para a aplicação, usamos a seguinte connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433; Database=PageTurner; User Id=sa; Password=Pageturner2024#;"
}
```

## Ligação Usando Azure Data Studio ou Outro Cliente SQL
1. Para ligar à base de dados SQL Server usando Azure Data Studio ou outro cliente MSSQL:

2. Abrir o Azure Data Studio:

3. Criar uma nova ligação e preencher:
```
Server: localhost,1433
Authentication Type: SQL Login
User name: sa
Password: Pageturner2024#
```

4. Podemos também usar uma connection string:
```
Server=localhost,1433; User Id=sa; Password=Pageturner2024#;
```
