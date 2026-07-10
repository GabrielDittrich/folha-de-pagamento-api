# Folha de Pagamento API

API REST para gerenciamento de funcionários e folhas de pagamento, desenvolvida com C#, ASP.NET Core, Entity Framework Core e SQLite.

O projeto permite cadastrar funcionários, gerar folhas de pagamento e consultar registros por funcionário, mês e ano. Também realiza cálculos relacionados à folha, como descontos e valores finais.

## Funcionalidades

### Funcionários

- Cadastro de funcionários
- Listagem de funcionários
- Busca de funcionário por ID
- Atualização de dados do funcionário
- Remoção de funcionário

### Folha de pagamento

- Cadastro de folha de pagamento
- Listagem de folhas cadastradas
- Busca de folha por CPF, mês e ano
- Remoção de folha de pagamento
- Cálculo de valores da folha com base nas regras implementadas no projeto

## Tecnologias utilizadas

- C#
- ASP.NET Core
- Entity Framework Core
- SQLite
- Minimal API
- Git e GitHub

## Estrutura do projeto

```text
folha-de-pagamento-api
├── FolhaDePagamento
│   ├── Backend
│   │   ├── Migrations
│   │   ├── Models
│   │   │   ├── AppDataContext.cs
│   │   │   ├── Funcionario.cs
│   │   │   └── Folha.cs
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── Backend.csproj
│   └── FolhaDePagamento.sln
└── README.md
```

## Como executar o projeto

### Pré-requisitos

Antes de começar, é necessário ter instalado:

- .NET SDK
- Git
- Entity Framework CLI

Caso não tenha o Entity Framework CLI instalado, execute:

```bash
dotnet tool install --global dotnet-ef
```

### Passo a passo

1. Clone o repositório:

```bash
git clone https://github.com/GabrielDittrich/folha-de-pagamento-api.git
```

2. Acesse a pasta do backend:

```bash
cd folha-de-pagamento-api/FolhaDePagamento/Backend
```

3. Restaure as dependências:

```bash
dotnet restore
```

4. Crie o banco de dados com as migrations:

```bash
dotnet ef database update
```

5. Execute o projeto:

```bash
dotnet run
```

A aplicação será iniciada em uma porta local exibida no terminal.

Exemplo:

```text
http://localhost:5000
```

> A porta pode variar conforme a configuração local do projeto.

## Banco de dados

O projeto utiliza SQLite com Entity Framework Core.

O arquivo `.db` não é versionado no GitHub. Para criar o banco localmente, execute:

```bash
dotnet ef database update
```

## Exemplos de requisições

### Cadastrar funcionário

**POST** `/api/funcionario/cadastrar`

```json
{
  "nome": "João da Silva",
  "cpf": "123.456.789-00"
}
```

### Listar funcionários

**GET** `/api/funcionario/listar`

### Cadastrar folha de pagamento

**POST** `/api/folha/cadastrar`

```json
{
  "quantidade": 1,
  "valor": 5000,
  "mes": 10,
  "ano": 2024,
  "funcionarioId": 1
}
```

### Buscar folha por CPF, mês e ano

```text
GET /api/folha/buscar/{cpf}/{mes}/{ano}
```

## Objetivo do projeto

Este projeto foi desenvolvido para praticar a criação de APIs REST com C#, ASP.NET Core, Entity Framework Core e SQLite, aplicando conceitos de CRUD, persistência de dados e regras de negócio relacionadas à folha de pagamento.

## Aprendizados

Durante o desenvolvimento deste projeto, foram praticados conceitos como:

- Criação de endpoints com ASP.NET Core
- Mapeamento de entidades com Entity Framework Core
- Uso de migrations
- Persistência de dados com SQLite
- Organização de models e contexto do banco de dados
- Implementação de regras de negócio em uma API
