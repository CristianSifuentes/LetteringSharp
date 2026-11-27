// ============================================================================
// Fase 3 — Pruebas de los records: validación, igualdad por valor y copia.
// ============================================================================
using Organismo.Biologia.Moleculas;

namespace Organismo.Pruebas;

public class AdnPruebas
{
    [Fact]
    public void NormalizaLaSecuenciaAMayusculas()
    {
        var adn = new Adn("atgc");

        Assert.Equal("ATGC", adn.Secuencia);
        Assert.Equal(4, adn.Longitud);
    }

    [Theory]
    [InlineData("ATGX")]
    [InlineData("AUGC")]   // U es ARN, no ADN
    [InlineData("")]
    [InlineData("   ")]
    public void RechazaSecuenciasInvalidas(string secuencia)
    {
        Assert.ThrowsAny<ArgumentException>(() => new Adn(secuencia));
    }

    [Fact]
    public void RechazaNull()
    {
        Assert.Throws<ArgumentNullException>(() => new Adn(null!));
    }

    [Fact]
    public void ReplicarDevuelveUnaCopiaIgualPeroDistinta()
    {
        var original = Adn.Humano;
        var copia = original.Replicar();

        Assert.Equal(original, copia);              // igualdad por valor (record)
        Assert.True(copia == original);
        Assert.NotSame(original, copia);            // pero son dos objetos
    }

    [Fact]
    public void DosAdnConLaMismaSecuenciaSonIguales()
    {
        Assert.Equal(new Adn("acgt"), new Adn("ACGT"));
    }

    [Fact]
    public void CalculaElContenidoGc()
    {
        Assert.Equal(0.5, new Adn("ATGC").ContenidoGc);
    }
}

public class OtrasMoleculasPruebas
{
    [Fact]
    public void GlucosaNegativaLanza()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Glucosa(-1));
    }

    [Fact]
    public void GlucosaCeroEsValida()
    {
        Assert.Equal(0, new Glucosa(0).Moleculas);
    }

    [Fact]
    public void ImpulsoAdmiteVoltajesNegativos()
    {
        Assert.Equal(-70.0, new ImpulsoElectrico(-70.0).Milivoltios);
    }

    [Fact]
    public void HormonaSinNombreLanza()
    {
        Assert.Throws<ArgumentException>(() => new Hormona(" ", 1.0));
    }

    [Fact]
    public void HormonaConConcentracionNegativaLanza()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Hormona("Insulina", -1.0));
    }
}
