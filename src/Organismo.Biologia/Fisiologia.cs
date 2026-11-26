
// Regla de ubicacion: en C# no existen las variables ni las funciones globales.
// Lo mas parecido a un global es un miembro const o static dentro de una static class.

namespace Organismo.Biologia;// Namespace file-scoped (C# 10.0) termina en ; y aplica a todo el archivo, 
//sin llaves ni en un nivel mas de sangria.

// Visible desde otros proyectos (La app y las pruebas)
public static class Fisiologia
{
    // No se puede crear con new Fisiologia() ni heredar de ella. Solo admite miembros static o const. Se usa asi: Fisiologia.UmbralDisparoMv. 
    public const double TemperaturaNormal = 37.0; // Temperatura corporal media en grados Celsius
    public const double PotencialReposoMv = -70.0;     // una neurona en silencio
    public const double UmbralDisparoMv = -55.0;       // a partir de aquí dispara
    // const Valor fijado al compilar. 
    // El compilador copia el numero en cada sitio donde se usa. 
    // Solo admite tipos simples: numeros, bool, char, string.
    public const int AtpRespiracionAerobica = 30;      // por glucosa, con mitocondrias
    public const int AtpGlucolisis = 2;                // por glucosa, sin mitocondrias
    public const int AtpPorImpulso = 3;
    public const int AtpPorContraccion = 2;

    // Lo mas parecido a un global es un miembro const o static de una static class.
    //  Propiedad de solo lectura que pertenece al tipo. Se calcula al ejecutar, una sola vez.
    public static TimeSpan VidaGlobuloRojo { get; } = TimeSpan.FromDays(120); // Vida media de un glóbulo rojo en circulación

}