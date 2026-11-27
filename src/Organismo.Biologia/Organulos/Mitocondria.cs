// ============================================================================
// Fase 4 — Clases con estado (encapsulación).
// Paso 1: 'class' en vez de 'record'.
// ============================================================================

// Fase 2 — Namespace file-scoped. Fase 3: la carpeta Organulos/ se refleja en el namespace.
namespace Organismo.Biologia.Organulos;

// Fase 4 · Paso 1 — 'class' frente a 'record' (Fase 3):
//   record → igualdad por VALOR: dos Adn con la misma secuencia son ==.
//            Pensado para datos inmutables; admite 'with'.
//   class  → igualdad por REFERENCIA: dos Mitocondria son == solo si son el MISMO objeto.
//            Pensada para objetos con identidad y estado que cambia (Paso 2 en adelante).
//            No admite 'with':  mitocondria with { }  → error CS8858.
// sealed: igual que en los records de la Fase 3, nadie puede heredar de ella.
public sealed class Mitocondria
{
}
