// ============================================================================
// Fase 3 — Moléculas pequeñas como 'sealed record'.
// Varios tipos pequeños y relacionados pueden compartir archivo.
// ============================================================================

// Fase 2 — Namespace file-scoped (C# 10).
namespace Organismo.Biologia.Moleculas;

// Fase 3 — Record con validación en la propiedad: el constructor rechaza valores negativos.
// 'throw' como expresión dentro de un operador condicional.
public sealed record Glucosa(int Moleculas)
{
    public int Moleculas { get; } = Moleculas >= 0
        ? Moleculas
        : throw new ArgumentOutOfRangeException(nameof(Moleculas), "La cantidad de moléculas de glucosa no puede ser negativa.");
}

// Fase 3 — Record posicional en una sola línea: propiedades { get; init; } generadas.
// Cualquier voltaje es válido (los potenciales de membrana pueden ser negativos).
public sealed record ImpulsoElectrico(double Milivoltios);

// Fase 3 — Record con validación de dos parámetros: nombre no vacío y concentración >= 0.
public sealed record Hormona(string Nombre, double Concentracion)
{
    public string Nombre { get; } = string.IsNullOrWhiteSpace(Nombre)
        ? throw new ArgumentException("La hormona necesita un nombre.", nameof(Nombre))
        : Nombre;

    public double Concentracion { get; } = Concentracion >= 0
        ? Concentracion
        : throw new ArgumentOutOfRangeException(nameof(Concentracion), "La concentración no puede ser negativa.");
}
