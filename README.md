# LetteringSharp
This is a repository for practice deep c# 0 to master 


# Fase1

git rm LetteringSharp.csproj Program.cs
rm -rf bin obj
dotnet new sln -n EukaryoticCell
# crea Directory.Build.props (contenido en el mensaje anterior)
dotnet new classlib -n Organismo.Biologia -o src/Organismo.Biologia
dotnet new console  -n Organismo.App      -o src/Organismo.App
dotnet new xunit    -n Organismo.Pruebas  -o tests/Organismo.Pruebas
rm src/Organismo.Biologia/Class1.cs
dotnet sln add src/Organismo.Biologia src/Organismo.App tests/Organismo.Pruebas
dotnet add src/Organismo.App reference src/Organismo.Biologia
dotnet add tests/Organismo.Pruebas reference src/Organismo.Biologia src/Organismo.App
dotnet build
