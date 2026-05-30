# 📦 Gestão de Inventário - API (Back-end)

Esta é a API construída em **.NET 8** utilizando os princípios da Clean Architecture. Ela é responsável por gerenciar as regras de negócio e a persistência de dados (Categorias e Produtos) utilizando o SQL Server.

## 🛠️ Pré-requisitos

Antes de começar, você precisará ter instalado em sua máquina:
* [Git](https://git-scm.com)
* [Docker e Docker Compose](https://www.docker.com/products/docker-desktop/) (Recomendado)
* [.NET 8 SDK](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0) (Caso queira rodar localmente sem Docker)
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Caso não utilize o banco via Docker)

---

## ⚙️ Configuração de Ambiente

Na raiz do projeto, crie um arquivo chamado `.env` baseado no arquivo de exemplo:
1. Copie o arquivo `.env.example` e renomeie para `.env`.
2. Defina a senha do banco de dados na variável `DB_PASSWORD`.

> **Nota:** Se for rodar a aplicação 100% local (sem Docker), configure a `ConnectionStrings:DefaultConnection` no seu arquivo `appsettings.Development.json`.

---

## 🐳 Executando com Docker (Recomendado)

Abra o seu terminal na **raiz do projeto** (onde o arquivo `docker-compose.yml` está localizado) e escolha uma das abordagens abaixo:

> **⚠️ Atenção com as portas:** As portas utilizadas pelos containers precisam estar livres na sua máquina. Este projeto usa a porta **8080** para a API e **1434** para o banco de dados. A porta 1434 foi escolhida intencionalmente para evitar conflito com a 1433, que costuma estar ocupada pelo serviço local do SQL Server Express. Caso alguma das portas esteja em uso, você pode interromper o serviço que a ocupa ou alterá-las manualmente no `docker-compose.yml` (apenas o número à esquerda do `:`, que representa a porta do host, ou seja, a sua máquina pessoal).

### 1. Rodar a Stack Completa (API + Banco de Dados)

O comando abaixo irá baixar as imagens, criar os containers do SQL Server e da API, e rodá-los em segundo plano.

```bash
docker compose up -d --build
```

A API estará disponível em: `http://localhost:8080`

### 2. Rodar apenas o Banco de Dados

Útil se você quiser rodar a API direto pela sua IDE (Visual Studio/Rider) para debugar, mas quer usar o banco do Docker.

```bash
docker compose up -d db
```

### 3. Rodar apenas a API

Útil se você já tem um SQL Server rodando na sua máquina ou rede local e quer subir apenas a aplicação no Docker. (Lembre-se de atualizar a Connection String no `appsettings`/env para apontar para o seu banco).

```bash
docker compose up -d --build api
```

---

## 💻 Executando Localmente (Sem Docker)

Se preferir rodar a aplicação diretamente na sua máquina, siga os passos abaixo na raiz do projeto:

### 1. Restaurar os pacotes NuGet

```bash
dotnet restore "WebAPI/WebAPI.csproj"
```

### 2. Rodar as Migrations (Entity Framework Core)

Certifique-se de que a Connection String no `appsettings.Development.json` está apontando para um banco ativo.

```bash
dotnet ef database update --project Infrastructure --startup-project WebAPI
```

> Alternativa via Package Manager Console no Visual Studio: `Update-Database`

### 3. Iniciar a API

```bash
dotnet run --project WebAPI/WebAPI.csproj
```

---

## 📄 Payloads Esperados para Teste

Abaixo estão os exemplos de JSON (Payloads) esperados pela API para as operações de criação (POST) e atualização (PUT).

### 🏷️ Categorias (`/api/v1/categorias`)

```json
{
  "nome": "Consoles de Videogame",
  "descricao": "Aparelhos de mesa e portáteis de última geração"
}
```

### 🎮 Produtos (`/api/v1/produtos`)

```json
{
  "nome": "PlayStation 5 Pro",
  "descricao": "Console de última geração com leitor de disco",
  "preco": 6000.00,
  "categoriaId": 1
}
```