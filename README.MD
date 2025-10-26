# 🚗 Sistema de Concessionária — Minimal API com .NET 9

![.NET](https://img.shields.io/badge/.NET%209.0-blueviolet?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![MariaDB](https://img.shields.io/badge/MariaDB-003545?style=for-the-badge&logo=mariadb&logoColor=white)
![JWT](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black)
![Tests](https://img.shields.io/badge/MSTest-passing-brightgreen?style=for-the-badge&logo=.net)

> 💡 Projeto autoral desenvolvido para praticar e demonstrar habilidades em **desenvolvimento backend com .NET 9**, aplicando **boas práticas de arquitetura**, **autenticação JWT**, **Entity Framework Core** e **testes automatizados**.
>
> A ideia foi construir uma API moderna e organizada que simula o sistema interno de uma **concessionária de veículos**, com gestão de administradores e cadastro de automóveis.

---

## 🧩 Sobre o Projeto

Este projeto nasceu como uma forma de consolidar meus conhecimentos em **Minimal APIs no .NET** e colocar em prática conceitos de **Clean Code**, **injeção de dependência**, **camadas bem definidas** e **testes unitários**.

A aplicação permite **gerenciar veículos e usuários (administradores e editores)**, com autenticação via **JWT** e autorização por perfil.  
Além disso, a API foi construída pensando na **organização, clareza e escalabilidade**, simulando um cenário real de backend corporativo.

---

## 🎯 Objetivos

- Praticar desenvolvimento de APIs REST com .NET 9
- Aplicar boas práticas de arquitetura e separação de responsabilidades
- Implementar autenticação e autorização seguras com **JWT**
- Utilizar **Entity Framework Core + MariaDB** para persistência
- Criar testes automatizados com **MSTest** para garantir a qualidade do código

---

## ⚙️ Principais Funcionalidades

### 👤 Administradores

- Cadastro e login com geração de token JWT
- Listagem, atualização e exclusão de administradores
- Controle de acesso por perfil (**Adm** ou **Editor**)

### 🚘 Veículos

- Cadastro de novos veículos
- Listagem com paginação
- Atualização e exclusão (restrita a administradores)

---

## 🧱 Estrutura do Projeto

```

📦 Api
┣ 📂 Dominio
┃ ┣ 📂 DTOs
┃ ┣ 📂 Entidades
┃ ┣ 📂 Interfaces
┃ ┣ 📂 ModelViews
┃ ┗ 📂 Servicos
┣ 📂 Infraestrutura
┃ ┗ 📂 Db
┣ 📄 Program.cs
┗ 📄 Startup.cs

📦 test
┣ 📂 Domain
┣ 📂 Mocks
┗ 📂 Requests

```

A arquitetura foi pensada para **manter o código limpo e de fácil manutenção**, separando domínio, infraestrutura e testes.

---

## 🧰 Tecnologias Utilizadas

| Tecnologia                    | Uso                                   |
| ----------------------------- | ------------------------------------- |
| **.NET 9 (C# 12)**            | Plataforma e linguagem principal      |
| **Entity Framework Core 9**   | ORM para manipulação de dados         |
| **MariaDB**                   | Banco de dados relacional             |
| **JWT (JSON Web Token)**      | Autenticação e autorização            |
| **Swagger / OpenAPI**         | Documentação da API                   |
| **MSTest**                    | Testes automatizados                  |
| **Dependency Injection (DI)** | Injeção de dependência nativa do .NET |

---

## 🧪 Testes Automatizados

Implementei testes para garantir o bom funcionamento dos serviços e endpoints, cobrindo operações de CRUD e autenticação.

Com **MSTest**, foram criados testes unitários e de integração que validam o comportamento do `AdministradorServico` e da API em execução.

```bash
dotnet test
```

Os testes rodam em um banco de dados isolado (MariaDB), limpando as tabelas entre execuções para garantir resultados consistentes.

---

## 🚀 Como Executar Localmente

### 📋 Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [MariaDB](https://mariadb.org/)
- Visual Studio / VS Code

### ⚙️ Passos

1. **Clone o repositório**

   ```bash
   git clone https://github.com/ArielBrandonFlores/minimal-api.git
   cd minimal-api
   ```

2. **Configure o arquivo `appsettings.json`**

   ```json
   {
     "ConnectionStrings": {
       "MySql": "server=localhost;database=concessionaria;user=root;password=suasenha"
     },
     "Jwt": "chave-secreta-para-gerar-token"
   }
   ```

3. **Aplique as migrações e crie o banco**

   ```bash
   dotnet ef database update
   ```

4. **Execute a aplicação**

   ```bash
   dotnet run
   ```

5. **Acesse o Swagger**

   ```
   http://localhost:5000/swagger
   ```

---

## 🔑 Exemplo de Login

**POST** `/administradores/login`

```json
{
  "email": "administrador@teste.com",
  "senha": "123456"
}
```

**Resposta:**

```json
{
  "email": "administrador@teste.com",
  "perfil": "Adm",
  "token": "<jwt_token>"
}
```

---

## 💡 Diferenciais Técnicos

- Estrutura de código limpa e desacoplada
- Injeção de dependências bem aplicada
- Uso de DTOs e ModelViews para separação de camadas
- Testes automatizados garantindo confiabilidade
- Paginação e validações implementadas
- Documentação da API via Swagger

---

## 🧠 Aprendizados e Motivação

Este projeto foi uma oportunidade de **aprofundar minha experiência com .NET**, especialmente na criação de **APIs Minimalistas, seguras e bem estruturadas**.
Durante o desenvolvimento, pratiquei:

- Organização de código em múltiplas camadas
- Validação de dados e tratamento de erros
- Controle de autenticação com JWT
- Boas práticas de testes e versionamento de banco

---
