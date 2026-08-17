# 🔄 Pipeline de Integração e Implantação Contínuos

**Status:** Template - Customizar conforme sua infraestrutura  
**Objetivo:** Automatizar testes, construção, e implantação

---

## 📋 Índice

- [Overview](#overview)
- [GitHub Actions (Recomendado)](#github-actions-recomendado)
- [Azure Pipelines](#azure-pipelines)
- [GitLab CI](#gitlab-ci)
- [Configuração de Secrets](#configuração-de-secrets)
- [Workflows](#workflows)

---

## 🚀 Visão Geral

### **Pipeline Desejado**

```
Push de Código → Construção → Teste → Verificação de Segurança → Docker Construção →
  → Implantação Dev → Implantação Preparação → Aprovação Prod → Implantação Prod
```

### **Componentes**

| Estágio | Objetivo | Tempo |
|---------|----------|-------|
| **Construção** | Compilar código | 3-5 min |
| **Teste** | Rodar testes unitários | 3-5 min |
| **SonarQube** | Análise de qualidade | 2-3 min |
| **Construção Docker** | Criar imagens | 5-10 min |
| **Push Registro** | Enviar para Docker Hub/ACR | 2-3 min |
| **Implantação Dev** | Implantação automática | 2-3 min |
| **Implantação Preparação** | Implantação automática | 2-3 min |
| **Implantação Prod** | Requer aprovação manual | conforme demanda |

**Total:** ~25-35 minutos (dev→preparação), +manual (prod)

---

## 🐙 GitHub Actions (Recomendado)

### **Setup**

1. Ir para: `.github/fluxos/`
2. Criar arquivos conforme abaixo

### **1. Construção e Teste (.github/fluxos/construcao-teste.yml)**

```yaml
name: 🏗️ Construção & Teste

on:
  push:
    branches: [ main, develop, 'feature/**' ]
  pull_request:
    branches: [ main, develop ]

env:
  DOTNET_VERSION: '9.0.x'
  NODE_VERSION: '18.x'

jobs:
  build-api:
    runs-on: ubuntu-latest
    name: 🔩 Construir API

    steps:
      - uses: actions/checkout@v4
        with:
          fetch-depth: 0  # Full history for versioning

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: ${{ env.DOTNET_VERSION }}

      - name: Cache NuGet
        uses: actions/cache@v3
        with:
          path: ~/.nuget/packages
          key: ${{ runner.os }}-nuget-${{ hashFiles('**/packages.lock.json') }}
          restore-keys: ${{ runner.os }}-nuget-

      - name: Restore dependencies
        run: dotnet restore src/retaguarda/Api/

      - name: Build
        run: dotnet build src/retaguarda/Api/ -c Release --no-restore

      - name: Run tests
        run: dotnet test src/retaguarda/Api.Tests/ -c Release --no-build --logger "trx"

      - name: Upload test results
        if: always()
        uses: actions/upload-artifact@v3
        with:
          name: test-results
          path: '**/*.trx'

      - name: Publish test results
        if: always()
        uses: EnricoMi/publish-unit-test-result-action@v2
        with:
          files: '**/*.trx'

  build-elsa:
    runs-on: ubuntu-latest
    name: 🔩 Construir Elsa

    steps:
      - uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: ${{ env.DOTNET_VERSION }}

      - name: Restore & Build
        run: |
          dotnet restore src/retaguarda/Retaguarda.PlanejadorFluxo/
          dotnet build src/retaguarda/Retaguarda.PlanejadorFluxo/ -c Release --no-restore

      - name: Run tests
        run: dotnet test src/retaguarda/Elsa.Tests/ -c Release --logger "trx"

  build-frontend:
    runs-on: ubuntu-latest
    name: 🔩 Construir Frontend

    steps:
      - uses: actions/checkout@v4

      - name: Setup Node
        uses: actions/setup-node@v3
        with:
          node-version: ${{ env.NODE_VERSION }}
          cache: 'npm'
          cache-dependency-path: src/interface_grafica/web/package-lock.json

      - name: Install dependencies
        run: npm install
        working-directory: src/interface_grafica/web

      - name: Build
        run: npm run build
        working-directory: src/interface_grafica/web

      - name: Lint
        run: npm run lint
        working-directory: src/interface_grafica/web

      - name: Upload artifact
        uses: actions/upload-artifact@v3
        with:
          name: frontend-dist
          path: src/interface_grafica/web/dist/
```

### **2. Verificação de Segurança (.github/fluxos/seguranca.yml)**

```yaml
name: 🔐 Verificação de Segurança

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]

jobs:
  sonarqube:
    runs-on: ubuntu-latest
    name: 📋 Análise SonarQube

    steps:
      - uses: actions/checkout@v4
        with:
          fetch-depth: 0

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '9.0.x'

      - name: Install SonarQube Scanner
        run: dotnet tool install --global dotnet-sonarscanner

      - name: Begin SonarQube
        env:
          SONAR_TOKEN: ${{ secrets.SONAR_TOKEN }}
          SONAR_HOST_URL: ${{ secrets.SONAR_HOST_URL }}
        run: |
          dotnet sonarscanner begin \
            /k:"seu-org_grp" \
            /d:sonar.host.url="${SONAR_HOST_URL}" \
            /d:sonar.login="${SONAR_TOKEN}"

      - name: Build
        run: dotnet build src/retaguarda/ -c Release

      - name: End SonarQube
        env:
          SONAR_TOKEN: ${{ secrets.SONAR_TOKEN }}
        run: dotnet sonarscanner end /d:sonar.login="${SONAR_TOKEN}"

  dependency-check:
    runs-on: ubuntu-latest
    name: 🔍 Verificação de Dependências

    steps:
      - uses: actions/checkout@v4

      - name: Run dependency-check
        uses: dependency-check/Dependency-Check_Action@main
        with:
          path: '.'
          format: 'JSON'
          args: >
            --enable-retired

      - name: Upload results
        uses: actions/upload-artifact@v3
        with:
          name: dependency-check-results
          path: reports/
```

### **3. Construção e Push do Docker (.github/fluxos/docker.yml)**

```yaml
name: 🐲 Construção de Imagem Docker & Push

on:
  push:
    branches: [ main, develop ]
    tags: [ 'v*' ]

env:
  REGISTRY: ${{ secrets.REGISTRY_URL }}  # seu-org.azurecr.io

jobs:
  docker-api:
    runs-on: ubuntu-latest
    name: 🐲 Construir Imagem da API

    permissions:
      contents: read
      packages: write

    steps:
      - uses: actions/checkout@v4

      - name: Set up Docker Buildx
        uses: docker/setup-buildx-action@v2

      - name: Login to Registry
        uses: docker/login-action@v2
        with:
          registry: ${{ env.REGISTRY }}
          username: ${{ secrets.REGISTRY_USERNAME }}
          password: ${{ secrets.REGISTRY_PASSWORD }}

      - name: Extract metadata
        id: meta
        uses: docker/metadata-action@v4
        with:
          images: ${{ env.REGISTRY }}/api
          tags: |
            type=ref,event=branch
            type=semver,pattern={{version}}
            type=semver,pattern={{major}}.{{minor}}
            type=sha

      - name: Build and push
        uses: docker/build-push-action@v4
        with:
          context: .
          file: ./Dockerfile.api
          push: ${{ github.event_name != 'pull_request' }}
          tags: ${{ steps.meta.outputs.tags }}
          labels: ${{ steps.meta.outputs.labels }}
          cache-from: type=gha
          cache-to: type=gha,mode=max

  docker-elsa:
    runs-on: ubuntu-latest
    name: 🐲 Construir Imagem do Elsa
    
    # ... similar ao acima, mas com Dockerfile.elsa

  docker-frontend:
    runs-on: ubuntu-latest
    name: 🐲 Construir Imagem do Frontend
    
    # ... similar ao acima, mas com Dockerfile.frontend
```

### **4. Implantação em Desenvolvimento (.github/fluxos/implantar-dev.yml)**

```yaml
name: 🚀 Implantar em Desenvolvimento

on:
  push:
    branches: [ develop ]
  workflow_run:
    workflows: [ "🐲 Construção de Imagem Docker & Push" ]
    types: [ completed ]

jobs:
  deploy:
    runs-on: ubuntu-latest
    if: ${{ github.event.workflow_run.conclusion == 'success' }}

    steps:
      - uses: actions/checkout@v4

      - name: Deploy to Dev (Docker Compose)
        env:
          DEPLOY_HOST: ${{ secrets.DEV_HOST }}
          DEPLOY_USER: ${{ secrets.DEV_USER }}
          DEPLOY_KEY: ${{ secrets.DEV_SSH_KEY }}
        run: |
          mkdir -p ~/.ssh
          echo "$DEPLOY_KEY" > ~/.ssh/id_rsa
          chmod 600 ~/.ssh/id_rsa
          
          ssh -o StrictHostKeyChecking=no $DEPLOY_USER@$DEPLOY_HOST << 'EOF'
            cd /opt/grp-dev
            docker-compose pull
            docker-compose up -d
            docker-compose exec api dotnet ef database update
          EOF

      - name: Verificação de Saúde
        env:
          DEPLOY_HOST: ${{ secrets.DEV_HOST }}
        run: |
          sleep 10
          curl -f http://${{ secrets.DEV_HOST }}/api/health || exit 1
```

### **5. Implantação em Preparação (.github/fluxos/implantar-preparacao.yml)**

```yaml
name: 📋 Implantar em Preparação

on:
  push:
    branches: [ main ]

jobs:
  deploy:
    runs-on: ubuntu-latest

    steps:
      - uses: actions/checkout@v4

      - name: Deploy to Staging
        env:
          DEPLOY_HOST: ${{ secrets.STAGING_HOST }}
          DEPLOY_USER: ${{ secrets.STAGING_USER }}
          DEPLOY_KEY: ${{ secrets.STAGING_SSH_KEY }}
        run: |
          # Similar ao dev, mas para staging
          
      - name: Run Smoke Tests
        run: |
          npm install --prefix tests/e2e
          npm run test:preparacao --prefix tests/e2e
```

### **6. Implantação em Produção (Manual) (.github/fluxos/implantar-prod.yml)**

```yaml
name: 🔴 Implantar em Produção

on:
  workflow_dispatch:  # Manual trigger
    inputs:
      version:
        description: 'Versão a implantar (ex: v1.0.0)'
        required: true
        type: string

jobs:
  approval:
    runs-on: ubuntu-latest
    name: ✋ Porta de Aprovação

    steps:
      - name: Aguardar aprovação
        id: approval
        uses: trstringer/manual-approval@v1
        with:
          secret: ${{ github.TOKEN }}
          approvers: ${{ secrets.PROD_APPROVERS }}  # usuarios,separados,virgula
issue-title: 'Implantar ${{ github.event.inputs.version }} em Produção'
        issue-body: 'Por favor, revise e aprove esta implantação em produção'

  deploy:
    runs-on: ubuntu-latest
    needs: approval

    steps:
      - uses: actions/checkout@v4
        with:
          ref: ${{ github.event.inputs.version }}

      - name: Deploy to Production
        env:
          DEPLOY_HOST: ${{ secrets.PROD_HOST }}
          DEPLOY_USER: ${{ secrets.PROD_USER }}
          DEPLOY_KEY: ${{ secrets.PROD_SSH_KEY }}
        run: |
          # Backup antes de deploy
          ssh $DEPLOY_USER@$DEPLOY_HOST \
            "cd /opt/grp-prod && ./backup.sh"
          
          # Deploy
          ssh $DEPLOY_USER@$DEPLOY_HOST << 'EOF'
            cd /opt/grp-prod
            docker-compose pull
            docker-compose up -d --scale api=3
            sleep 10
            docker-compose exec api dotnet ef database update
          EOF

      - name: Smoke Tests
        run: |
          # Validar que produção está UP
          curl -f https://seu-dominio.com/api/health

      - name: Notify Slack
        if: success()
        uses: slackapi/slack-github-action@v1
        with:
          webhook-url: ${{ secrets.SLACK_WEBHOOK }}
          payload: |
            {
              "text": "✅ Deployment ${{ github.event.inputs.version }} successful!",
              "channel": "#deployments"
            }
```

---

## ☁️ Azure Pipelines

### **.azure-pipelines/pipeline.yml**

```yaml
trigger:
  branches:
    include:
      - main
      - develop
      - feature/*

pr:
  branches:
    include:
      - main
      - develop

pool:
  vmImage: 'ubuntu-latest'

variables:
  buildConfiguration: 'Release'
  dotnetVersion: '9.0.x'
  nodeVersion: '18.x'

stages:
  - stage: Build
    displayName: Build & Test
    jobs:
      - job: BuildAPI
        displayName: Build API
        steps:
          - task: UseDotNet@2
            inputs:
              version: $(dotnetVersion)

          - task: DotNetCoreCLI@2
            displayName: Restore
            inputs:
              command: 'restore'
              projects: 'src/retaguarda/Api/*.csproj'

          - task: DotNetCoreCLI@2
            displayName: Build
            inputs:
              command: 'build'
              arguments: '--configuration $(buildConfiguration)'

          - task: DotNetCoreCLI@2
            displayName: Test
            inputs:
              command: 'test'
              arguments: '--configuration $(buildConfiguration) --logger trx'

          - task: PublishTestResults@2
            inputs:
              testResultsFormat: 'VSTest'
              testResultsFiles: '**/*.trx'

  - stage: Docker
    displayName: Build & Push Docker
    dependsOn: Build
    condition: succeeded()
    jobs:
      - job: BuildDocker
        displayName: Build Docker Images
        steps:
          - task: Docker@2
            displayName: Build API Image
            inputs:
              command: build
              Dockerfile: Dockerfile.api
              tags: |
                $(Build.Repository.Name):latest
                $(Build.Repository.Name):$(Build.BuildId)

          - task: Docker@2
            displayName: Push to Registry
            inputs:
              command: push
              containerRegistry: 'Docker Hub'
              repository: $(Build.Repository.Name)
              tags: |
                latest
                $(Build.BuildId)

  - stage: DeployDev
    displayName: Deploy to Dev
    dependsOn: Docker
    condition: succeeded()
    jobs:
      - deployment: DeployDev
        environment: 'dev'
        strategy:
          runOnce:
            deploy:
              steps:
                - task: SSH@0
                  inputs:
                    sshEndpoint: 'dev-server'
                    runOptions: 'commands'
                    commands: |
                      cd /opt/grp-dev
                      docker-compose pull
                      docker-compose up -d

  - stage: DeployProd
    displayName: Deploy to Production
    dependsOn: DeployDev
    condition: and(succeeded(), eq(variables['build.sourceBranch'], 'refs/heads/main'))
    jobs:
      - deployment: ApproveProd
        environment: 'prod'
        strategy:
          runOnce:
            deploy:
              steps:
                - task: SSH@0
                  inputs:
                    sshEndpoint: 'prod-server'
                    runOptions: 'commands'
                    commands: |
                      cd /opt/grp-prod
                      ./backup.sh
                      docker-compose pull
                      docker-compose up -d
```

---

## 🔐 Configuração de Secrets

### **GitHub Secrets**

```bash
# Settings > Secrets > Actions
REGISTRY_URL=seu-org.azurecr.io
REGISTRY_USERNAME=seu-usuario
REGISTRY_PASSWORD=seu-pat-token

DEV_HOST=dev.seu-dominio.com
DEV_USER=deploy
DEV_SSH_KEY=<chave-ssh-privada>

STAGING_HOST=preparacao.seu-dominio.com
STAGING_USER=deploy
STAGING_SSH_KEY=<chave-ssh-privada>

PROD_HOST=prod.seu-dominio.com
PROD_USER=deploy
PROD_SSH_KEY=<chave-ssh-privada>
PROD_APPROVERS=usuario1,usuario2

SONAR_HOST_URL=https://sonarqube.seu-dominio.com
SONAR_TOKEN=<token-sonarqube>

SLACK_WEBHOOK=https://hooks.slack.com/services/...
```

### **Como Gerar SSH Key**

```bash
# Gerar chave
ssh-keygen -t rsa -b 4096 -f ~/.ssh/github_actions -N ""

# Adicionar chave pública ao servidor
ssh-copy-id -i ~/.ssh/github_actions.pub deploy@dev.seu-dominio.com

# Copiar chave privada para GitHub Secrets
cat ~/.ssh/github_actions | pbcopy  # macOS
cat ~/.ssh/github_actions | xclip   # Linux
```

---

## 🔄 Fluxos de Trabalho Recomendados

### **Iniciante**

```
✅ Build + Test (todas as branches)
✅ Deploy Dev (automático em develop)
⚠️ Deploy Prod (manual em main)
```

### **Intermediário**

```
✅ Construção + Teste (todas as ramificações)
✅ Verificação de Segurança (solicitações de pull)
✅ Construção Docker (develop + main)
✅ Implantação Dev (automática)
✅ Implantação Preparação (automática em main)
✅ Implantação Prod (manual com aprovação)
```

### **Avançado**

```
✅ Construção + Teste + SonarQube (todas)
✅ Verificação de Segurança (Verificação de Dependências + Trivy)
✅ Construção Docker (multi-arquíitetura)
✅ Implantação Dev/Preparação/Prod (completo)
✅ Smoke Tests pós-deployment
✅ Notificações (Slack, Teams, Email)
✅ Rollback automático se falhar
✅ Métricas e alertas
```

---

## ✅ Próximos Passos

1. [ ] Escolher provedor (GitHub Actions, Azure Pipelines, GitLab CI)
2. [ ] Criar arquivo YAML
3. [ ] Adicionar secrets
4. [ ] Testar pipeline com push
5. [ ] Configurar aprovações para produção
6. [ ] Adicionar notificações (Slack/Teams)
7. [ ] Documentar fluxo no README

---

**Referências:**
- [GitHub Actions Docs](https://docs.github.com/en/actions)
- [Azure Pipelines Docs](https://docs.microsoft.com/en-us/azure/devops/pipelines)
- [Docker Build Action](https://github.com/docker/build-push-action)
