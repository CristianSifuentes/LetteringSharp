# LetteringSharp
This is a repository for practice deep c# 0 to master 


# Fase1

El esqueleto: una solución con tres proyectos (`Organismo.Biologia` → .dll, `Organismo.App` → `cuerpo`, `Organismo.Pruebas` → xUnit) y un `Directory.Build.props` con las reglas comunes a todos.

```bash
# 0. Limpiar la raíz: un .csproj en la raíz compilaría también src/ y tests/
git rm LetteringSharp.csproj Program.cs
rm -rf bin obj

# 1. La solución y las reglas comunes
dotnet new sln -n EukaryoticCell
touch Directory.Build.props          # es un ARCHIVO, no una carpeta (no usar mkdir)
code Directory.Build.props           # contenido abajo

# 2. Los tres proyectos
dotnet new classlib -n Organismo.Biologia -o src/Organismo.Biologia
dotnet new console  -n Organismo.App      -o src/Organismo.App
dotnet new xunit    -n Organismo.Pruebas  -o tests/Organismo.Pruebas
rm src/Organismo.Biologia/Class1.cs

# 3. Añadirlos a la solución y conectarlos: App → Biologia, Pruebas → Biologia + App
dotnet sln add src/Organismo.Biologia src/Organismo.App tests/Organismo.Pruebas
dotnet add src/Organismo.App reference src/Organismo.Biologia
dotnet add tests/Organismo.Pruebas reference src/Organismo.Biologia src/Organismo.App

# 4. Ajustar los .csproj (contenido abajo)
code src/Organismo.App/Organismo.App.csproj
code src/Organismo.Biologia/Organismo.Biologia.csproj
code tests/Organismo.Pruebas/Organismo.Pruebas.csproj

# 5. Comprobar y guardar
dotnet build                         # 0 Advertencia(s), 0 Errores
dotnet test                          # Passed: 1
dotnet run --project src/Organismo.App
git add -A && git commit -m "Fase 1: solución, tres proyectos y reglas comunes"
```

`Directory.Build.props`: MSBuild lo carga antes de cada `.csproj` que esté debajo de esta carpeta.

```xml
<Project>
  <!-- Reglas comunes: cada proyecto de la solución las hereda. -->
  <PropertyGroup>
    <TargetFramework>net11.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <AnalysisLevel>latest</AnalysisLevel>
    <InvariantGlobalization>true</InvariantGlobalization>
  </PropertyGroup>
</Project>
```

Ajustes de los `.csproj`:
- **Los tres:** quitar `TargetFramework`, `ImplicitUsings` y `Nullable` (ya vienen de `Directory.Build.props`; si se repiten, gana el `.csproj`).
- **`Organismo.App.csproj`:** `<AssemblyName>cuerpo</AssemblyName>` dentro del `<PropertyGroup>` → el ejecutable se llama `cuerpo`.
- **`Organismo.Biologia.csproj`:** `<ItemGroup><InternalsVisibleTo Include="Organismo.Pruebas" /></ItemGroup>` → las pruebas ven lo `internal`.


# Fase2

Fisiologia: el «global» de C#. Todo miembro vive dentro de un tipo; lo más parecido a un global es una `static class` con miembros `const` o `static`.

```bash
# 1. Crear la clase y abrirla en el editor
touch src/Organismo.Biologia/Fisiologia.cs
code src/Organismo.Biologia/Fisiologia.cs

# 2. Compilar (esperado: 0 Advertencia(s), 0 Errores)
dotnet build

# 3. Usarla desde la App (editar Program.cs) y ejecutar
dotnet run --project src/Organismo.App

# 4. Experimentos: romper el código, compilar, leer el error y deshacer el cambio
#    a) double temperatura = 37;               debajo del namespace     → CS0116
#    b) public int Contador;                   dentro de Fisiologia     → CS0708
#    c) public const TimeSpan VidaGlobuloRojo  en vez de static { get } → CS0283 + CS0133
#    d) var f = new Fisiologia();              en Program.cs            → CS0712 + CS0723
dotnet build

# 5. Comprobar y guardar
dotnet build
dotnet test
dotnet run --project src/Organismo.App
git add -A && git commit -m "Fase 2: Fisiologia, el «global» de C#"
```


# Fase3

Moléculas y visibilidad: `record` para datos con igualdad por valor, y la escalera de visibilidad de tipos `public` → `internal` → `file`.

