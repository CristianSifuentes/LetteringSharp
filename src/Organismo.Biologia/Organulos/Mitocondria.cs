// ============================================================================
// Fase 4 — Clases con estado (encapsulación).
// Paso 1: 'class' en vez de 'record'.
// Paso 2: propiedad { get; private set; }.
// Paso 3: método de instancia que cambia el estado.
// ============================================================================

// Fase 2 — Namespace file-scoped. Fase 3: la carpeta Organulos/ se refleja en el namespace.
using Organismo.Biologia.Moleculas;

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
    // Fase 4 · Paso 2 — { get; private set; }: ventanilla de solo lectura.
    //   get         → público: cualquiera LEE  m.Atp
    //   private set → solo el código DENTRO de Mitocondria puede ESCRIBIR Atp.
    // Desde fuera,  m.Atp = 9999;  → error CS0272 (el descriptor set es inaccesible).
    // Así nadie fabrica ATP de la nada: el estado solo cambiará con métodos de la clase (Paso 3).
    // Compáralo con Adn.Secuencia { get; } (Fase 3): ni siquiera desde dentro puede cambiar.
    // Sin inicializar, un int vale 0 (valor por defecto).
    public int Atp { get; private set; }

    // Fase 4 · Paso 3 — Método de INSTANCIA: se llama sobre un objeto (mitocondria.Respirar(g))
    // y cambia el estado de ESE objeto. Cada Mitocondria tiene su propio Atp.
    // Frente a 'static' (Fase 2): Fisiologia.AtpRespiracionAerobica pertenece al tipo, hay uno solo.
    // Aquí se juntan las tres fases:
    //   Fisiologia.AtpRespiracionAerobica → Fase 2 (const en static class)
    //   Glucosa                           → Fase 3 (record que ya validó Moleculas >= 0)
    //   Atp += ...                        → Fase 4 (private set usado desde DENTRO de la clase)
    public void Respirar(Glucosa glucosa)
    {
        Atp += glucosa.Moleculas * Fisiologia.AtpRespiracionAerobica;
    }
}
