#!/bin/bash

# Encontrar o caminho para o arquivo .csproj
projeto=$(find . -name '*.csproj' -print -quit)

# Verificar se o arquivo .csproj foi encontrado
if [ -z "$projeto" ]; then
    echo "Arquivo .csproj não encontrado."
    exit 1
fi

# Executar os testes
dotnet test "$projeto"

# Verificar o código de saída dos testes
if [ $? -ne 0 ]; then
    echo "Testes falharam. A aplicação não será iniciada."
    exit 1
fi

# Extrair o diretório do arquivo .csproj
diretorio=$(dirname "$projeto")

# Se os testes passarem, iniciar a aplicação
dotnet run --project "$diretorio"
