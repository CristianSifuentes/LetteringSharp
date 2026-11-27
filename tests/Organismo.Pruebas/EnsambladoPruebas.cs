// ============================================================================
// Fase 3 — internal + InternalsVisibleTo.
// Ensamblado es 'internal' en Organismo.Biologia. Esta prueba compila solo porque
// Organismo.Biologia.csproj declara <InternalsVisibleTo Include="Organismo.Pruebas" />.
// Quita esa línea del .csproj y este archivo dará CS0122.
// ============================================================================
using Organismo.Biologia;

namespace Organismo.Pruebas;

public class EnsambladoPruebas
{
    [Theory]
    [InlineData("GGCC", 1.0)]
    [InlineData("ATAT", 0.0)]
    [InlineData("ATGC", 0.5)]
    [InlineData("", 0.0)]
    public void CalculaLaProporcionGc(string secuencia, double esperado)
    {
        Assert.Equal(esperado, Ensamblado.ProporcionGc(secuencia));
    }
}
