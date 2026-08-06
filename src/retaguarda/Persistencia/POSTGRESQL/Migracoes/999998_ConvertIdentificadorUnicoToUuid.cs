using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retaguarda.Persistencia.POSTGRESQL.Migracoes
{
    [Migration("999998_ConvertIdentificadorUnicoToUuid")]
    public partial class ConvertIdentificadorUnicoToUuid : Migration
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
    FOR r IN SELECT table_schema, table_name, column_name FROM information_schema.columns WHERE column_name = 'IdentificadorUnico' AND table_schema = 'public' LOOP
        BEGIN
            EXECUTE format('ALTER TABLE %I.%I ALTER COLUMN %I TYPE uuid USING (%I::uuid)', r.table_schema, r.table_name, r.column_name, r.column_name);
            EXECUTE format('ALTER TABLE %I.%I ALTER COLUMN %I SET DEFAULT ''00000000-0000-0000-0000-000000000000''::uuid', r.table_schema, r.table_name, r.column_name);
        EXCEPTION WHEN others THEN
            -- ignore errors (e.g., invalid data that cannot be cast)
            RAISE NOTICE 'Could not convert %.% to uuid: %', r.table_schema, r.table_name, SQLERRM;
        END;
    END LOOP;
END
$$;
");
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
    FOR r IN SELECT table_schema, table_name, column_name FROM information_schema.columns WHERE column_name = 'IdentificadorUnico' AND table_schema = 'public' LOOP
        BEGIN
            -- Attempt to convert back to char(36)
            EXECUTE format('ALTER TABLE %I.%I ALTER COLUMN %I TYPE char(36) USING (CAST(%I AS text))', r.table_schema, r.table_name, r.column_name, r.column_name);
            EXECUTE format('ALTER TABLE %I.%I ALTER COLUMN %I SET DEFAULT ''00000000-0000-0000-0000-000000000000''', r.table_schema, r.table_name, r.column_name);
        EXCEPTION WHEN others THEN
            RAISE NOTICE 'Could not revert %.% from uuid: %', r.table_schema, r.table_name, SQLERRM;
        END;
    END LOOP;
END
$$;
");
            }
        }
    }
}
