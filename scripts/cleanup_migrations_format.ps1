$files = Get-ChildItem -Path 'src/retaguarda/Persistencia/POSTGRESQL/Migracoes/GRP' -Filter '*.cs' -Recurse
foreach ($f in $files) {
    $path = $f.FullName
    Write-Output "Processing: $path"
    $c = Get-Content -Raw -Encoding UTF8 $path
    # Remove lines that contain only a comma (likely leftover after annotation removal)
    $c = $c -replace "(?m)^[ \t]*,[ \t]*$",""
    # Collapse occurrences where an extra blank line separates closing brace and semicolon: replace ')\n\n                );' -> ');'
    $c = $c -replace "\)\s*\r?\n\s*\r?\n\s*\);"," );"
    # Also collapse patterns like '})\n\s*\);' -> '});'
    $c = $c -replace "\}\)\s*\r?\n\s*\);","});"
    # Remove repeated empty parenthesis/semicolons sequences
    $c = $c -replace "\)\s*\r?\n\s*\);"," );"
    Set-Content -Path $path -Value $c -Encoding UTF8
    Write-Output "fixed: $path"
}
Write-Output "done"