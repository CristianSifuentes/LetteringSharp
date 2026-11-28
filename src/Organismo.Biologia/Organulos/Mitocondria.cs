// ============================================================================
// Fase 4 — Clases con estado (encapsulación).
// Paso 1: 'class' en vez de 'record'.
// Paso 2: propiedad { get; private set; }.
// Paso 3: método de instancia que cambia el estado.
// Paso 4: invariante (Atp nunca baja de 0) y validar ANTES de cambiar el estado.
// Paso 5: propiedad calculada sobre estado que cambia.
// Paso 6: sobrecarga de métodos.
// Paso 7: constructor normal y encadenar constructores con : this(...).
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
    // Su valor inicial lo pone el constructor (Paso 7).
    public int Atp { get; private set; }

    // Fase 4 · Paso 5 — Propiedad CALCULADA (expression-bodied, '=>'): no guarda nada,
    // se evalúa cada vez que se lee, a partir de Atp.
    // En la Fase 3, Adn.Longitud también era calculada, pero Secuencia nunca cambiaba:
    // siempre daba lo mismo. Aquí Atp cambia, así que EstaAgotada puede dar true, luego false,
    // luego true otra vez. No hace falta mantenerla "sincronizada": nunca se queda desfasada.
    // No tiene 'set': no se puede asignar ni desde dentro (solo se cambia cambiando Atp).
    public bool EstaAgotada => Atp == 0;

    // Fase 4 · Paso 7 — Constructor NORMAL: se llama como la clase y no devuelve nada.
    // Se ejecuta una vez, al hacer new, y deja el objeto en un estado válido desde el principio.
    // Frente al constructor primario del record (Fase 3: Adn(string Secuencia)), este:
    //   - no genera propiedades: las asignamos nosotros (Atp = atpInicial);
    //   - puede tener cuerpo { } con la validación y la lógica que haga falta.
    // Aplica el invariante del Paso 4 también al NACER: no hay mitocondrias con ATP negativo.
    public Mitocondria(int atpInicial)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(atpInicial);
        Atp = atpInicial;   // private set: desde dentro sí se puede escribir
    }

    // Fase 4 · Paso 7 — Encadenar constructores con ': this(...)'.
    // En cuanto declaras un constructor con parámetros, C# deja de generar el constructor
    // vacío automático, y  new Mitocondria()  dejaría de compilar (CS7036).
    // Este lo recupera reutilizando el otro: ': this(0)' llama a Mitocondria(0) ANTES de su
    // cuerpo (vacío). Así la validación y la asignación viven en un solo constructor.
    public Mitocondria() : this(0)
    {
    }

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

    // Fase 4 · Paso 6 — SOBRECARGA: mismo nombre, distintos parámetros.
    // El compilador elige la versión al COMPILAR mirando el tipo del argumento:
    //   m.Respirar(new Glucosa(2)) → Respirar(Glucosa)
    //   m.Respirar(2)              → Respirar(int)
    // Solo cuentan los parámetros, NO el tipo de retorno:
    //   public int Respirar(int otro) => 0;  → error CS0111 (ya existe un Respirar(int)).
    // Delega en la otra versión en vez de copiar la fórmula: la lógica vive en un solo sitio,
    // y la validación llega gratis: new Glucosa(-1) ya lanza (Fase 3).
    public void Respirar(int moleculas) => Respirar(new Glucosa(moleculas));

    // Fase 4 · Paso 4 — Invariante: una regla que el objeto cumple SIEMPRE → Atp >= 0.
    // Orden obligatorio: 1) validar  2) cambiar el estado.
    // Si algo falla, la excepción salta ANTES de tocar Atp, así el objeto queda exactamente
    // como estaba (nunca a medias). Si restáramos primero y validáramos después,
    // un fallo dejaría Atp en negativo: el invariante roto.
    public void Gastar(int atp)
    {
        // 1) Validar los argumentos: pedir una cantidad negativa no tiene sentido.
        //    Helper de la BCL, como ArgumentException.ThrowIfNullOrWhiteSpace en Adn (Fase 3).
        ArgumentOutOfRangeException.ThrowIfNegative(atp);

        // 1) Validar el estado: el argumento es correcto, pero el objeto no puede cumplirlo ahora.
        //    InvalidOperationException = "no en este momento", frente a ArgumentException = "dato malo".
        if (atp > Atp)
        {
            throw new InvalidOperationException($"ATP insuficiente: se piden {atp} y solo hay {Atp}.");
        }

        // 2) Cambiar el estado: aquí ya sabemos que el resultado será >= 0.
        Atp -= atp;
    }
}
