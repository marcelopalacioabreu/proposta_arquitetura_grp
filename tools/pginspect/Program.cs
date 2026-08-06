using System;
using Npgsql;

var conn = Environment.GetEnvironmentVariable("PG_CONN") ?? "Host=localhost;Port=5432;Database=grp_banco_01;Username=postgres;Password=postgres";
using var c = new NpgsqlConnection(conn);
await c.OpenAsync();

var cmd = new NpgsqlCommand(@"SELECT table_schema, table_name, column_name, data_type FROM information_schema.columns WHERE lower(column_name) = 'identificadorunico' AND data_type LIKE 'character%';", c);
using var reader = await cmd.ExecuteReaderAsync();
var found = false;
while (await reader.ReadAsync())
{
    found = true;
    Console.WriteLine($"{reader.GetString(0)}.{reader.GetString(1)} - {reader.GetString(3)}");
}
if (!found) Console.WriteLine("No character-type IdentificadorUnico columns found.");
