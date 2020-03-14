dotnet pack "src\Api" --configuration=Release --output "..\..\packages"
dotnet pack "src\Core" --configuration=Release --output "..\..\packages"
dotnet pack "src\DataDump" --configuration=Release --output "..\..\packages"
dotnet pack "src\IO" --configuration=Release --output "..\..\packages"
