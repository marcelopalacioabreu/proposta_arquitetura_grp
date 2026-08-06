# Retaguarda.PlanejadorFluxo

Esqueleto mínimo do projeto Host do Planejador de Fluxos (Elsa).

Passos iniciais:

- Para restaurar e compilar:

```powershell
dotnet restore
dotnet build
```

- Para adicionar Elsa e persistência PostgreSQL, adicione `PackageReference` apropriados ao `.csproj` e então execute `dotnet restore`.
Retaguarda.PlanejadorFluxo
=========================

Projeto mínimo que expõe um proxy para um Elsa Studio/Server existente e compartilha o keyring de DataProtection

Como usar (desenvolvimento):

- Execute a API principal (Retaguarda.Api) normalmente (ex.: `dotnet run --project src/retaguarda/Api`).
- Execute o Elsa Studio/Server em `http://localhost:4500` (padrão) ou ajuste `appsettings.json` `Elsa:BaseUrl`.
- Execute o PlanejadorFluxo (este projeto): `dotnet run --project src/retaguarda/Retaguarda.PlanejadorFluxo` (irá rodar em uma porta aleatória, ex.: 6000).
- No front-end (Vite) já foi adicionado proxy para `/planejadorDeFluxo` apontando para `http://localhost:6000`.

Observações:
- Este projeto apenas implementa um proxy simples. Para integrar diretamente o Elsa Studio no ASP.NET Core consulte a documentação oficial do Elsa.
- As chaves de DataProtection são persistidas em `data-protection-keys` na raiz do workspace para compartilhar com a API.
