$files = Get-ChildItem -Path 'c:\PROJETOS\proposta_arquitetura_grp\src\retaguarda\Persistencia\POSTGRESQL\Migracoes\GRP' -Recurse -Include *.cs,*.Designer.cs -File
foreach ($f in $files) {
    $p = $f.FullName
    $c = Get-Content -Raw -LiteralPath $p
    $c = $c -replace 'datetime\(6\)', 'timestamp without time zone'
    $c = $c -replace 'tinyint\(1\)', 'boolean'
    $c = $c -replace 'longtext', 'text'
    Set-Content -Value $c -Encoding UTF8 -LiteralPath $p
}
Write-Output 'Replacements complete'
