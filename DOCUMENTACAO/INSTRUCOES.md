# INSTRUÇÕES para restaurar, criar migrations e executar a API

Pré-requisitos
- .NET SDK 8 instalado (verifique com `dotnet --version`).
- MySQL rodando em `localhost:3306` com usuário `root` (senha vazia conforme configuração atual).

Arquivos importantes
- `src/retaguarda/Api/appsettings.json` — connection string usada pela aplicação.
- `src/retaguarda/Persistencia` (ou `src/retaguarda/Persistencia/MYSQL`) — projeto onde ficam `ApplicationDbContext` e as migrations.

Resumo: o EF carrega migrations a partir da assembly compilada e do `DbContext` que você indicar. A organização em pastas (`Migracoes/MYSQL`) é apenas física — informe o projeto/assembly e o `--context` ao rodar os comandos para aplicar a migração correta.

Passos (PowerShell)

1. Ir para o projeto de API (startup project):

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

4. Criar uma migration (ex.: numerada `000002_descricao`) — especifique o projeto que contém as migrations com `-p`, o startup project com `-s`, o `--context` e a pasta de saída `--output-dir` se quiser que a migration vá para `Migracoes/MYSQL`:

```powershell
dotnet ef migrations add 000002_descricao -p ..\Persistencia\ -s . --context ApplicationDbContext --output-dir Migracoes/MYSQL
```

Notas:
- `-p ..\Persistencia\` = projeto que contém o `DbContext` e onde as migrations serão adicionadas.
- `-s .` = projeto de startup (neste caso `Api`) usado para construir e executar a aplicação durante o processo de design-time.
- `--context ApplicationDbContext` = especifica qual `DbContext` usar (importante quando há vários).

5. Aplicar as migrations ao banco (executa todas as migrations pendentes para o `DbContext` especificado):

```powershell
dotnet ef database update -p ..\Persistencia\ -s . --context ApplicationDbContext
```

Para aplicar até uma migration específica:

```powershell
dotnet ef database update 000001_InitialCreate -p ..\Persistencia\ -s . --context ApplicationDbContext
```

Considerações para múltiplos bancos / contexts
- Se você tem dois bancos (por exemplo MySQL e SQL Server) prefira ter um `DbContext` por banco e/ou um projeto `Persistencia` por banco (`Persistencia.MYSQL`, `Persistencia.SqlServer`).
- Alternativamente, mantenha `DbContext`s no mesmo projeto mas configure `MigrationsAssembly` e `MigrationsHistoryTable` ao configurar o provider para separar os históricos:

```csharp
options.UseMySql(connString, serverVersion, o =>
	o.MigrationsAssembly("Retaguarda.Persistencia")
	 .MigrationsHistoryTable("__EFMigrationsHistory_MYSQL")
);
```

Sobre os arquivos `.Designer.cs` e o snapshot
- O EF gera para cada migration um par de arquivos: `0000XX_Nome.cs` (a classe `Migration`) e `0000XX_Nome.Designer.cs` (contém `BuildTargetModel` / o snapshot parcial). Há também o `ApplicationDbContextModelSnapshot.cs` que representa o modelo atual do projeto.
- Normalmente você NÃO precisa editar os `.Designer.cs` — use `dotnet ef migrations add` para que o EF gere corretamente a migration e o designer.
- Se você **criar migrations manualmente**, deve garantir que a classe `Migration`, o `.Designer.cs` (ou o snapshot) e o `ApplicationDbContextModelSnapshot.cs` reflitam o estado do modelo para que futuras migrations funcionem corretamente.

Boas práticas rápidas
- Use `--output-dir` para manter migrations organizadas por banco (ex.: `Migracoes/MYSQL`).
- Use nomes numerados (`000001_...`, `000002_...`) se quiser um esquema de versionamento legível.
- Prefira `dotnet ef migrations add 00000X_nome` para que a ferramenta gere arquivos com o nome correto — evita editar atributos manualmente.

Problemas comuns
- Erro: "dotnet ef not found" → execute o passo 3 e reinicie o terminal.
- Erro de compilação ao rodar `dotnet ef` → verifique se o `startup project` compila e se o namespace/assembly onde `ApplicationDbContext` está definido é referenciado corretamente no `Program.cs`.
- Erro de conexão MySQL → verifique se o serviço MySQL está ativo e se as credenciais em `appsettings.json` estão corretas.

Exemplo: criar e aplicar migration numerada para MySQL

```powershell
cd src\retaguarda\Api
dotnet restore
dotnet ef migrations add 000002_adiciona_setor -p ..\Persistencia\ -s . --context ApplicationDbContext --output-dir Migracoes/MYSQL
dotnet ef database update -p ..\Persistencia\ -s . --context ApplicationDbContext
```

Se quiser, eu posso gerar e aplicar uma migration numerada agora (`000002_adiciona_setor`) com base nas alterações atuais do modelo — quer que eu faça isso? 
