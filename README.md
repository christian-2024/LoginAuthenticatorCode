# 📌 Projeto Login Authenticator

Este projeto foi desenvolvido com **back-end em C# (DDD)** e **front-end em Vite**, simulando uma tela de login com **email e senha**.  
O objetivo é demonstrar boas práticas de arquitetura de software, separação de camadas e integração entre front-end e back-end.

---

## 🚀 Tecnologias Utilizadas

### 🔹 Back-end
- C# / .NET
- Arquitetura **Domain-Driven Design (DDD)**
- Entity Framework (se aplicável)
- REST API

### 🔹 Front-end
- [Vite](https://vitejs.dev/)
- HTML, CSS, JavaScript/TypeScript
- Integração com API

---

## 📂 Estrutura do Projeto

📦 Projeto
┣ 📂 backend
┃ ┣ 📂 LoginAuthenticatorCode.CrossCutting
┃ ┣ 📂 LoginAuthenticatorCode.Domain
┃ ┣ 📂 LoginAuthenticatorCode.Service
┃ ┣ 📂 LoginAuthenticatorCode.Data
┃ ┣ 📂 LoginAuthenticatorCode.Shared
┃ ┗ 📂 LoginAuthenticatorCode.WebApi
┃
┣ 📂 frontend
┃ ┣ 📂 src
┃ ┣ 📂 public
┃ ┗ vite.config.js
┃
┣ README.md
┗ ...


---

## ⚙️ Funcionalidades

- [x] Tela de login (email e senha)  
- [x] Comunicação front-end ↔ back-end  
- [x] Validação de autenticação real (em desenvolvimento)  
- [x] Integração com banco de dados  

---

## ▶️ Como Executar o Projeto

### 🔹 Back-end
1. Acesse a pasta `backend`
2. Restaure as dependências:
   ```bash
   dotnet restore

### Front-end
1. Acessar o cmd da pasta do front
2. Execultar 
   ```bash
   npm install
