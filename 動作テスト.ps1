$targets = Get-ChildItem -Path $PSScriptRoot/target -Recurse -Include *金額的NG*.txt
foreach($target in $targets){
    Copy-Item $target $PSScriptRoot/dest
}