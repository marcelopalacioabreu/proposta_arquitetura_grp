# Padrão de Enumerações em C#

## Visão Geral

As enumerações centralizam valores conhecidos do sistema e fornecem métodos padrão para conversão entre valores (BD) e textos (UI).

## Estrutura de Diretório

```
src/retaguarda/Metadados/Contratos/Enumeracoes/
├── pessoa_tipos.json
├── situacao_documentos.json
└── ... outras enumerações
```

## Formato JSON

### Estrutura Básica

```json
{
  "pessoa_tipos": {
    "chave": "pessoa.tipos",
    "descricao": "Tipos de Pessoa (Física/Jurídica)",
    "valores": [
      {
        "valor": "F",
        "texto": "Pessoa Física",
        "descricao": "Cadastro de pessoa física"
      },
      {
        "valor": "J",
        "texto": "Pessoa Jurídica",
        "descricao": "Cadastro de pessoa jurídica"
      }
    ]
  }
}
```

### Propriedades

| Propriedade | Tipo | Descrição |
|---|---|---|
| `chave` | string | Identificador único (ex: `pessoa.tipos`) |
| `descricao` | string | Descrição do que a enumeração representa |
| `valores` | array | Array de itens de enumeração |
| `valores[].valor` | string | Valor salvo no banco de dados |
| `valores[].texto` | string | Texto exibido na UI |
| `valores[].descricao` | string | Descrição opcional do item |

## Classe C# Base

Todas as enumerações devem herdar de `IEnumeracao`:

```csharp
namespace Retaguarda.Metadados.Contracts
{
    public interface IEnumeracao
    {
        /// <summary>Valor armazenado no banco de dados</summary>
        string Valor { get; }
        
        /// <summary>Texto exibido na UI</summary>
        string Texto { get; }
        
        /// <summary>Descrição opcional</summary>
        string? Descricao { get; }
    }
    
    /// <summary>Classe base para enumerações com valores conhecidos</summary>
    public abstract class Enumeracao : IEnumeracao
    {
        public string Valor { get; }
        public string Texto { get; }
        public string? Descricao { get; }
        
        protected Enumeracao(string valor, string texto, string? descricao = null)
        {
            Valor = valor ?? throw new ArgumentNullException(nameof(valor));
            Texto = texto ?? throw new ArgumentNullException(nameof(texto));
            Descricao = descricao;
        }
        
        public override string ToString() => Texto;
        
        public override bool Equals(object? obj) =>
            obj is Enumeracao e && e.Valor == Valor;
        
        public override int GetHashCode() => Valor.GetHashCode();
    }
}
```

## Exemplo: Enumeração de Tipo de Pessoa

### 1. Arquivo JSON

**Arquivo**: `src/retaguarda/Metadados/Contratos/Enumeracoes/pessoa_tipos.json`

```json
{
  "pessoa_tipos": {
    "chave": "pessoa.tipos",
    "descricao": "Tipos de Pessoa (Física/Jurídica)",
    "valores": [
      {
        "valor": "F",
        "texto": "Pessoa Física",
        "descricao": "Cadastro de pessoa física"
      },
      {
        "valor": "J",
        "texto": "Pessoa Jurídica",
        "descricao": "Cadastro de pessoa jurídica"
      }
    ]
  }
}
```

### 2. Classe C#

**Arquivo**: `src/retaguarda/Dominio/Enumeracoes/TipoPessoa.cs`

```csharp
using Retaguarda.Metadados.Contracts;

namespace Retaguarda.Dominio.Enumeracoes
{
    /// <summary>Tipos de pessoa (Física ou Jurídica)</summary>
    public class TipoPessoa : Enumeracao
    {
        public static readonly TipoPessoa Fisica = new("F", "Pessoa Física", "Cadastro de pessoa física");
        public static readonly TipoPessoa Juridica = new("J", "Pessoa Jurídica", "Cadastro de pessoa jurídica");
        
        private TipoPessoa(string valor, string texto, string? descricao = null)
            : base(valor, texto, descricao)
        {
        }
        
        /// <summary>Converte string em enumeração</summary>
        public static TipoPessoa? ConverterDe(string? valor)
        {
            return valor?.ToUpper() switch
            {
                "F" => Fisica,
                "J" => Juridica,
                _ => null
            };
        }
        
        /// <summary>Retorna todos os valores disponíveis</summary>
        public static IEnumerable<TipoPessoa> ObterTodos() =>
            new[] { Fisica, Juridica };
    }
}
```

### 3. Uso em Entidade

