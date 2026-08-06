$files = Get-ChildItem -Path 'src/retaguarda/Persistencia/POSTGRESQL/Migracoes/GRP' -Filter '*.cs' -Recurse
foreach ($f in $files) {
    $path = $f.FullName
    Write-Output "Processing: $path"
    $c = Get-Content -Raw -Encoding UTF8 $path
    # remove any remaining .Annotation("MySql:CharSet", "utf8mb4") with optional whitespace and optional trailing comma/semicolon
    $c = $c -replace '\.Annotation\("MySql:CharSet",\s*"utf8mb4"\s*\)\s*,', ','
    $c = $c -replace '\.Annotation\("MySql:CharSet",\s*"utf8mb4"\s*\)\s*;', ''
    $c = $c -replace '\.Annotation\("MySql:CharSet",\s*"utf8mb4"\s*\)', ''
    # normalize ')
    ;' to ');'
    $c = $c -replace '\)\s*\r?\n\s*;',' );'
    $c = $c -replace '\)\s*\r?\n\s*\)','))'
    Set-Content -Path $path -Value $c -Encoding UTF8
    Write-Output "fixed: $path"
}
Write-Output "done"