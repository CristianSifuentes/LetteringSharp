// ============================================================================
// Fase 4 — Pruebas de clases con estado.
// Cada prueba sigue el patrón AAA: Arrange (preparar), Act (actuar), Assert (comprobar).
// ============================================================================
using Organismo.Biologia;
using Organismo.Biologia.Moleculas;
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

    // Fase 4 · Paso 2 — Atp se puede leer desde fuera y empieza en 0 (valor por defecto de int).
    // Escribir  mitocondria.Atp = 5;  aquí no compilaría: CS0272.
    [Fact]
    public void UnaMitocondriaNuevaEmpiezaSinAtp()
    {
        // Arrange
        var mitocondria = new Mitocondria();

        // Act
        var atp = mitocondria.Atp;   // leer sí está permitido (get público)

        // Assert
        Assert.Equal(0, atp);
    }

    // Fase 4 · Paso 3 — Un método de instancia cambia el estado del objeto.
    [Fact]
    public void RespirarDosGlucosasProduceSesentaAtp()
    {
        // Arrange
        var mitocondria = new Mitocondria();

        // Act
        mitocondria.Respirar(new Glucosa(2));

        // Assert
        Assert.Equal(2 * Fisiologia.AtpRespiracionAerobica, mitocondria.Atp);   // 60
    }

    // Fase 4 · Paso 3 — El estado se conserva entre llamadas: respirar dos veces acumula.
    [Fact]
    public void RespirarDosVecesAcumula()
    {
        // Arrange
        var mitocondria = new Mitocondria();

        // Act
        mitocondria.Respirar(new Glucosa(1));
        mitocondria.Respirar(new Glucosa(1));

        // Assert
        Assert.Equal(60, mitocondria.Atp);
    }

    // Fase 4 · Paso 3 — Cada objeto tiene su propio estado (une Paso 1 y Paso 3).
    // Con un miembro 'static' esto sería imposible: habría un único Atp compartido.
    [Fact]
    public void DosMitocondriasTienenAtpIndependiente()
    {
        // Arrange
        var activa = new Mitocondria();
        var quieta = new Mitocondria();

        // Act
        activa.Respirar(new Glucosa(1));

        // Assert
        Assert.Equal(30, activa.Atp);
        Assert.Equal(0, quieta.Atp);
    }

    // Fase 4 · Paso 4 — Gastar resta del ATP disponible.
    [Fact]
    public void GastarRestaAtp()
    {
        // Arrange
        var mitocondria = new Mitocondria();
        mitocondria.Respirar(new Glucosa(1));   // 30

        // Act
        mitocondria.Gastar(Fisiologia.AtpPorImpulso);   // 3

        // Assert
        Assert.Equal(27, mitocondria.Atp);
    }

    // Fase 4 · Paso 4 — Gastar exactamente todo es válido: el invariante es >= 0, no > 0.
    [Fact]
    public void GastarTodoDejaCero()
    {
        // Arrange
        var mitocondria = new Mitocondria();
        mitocondria.Respirar(new Glucosa(1));

        // Act
        mitocondria.Gastar(30);

        // Assert
        Assert.Equal(0, mitocondria.Atp);
    }

    // Fase 4 · Paso 4 — La clave del invariante: la excepción salta ANTES de cambiar el estado.
    [Fact]
    public void GastarMasDeLoQueHayLanzaYNoCambiaElAtp()
    {
        // Arrange
        var mitocondria = new Mitocondria();
        mitocondria.Respirar(new Glucosa(1));   // 30

        // Act
        var accion = () => mitocondria.Gastar(31);

        // Assert
        Assert.Throws<InvalidOperationException>(accion);
        Assert.Equal(30, mitocondria.Atp);      // intacto: no quedó a medias
    }

    // Fase 4 · Paso 4 — Un argumento inválido también se rechaza sin tocar el estado.
    [Fact]
    public void GastarUnaCantidadNegativaLanza()
    {
        // Arrange
        var mitocondria = new Mitocondria();
        mitocondria.Respirar(new Glucosa(1));

        // Act
        var accion = () => mitocondria.Gastar(-5);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(accion);
        Assert.Equal(30, mitocondria.Atp);
    }

    // Fase 4 · Paso 5 — La misma propiedad calculada cambia de valor según el estado.
    [Fact]
    public void EstaAgotadaSigueAlAtp()
    {
        // Arrange
        var mitocondria = new Mitocondria();

        // Act + Assert: nueva → agotada
        Assert.True(mitocondria.EstaAgotada);

        // Act + Assert: respira → ya no está agotada
        mitocondria.Respirar(new Glucosa(1));
        Assert.False(mitocondria.EstaAgotada);

        // Act + Assert: gasta todo → agotada otra vez
        mitocondria.Gastar(mitocondria.Atp);
        Assert.True(mitocondria.EstaAgotada);
    }
}
