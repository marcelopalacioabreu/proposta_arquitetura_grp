$files = Get-ChildItem -Path 'src/retaguarda/Persistencia/POSTGRESQL/Migracoes/GRP' -Filter '*.cs' -Recurse
foreach ($f in $files) {
    $path = $f.FullName
    Write-Output "Processing: $path"
    $c = Get-Content -Raw -Encoding UTF8 $path
    $c = $c -replace '\.Annotation\("MySql:CharSet",\s*"utf8mb4"\)\.OldAnnotation\("MySql:CharSet",\s*"utf8mb4"\)\s*;', "`n                );"
    $c = $c -replace '\.Annotation\("MySql:CharSet",\s*"utf8mb4"\)\s*;', "`n                );"
    $c = $c -replace '\.Annotation\("MySql:CharSet",\s*"utf8mb4"\)\s*,', ','
    $c = $c -replace '\.OldAnnotation\("MySql:CharSet",\s*"utf8mb4"\)\s*;', ';'
    Set-Content -Path $path -Value $c -Encoding UTF8
    Write-Output "fixed: $path"
}
Write-Output "done"