```csharp
namespace Retaguarda.Dominio.Entidades
{
    public class Pessoa : MultilocatarioEntidade
    {
        /// <summary>Tipo de pessoa (F=Física, J=Jurídica)</summary>
        public string TipoPessoaChave { get; set; } = string.Empty;
        
        public string Nome { get; set; } = string.Empty;
        
        public string Documento { get; set; } = string.Empty;
        
        // Propriedade computada para facilitar acesso
        [NotMapped]
        public TipoPessoa? TipoPessoa =>
            TipoPessoa.ConverterDe(TipoPessoaChave);
    }
}
```

### 4. Uso em DTO

```csharp
namespace Retaguarda.DTO.Dtos
{
    public class PessoaDto
    {
        public long Id { get; set; }
        
        public string Nome { get; set; } = string.Empty;
        
        /// <summary>Chave de tipo: F ou J</summary>
        public string TipoPessoaChave { get; set; } = string.Empty;
        
        /// <summary>Texto do tipo (derivado, apenas leitura)</summary>
        public string? TipoPessoaTexto =>
            TipoPessoa.ConverterDe(TipoPessoaChave)?.Texto;
        
        public string Documento { get; set; } = string.Empty;
    }
}
```

### 5. Metadados (UI)

```json
{
  "campo": "pessoa.tipoPessoaChave",
  "label": "Tipo",
  "tipo": "select",
  "enumeracao": "pessoa.tipos",
  "col": 4
}
```

## Endpoint para Carregar Enumerações

### Controller: MetaController

```csharp
[HttpGet("enumeracoes/{chave}")]
public IActionResult CarregarEnumeracao(string chave)
{
    var enumeracao = CarregarDoJSON(chave);
    if (enumeracao == null)
        return NotFound();
    
    return OkData(enumeracao.Values);
}
```

### Resposta

```json
{
  "envelope": {
    "status": "OK"
  },
  "data": [
    {
      "valor": "F",
      "texto": "Pessoa Física",
      "descricao": "Cadastro de pessoa física"
    },
    {
      "valor": "J",
      "texto": "Pessoa Jurídica",
      "descricao": "Cadastro de pessoa jurídica"
    }
  ]
}
```

## Padrões Comuns

### Enumeração Simples

```csharp
public class Situacao : Enumeracao
{
    public static readonly Situacao Ativa = new("A", "Ativa");
    public static readonly Situacao Inativa = new("I", "Inativa");
    
    private Situacao(string valor, string texto) : base(valor, texto) { }
    
    public static Situacao? ConverterDe(string? valor) =>
        valor?.ToUpper() switch
        {
            "A" => Ativa,
            "I" => Inativa,
            _ => null
        };
}
```

### Enumeração com Grupos

```json
{
  "tipo_contato": {
    "chave": "tipo.contato",
    "descricao": "Tipos de Contato",
    "grupos": [
      {
        "nome": "Telefone",
        "valores": [
          {"valor": "TEL", "texto": "Telefone Fixo"},
          {"valor": "CEL", "texto": "Celular"}
        ]
      },
      {
        "nome": "Digital",
        "valores": [
          {"valor": "EMAIL", "texto": "E-mail"},
          {"valor": "SITE", "texto": "Website"}
        ]
      }
    ]
  }
}
```

## Validação em Formulários

No Frontend, ao usar `enumeracao` nos metadados:

```jsx
// Componente SelectField carrega automaticamente as opções
<select name="tipoPessoaChave">
  <option value="">-- selecione --</option>
  {opcoes.map(o => (
    <option key={o.valor} value={o.valor}>
      {o.texto}
    </option>
  ))}
</select>
```

## Validação no Backend

```csharp
[ApiController]
[Route("api/pessoas")]
public class PessoaController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] PessoaDto dto)
    {
        var tipo = TipoPessoa.ConverterDe(dto.TipoPessoaChave);
        if (tipo == null)
            return BadRequest(new { erro = "Tipo de pessoa inválido" });
        
        // ... continuar com a lógica
    }
}
```

## Boas Práticas

1. **Um arquivo JSON por enumeração**: Facilita manutenção e versionamento
2. **Valor curto**: Use códigos como "F", "J", "A", "I" (economiza espaço no BD)
3. **Texto descritivo**: Deixe clara a intenção para o usuário final
4. **Imutável em C#**: Uma vez criada, a instância não muda
5. **Singleton para C#**: Use `static readonly` para garantir única instância
6. **Conversão segura**: Sempre retorne `null` em conversões inválidas
7. **ToString()**: Implemente para facilitar debug e logging

## Integração com FormData

Quando enviado via `FormData` no JavaScript:

```javascript
const formData = new FormData(form)
const obj = {}
for (const [k, v] of formData.entries()) {
  obj[k] = v // "F" ou "J" será enviado
}
```

O valor é sempre a chave, não o texto!
