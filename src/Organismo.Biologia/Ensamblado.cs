// ============================================================================
// Fase 3 — internal: visible en todo el ensamblado, invisible fuera de él.
//
// Un ensamblado es el .dll que produce un proyecto (aquí Organismo.Biologia.dll).
// Todo lo 'internal' lo ven todos los archivos de este proyecto, pero no la App.
// Excepción: el .csproj declara <InternalsVisibleTo Include="Organismo.Pruebas" />
// (preparado en la Fase 1), así que las pruebas también lo ven.
//
// Escalera de visibilidad de tipos, de más a menos abierta:
//   public   → cualquier proyecto que referencie este .dll       (Fisiologia, Adn)
//   internal → solo este .dll (+ InternalsVisibleTo)             (Ensamblado)
//   file     → solo el archivo .cs donde se declara (C# 11)      (Bases, en Adn.cs)
// ============================================================================

namespace Organismo.Biologia;

// Fase 3 — Sin modificador, un tipo de nivel superior ya es 'internal'.
// Aquí se escribe explícito para que se vea.
internal static class Ensamblado
{
    // Fase 3 — Herramienta interna: Adn la usa para exponer ContenidoGc,
    // pero quien use la biblioteca no puede llamarla directamente.
    // Proporción de bases G y C (0.0 a 1.0) en una secuencia ya validada.
    internal static double ProporcionGc(string secuencia)
    {
        if (secuencia.Length == 0)
        {
            return 0.0;
        }

        // Fase 3 — LINQ: Count con una lambda cuenta las letras que cumplen la condición.
        var gc = secuencia.Count(b => b is 'G' or 'C');
        return (double)gc / secuencia.Length;
    }
}
