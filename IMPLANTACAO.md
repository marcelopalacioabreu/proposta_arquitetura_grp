# 🚀 DEPLOYMENT.md - Guia de Deployment em Produção

**Status:** Recomendado implementar ANTES de ir ao ar  
**Últimas Atualizações:** Baseado em arquitetura documentada  
**Escopo:** API, Elsa, Frontend e Nginx Proxy

---

## 📋 Checklist Pré-Deployment

Antes de fazer deploy em produção, confirme:

- [ ] Código em branch `main` com todas as features testadas
- [ ] Testes automatizados passando 100%
- [ ] Migrations criadas e testadas localmente
- [ ] `.env` production definido com todas as variáveis
- [ ] JWT Secret gerado e armazenado com segurança
- [ ] Certificado SSL/TLS obtido (Let's Encrypt ou CA corporativo)
- [ ] Backup do BD existente (se houver)
- [ ] Plano de rollback preparado
- [ ] Time de operação notificado
- [ ] Janela de manutenção agendada (se necessário)

---

## 🔑 Variáveis de Ambiente em Produção

### Obrigatórias

```bash
# .NET Runtime
ASPNETCORE_ENVIRONMENT=Production

# Banco de Dados
Persistence__Provider=Postgres                    # ou MySQL
Persistence__Connection=Server=db.prod.com;...   # Connection string

# JWT / Autenticação
Jwt__Key=<GERAR-COM-32-BYTES>                    # Use: openssl rand -base64 32
Jwt__Issuer=https://seu-dominio.com
Jwt__Audience=https://seu-dominio.com
Jwt__ExpirationMinutes=60
Jwt__RefreshTokenExpirationDays=30

# Cookies
Jwt__Cookie__Name=access_token
Jwt__Cookie__SameSite=Lax
Jwt__Cookie__Secure=true                         # SEMPRE true em HTTPS

# Elsa (PlanejadorFluxo)
Elsa__ServerUrl=http://elsa:6001                 # Internal network
Elsa__ApiUrl=http://elsa:6001                    # Internal network

# DataProtection Keys (compartilhado entre API e Elsa)
# ⚠️ Usar diretório específico em produção: /app/data-protection-keys/
```

### Opcionais (com valores padrão)

```bash
# Logging
Serilog__MinimumLevel=Information
Logging__LogLevel__Default=Information

# CORS
Cors__AllowedOrigins=https://seu-dominio.com

# Rate Limiting (valores padrão)
RateLimit__RequestsPerMinute=100
RateLimit__PerIpAddress=true

# Timeouts
ConnectionTimeout=30
CommandTimeout=300
```

---

## 🐳 Dockerfiles Oficiais

### 1. **API (Dockerfile.api)**

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/retaguarda/Api/Retaguarda.Api.csproj", "src/retaguarda/Api/"]
COPY ["src/retaguarda/Dominio/Retaguarda.Dominio.csproj", "src/retaguarda/Dominio/"]
COPY ["src/retaguarda/Persistencia/Retaguarda.Persistencia.csproj", "src/retaguarda/Persistencia/"]
COPY ["src/retaguarda/Repositorios/Retaguarda.Repositorios.csproj", "src/retaguarda/Repositorios/"]
COPY ["src/retaguarda/Retaguarda.DTO/Retaguarda.DTO.csproj", "src/retaguarda/Retaguarda.DTO/"]
COPY ["src/retaguarda/Retaguarda.Metadados/Retaguarda.Metadados.csproj", "src/retaguarda/Retaguarda.Metadados/"]
COPY ["src/retaguarda/Servicos/Retaguarda.Servicos.csproj", "src/retaguarda/Servicos/"]

# Restore
RUN dotnet restore "src/retaguarda/Api/Retaguarda.Api.csproj"

# Copy everything else
COPY . .

# Build
RUN dotnet build "src/retaguarda/Api/Retaguarda.Api.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "src/retaguarda/Api/Retaguarda.Api.csproj" -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Create data-protection-keys directory
RUN mkdir -p /app/data-protection-keys

# Copy published app
COPY --from=publish /app/publish .

EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:5000/api/health || exit 1

ENTRYPOINT ["dotnet", "Retaguarda.Api.dll"]
```

**Build e Tag:**
```bash
docker build -f Dockerfile.api -t seu-registro.azurecr.io/api:v1.0.0 .
docker push seu-registro.azurecr.io/api:v1.0.0
```

### 2. **Elsa/PlanejadorFluxo (Dockerfile.elsa)**

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/retaguarda/Retaguarda.PlanejadorFluxo/Retaguarda.PlanejadorFluxo.csproj", "src/retaguarda/Retaguarda.PlanejadorFluxo/"]
COPY ["src/retaguarda/Dominio/Retaguarda.Dominio.csproj", "src/retaguarda/Dominio/"]
COPY ["src/retaguarda/Persistencia/Retaguarda.Persistencia.csproj", "src/retaguarda/Persistencia/"]
COPY ["src/retaguarda/Repositorios/Retaguarda.Repositorios.csproj", "src/retaguarda/Repositorios/"]
COPY ["src/retaguarda/Retaguarda.DTO/Retaguarda.DTO.csproj", "src/retaguarda/Retaguarda.DTO/"]
COPY ["src/retaguarda/Retaguarda.Metadados/Retaguarda.Metadados.csproj", "src/retaguarda/Retaguarda.Metadados/"]
COPY ["src/retaguarda/Servicos/Retaguarda.Servicos.csproj", "src/retaguarda/Servicos/"]

RUN dotnet restore "src/retaguarda/Retaguarda.PlanejadorFluxo/Retaguarda.PlanejadorFluxo.csproj"

COPY . .
RUN dotnet build "src/retaguarda/Retaguarda.PlanejadorFluxo/Retaguarda.PlanejadorFluxo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/retaguarda/Retaguarda.PlanejadorFluxo/Retaguarda.PlanejadorFluxo.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

RUN mkdir -p /app/data-protection-keys

COPY --from=publish /app/publish .

EXPOSE 6001
ENV ASPNETCORE_URLS=http://+:6001

HEALTHCHECK --interval=30s --timeout=10s --start-period=10s --retries=3 \
  CMD curl -f http://localhost:6001/api/health || exit 1

ENTRYPOINT ["dotnet", "Retaguarda.PlanejadorFluxo.dll"]
```

### 3. **Frontend (Dockerfile.frontend)**

```dockerfile
FROM node:18-alpine AS build
WORKDIR /app

COPY src/interface_grafica/web/package*.json ./
RUN npm install

COPY src/interface_grafica/web/ .
RUN npm run build

# Nginx stage
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx-frontend.conf /etc/nginx/conf.d/default.conf

EXPOSE 3000
CMD ["nginx", "-g", "daemon off;"]
```

**nginx-frontend.conf:**
```nginx
server {
    listen 3000;
    server_name _;

    root /usr/share/nginx/html;
    index index.html;

    # SPA: todas as rotas -> index.html
    location / {
        try_files $uri $uri/ /index.html;
    }

    # Não cachear HTML
    location ~* \.html$ {
        add_header Cache-Control "no-cache, no-store, must-revalidate";
    }

    # Cachear assets
    location ~* \.(js|css|png|jpg|jpeg|gif|ico|woff|woff2|ttf)$ {
        add_header Cache-Control "public, max-age=31536000, immutable";
    }

    # Proxy para API
    location /api/ {
        proxy_pass http://nginx-proxy:80;
        proxy_redirect off;
    }
}
```

---

## 🐳 Docker Compose para Produção

### **docker-compose.prod.yml**

```yaml
version: '3.9'

services:
  # PostgreSQL
  postgres:
    image: postgres:14-alpine
    container_name: grp-postgres
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: ${DB_PASSWORD}
      POSTGRES_DB: retaguarda
    volumes:
      - postgres_data:/var/lib/postgresql/data
    networks:
      - backend
    restart: unless-stopped
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U postgres"]
      interval: 10s
      timeout: 5s
      retries: 5

  # API
  api:
    image: ${REGISTRY}/api:${API_VERSION}
    container_name: grp-api
    environment:
      ASPNETCORE_ENVIRONMENT: Production
      Persistence__Provider: Postgres
      Persistence__Connection: Server=postgres;Port=5432;Database=retaguarda;User Id=postgres;Password=${DB_PASSWORD};
      Jwt__Key: ${JWT_KEY}
      Jwt__Issuer: ${JWT_ISSUER}
      Jwt__Audience: ${JWT_AUDIENCE}
      Jwt__Cookie__Secure: "true"
      Jwt__Cookie__SameSite: Lax
      Elsa__ServerUrl: http://elsa:6001
    depends_on:
      postgres:
        condition: service_healthy
    networks:
      - backend
    restart: unless-stopped
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:5000/api/health"]
      interval: 30s
      timeout: 10s
      retries: 3

  # Elsa / PlanejadorFluxo
  elsa:
    image: ${REGISTRY}/elsa:${ELSA_VERSION}
    container_name: grp-elsa
    environment:
      ASPNETCORE_ENVIRONMENT: Production
      Persistence__Provider: Postgres
      Persistence__Connection: Server=postgres;Port=5432;Database=retaguarda;User Id=postgres;Password=${DB_PASSWORD};
      Jwt__Key: ${JWT_KEY}
      Jwt__Issuer: ${JWT_ISSUER}
      Jwt__Audience: ${JWT_AUDIENCE}
    depends_on:
      postgres:
        condition: service_healthy
    networks:
      - backend
    restart: unless-stopped
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:6001/api/health"]
      interval: 30s
      timeout: 10s
      retries: 3

  # Frontend
  frontend:
    image: ${REGISTRY}/frontend:${FRONTEND_VERSION}
    container_name: grp-frontend
    networks:
      - backend
    restart: unless-stopped

  # Nginx Reverse Proxy
  nginx:
    image: nginx:alpine
    container_name: grp-nginx
    ports:
      - "80:80"
      - "443:443"
    volumes:
      - ./nginx-prod.conf:/etc/nginx/nginx.conf:ro
      - ./ssl:/etc/nginx/ssl:ro
    depends_on:
      - api
      - frontend
    networks:
      - backend
    restart: unless-stopped

networks:
  backend:
    driver: bridge

volumes:
  postgres_data:
    driver: local
```

### **`.env.prod`**

```bash
# Registry (Azure, Docker Hub, etc)
REGISTRY=seu-registro.azurecr.io
API_VERSION=v1.0.0
ELSA_VERSION=v1.0.0
FRONTEND_VERSION=v1.0.0

# Database
DB_PASSWORD=SuperSenha123!

# JWT
JWT_KEY=<RESULTADO-DE: openssl rand -base64 32>
JWT_ISSUER=https://seu-dominio.com
JWT_AUDIENCE=https://seu-dominio.com
```

---

## 🚀 Comandos de Deployment

### **Opção 1: Docker Compose (Recomendado para Começar)**

```bash
# 1. Criar diretório de produção
mkdir -p /opt/grp-prod
cd /opt/grp-prod

# 2. Copiar arquivos
cp docker-compose.prod.yml .
cp .env.prod .
cp nginx-prod.conf .

# 3. Gerar JWT Key (IMPORTANTE!)
openssl rand -base64 32 > jwt.key
export JWT_KEY=$(cat jwt.key)

# 4. Editar .env.prod com valores reais
nano .env.prod

# 5. Criar rede (se não existir)
docker network create backend

# 6. Subir stack
docker-compose -f docker-compose.prod.yml up -d

# 7. Verificar status
docker-compose -f docker-compose.prod.yml ps
docker-compose -f docker-compose.prod.yml logs -f

# 8. Executar migrations (primeira vez)
docker-compose -f docker-compose.prod.yml exec api \
  dotnet ef database update --project src/retaguarda/Persistencia/
```

### **Opção 2: Kubernetes**

```bash
# (Requer kubectl configurado)

# 1. Criar namespace
kubectl create namespace grp-prod

# 2. Criar secrets
kubectl create secret generic grp-secrets \
  --from-literal=db-password=$(openssl rand -base64 32) \
  --from-literal=jwt-key=$(openssl rand -base64 32) \
  -n grp-prod

# 3. Aplicar manifests (criar primeiro!)
kubectl apply -f k8s-api-deployment.yml -n grp-prod
kubectl apply -f k8s-elsa-deployment.yml -n grp-prod
kubectl apply -f k8s-frontend-deployment.yml -n grp-prod
kubectl apply -f k8s-nginx-ingress.yml -n grp-prod

# 4. Verificar
kubectl get pods -n grp-prod
kubectl logs -f deployment/api -n grp-prod
```

### **Opção 3: Terraform (IaC)**

```hcl
# main.tf - Deploy em Azure Container Instances (exemplo)

terraform {
  required_providers {
    azurerm = { version = "~> 3.0" }
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_container_group" "grp" {
  name                = "grp-prod-containers"
  location            = "brazilsouth"
  resource_group_name = azurerm_resource_group.grp.name
  os_type             = "Linux"
  ip_address_type     = "Public"

  container {
    name   = "api"
    image  = "${var.registry}/api:${var.api_version}"
    cpu    = "1.0"
    memory = "2.0"
    environment_variables = {
      ASPNETCORE_ENVIRONMENT = "Production"
      # ... more vars
    }
  }

  container {
    name   = "elsa"
    image  = "${var.registry}/elsa:${var.elsa_version}"
    cpu    = "1.0"
    memory = "2.0"
  }

  container {
    name   = "frontend"
    image  = "${var.registry}/frontend:${var.frontend_version}"
    cpu    = "0.5"
    memory = "1.0"
  }

  container {
    name  = "nginx"
    image = "nginx:alpine"
    ports {
      port     = 80
      protocol = "TCP"
    }
  }
}
```

---

## 🔄 Rollback Procedures

### **Se Docker Compose:**

```bash
# 1. Verificar versão atual
docker-compose -f docker-compose.prod.yml ps

# 2. Parar stack problemático
docker-compose -f docker-compose.prod.yml down

# 3. Restaurar versão anterior em .env.prod
nano .env.prod
# Mudar: API_VERSION=v1.0.0-old, etc

# 4. Subir versão anterior
docker-compose -f docker-compose.prod.yml up -d

# 5. Reverter migrations se necessário
docker-compose -f docker-compose.prod.yml exec api \
  dotnet ef database update <PREVIOUS_MIGRATION> --project src/retaguarda/Persistencia/
```

### **Se Kubernetes:**

```bash
# Rollback automático (Kubernetes descobre sozinho)
kubectl rollout history deployment/api -n grp-prod
kubectl rollout undo deployment/api -n grp-prod
```

---

## 📊 Pós-Deployment: Verificação

```bash
# 1. Verificar status dos containers
docker-compose -f docker-compose.prod.yml ps

# 2. Verificar health checks
curl -X GET http://seu-dominio.com/api/health
curl -X GET http://seu-dominio.com/api/ready

# 3. Verificar logs
docker-compose logs -f api
docker-compose logs -f elsa
docker-compose logs -f nginx

# 4. Verificar conexão ao BD
docker-compose exec api dotnet user-secrets get "Persistence__Connection"

# 5. Verificar certificado SSL
openssl s_client -connect seu-dominio.com:443 -servername seu-dominio.com

# 6. Testar login
curl -X POST http://seu-dominio.com/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin"}'
```

---

## 🚨 Troubleshooting em Produção

Veja [PRODUCAO_RUNBOOK.md](PRODUCAO_RUNBOOK.md) para:
- Comandos de debug
- Logs
- Métricas
- Recuperação de falhas comuns

---

## ✅ Próximos Passos

1. [ ] Criar Dockerfiles (use templates acima)
2. [ ] Testar localmente com docker-compose
3. [ ] Gerar certificado SSL (Let's Encrypt + certbot)
4. [ ] Criar infraestrutura em nuvem (Azure, AWS, GCP)
5. [ ] Configurar backups automáticos
6. [ ] Configurar monitoring (Datadog, New Relic, etc)
7. [ ] Executar testes de carga
8. [ ] Fazer deployment in staging
9. [ ] Validar com usuários
10. [ ] Fazer deployment em produção

---

**Próximo arquivo recomendado:** [PRODUCAO_RUNBOOK.md](PRODUCAO_RUNBOOK.md)
