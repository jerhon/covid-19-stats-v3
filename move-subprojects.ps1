# Copies files from base directory out to their respective project directories.
#
# Assumming the Project files are named like this where the directory name matches the project file name.
#
# src/*/*.csproj
# test/*/*Test.csproj
#

New-Item "./src" -ErrorAction SilentlyContinue -ItemType "directory"
New-Item "./test" -ErrorAction SilentlyContinue -ItemType "directory"

$files = (Get-ChildItem .)
foreach ($file in $files) {
    $fileName = $file.Name
    $dir = [System.IO.Path]::GetFileNameWithoutExtension($file.Name)
    if ($file.Name -like "*.Tests.csproj") {
        mkdir "./test/$dir"
        mv $file "./test/$dir/$fileName"
        Write-Host "Placed $fileName in ./test/$dir"
    }
    elseif ($file.Name -like "*.csproj") {
        mkdir "./src/$dir"
        mv $file "./src/$dir/$fileName"
        Write-Host "Placed $fileName in ./src/$dir"
    }
}
