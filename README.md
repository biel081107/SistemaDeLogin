# 🛠️ Sistema de Gerenciamento para Loja de Eletrônicos

Este é um sistema web desenvolvido em **ASP.NET Core MVC**, com foco em praticar **ASP.NET Identity** para controle de autenticação e permissões de usuários. A aplicação simula o funcionamento de uma loja/assistência técnica de eletrônicos, permitindo o gerenciamento de clientes, serviços prestados e peças compradas.

---

## 🔐 Autenticação e Perfis de Acesso

A aplicação utiliza o **ASP.NET Identity** para gerenciamento completo de usuários.

- 👨‍🔧 **Funcionários**: podem visualizar e cadastrar clientes e serviços.
- 👑 **Administradores**: têm acesso total, incluindo edição, exclusão de registros, e gerenciamento de peças e contas.

> ✅ **Conta de administrador criada automaticamente para testes:**
> - **Email:** `admin@gmail.com`  
> - **Senha:** `Admin@7221`

---

## ⚙️ Funcionalidades

### 👥 Gerenciamento de Clientes
- Cadastro e edição de informações básicas.
- Visualização individual dos clientes.

### 🛠️ Serviços Prestados
- Associados diretamente a cada cliente.
- Informações sobre o tipo de serviço, status, descrição etc.

### 🧩 Gerenciamento de Peças
- Registro de peças já compradas ou pendentes.
- Controle de quantidade, status de compra.

### 🔒 Acesso Restrito por Nível
- Somente **admins** podem **excluir** registros e acessar determinadas áreas administrativas (ainda em desenvolvimento).

---

## 🎯 Objetivo do Projeto

Este projeto foi criado com o objetivo de:

- Praticar autenticação e autorização com **ASP.NET Identity**
- Simular uma aplicação real de assistência técnica
- Consolidar conhecimentos em **Entity Framework Core**, estrutura MVC e controle de permissões

---
## 🚀 Como Executar

1. Clone o repositório
2. Abra o projeto no Visual Studio ou VS Code
3. Execute `Update-Database` no console do Gerenciador de Pacotes (Package Manager Console)
4. Rode o projeto (`F5` ou `Ctrl + F5`)

---

## 📌 Observações

- Este sistema é um projeto de estudo com fins didáticos.
- A conta `admin@gmail.com` com a senha `Admin@7221` é criada automaticamente na primeira execução para fins de testes.
- Algumas melhorias estão previstas para as próximas versões.

---

## 📅 Próximos Passos

- Melhorar o front-end (layout, design, responsividade e experiência do usuário)
- Adicionar autenticação com recuperação de senha por e-mail
- Criar dashboard com indicadores e estatísticas
- Implementar filtros e paginação nas listas
- Aplicar testes automatizados (unitários e integração)

---

## 👨‍💻 Desenvolvedor

**Gabriel Olímpio**  
📧 [contatoolimpiodev@gmail.com]  
🔗 [www.linkedin.com/in/olimpiodev]

---

⭐ Se você gostou desse projeto, não esqueça de deixar uma ⭐ aqui no GitHub!