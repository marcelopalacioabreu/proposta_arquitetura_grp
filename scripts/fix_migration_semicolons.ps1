$files = Get-ChildItem -Path 'src/retaguarda/Persistencia/POSTGRESQL/Migracoes/GRP' -Filter '*.cs' -Recurse
foreach ($f in $files) {
    $path = $f.FullName
    Write-Output "Processing: $path"
    $c = Get-Content -Raw -Encoding UTF8 $path
    $c = $c -replace '\)\s*\r?\n\s*;',' );'
    # also replace occurrences where a closing brace is followed by newline and a closing parenthesis semicolon line
    $c = $c -replace '\}\s*\r?\n\s*;','}\n';
    Set-Content -Path $path -Value $c -Encoding UTF8
    Write-Output "fixed: $path"
}
Write-Output "done"