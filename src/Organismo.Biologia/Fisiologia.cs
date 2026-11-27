// ============================================================================
// Fase 2 — Fisiologia: el «global» de C#.
// Regla de ubicación: en C# no existen las variables ni las funciones globales.
// Lo más parecido a un global es un miembro const o static dentro de una static class.
// ============================================================================

// Fase 2 — Namespace file-scoped (C# 10): termina en ';' y aplica a todo el archivo,
// sin llaves ni un nivel más de sangría.
namespace Organismo.Biologia;

// Fase 2 — public: visible desde otros proyectos (la App y las pruebas).
// static class: no se puede crear con new Fisiologia() ni heredar de ella.
// Solo admite miembros static o const. Se usa así: Fisiologia.UmbralDisparoMv.
public static class Fisiologia
{
    // Fase 2 — const: valor fijado al compilar.
    // El compilador copia el número en cada sitio donde se usa.
    // Solo admite tipos simples: números, bool, char, string.
    public const double TemperaturaNormal = 37.0;      // temperatura corporal media en grados Celsius
    public const double PotencialReposoMv = -70.0;     // una neurona en silencio
    public const double UmbralDisparoMv = -55.0;       // a partir de aquí dispara

    public const int AtpRespiracionAerobica = 30;      // por glucosa, con mitocondrias
    public const int AtpGlucolisis = 2;                // por glucosa, sin mitocondrias
    public const int AtpPorImpulso = 3;
    public const int AtpPorContraccion = 2;

    // Fase 2 — static { get; }: propiedad de solo lectura que pertenece al tipo.
    // Se calcula al ejecutar, una sola vez. TimeSpan no puede ser const (CS0283).
    public static TimeSpan VidaGlobuloRojo { get; } = TimeSpan.FromDays(120); // vida media de un glóbulo rojo en circulación
}
