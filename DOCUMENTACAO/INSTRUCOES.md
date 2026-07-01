# INSTRUÇÕES para restaurar, criar migrations e executar a API

Pré-requisitos
- .NET SDK 8 instalado (verifique com `dotnet --version`).
- MySQL rodando em `localhost:3306` com usuário `root` (senha vazia conforme configuração atual).

Arquivos importantes
- `src/retaguarda/Api/appsettings.json` — connection string usada pela aplicação.
 - `src/retaguarda/Persistencia/ApplicationDbContext.cs` — contexto do Entity Framework.

Passos (PowerShell)

1. Ir para o projeto de API:

```powershell
cd src\retaguarda\Api
```

2. Restaurar dependências:

```powershell
dotnet restore
```

3. Instalar a ferramenta `dotnet-ef` (se ainda não estiver instalada):

```powershell
dotnet tool install --global dotnet-ef
```

4. Criar a migration inicial (o projeto de migrations é o `Persistencia`, o projeto de inicialização é a `Api`):

```powershell
dotnet ef migrations add InitialCreate -p ..\Persistencia\ -s . --context ApplicationDbContext
```

5. Aplicar a migration e atualizar o banco de dados:

```powershell
dotnet ef database update -p ..\Persistencia\ -s . --context ApplicationDbContext
```

Observação: se o usuário `root` não tiver permissão para criar o banco, crie manualmente o banco `demonstracao_arquitetura` no MySQL antes de rodar o passo 5.

6. Executar a API (opcional, para testes):

```powershell
dotnet run
```

Como gerar novas migrations

 - Após alterar modelos ou `ApplicationDbContext`, repita o passo 4 trocando `InitialCreate` por um nome relevante, por exemplo `AddTabelaX`.

Alterar connection string

- Atualize `src/retaguarda/Api/appsettings.json` para apontar para outro servidor, porta, usuário ou senha.

Problemas comuns
- Erro: "dotnet ef not found" → execute o passo 3 e reinicie o terminal.
- Erro de conexão MySQL → verifique se o serviço MySQL está ativo e se as credenciais em `appsettings.json` estão corretas.

Se quiser, eu posso rodar os comandos de criação de migration e `database update` localmente agora (preciso que o MySQL esteja acessível). 
