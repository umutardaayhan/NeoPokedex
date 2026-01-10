# --- AYARLAR ---
$outputFile = "Full_Project_Source_v1.3.txt"
# Hangi dosya türlerini alacağız?
$extensions = @(".cs", ".cshtml", ".css", ".js", ".json", ".csproj", ".gitignore", ".md")
# Hangi klasörlere girmeyeceğiz?
$excludeFolders = @("bin", "obj", "publish", ".git", ".vs", "lib", ".vscode", "Migrations")

# Eğer eski çıktı dosyası varsa sil
if (Test-Path $outputFile) { Remove-Item $outputFile }

# Dosyaları Bul
$files = Get-ChildItem -Path . -Recurse | Where-Object {
    !$_.PSIsContainer -and ($extensions -contains $_.Extension)
}

Write-Host "Neo Pokedex v1.3 Kodları Taranıyor..." -ForegroundColor Cyan

foreach ($file in $files) {
    $relativePath = $file.FullName.Substring($PWD.Path.Length + 1)
    
    $skip = $false
    foreach ($exclude in $excludeFolders) {
        if ($relativePath -match "\\$exclude\\" -or $relativePath -match "^$exclude\\") { 
            $skip = $true; break 
        }
    }
    
    if ($file.Name -eq "bundlecode.ps1" -or $file.Name -eq $outputFile) { $skip = $true }

    if (-not $skip) {
        $header = "`r`n" + ("=" * 80) + "`r`n" + 
                  "FILE PATH: $relativePath`r`n" + 
                  ("=" * 80) + "`r`n"
        Add-Content -Path $outputFile -Value $header -Encoding UTF8
        try {
            $content = Get-Content -Path $file.FullName -Raw -ErrorAction Stop
            Add-Content -Path $outputFile -Value $content -Encoding UTF8
        } catch { Write-Host "HATA: $relativePath okunamadı" -ForegroundColor Red }
        Write-Host "Eklendi: $relativePath" -ForegroundColor Green
    }
}

Write-Host "`nİŞLEM TAMAMLANDI! v1.3 Hazır. 🚀" -ForegroundColor Yellow