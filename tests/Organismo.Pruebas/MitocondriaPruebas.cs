// ============================================================================
// Fase 4 — Pruebas de clases con estado.
// Cada prueba sigue el patrón AAA: Arrange (preparar), Act (actuar), Assert (comprobar).
// ============================================================================
using Organismo.Biologia.Organulos;

namespace Organismo.Pruebas;

public class MitocondriaPruebas
{
    // Fase 4 · Paso 1 — Igualdad por referencia: dos objetos nuevos nunca son iguales,
    // aunque "contengan lo mismo". Compáralo con DosAdnConLaMismaSecuenciaSonIguales (Fase 3).
    [Fact]
    public void DosMitocondriasNuevasSonDistintas()
    {
        // Arrange
        var a = new Mitocondria();
        var b = new Mitocondria();

        // Act + Assert
        Assert.NotEqual(a, b);
        Assert.False(a == b);
    }

    // Fase 4 · Paso 1 — Una referencia copiada apunta al MISMO objeto.
    [Fact]
    public void LaMismaReferenciaEsIgual()
    {
        // Arrange
        var a = new Mitocondria();

        // Act
        var otra = a;   // no copia el objeto: copia la referencia

        // Assert
        Assert.Same(a, otra);
        Assert.True(a == otra);
    }
}
