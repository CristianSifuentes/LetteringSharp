namespace Organismo.Biologia.Moleculas; // Namespace file-scoped (C# 10.0) termina en ; y aplica a todo el archivo, 
//sin llaves ni en un nivel mas de sangria.

public sealed record Glucosa(int Moleculas)
{
    public int Moleculas { get; } = Moleculas
      >= 0 ? Moleculas : throw new ArgumentOutOfRangeException(nameof(Moleculas), "La cantidad de moléculas de glucosa no puede ser negativa.");
}


public sealed record ImpulsoElectrico(double Milivoltios);
public sealed record Hormona(string Nombre, double Concentracion);