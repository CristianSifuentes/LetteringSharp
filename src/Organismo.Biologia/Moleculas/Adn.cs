// ============================================================================
// Fase 3 — Moléculas como 'record' y el modificador 'file'.
// ============================================================================

// Fase 2 — Namespace file-scoped (C# 10). Fase 3: la carpeta Moleculas/ se refleja
// en el namespace Organismo.Biologia.Moleculas (convención, no obligación).
namespace Organismo.Biologia.Moleculas;

// Fase 3 — sealed record posicional: el compilador genera constructor, Equals/GetHashCode
// por valor (dos Adn con la misma secuencia son ==), ToString y Deconstruct.
// sealed: nadie puede heredar de Adn.
public sealed record Adn(string Secuencia)
{
    // Fase 3 — Se redefine la propiedad del parámetro posicional para validarla al construir.
    // Solo tiene 'get' (sin 'init'), así que después de construirse no cambia nunca.
    public string Secuencia { get; } = Bases.Validar(Secuencia);

    // Fase 3 — Propiedad calculada (expression-bodied): se evalúa cada vez que se lee.
    public int Longitud => Secuencia.Length;

    // Fase 3 — public usando algo internal: Ensamblado no se ve desde la App,
    // pero esta propiedad pública sí, y por dentro la usa.
    public double ContenidoGc => Ensamblado.ProporcionGc(Secuencia);

    // Fase 2 + 3 — Miembro static de solo lectura: una instancia compartida para todo el programa.
    public static Adn Humano { get; } = new("ATGGTGCACCTGACTCCTGAGGAG");   // inicio del gen de la β-globina

    // Fase 3 — 'with' crea una copia del record. Como Secuencia no tiene 'init',
    // aquí solo puede usarse vacío: with { }.
    // Escribir  adn with { Secuencia = "ACGT" }  da error CS0200 (propiedad de solo lectura).
    // Aunque tuviera 'init', 'with' NO vuelve a pasar por Bases.Validar: se colaría basura.
    public Adn Replicar() => this with { };
}

// Fase 3 — 'file' (C# 11): el tipo más encapsulado que existe. Solo este archivo lo ve.
// Ni Moleculas.cs, ni Ensamblado.cs, ni la App, ni las pruebas pueden usar Bases (CS0103).
file static class Bases
{
    // Fase 3 — private: ni siquiera el resto de este archivo ve 'Validas', solo Bases.
    private const string Validas = "ACGT";

    public static string Validar(string secuencia)
    {
        // Fase 3 — Helper de la BCL (Base Class Library, las clases que trae .NET):
        // lanza ArgumentNullException si es null y ArgumentException si está vacía o en blanco.
        ArgumentException.ThrowIfNullOrWhiteSpace(secuencia);
        var normal = secuencia.ToUpperInvariant();

        // Fase 3 — LINQ: comprueba que cada letra esté en "ACGT". Validas.Contains se pasa como función
        // (method group). Expresión condicional + 'throw' como expresión (C# 7).
        return normal.All(Validas.Contains)
            ? normal
            : throw new ArgumentException($"La secuencia de ADN contiene bases no válidas. Solo se permiten: {Validas}", nameof(secuencia));
    }
}
