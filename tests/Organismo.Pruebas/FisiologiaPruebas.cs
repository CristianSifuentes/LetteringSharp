// ============================================================================
// Fase 1 — Proyecto xUnit: [Fact] es una prueba sin parámetros,
// [Theory] + [InlineData] repite la misma prueba con varios datos.
// 'using Xunit;' viene del <Using Include="Xunit" /> del .csproj.
// ============================================================================
using Organismo.Biologia;

namespace Organismo.Pruebas;

// Fase 2 — Las constantes de Fisiologia tienen sentido biológico.
public class FisiologiaPruebas
{
    [Fact]
    public void ElUmbralDeDisparoEstaPorEncimaDelReposo()
    {
        Assert.True(Fisiologia.UmbralDisparoMv > Fisiologia.PotencialReposoMv);
    }

    [Fact]
    public void LaRespiracionAerobicaRindeMasQueLaGlucolisis()
    {
        Assert.True(Fisiologia.AtpRespiracionAerobica > Fisiologia.AtpGlucolisis);
    }

    [Fact]
    public void UnGloboRojoViveCientoVeinteDias()
    {
        Assert.Equal(TimeSpan.FromDays(120), Fisiologia.VidaGlobuloRojo);
    }
}
