# Instruções de desenvolvimento

Essas instruções cobrem pré-requisitos, criação do banco, execução de migrations/seed e como rodar frontend e backend em modo desenvolvimento.

## Pré-requisitos
- MySQL Server (local) — exemplo: MySQL 8.x
- .NET SDK 8.0 (net8.0)
- Node.js 18+ e npm
- (Opcional) `dotnet-ef` se desejar rodar migrations manualmente: `dotnet tool install --global dotnet-ef`

## Configurar banco de dados
1. Crie o banco MySQL usado pela aplicação (exemplo):

```sql
CREATE DATABASE demonstracao_arquitetura CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
```

2. Configure a string de conexão em `src/retaguarda/Api/appsettings.Development.json` ou via variável de ambiente `ConnectionStrings__DefaultConnection` no formato:

```
server=localhost;user=root;password=senha;database=demonstracao_arquitetura;TreatTinyAsBoolean=false;CharSet=utf8mb4;
```

(O arquivo `appsettings.*.json` pode não existir no repositório; usar variáveis de ambiente é recomendado.)

## Aplicar migrations e seed
Você pode usar o script pronto no root do repositório:

- Windows (PowerShell / CMD):

```
iniciar-em-modo-deselvolvimento.bat
```

O script faz:
- `dotnet ef database update` (aplica migrations do projeto Persistencia usando o startup project Api)
- inicia o backend (dotnet run) em uma nova janela
- inicia o frontend (npm install && npm run dev) em outra janela

> Observação: o backend já executa um seeder de desenvolvimento que cria o usuário `admin` com senha `admin` caso não exista.

## Rodando manualmente
1. Aplicar migrations:

```powershell
dotnet ef database update --project src\retaguarda\Persistencia\Retaguarda.Persistencia.csproj --startup-project src\retaguarda\Api\Retaguarda.Api.csproj --context ApplicationDbContext
```

2. Iniciar backend:

```powershell
dotnet run --project src\retaguarda\Api\Retaguarda.Api.csproj
```

3. Iniciar frontend (pasta `interface_grafica/web`):

```bash
cd interface_grafica/web
npm install
npm run dev
```

## Endpoints importantes
- Metadados: `GET /meta/screens`, `GET /meta/modulos`, `GET /meta/components`
- API Organizações: `GET /api/organizacoes`, `POST /api/organizacoes`, `PUT /api/organizacoes/{id}`, `DELETE /api/organizacoes/{id}`
- API Setores: `GET /api/setores`, `POST /api/setores`, `PUT /api/setores/{id}`, `DELETE /api/setores/{id}`

## Notas de segurança
- As credenciais padrão `admin/admin` são apenas para desenvolvimento. Altere em produção.
- Configure `Jwt:Key` e outras secrets via `appsettings` ou gerenciador de segredos.

## Problemas comuns
- Erro ao aplicar migrations: verifique a string de conexão e se o MySQL está acessível.
- Erro de porta já em uso: ajuste `ASPNETCORE_URLS` ou a porta do DevServer do Vite.

---
