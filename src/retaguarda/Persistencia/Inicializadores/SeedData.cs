using System;
using Microsoft.Extensions.DependencyInjection;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Interfaces;

namespace Retaguarda.Api.Data
{
    public static class SeedData
    {
        // Creates a default administrator user if missing. Intended for development only.
        public static void EnsureSeed(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var sp = scope.ServiceProvider;
            var repo = sp.GetService<IUsuarioRepositorio>();
            var svc = sp.GetService<IUsuarioServico>();
            if (repo == null || svc == null) return;

            try
            {
                var exists = repo.ObterPorUsernameAsync("admin").GetAwaiter().GetResult();
                if (exists == null)
                {
                    // default credentials: admin / admin
                    svc.CriarUsuarioAsync("admin", "admin", "Administrador", "admin@local").GetAwaiter().GetResult();
                }
            }
            catch
            {
                // swallow errors during seed to avoid blocking startup in dev
            }
        }
    }
}
