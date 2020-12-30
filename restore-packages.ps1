$npmProjects = (Get-ChildItem . -Recurse -Filter "package.json" -Exclude "node_modules" | Where-Object { -not $_.FullName.Contains('node_modules') })

Push-Location
foreach ($npmProject in $npmProjects) {
    Write-Host "Restoring... " + $npmProject.FullName
    Set-Location $npmProject.Directory.FullName
    npm install
}
Pop-Location
