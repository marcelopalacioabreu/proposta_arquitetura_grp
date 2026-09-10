using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Persistencia;
using Retaguarda.Repositorios.Base;
using Retaguarda.Repositorios.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Retaguarda.Repositorios
{
    /// <summary>
    /// Interface do repositório de recuperação de senha
    /// </summary>
    public interface IRecuperacaoSenhaRepositorio : IRepositorioBase<RecuperacaoSenha>
    {
        /// <summary>
        /// Busca um registro de recuperação por usuário ID que ainda não foi utilizado e não expirou
        /// </summary>
        Task<RecuperacaoSenha?> ObterValidoPorUsuarioIdAsync(long usuarioId);

        /// <summary>
        /// Lista todos os registros de recuperação pendentes (não utilizados) de um usuário
        /// </summary>
        Task<List<RecuperacaoSenha>> ObterTodosPendentesAsync(long usuarioId);

        /// <summary>
        /// Lista todos os registros de recuperação válidos (não expirados, não utilizados)
        /// Usado para validar tokens genéricos
        /// </summary>
        Task<List<RecuperacaoSenha>> ObterTodosValidosAsync();
    }

    /// <summary>
    /// Implementação do repositório de recuperação de senha
    /// </summary>
    public class RecuperacaoSenhaRepositorio : RepositorioBase<RecuperacaoSenha>, IRecuperacaoSenhaRepositorio
    {
        private readonly IApplicationDbContext _context;

        public RecuperacaoSenhaRepositorio(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor) 
            : base(context, httpContextAccessor)
        {
            _context = context;
        }

        /// <summary>
        /// Busca um token válido (não expirado, não utilizado) para um usuário
        /// </summary>
        public async Task<RecuperacaoSenha?> ObterValidoPorUsuarioIdAsync(long usuarioId)
        {
            var agora = DateTime.UtcNow;

            return await Task.Run(() =>
                _context.RecuperacoesSenha
                    .Where(x => x.UsuarioId == usuarioId &&
                                !x.Utilizado &&
                                x.DataExpiracao > agora &&
                                x.Ativo)
                    .OrderByDescending(x => x.DataInsercao)
                    .FirstOrDefault()
            );
        }

        /// <summary>
        /// Lista todos os registros pendentes de um usuário
        /// </summary>
        public async Task<List<RecuperacaoSenha>> ObterTodosPendentesAsync(long usuarioId)
        {
            var agora = DateTime.UtcNow;

            return await Task.Run(() =>
                _context.RecuperacoesSenha
                    .Where(x => x.UsuarioId == usuarioId &&
                                !x.Utilizado &&
                                x.DataExpiracao > agora &&
                                x.Ativo)
                    .OrderByDescending(x => x.DataInsercao)
                    .ToList()
            );
        }

        /// <summary>
        /// Lista todos os registros válidos (não expirados, não utilizados)
        /// </summary>
        public async Task<List<RecuperacaoSenha>> ObterTodosValidosAsync()
        {
            var agora = DateTime.UtcNow;

            return await Task.Run(() =>
                _context.RecuperacoesSenha
                    .Include(x => x.Usuario)
                    .Where(x => !x.Utilizado &&
                                x.DataExpiracao > agora &&
                                x.Ativo)
                    .OrderByDescending(x => x.DataInsercao)
                    .ToList()
            );
        }
    }
}
