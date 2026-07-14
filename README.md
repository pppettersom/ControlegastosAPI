# Controle de Gastos

Aplicação desenvolvida para gerenciamento de despesas residenciais, permitindo o cadastro de pessoas, lançamento de transações e consulta de totais financeiros.

**Nota:** Este repositório foi inicializado a partir da pasta do backend. 
Por conta disso, o diretório ControlegastosAPI contém os arquivos backend na raiz do projeto 
(não em uma subpasta própria), enquanto ControlegastosFrontend está organizado como subdiretório.
Não corrigi a estrutura para preservar o histórico de commits.

## Tecnologias utilizadas

### Backend
- C#
- .NET 8
- Entity Framework Core
- SQLite
- Swagger

### Frontend
- React
- TypeScript
- Vite
- Axios
  
## Regras de negócio
- Ao excluir uma pessoa, todas as transações associadas são removidas (cascade delete)
- Pessoas menores de idade não podem cadastrar Receitas
  
## Funcionalidades

### Pessoas
- Cadastro de pessoas
- Listagem de pessoas cadastradas
- Exclusão de pessoas

### Transações
- Cadastro de receitas e despesas
- Associação de transações a pessoas
- Listagem de movimentações

### Resumo financeiro
- Total de receitas
- Total de despesas
- Saldo geral
- Resumo individual por pessoa

## Como executar o projeto

### Backend

Acesse a pasta do repositório
Execute:
dotnet restore
dotnet run

A API será iniciada utilizando a configuração local do projeto.

---

### Frontend

Acesse a pasta:
cd ControlegastosFrontend

Instale as dependências:
npm install

Execute:
npm run dev


---

## Banco de dados

O projeto utiliza SQLite para persistência dos dados.

O arquivo do banco acompanha o projeto para facilitar a execução e avaliação da aplicação.

## Observações

O frontend realiza comunicação com a API .NET através de requisições HTTP utilizando Axios.
