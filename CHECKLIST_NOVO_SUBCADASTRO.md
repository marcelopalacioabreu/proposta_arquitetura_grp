# Checklist: Implementar Novo Subcadastro

## 📋 Guia Passo a Passo

Use este checklist ao adicionar um novo subcadastro a qualquer cadastro.

---

## Phase 1: Planejamento

- [ ] **Definir a entidade**: Qual será associada? (ex: Endereço, Setor, Contato)
- [ ] **Identificar campos**: Quais dados serão gerenciados? (ex: tipo, valor, padrão)
- [ ] **Endpoint existente?**: Já existe API para listar opções? (ex: `/api/setores`)
- [ ] **Seleção necessária?**: Precisa marcar como padrão? (radio ou checkbox)
- [ ] **Máximo de linhas?**: Há limite? (ex: máx 5 endereços)
- [ ] **Campos obrigatórios**: Quais devem ser sempre preenchidos?

---

## Phase 2: Backend - DTO

- [ ] **Criar arquivo DTO**
  - Localização: `src/retaguarda/Retaguarda.DTO/Dtos/`
  - Naming: `{Entidade}{SubcadastroNome}Dto.cs`
  - Exemplo: `SetorAtuacaoDto.cs`

- [ ] **Estruturar DTO**
  ```csharp
  public class SetorAtuacaoDto
  {
      public long? Id { get; set; }
      public long SetorId { get; set; }
      public long OrganizacaoUnidadeId { get; set; }
      public bool EhPadrao { get; set; }
  }
  ```

- [ ] **Adicionar validação** (atributos)
  ```csharp
  [Required]
  public long SetorId { get; set; }
  ```

- [ ] **Adicionar comentários XML**
  ```csharp
  /// <summary>Identificador do setor para esta atuação</summary>
  public long SetorId { get; set; }
  ```

---

## Phase 3: Backend - Entidade (se necessário)

- [ ] **Verificar se entidade existe** (ex: `SetorUsuario`)
- [ ] **Se não existe, criar**:
  - Localização: `src/retaguarda/Dominio/Entidades/`
  - Herdar de `MultilocatarioEntidade`
  - Incluir campos de relacionamento

- [ ] **Adicionar navegação na entidade principal**
  ```csharp
  public virtual List<SetorUsuario> Atuacoes { get; set; } = new();
  ```

---

## Phase 4: Backend - Controller

- [ ] **Localizar Controller da entidade principal**
  - Exemplo: `UsuarioController.cs`

- [ ] **Estender DTO recebido** para incluir subcadastro
  ```csharp
  public class CriarUsuarioDto
  {
      public string Nome { get; set; }
      public List<SetorAtuacaoDto> Atuacoes { get; set; } = new();
  }
  ```

- [ ] **Processar subcadastro no serviço**
  ```csharp
  [HttpPost]
  public async Task<IActionResult> Criar([FromBody] CriarUsuarioDto dto)
  {
      var usuario = _mapper.Map<Usuario>(dto);
      
      // Processar atuações
      foreach (var atu in dto.Atuacoes)
      {
          usuario.Atuacoes.Add(new SetorUsuario 
          { 
              SetorId = atu.SetorId,
              OrganizacaoUnidadeId = atu.OrganizacaoUnidadeId,
              EhPadrao = atu.EhPadrao
          });
      }
      
      await _db.SaveChangesAsync();
      return CreatedAtAction(nameof(ObterPorId), usuario);
  }
  ```

- [ ] **Usar transação** para atomicidade
  ```csharp
  using (var transaction = await _db.Database.BeginTransactionAsync())
  {
      try
      {
          // ... código ...
          await _db.SaveChangesAsync();
          await transaction.CommitAsync();
      }
      catch
      {
          await transaction.RollbackAsync();
          throw;
      }
  }
  ```

---

## Phase 5: Frontend - JSON de Metadados

- [ ] **Localizar arquivo de metadados**
  - Exemplo: `src/retaguarda/Metadados/Contratos/Telas/cliente/painel/usuarios/cadastro.json`

- [ ] **Adicionar array `subcadastros`** se não existir
  ```json
  {
    "usuarioCadastro": {
      "tipo": "TELA_CADASTRO",
      "itens": [...],
      "subcadastros": [...]
    }
  }
  ```

- [ ] **Definir subcadastro**
  ```json
  {
    "nome": "atuacao",
    "titulo": "Atuação em Setores",
    "endpoint": "/api/setores",
    "campoArmazenamento": "atuacoes",
    "chaveLocal": "id",
    "colunas": [
      {
        "campo": "organizacaoUnidadeId",
        "label": "Unidade",
        "tipo": "select",
        "endpoint": "/api/organizacao_unidades",
        "col": 5
      },
      {
        "campo": "setorId",
        "label": "Setor",
        "tipo": "select",
        "endpoint": "/api/setores",
        "col": 5
      },
      {
        "campo": "ehPadrao",
        "label": "Padrão",
        "tipo": "checkbox",
        "col": 2
      }
    ],
    "selecao": {
      "campo": "ehPadrao",
      "label": "Definir como padrão",
      "singleSelecao": true,
      "mergeNoPrincipal": false
    },
    "maxLinhas": null
  }
  ```

- [ ] **Validar JSON** (usar ferramenta online ou plugin VSCode)

---

## Phase 6: Frontend - Verificação de Componentes

- [ ] **TelaCadastro.jsx já suporta subcadastros?**
  - ✅ Sim: Nenhuma mudança necessária
  - ❌ Não: Importar e estender com código do PR

- [ ] **SubtabelaCadastro.jsx disponível?**
  - ✅ Sim: Será renderizado automaticamente
  - ❌ Não: Copiar do repositório

---