| Modificador | Quién lo ve | Ejemplo |
|---|---|---|
| `public` | cualquier proyecto que referencie el .dll | `Fisiologia`, `Adn`, `Glucosa` |
| `internal` | solo `Organismo.Biologia` (+ `Organismo.Pruebas` por `InternalsVisibleTo`) | `Ensamblado` |
| `file` | solo el archivo `.cs` donde se declara (C# 11) | `Bases` en `Adn.cs` |
| `private` | solo el tipo que lo contiene | `Bases.Validas` |

```bash
# 1. Las moléculas (sealed record) y las herramientas internas
mkdir -p src/Organismo.Biologia/Moleculas
code src/Organismo.Biologia/Moleculas/Adn.cs         # record Adn + file class Bases
code src/Organismo.Biologia/Moleculas/Moleculas.cs   # Glucosa, ImpulsoElectrico, Hormona
code src/Organismo.Biologia/Ensamblado.cs            # internal static class Ensamblado

# 2. Pruebas: records, validación e internal (gracias a InternalsVisibleTo de la Fase 1)
git rm tests/Organismo.Pruebas/UnitTest1.cs
code tests/Organismo.Pruebas/MoleculasPruebas.cs
code tests/Organismo.Pruebas/EnsambladoPruebas.cs

# 3. Usarlas desde la App (editar Program.cs) y ejecutar
dotnet run --project src/Organismo.App

# 4. Experimentos: romper el código, compilar, leer el error y deshacer el cambio
#    a) Ensamblado.ProporcionGc("ACGT");          en Program.cs    → CS0122 (internal)
#    b) Bases.Validar("ACGT");                    en Program.cs    → CS0103 (file)
#    c) Bases.Validar("ACGT");                    en Moleculas.cs  → CS0103 (file: ni el mismo proyecto)
#    d) Adn.Humano with { Secuencia = "A" }       en cualquier sitio → CS0200 (solo get)
#    e) quitar <InternalsVisibleTo> del .csproj   → CS0122 en EnsambladoPruebas.cs
dotnet build

# 5. Comprobar y guardar
dotnet build                         # 0 Advertencia(s), 0 Errores
dotnet test                          # Passed: 21
dotnet run --project src/Organismo.App
git add -A && git commit -m "Fase 3: moléculas como record y visibilidad public/internal/file"
```

Qué genera el compilador para un `record` posicional como `Adn(string Secuencia)`:
- Constructor, propiedades, `Deconstruct` y `ToString` → `Adn { Secuencia = ATGC, ... }`.
- `Equals`, `GetHashCode`, `==` y `!=` **por valor**: `new Adn("acgt") == new Adn("ACGT")` es `true`.
- Expresión `with`: copia el objeto. **No** vuelve a ejecutar la validación del constructor, por eso `Secuencia` solo tiene `get`.


# Fase4

Clases con estado (encapsulación): una `Mitocondria` que produce y gasta ATP y una `Neurona` que lo consume. Si los records de la Fase 3 eran **datos** que no cambian, una `class` es un **objeto** con identidad cuyo estado cambia, pero solo a través de sus métodos.

| Paso | Concepto | Dónde |
|---|---|---|
| 1 | `class` frente a `record`: igualdad por referencia, sin `with` | `Mitocondria` |
| 2 | `{ get; private set; }`: se lee desde fuera, se escribe solo desde dentro | `Mitocondria.Atp` |
| 3 | Método de instancia que cambia el estado (frente a `static`) | `Respirar(Glucosa)` |
| 4 | Invariante: validar **antes** de cambiar el estado | `Gastar(int)` |
| 5 | Propiedad calculada sobre estado que cambia | `EstaAgotada => Atp == 0` |
| 6 | Sobrecarga: mismo nombre, otros parámetros | `Respirar(int)` |
| 7 | Constructor normal y `: this(...)` | `Mitocondria(int)`, `Mitocondria()` |
| 8 | Campo `private readonly` y la convención `_nombre` | `_capacidadMaxima` |
| 9 | Primary constructor de clase (C# 12) | `Neurona(Mitocondria mitocondria)` |

```bash
# 1. Las clases con estado
mkdir -p src/Organismo.Biologia/Organulos src/Organismo.Biologia/Celulas
code src/Organismo.Biologia/Organulos/Mitocondria.cs   # pasos 1-8
code src/Organismo.Biologia/Celulas/Neurona.cs         # paso 9

# 2. Pruebas de estado: Arrange / Act / Assert
code tests/Organismo.Pruebas/MitocondriaPruebas.cs
code tests/Organismo.Pruebas/NeuronaPruebas.cs

# 3. Usarlas desde la App (editar Program.cs) y ejecutar
dotnet run --project src/Organismo.App

# 4. Experimentos: romper el código, compilar, leer el error y deshacer el cambio
#    a) mitocondria with { }                          en Program.cs        → CS8858 (with solo en records)
#    b) mitocondria.Atp = 9999;                       en Program.cs        → CS0272 (set privado)
#    c) public int Respirar(int otro) => 0;           en Mitocondria       → CS0111 (el retorno no cuenta)
#    d) borrar  public Mitocondria() : this(0) { }    en Mitocondria       → CS7036 en cada new Mitocondria()
#    e) public void Romper() { _capacidadMaxima = 5; } en Mitocondria      → CS0191 (readonly)
#    f) neurona.mitocondria                           en Program.cs        → CS1061 (no genera propiedad)
#    g) en Neurona: inicializar el campo con  = mitocondria;  y usar  mitocondria.Gastar(...)
#       en Disparar                                                        → CS9124 (dos copias del parámetro)
dotnet build

# 5. Comprobar y guardar
dotnet build                         # 0 Advertencia(s), 0 Errores
dotnet test                          # Passed: 46
dotnet run --project src/Organismo.App
git add -A && git commit -m "Fase 4: clases con estado (encapsulación)"
```

`record` frente a `class`, en una línea cada uno:
- **`record`** (Fase 3): dos `Adn` con la misma secuencia son `==`. Inmutable, se copia con `with`.
- **`class`** (Fase 4): dos `Mitocondria` son `==` solo si son el mismo objeto. Si dos variables apuntan a la misma mitocondria y una gasta ATP, la otra lo ve.

Orden obligatorio en un método que cambia el estado: **1) validar argumentos, 2) validar el estado, 3) cambiar el estado**. Si algo falla, el objeto queda exactamente como estaba.
