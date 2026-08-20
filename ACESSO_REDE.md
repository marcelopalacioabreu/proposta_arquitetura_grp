# 🌐 Como Acessar o Software em Outra Máquina

## 📋 Configuração Padrão (Este Projeto)

Tanto o **Frontend** (Vite) quanto o **Backend** (ASP.NET) foram configurados para escutar em **0.0.0.0** (todas as interfaces):

### Frontend (Vite)
```javascript
// vite.config.js
server: {
  host: '0.0.0.0',    // Escuta em todas as interfaces
  port: 5173,
  proxy: { /* ... */ }
}
```

### Backend (ASP.NET)
```csharp
// Program.cs
app.Run("http://0.0.0.0:5000");      // API
app.Run("http://0.0.0.0:6001");      // PlanejadorFluxo
```

---

## 🚀 Como Acessar

### Pré-requisito: Descobrir o IP

Abra PowerShell e execute:
```powershell
ipconfig
```

Procure por "IPv4 Address" (ex: `192.168.1.100`)

### De Outra Máquina

**Na máquina cliente**, abra o navegador e acesse:

```
http://192.168.1.100:5173
```

(Substitua `192.168.1.100` pelo IP real)

---

## ✅ Checklist

- [x] Vite escutando em `0.0.0.0:5173`
- [x] ASP.NET API escutando em `0.0.0.0:5000`
- [x] PlanejadorFluxo escutando em `0.0.0.0:6001`
- [ ] Firewall permitindo acesso às portas 5173, 5000, 6001
- [ ] Máquina cliente tem conectividade de rede

---

## 🔧 Se não Funcionar

### Problema: "Recusado" ou "Conexão recusada"

**Solução**: Verificar firewall
```powershell
# Permitir portas no Windows Firewall
New-NetFirewallRule -DisplayName "Vite" -Direction Inbound -Action Allow -Protocol TCP -LocalPort 5173
New-NetFirewallRule -DisplayName "API" -Direction Inbound -Action Allow -Protocol TCP -LocalPort 5000
New-NetFirewallRule -DisplayName "Planejador" -Direction Inbound -Action Allow -Protocol TCP -LocalPort 6001
```

### Problema: "Timeout" ao carregar dados de `/api`

**Causa**: Backend provavelmente está escutando apenas em localhost  
**Solução**: Verificar se as mudanças no `Program.cs` foram aplicadas:

```csharp
app.Run("http://0.0.0.0:5000");  // ✅ Correto
app.Run();                         // ❌ Errado (localhost)
```

---

## 🔐 Segurança em Produção

⚠️ **NÃO usar em produção!**

Em produção, usar:
- Reverse proxy (nginx, IIS)
- HTTPS (certificados SSL)
- Autenticação e autorização
- VPN ou acesso restrito

---

## 📝 Exemplo de Produção

### nginx proxy
```nginx
server {
    listen 80;
    server_name seu-dominio.com;

    location / {
        proxy_pass http://localhost:5173;
        proxy_set_header Host $host;
    }

    location /api {
        proxy_pass http://localhost:5000;
    }
}
```

---

## ✨ Status

✅ Configuração de rede completa  
✅ Todos os servidores escutando em 0.0.0.0  
✅ Pronto para acesso de outra máquina
