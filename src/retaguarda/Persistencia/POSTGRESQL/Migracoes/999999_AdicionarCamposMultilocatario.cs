using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retaguarda.Persistencia.POSTGRESQL.Migracoes
{
    [Migration("999999_AdicionarCamposMultilocatario")]
    public partial class AdicionarCamposMultilocatario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder.ActiveProvider?.Contains("Npgsql") == true)
                {
                    migrationBuilder.Sql(@"
    DO $$
    DECLARE
        r RECORD;
    BEGIN
        FOR r IN SELECT table_schema, table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE' LOOP
            -- IdentificadorUnico (use uuid type for Postgres)
            EXECUTE format('ALTER TABLE %I.%I ADD COLUMN IF NOT EXISTS %I uuid NOT NULL DEFAULT ''00000000-0000-0000-0000-000000000000''::uuid', r.table_schema, r.table_name, 'IdentificadorUnico');
        -- IdentificadorUnicoAmigavel
        EXECUTE format('ALTER TABLE %I.%I ADD COLUMN IF NOT EXISTS %I text NOT NULL DEFAULT '''''',', r.table_schema, r.table_name, 'IdentificadorUnicoAmigavel');
        -- DataInsercao
        EXECUTE format('ALTER TABLE %I.%I ADD COLUMN IF NOT EXISTS %I timestamp without time zone NOT NULL DEFAULT now()', r.table_schema, r.table_name, 'DataInsercao');
        -- DataAlteracao
        EXECUTE format('ALTER TABLE %I.%I ADD COLUMN IF NOT EXISTS %I timestamp without time zone NULL', r.table_schema, r.table_name, 'DataAlteracao');
        -- OrganizacaoId
        EXECUTE format('ALTER TABLE %I.%I ADD COLUMN IF NOT EXISTS %I bigint NULL', r.table_schema, r.table_name, 'OrganizacaoId');
        -- OrganizacaoUnidadeId
        EXECUTE format('ALTER TABLE %I.%I ADD COLUMN IF NOT EXISTS %I bigint NULL', r.table_schema, r.table_name, 'OrganizacaoUnidadeId');
        -- SetorId
        EXECUTE format('ALTER TABLE %I.%I ADD COLUMN IF NOT EXISTS %I bigint NULL', r.table_schema, r.table_name, 'SetorId');
        -- Ativo
        EXECUTE format('ALTER TABLE %I.%I ADD COLUMN IF NOT EXISTS %I boolean NOT NULL DEFAULT true', r.table_schema, r.table_name, 'Ativo');
        -- UsuarioInsercaoId
        EXECUTE format('ALTER TABLE %I.%I ADD COLUMN IF NOT EXISTS %I bigint NULL', r.table_schema, r.table_name, 'UsuarioInsercaoId');
        -- UsuarioAlteracaoId
        EXECUTE format('ALTER TABLE %I.%I ADD COLUMN IF NOT EXISTS %I bigint NULL', r.table_schema, r.table_name, 'UsuarioAlteracaoId');
        -- Versao
        EXECUTE format('ALTER TABLE %I.%I ADD COLUMN IF NOT EXISTS %I bigint NOT NULL DEFAULT 1', r.table_schema, r.table_name, 'Versao');
    END LOOP;
END
$$;
");
            }
            else
            {
                // Fallback for non-Postgres providers: attempt to add to some known tables using migrationBuilder APIs.
                // Add to core tables to be safe; if a column exists, this will throw during migration.
                var tables = new[] { "Organizacoes", "Perfis", "Usuarios", "Pessoas", "Municipios", "Bairros", "Contatos", "Documentos" };
                foreach (var t in tables)
                {
                    try
                    {
                        migrationBuilder.AddColumn<Guid>(name: "IdentificadorUnico", table: t, type: "uuid", nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
                    }
                    catch { }
                    try
                    {
                        migrationBuilder.AddColumn<string>(name: "IdentificadorUnicoAmigavel", table: t, type: "text", nullable: false, defaultValue: "");
                    }
                    catch { }
                    try
                    {
                        migrationBuilder.AddColumn<DateTime>(name: "DataInsercao", table: t, type: "timestamp without time zone", nullable: false, defaultValueSql: "now()");
                    }
                    catch { }
                    try
                    {
                        migrationBuilder.AddColumn<DateTime>(name: "DataAlteracao", table: t, type: "timestamp without time zone", nullable: true);
                    }
                    catch { }
                    try
                    {
                        migrationBuilder.AddColumn<long?>(name: "OrganizacaoId", table: t, type: "bigint", nullable: true);
                    }
                    catch { }
                    try
                    {
                        migrationBuilder.AddColumn<long?>(name: "OrganizacaoUnidadeId", table: t, type: "bigint", nullable: true);
                    }
                    catch { }
                    try
                    {
                        migrationBuilder.AddColumn<long?>(name: "SetorId", table: t, type: "bigint", nullable: true);
                    }
                    catch { }
                    try
                    {
                        migrationBuilder.AddColumn<bool>(name: "Ativo", table: t, type: "boolean", nullable: false, defaultValue: true);
                    }
                    catch { }
                    try
                    {
                        migrationBuilder.AddColumn<long?>(name: "UsuarioInsercaoId", table: t, type: "bigint", nullable: true);
                    }
                    catch { }
                    try
                    {
                        migrationBuilder.AddColumn<long?>(name: "UsuarioAlteracaoId", table: t, type: "bigint", nullable: true);
                    }
                    catch { }
                    try
                    {
                        migrationBuilder.AddColumn<long>(name: "Versao", table: t, type: "bigint", nullable: false, defaultValue: 1L);
                    }
                    catch { }
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder.ActiveProvider?.Contains("Npgsql") == true)
            {
                migrationBuilder.Sql(@"
DO $$
DECLARE
    r RECORD;
BEGIN
    FOR r IN SELECT table_schema, table_name FROM information_schema.tables WHERE table_schema = 'public' AND table_type = 'BASE TABLE' LOOP
        EXECUTE format('ALTER TABLE %I.%I DROP COLUMN IF EXISTS %I', r.table_schema, r.table_name, 'IdentificadorUnico');
        EXECUTE format('ALTER TABLE %I.%I DROP COLUMN IF EXISTS %I', r.table_schema, r.table_name, 'IdentificadorUnicoAmigavel');
        EXECUTE format('ALTER TABLE %I.%I DROP COLUMN IF EXISTS %I', r.table_schema, r.table_name, 'DataInsercao');
        EXECUTE format('ALTER TABLE %I.%I DROP COLUMN IF EXISTS %I', r.table_schema, r.table_name, 'DataAlteracao');
        EXECUTE format('ALTER TABLE %I.%I DROP COLUMN IF EXISTS %I', r.table_schema, r.table_name, 'OrganizacaoId');
        EXECUTE format('ALTER TABLE %I.%I DROP COLUMN IF EXISTS %I', r.table_schema, r.table_name, 'OrganizacaoUnidadeId');
        EXECUTE format('ALTER TABLE %I.%I DROP COLUMN IF EXISTS %I', r.table_schema, r.table_name, 'SetorId');
        EXECUTE format('ALTER TABLE %I.%I DROP COLUMN IF EXISTS %I', r.table_schema, r.table_name, 'Ativo');
        EXECUTE format('ALTER TABLE %I.%I DROP COLUMN IF EXISTS %I', r.table_schema, r.table_name, 'UsuarioInsercaoId');
        EXECUTE format('ALTER TABLE %I.%I DROP COLUMN IF EXISTS %I', r.table_schema, r.table_name, 'UsuarioAlteracaoId');
        EXECUTE format('ALTER TABLE %I.%I DROP COLUMN IF EXISTS %I', r.table_schema, r.table_name, 'Versao');
    END LOOP;
END
$$;
");
            }
            else
            {
                var tables = new[] { "Organizacoes", "Perfis", "Usuarios", "Pessoas", "Municipios", "Bairros", "Contatos", "Documentos" };
                foreach (var t in tables)
                {
                    try { migrationBuilder.DropColumn(name: "IdentificadorUnico", table: t); } catch { }
                    try { migrationBuilder.DropColumn(name: "IdentificadorUnicoAmigavel", table: t); } catch { }
                    try { migrationBuilder.DropColumn(name: "DataInsercao", table: t); } catch { }
                    try { migrationBuilder.DropColumn(name: "DataAlteracao", table: t); } catch { }
                    try { migrationBuilder.DropColumn(name: "OrganizacaoId", table: t); } catch { }
                    try { migrationBuilder.DropColumn(name: "OrganizacaoUnidadeId", table: t); } catch { }
                    try { migrationBuilder.DropColumn(name: "SetorId", table: t); } catch { }
                    try { migrationBuilder.DropColumn(name: "Ativo", table: t); } catch { }
                    try { migrationBuilder.DropColumn(name: "UsuarioInsercaoId", table: t); } catch { }
                    try { migrationBuilder.DropColumn(name: "UsuarioAlteracaoId", table: t); } catch { }
                    try { migrationBuilder.DropColumn(name: "Versao", table: t); } catch { }
                }
            }
        }
    }
}



