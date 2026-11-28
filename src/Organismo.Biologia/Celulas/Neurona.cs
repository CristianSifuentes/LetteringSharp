// ============================================================================
// Fase 4 — Clases con estado (encapsulación).
// Paso 9: primary constructor de clase (C# 12).
// ============================================================================

// Fase 2 — Namespace file-scoped. Fase 3: la carpeta Celulas/ se refleja en el namespace.
using Organismo.Biologia.Organulos;

namespace Organismo.Biologia.Celulas;

// Fase 4 · Paso 9 — PRIMARY CONSTRUCTOR de clase: los parámetros van junto al nombre del tipo.
// Parece el de un record (Fase 3: Adn(string Secuencia)), pero se comporta distinto:
//   record → genera una PROPIEDAD pública por parámetro:  adn.Secuencia  existe.
//   class  → NO genera nada público: 'mitocondria' es solo un parámetro visible dentro
//            de la clase.  neurona.mitocondria  desde fuera → error CS1061.
// Tampoco tiene cuerpo { }: para validar, se usa un inicializador de campo (abajo).
public sealed class Neurona(Mitocondria mitocondria)
{
    // Fase 4 · Paso 9 — Guardar el parámetro en un campo private readonly (Paso 8).
    // Así se valida al construir y queda protegido: el parámetro de un primary constructor
    // NO es readonly (se podría reasignar dentro de la clase); el campo sí.
    // Regla: o usas el parámetro en los métodos, o lo copias a un campo, pero NO ambas cosas.
    // Si en Disparar escribieras 'mitocondria.Gastar(...)' (sin '_'), habría DOS copias:
    // el campo y el parámetro capturado por el compilador.
    //   - Con  = mitocondria;  (asignación directa) el compilador avisa: CS9124, que aquí
    //     es error por TreatWarningsAsErrors (Directory.Build.props, Fase 1).
    //   - Con  = mitocondria ?? throw ...  (como abajo) NO avisa, pero las dos copias
    //     existen igual. Por eso la convención _nombre ayuda: en los métodos, usa siempre '_'.
    private readonly Mitocondria _mitocondria =
        mitocondria ?? throw new ArgumentNullException(nameof(mitocondria));

    // Fase 4 · Paso 2 — Contador de lectura pública y escritura privada.
    public int Impulsos { get; private set; }

    // Fase 4 · Paso 9 — Una clase usa a otra: la Neurona no fabrica ATP, se lo pide
    // a SU mitocondria (la misma referencia que le pasaron, Paso 1).
    // Paso 4 — Invariante: si Gastar lanza (no hay ATP), Impulsos no llega a cambiar.
    public void Disparar()
    {
        _mitocondria.Gastar(Fisiologia.AtpPorImpulso);
        Impulsos++;
    }
}
