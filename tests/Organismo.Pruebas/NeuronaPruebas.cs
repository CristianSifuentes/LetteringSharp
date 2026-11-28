// ============================================================================
// Fase 4 · Paso 9 — Pruebas del primary constructor de clase (Neurona).
// Patrón AAA: Arrange (preparar), Act (actuar), Assert (comprobar).
// ============================================================================
using Organismo.Biologia;
using Organismo.Biologia.Celulas;
using Organismo.Biologia.Organulos;

namespace Organismo.Pruebas;

public class NeuronaPruebas
{
    // Disparar gasta ATP de la mitocondria que se le pasó al construir.
    [Fact]
    public void DispararGastaAtpDeSuMitocondria()
    {
        // Arrange
        var mitocondria = new Mitocondria(atpInicial: 30);
        var neurona = new Neurona(mitocondria);

        // Act
        neurona.Disparar();

        // Assert: es la MISMA mitocondria (referencia, Paso 1), así que vemos el gasto desde fuera
        Assert.Equal(30 - Fisiologia.AtpPorImpulso, mitocondria.Atp);
        Assert.Equal(1, neurona.Impulsos);
    }

    // Sin ATP suficiente, Disparar lanza y la neurona no cuenta el impulso (invariante, Paso 4).
    [Fact]
    public void SinAtpNoDisparaYNoCuentaElImpulso()
    {
        // Arrange
        var neurona = new Neurona(new Mitocondria());   // 0 ATP

        // Act
        var accion = () => neurona.Disparar();

        // Assert
        Assert.Throws<InvalidOperationException>(accion);
        Assert.Equal(0, neurona.Impulsos);
    }

    // Dispara hasta agotar la mitocondria: 30 ATP / 3 por impulso = 10 impulsos.
    [Fact]
    public void DisparaHastaAgotarLaMitocondria()
    {
        // Arrange
        var mitocondria = new Mitocondria(atpInicial: 30);
        var neurona = new Neurona(mitocondria);

        // Act
        while (!mitocondria.EstaAgotada)
        {
            neurona.Disparar();
        }

        // Assert
        Assert.Equal(30 / Fisiologia.AtpPorImpulso, neurona.Impulsos);
    }

    // El inicializador del campo valida el parámetro del primary constructor.
    [Fact]
    public void RechazaUnaMitocondriaNula()
    {
        // Arrange + Act
        var accion = () => new Neurona(null!);

        // Assert
        Assert.Throws<ArgumentNullException>(accion);
    }
}
