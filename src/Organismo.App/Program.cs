// ============================================================================
// Fase 1 — Top-level statements: Program.cs no necesita clase ni Main.
// El compilador genera ambos; el ejecutable se llama 'cuerpo' (ver .csproj).
// ============================================================================
using Organismo.Biologia;
using Organismo.Biologia.Celulas;
using Organismo.Biologia.Moleculas;
using Organismo.Biologia.Organulos;

// ----------------------------------------------------------------------------
// Fase 2 — Fisiologia: constantes y estáticos «globales», sin crear objetos.
// ----------------------------------------------------------------------------
Console.WriteLine("== Fase 2: Fisiologia ==");
Console.WriteLine($"Temperatura normal: {Fisiologia.TemperaturaNormal} °C");
Console.WriteLine($"Potencial de reposo: {Fisiologia.PotencialReposoMv} mV");
Console.WriteLine($"Umbral de disparo: {Fisiologia.UmbralDisparoMv} mV");
Console.WriteLine($"ATP en respiración aeróbica: {Fisiologia.AtpRespiracionAerobica}");
Console.WriteLine($"ATP en glucólisis: {Fisiologia.AtpGlucolisis}");
Console.WriteLine($"ATP por impulso: {Fisiologia.AtpPorImpulso}");
Console.WriteLine($"ATP por contracción: {Fisiologia.AtpPorContraccion}");
Console.WriteLine($"Vida media de un glóbulo rojo: {Fisiologia.VidaGlobuloRojo.TotalDays} días");

// ----------------------------------------------------------------------------
// Fase 3 — Records, igualdad por valor y visibilidad (public / internal / file).
// ----------------------------------------------------------------------------
Console.WriteLine();
Console.WriteLine("== Fase 3: Moléculas ==");

// Fase 3 — La validación normaliza a mayúsculas.
var adn = new Adn("atgcatgc");
Console.WriteLine($"ADN: {adn.Secuencia} ({adn.Longitud} bases, GC = {adn.ContenidoGc:P0})");

// Fase 3 — ToString generado por el record.
Console.WriteLine($"ADN humano: {Adn.Humano}");

// Fase 3 — Igualdad por valor: copia igual (==), pero es otro objeto en memoria.
var copia = adn.Replicar();
Console.WriteLine($"Copia == original: {copia == adn}");
Console.WriteLine($"Mismo objeto: {ReferenceEquals(copia, adn)}");

// Fase 3 — Validación en el constructor: una base inválida lanza excepción.
try
{
    _ = new Adn("ATGX");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Rechazado: {ex.Message}");
}

Console.WriteLine(new Glucosa(Moleculas: 1));
Console.WriteLine(new ImpulsoElectrico(Fisiologia.UmbralDisparoMv));
Console.WriteLine(new Hormona("Insulina", 12.5));

// Fase 3 — Esto NO compila desde la App (descoméntalo para ver el error):
// Ensamblado.ProporcionGc("ACGT");   // CS0122: 'Ensamblado' es inaccesible por su nivel de protección (internal)
// Bases.Validar("ACGT");             // CS0103: 'Bases' no existe en este contexto (file)

// ----------------------------------------------------------------------------
// Fase 4 — Clases con estado: una Mitocondria produce ATP y una Neurona lo gasta.
// ----------------------------------------------------------------------------
Console.WriteLine();
Console.WriteLine("== Fase 4: Clases con estado ==");

// Fase 4 · Pasos 7-8 — Constructor con argumentos con nombre; capacidad readonly.
var mitocondria = new Mitocondria(atpInicial: 0, capacidadMaxima: 100);
Console.WriteLine($"Mitocondria nueva: {mitocondria.Atp} ATP (capacidad {mitocondria.CapacidadMaxima}), agotada: {mitocondria.EstaAgotada}");

// Fase 4 · Pasos 3 y 6 — Método de instancia y su sobrecarga Respirar(int).
mitocondria.Respirar(new Glucosa(1));
mitocondria.Respirar(1);
Console.WriteLine($"Tras respirar 2 glucosas: {mitocondria.Atp} ATP, agotada: {mitocondria.EstaAgotada}");

// Fase 4 · Paso 9 — La Neurona recibe la MISMA mitocondria (referencia, Paso 1).
var neurona = new Neurona(mitocondria);
while (!mitocondria.EstaAgotada)
{
    neurona.Disparar();
}
Console.WriteLine($"La neurona disparó {neurona.Impulsos} impulsos hasta agotar la mitocondria ({mitocondria.Atp} ATP)");

// Fase 4 · Paso 4 — Invariante: sin ATP, Disparar lanza y el estado queda intacto.
try
{
    neurona.Disparar();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Rechazado: {ex.Message} Impulsos sigue en {neurona.Impulsos}.");
}

// Fase 4 — Esto NO compila (descoméntalo para ver el error):
// mitocondria.Atp = 9999;          // CS0272: el set es privado (Paso 2)
// _ = mitocondria with { };        // CS8858: 'with' solo existe en records (Paso 1)
// _ = neurona.mitocondria;         // CS1061: el primary constructor de clase no genera propiedades (Paso 9)