## Phase 7: Testes Frontend

- [ ] **Teste de Renderização**
  - [ ] Carregar tela de cadastro
  - [ ] Verificar se subtabela aparece
  - [ ] Verificar se opções carregam

- [ ] **Teste de Adição**
  - [ ] Preencher campos da nova linha
  - [ ] Clicar "Adicionar"
  - [ ] Verificar se linha aparece na tabela
  - [ ] Verificar se button de adicionar permanece disponível

- [ ] **Teste de Remoção**
  - [ ] Clicar botão "Remover" de uma linha
  - [ ] Verificar se linha desaparece
  - [ ] Verificar se dados são removidos internamente

- [ ] **Teste de Seleção/Padrão**
  - [ ] Marcar checkbox de padrão
  - [ ] Verificar se apenas uma linha pode estar marcada (se radio)
  - [ ] Desmarcar e verificar comportamento

- [ ] **Teste de Submissão**
  - [ ] Preencher formulário principal
  - [ ] Adicionar 2-3 linhas no subcadastro
  - [ ] Clicar "Salvar"
  - [ ] Verificar se request inclui `atuacoes: [...]`
  - [ ] Verificar resposta 201/200

---

## Phase 8: Testes Backend

- [ ] **Teste de DTO Inválido**
  - [ ] Enviar sem campo obrigatório
  - [ ] Verificar se retorna 400 com mensagem clara

- [ ] **Teste de Transação**
  - [ ] Enviar dados válidos
  - [ ] Verificar se tudo foi persistido
  - [ ] Desligar BD no meio da operação
  - [ ] Verificar se nada foi persistido (rollback)

- [ ] **Teste de Recuperação**
  - [ ] Criar entidade com subcadastro
  - [ ] GET `/{id}` para verificar
  - [ ] Verificar se `atuacoes` está presente
  - [ ] Verificar se dados estão corretos

- [ ] **Teste de Edição**
  - [ ] Modificar linhas existentes
  - [ ] Adicionar novas linhas
  - [ ] Remover linhas
  - [ ] Verificar se tudo foi atualizado

---

## Phase 9: Documentação

- [ ] **Adicionar ao README local** (se aplicável)
- [ ] **Criar exemplo no `EXEMPLOS_SUBCADASTROS.md`** (arquivo novo)
  ```markdown
  ## Exemplo: Atuação de Usuário
  
  [descrição, JSON, código]
  ```
- [ ] **Adicionar ao arquivo de diagramas** (se novo padrão)

---

## Phase 10: Code Review

- [ ] **Verificar com equipe**:
  - [ ] DTO está bem estruturado?
  - [ ] Validações são suficientes?
  - [ ] Transação está correta?
  - [ ] JSON segue padrão?
  - [ ] Documentação é clara?

- [ ] **Otimizações**:
  - [ ] Há queries N+1?
  - [ ] Índices no banco para campos de filtro?
  - [ ] Lazy loading em relacionamentos?

---

## Phase 11: Deploy

- [ ] **Criar migração de banco** (se necessário)
  ```bash
  dotnet ef migrations add AddSetorUsuario
  dotnet ef database update
  ```

- [ ] **Testar em staging**
- [ ] **Merging para main**
- [ ] **Comunicar mudança à equipe**

---

## Checklist Rápido (TL;DR)

```
☐ 1. Defina DTO em Retaguarda.DTO/Dtos/
☐ 2. Defina Entidade em Dominio/Entidades/ (se novo)
☐ 3. Processe DTO no Controller com transação
☐ 4. Adicione subcadastro ao JSON de metadados
☐ 5. Teste frontend: adicionar, remover, marcar, salvar
☐ 6. Teste backend: validação, transação, recuperação
☐ 7. Documente a mudança
☐ 8. Code review
☐ 9. Deploy com migração (se necessário)
```

---

## Arquivos Modificados - Exemplo Completo

```
src/retaguarda/Retaguarda.DTO/Dtos/
  └─ SetorAtuacaoDto.cs (novo)

src/retaguarda/Dominio/Entidades/
  └─ SetorUsuario.cs (novo, opcional)

src/retaguarda/Api/Controllers/
  └─ UsuarioController.cs (modificado)

src/retaguarda/Metadados/Contratos/Telas/cliente/painel/usuarios/
  └─ cadastro.json (modificado)

[Frontend: sem mudanças! SubtabelaCadastro já existe]
```

---

## Troubleshooting

### SubtabelaCadastro não aparece
- ✅ Verificar se JSON é válido
- ✅ Verificar se `meta.subcadastros` está preenchido
- ✅ Abrir console do navegador (F12) e procurar erros

### Opções não carregam
- ✅ Verificar se endpoint está correto
- ✅ Verificar se API retorna dados
- ✅ Verificar format

ação: `items`, `data`, etc.

### Dados não salvam
- ✅ Verificar se `campoArmazenamento` está correto
- ✅ Verificar se `construirObjetoFormulario()` agrega dados
- ✅ Verificar se Controller recebe `atuacoes`

### Erro 400 na submissão
- ✅ Verificar console do navegador (FormData)
- ✅ Verificar validação no DTO (Required, StringLength, etc.)
- ✅ Verificar se tipos dos dados estão corretos (string vs number)

---

## Referências

- 📖 [ORIENTACAO_SUBCADASTROS.md](./ORIENTACAO_SUBCADASTROS.md)
- 📖 [ORIENTACAO_ENUMERACOES.md](./ORIENTACAO_ENUMERACOES.md)
- 📖 [SOLUCAO_SUBCADASTROS.md](./SOLUCAO_SUBCADASTROS.md)
- 📊 [DIAGRAMAS_ARQUITETURA.md](./DIAGRAMAS_ARQUITETURA.md)
