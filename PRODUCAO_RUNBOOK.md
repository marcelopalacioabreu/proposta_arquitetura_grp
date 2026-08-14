# 📘 PRODUCAO_RUNBOOK.md - Operação em Produção

**Status:** Use como referência diária  
**Escopo:** Como operar, debugar e recuperar de falhas em produção  

---

## 🎯 Índice Rápido

- [Startup & Health Checks](#-startup--health-checks)
- [Comandos Essenciais](#-comandos-essenciais)
- [Debugging de Problemas Comuns](#-debugging-de-problemas-comuns)
- [Performance & Escalabilidade](#-performance--escalabilidade)
- [Backup & Disaster Recovery](#-backup--disaster-recovery)
- [Monitoramento & Alertas](#-monitoramento--alertas)

---

## 🔧 Inicialização & Verificação de Saúde

### **Status Geral**

```bash
# Ver status de todos os containers
docker-compose -f docker-compose.prod.yml ps

# Esperado:
# NAME         STATUS
# grp-nginx    Up 2 hours (healthy)
# grp-api      Up 2 hours (healthy)
# grp-elsa     Up 2 hours (healthy)
# grp-postgres Up 2 hours (healthy)
```

### **Verificação de Saúde Detalhada**

```bash
# API
curl -s http://seu-dominio.com/api/health | jq .
# Resposta esperada: { "status": "Healthy" }

# Elsa
curl -s http://seu-dominio.com/api/elsa/health | jq .

# Database
docker-compose -f docker-compose.prod.yml exec postgres \
  psql -U postgres -d retaguarda -c "SELECT 1;"

# Nginx
curl -s -I http://seu-dominio.com | head -1
# Esperado: HTTP/1.1 200 OK
```

### **Iniciar Serviços (Ordem Importante)**

```bash
# Se todos estão offline:

cd /opt/grp-prod

# 1. Banco de dados primeiro
docker-compose -f docker-compose.prod.yml up -d postgres
sleep 30  # Aguardar inicialização

# 2. Serviços de aplicação
docker-compose -f docker-compose.prod.yml up -d api elsa

# 3. Frontend + Proxy
docker-compose -f docker-compose.prod.yml up -d frontend nginx

# 4. Verificar
docker-compose -f docker-compose.prod.yml ps
```

### **Parar Serviços (Ordem Inversa)**

```bash
# Se precisar parar (maintenance, updates):

cd /opt/grp-prod

# 1. Parar proxy (aceita requisições queued)
docker-compose -f docker-compose.prod.yml stop nginx

# 2. Parar apps (permitem conexões existentes terminar)
docker-compose -f docker-compose.prod.yml stop api elsa

# 3. Por último, banco de dados
docker-compose -f docker-compose.prod.yml stop postgres

# 4. Verificar que tudo parou
docker-compose -f docker-compose.prod.yml ps
```

---

## 💻 Comandos Essenciais

### **Logs em Tempo Real**

```bash
# Todos os serviços
docker-compose -f docker-compose.prod.yml logs -f --tail=50

# Apenas API
docker-compose -f docker-compose.prod.yml logs -f api --tail=100

# Apenas erros
docker-compose -f docker-compose.prod.yml logs api 2>&1 | grep -i error | tail -50

# Com timestamps
docker-compose -f docker-compose.prod.yml logs -f --timestamps

# Últimas 100 linhas de cada serviço
for svc in api elsa nginx postgres; do
  echo "=== $svc ==="
  docker-compose -f docker-compose.prod.yml logs --tail=20 $svc
done
```

### **Performance & Recursos**

```bash
# CPU, memória, I/O de todos containers
docker stats --no-stream

# Esperados (aprox):
# Container          CPU      Mem
# grp-api           5-10%    300-500MB
# grp-elsa          3-5%     200-400MB
# grp-postgres      2-5%     400-600MB
# grp-nginx         0.1%     20-50MB

# Se algum está acima, investigar com:
docker top <container_name>
docker inspect <container_name> | jq '.[0].State'
```

### **Reiniciar Serviços**

```bash
# Reiniciar apenas um (sem downtime para outros)
docker-compose -f docker-compose.prod.yml restart api

# Forçar rebuild + restart (se código mudou)
docker-compose -f docker-compose.prod.yml up -d --build api

# Aguardar saúde
docker-compose -f docker-compose.prod.yml ps | grep api
```

### **Executar Comandos Dentro de Container**

```bash
# Terminal interativo na API
docker-compose -f docker-compose.prod.yml exec api bash

# Rodar migrations
docker-compose -f docker-compose.prod.yml exec api \
  dotnet ef database update --project src/retaguarda/Persistencia/

# Verificar variáveis de ambiente
docker-compose -f docker-compose.prod.yml exec api env | grep -i jwt

# Executar SQL no Postgres
docker-compose -f docker-compose.prod.yml exec postgres \
  psql -U postgres -d retaguarda -c "SELECT version();"
```

---

## 🔍 Debugging de Problemas Comuns

### **Site Está Offline / Devolve 503**

```bash
# 1. Verificar containers
docker-compose -f docker-compose.prod.yml ps

# Se algum tem "Exit X":
docker-compose -f docker-compose.prod.yml logs <container>

# 2. Verificar logs do nginx
docker-compose -f docker-compose.prod.yml logs nginx | grep -i error | tail -20

# 3. Verificar conectividade API
docker-compose -f docker-compose.prod.yml exec nginx \
  curl -v http://api:5000/api/health

# 4. Reiniciar o serviço problemático
docker-compose -f docker-compose.prod.yml restart <container>

# 5. Se ainda offline, ver logs detalhados
docker-compose -f docker-compose.prod.yml logs <container> --tail=100
```

### **Erro de Login / Autenticação**

```bash
# 1. Verificar JWT Key está definida
docker-compose -f docker-compose.prod.yml exec api env | grep JWT_KEY

# 2. Verificar DataProtection Keys
docker exec grp-api ls -la /app/data-protection-keys/

# 3. Testar login direto (sem proxy)
docker-compose -f docker-compose.prod.yml exec api \
  curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin"}'

# 4. Verificar cookie no navegador (DevTools > Application > Cookies)
# Procurar por: access_token
# Deve ter: HttpOnly ✓, Secure ✓ (prod), SameSite=Lax ✓

# 5. Se cookie não está sendo enviado, verificar Domain:
# Domain deve estar vazio (implícito = mesmo host)
```

### **Fluxos de Trabalho não Executam / Erro com Elsa**

```bash
# 1. Verificar saúde do Elsa
curl -s http://seu-dominio.com/api/elsa/health | jq .

# 2. Verificar conectividade API -> Elsa
docker-compose -f docker-compose.prod.yml exec api \
  curl -v http://elsa:6001/api/health

# 3. Verificar logs do Elsa
docker-compose -f docker-compose.prod.yml logs elsa --tail=100 | grep -i error

# 4. Verificar atividades registradas
docker-compose -f docker-compose.prod.yml exec elsa bash
# Dentro do container:
find /app -name "*Activity.cs" | head -20

# 5. Reiniciar Elsa
docker-compose -f docker-compose.prod.yml restart elsa
```

### **Erro de Banco de Dados**

```bash
# 1. Verificar saúde do Postgres
docker-compose -f docker-compose.prod.yml exec postgres \
  pg_isready -U postgres

# 2. Conectar e testar
docker-compose -f docker-compose.prod.yml exec postgres \
  psql -U postgres -d retaguarda -c "SELECT COUNT(*) FROM organizacoes;"

# 3. Ver tamanho da base
docker-compose -f docker-compose.prod.yml exec postgres \
  psql -U postgres -d retaguarda -c "\l+" | grep retaguarda

# 4. Ver conexões ativas
docker-compose -f docker-compose.prod.yml exec postgres \
  psql -U postgres -d retaguarda -c "SELECT pid, usename, state FROM pg_stat_activity;"

# 5. Se conexões presas, matar:
docker-compose -f docker-compose.prod.yml exec postgres \
  psql -U postgres -d retaguarda -c "SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = 'retaguarda' AND pid <> pg_backend_pid();"
```

### **Alto Uso de Memória**

```bash
# 1. Ver qual container consome mais
docker stats --no-stream | sort -k4 -rh

# 2. Se é a API:
docker-compose -f docker-compose.prod.yml logs api | grep -i "OutOfMemory"

# 3. Aumentar memória (edit docker-compose.prod.yml)
# Adicionar em serviço:
# deploy:
#   resources:
#     limits:
#       memory: 1G

# 4. Reiniciar
docker-compose -f docker-compose.prod.yml up -d
```

### **Certificado SSL Expirado / Erro HTTPS**

```bash
# 1. Verificar expiração
openssl s_client -connect seu-dominio.com:443 -servername seu-dominio.com 2>/dev/null | \
  openssl x509 -noout -dates

# 2. Renovar com certbot (Let's Encrypt)
certbot renew --force-renewal --email seu-email@dominio.com

# 3. Verificar após renovar
openssl s_client -connect seu-dominio.com:443 -servername seu-dominio.com 2>/dev/null | \
  openssl x509 -noout -dates
```

### **Erro 404 em Rotas da API**

```bash
# 1. Verificar se endpoint existe
docker-compose -f docker-compose.prod.yml exec api bash
# Dentro:
grep -r "api/minhas-organizacoes" --include="*.cs" src/

# 2. Testar direto na API (sem proxy)
curl -s http://localhost:5000/api/minhas-organizacoes \
  -H "Authorization: Bearer <JWT_TOKEN>"

# 3. Testar através do proxy
curl -s http://seu-dominio.com/api/minhas-organizacoes \
  -H "Authorization: Bearer <JWT_TOKEN>"

# 4. Se funciona direto mas não no proxy, problema no nginx.conf
docker-compose -f docker-compose.prod.yml logs nginx
```

---

## 📊 Performance & Escalabilidade

### **Benchmarks Esperados**

| Métrica | Esperado | Alert > |
|---------|----------|---------|
| Response Time (API) | 100-500ms | 2000ms |
| CPU API | 5-10% | 80% |
| Memória API | 300-500MB | 800MB |
| DB Connections | 5-20 | 50 |
| Requests/Segundo | 100-500 | 1000 |
| Erro Rate | < 1% | > 5% |

### **Teste de Carga (Apache Bench)**

```bash
# Teste rápido (1000 requisições, 10 concorrentes)
ab -n 1000 -c 10 https://seu-dominio.com/api/minhas-organizacoes

# Teste prolongado (5 min, 50 concorrentes)
ab -t 300 -c 50 https://seu-dominio.com/api/minhas-organizacoes

# Com autenticação (usando curl + jq)
TOKEN=$(curl -s -X POST https://seu-dominio.com/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin"}' | jq -r '.accessToken')

ab -n 1000 -c 10 \
  -H "Authorization: Bearer $TOKEN" \
  https://seu-dominio.com/api/minhas-organizacoes
```

### **Aumentar Capacidade**

```bash
# 1. Aumentar limites de recurso (docker-compose.prod.yml)
services:
  api:
    deploy:
      resources:
        limits:
          cpus: '2'
          memory: 2G
        reservations:
          cpus: '1'
          memory: 1G

# 2. Scale horizontal (múltiplas instâncias com load balancer)
# Requer mudança em nginx.conf:
upstream api {
  server api-1:5000;
  server api-2:5000;
  server api-3:5000;
}

# 3. Aumentar rate limits em nginx.conf
limit_req_zone $binary_remote_addr zone=api_limit:10m rate=10r/s;

# 4. Implementar caching
proxy_cache_path /var/cache/nginx levels=1:2 keys_zone=api_cache:10m;
location /api/ {
  proxy_cache api_cache;
  proxy_cache_valid 200 10m;
}
```

### **Connection Pooling**

```bash
# Verificar pool de conexões de BD
docker-compose -f docker-compose.prod.yml exec postgres \
  psql -U postgres -d retaguarda -c "\
    SELECT datname, usename, count(*) 
    FROM pg_stat_activity 
    GROUP BY datname, usename;"

# Se muitas conexões presas, aumentar timeout em appsettings.json
# "ConnectionTimeout": 30,
# "CommandTimeout": 300,
```

---

## 💾 Backup & Disaster Recovery

### **Backup Manual (Imediato)**

```bash
# 1. Backup do Postgres
docker-compose -f docker-compose.prod.yml exec postgres \
  pg_dump -U postgres retaguarda > backup-$(date +%Y%m%d-%H%M%S).sql

# 2. Backup das chaves DataProtection
tar -czf backup-keys-$(date +%Y%m%d-%H%M%S).tar.gz /opt/grp-prod/data-protection-keys/

# 3. Backup da config
tar -czf backup-config-$(date +%Y%m%d-%H%M%S).tar.gz \
  /opt/grp-prod/.env.prod \
  /opt/grp-prod/docker-compose.prod.yml \
  /opt/grp-prod/nginx-prod.conf

# 4. Armazenar em local seguro
aws s3 cp backup-*.sql s3://seu-bucket-backup/
aws s3 cp backup-*.tar.gz s3://seu-bucket-backup/
```

### **Backup Automatizado (Cron)**

```bash
# Editar crontab
crontab -e

# Adicionar (backup diário às 2:00 AM)
0 2 * * * /opt/grp-prod/backup.sh

# Criar script: /opt/grp-prod/backup.sh
#!/bin/bash
set -e

BACKUP_DIR="/backup/grp"
DATE=$(date +%Y%m%d-%H%M%S)

# Backup DB
docker-compose -f /opt/grp-prod/docker-compose.prod.yml exec -T postgres \
  pg_dump -U postgres retaguarda | gzip > "$BACKUP_DIR/db-$DATE.sql.gz"

# Backup keys
tar -czf "$BACKUP_DIR/keys-$DATE.tar.gz" /opt/grp-prod/data-protection-keys/

# Upload to S3
aws s3 sync "$BACKUP_DIR" s3://seu-bucket-backup/

# Remover backups locais com mais de 30 dias
find "$BACKUP_DIR" -mtime +30 -delete

echo "Backup completed: $DATE"
```

### **Restore from Backup**

```bash
# 1. Se BD corrompida
docker-compose -f docker-compose.prod.yml stop api elsa

# 2. Restaurar backup
docker-compose -f docker-compose.prod.yml exec postgres \
  psql -U postgres -d retaguarda < backup-20260814-020000.sql

# 3. Reiniciar serviços
docker-compose -f docker-compose.prod.yml up -d api elsa

# 4. Verificar
docker-compose -f docker-compose.prod.yml exec postgres \
  psql -U postgres -d retaguarda -c "SELECT COUNT(*) FROM organizacoes;"
```

### **Disaster Recovery (Servidor Inteiro Perdido)**

```bash
# 1. Provisionar novo servidor com mesma especificação
# (Docker, Docker Compose, etc)

# 2. Copiar arquivos de backup
aws s3 cp s3://seu-bucket-backup/db-latest.sql.gz .
aws s3 cp s3://seu-bucket-backup/keys-latest.tar.gz .

# 3. Restaurar estrutura
mkdir -p /opt/grp-prod/data-protection-keys
tar -xzf keys-latest.tar.gz -C /opt/grp-prod/

# 4. Subir containers
docker-compose -f docker-compose.prod.yml up -d

# 5. Restaurar BD
gunzip -c db-latest.sql.gz | \
  docker-compose -f docker-compose.prod.yml exec -T postgres \
  psql -U postgres -d retaguarda

# 6. Verificar
curl -s http://novo-servidor.com/api/health
```

---

## 📈 Monitoramento & Alertas

### **Verificação Manual (Diária)**

```bash
# 1. Executar health check
curl -s http://seu-dominio.com/api/health | jq '.status'

# 2. Verificar recursos
docker stats --no-stream | tail -5

# 3. Verificar logs de erro
for svc in api elsa; do
  echo "=== $svc errors ==="
  docker-compose -f docker-compose.prod.yml logs $svc | grep -i error | tail -5
done

# 4. Verificar alertas de BD
docker-compose -f docker-compose.prod.yml exec postgres \
  psql -U postgres -d retaguarda -c "SELECT * FROM pg_stat_statements ORDER BY mean_time DESC LIMIT 5;"
```

### **Configurar Monitoramento Centralizado (Prometheus + Grafana)**

```yaml
# prometheus.yml
global:
  scrape_interval: 15s

scrape_configs:
  - job_name: 'docker'
    static_configs:
      - targets: ['localhost:8080']

  - job_name: 'api'
    static_configs:
      - targets: ['api:5000']
    metrics_path: '/metrics'

  - job_name: 'postgres'
    static_configs:
      - targets: ['postgres_exporter:9187']
```

```bash
# Adicionar ao docker-compose.prod.yml

services:
  prometheus:
    image: prom/prometheus
    volumes:
      - ./prometheus.yml:/etc/prometheus/prometheus.yml
    ports:
      - "9090:9090"

  grafana:
    image: grafana/grafana
    environment:
      GF_SECURITY_ADMIN_PASSWORD: admin
    ports:
      - "3000:3000"
    depends_on:
      - prometheus

# Acessar: http://localhost:3000 (admin/admin)
```

### **Alertas Críticos (Email/Slack)**

```bash
# Use: AlertManager com Prometheus

# alerting_rules.yml
groups:
  - name: API
    rules:
      - alert: APIDown
        expr: up{job="api"} == 0
        for: 5m
        annotations:
          summary: "API está offline"

      - alert: HighErrorRate
        expr: rate(errors_total[5m]) > 0.05
        annotations:
          summary: "Taxa de erro > 5%"

      - alert: HighMemory
        expr: container_memory_usage_bytes{name="grp-api"} > 800000000
        annotations:
          summary: "API usando > 800MB RAM"
```

### **Logs Centralizados (Serilog + ELK)**

```csharp
// Program.cs (API e Elsa)

var builder = WebApplication.CreateBuilder(args);

// Serilog -> Elasticsearch
builder.Services.AddSerilog(new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(
        new ElasticsearchSinkOptions(
            new Uri("http://elasticsearch:9200"))
        {
            AutoRegisterTemplate = true,
            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
        })
    .CreateLogger());

// Depois acessar: http://kibana:5601
```

---

## 📞 Escalation Path

### **Se problema não pode ser resolvido**

1. **Tomar screenshot/logs** do problema
2. **Ativar modo de DEBUG** (Serilog LogLevel = Debug)
3. **Documentar passos de reprodução**
4. **Contatar time de desenvolvimento com:**
   - Logs completos
   - Timestamp exato do problema
   - Passos para reproduzir
   - Environment vars (sem secrets!)

---

## ✅ Verificação Diária (5 min)

```bash
#!/bin/bash
# daily-check.sh

echo "=== Status de Containers ==="
docker-compose -f docker-compose.prod.yml ps

echo "=== Health Checks ==="
curl -s http://seu-dominio.com/api/health | jq '.status'
curl -s http://seu-dominio.com/api/elsa/health | jq '.status'

echo "=== Recursos ==="
docker stats --no-stream | grep -E "grp-|NAME"

echo "=== Erros Recentes ==="
docker-compose -f docker-compose.prod.yml logs --since 30m | grep -i error

echo "=== Certificado SSL ==="
openssl s_client -connect seu-dominio.com:443 -servername seu-dominio.com 2>/dev/null | \
  openssl x509 -noout -dates

echo "✅ Check Complete!"
```

**Rodar todos os dias:**
```bash
chmod +x /opt/grp-prod/daily-check.sh
0 9 * * * /opt/grp-prod/daily-check.sh | mail -s "GRP Daily Check" ops@dominio.com
```

---

**Próximos arquivos:** [CONTRIBUTING.md](CONTRIBUTING.md) | [CI-CD.md](CI-CD.md)
