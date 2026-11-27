namespace Organismo.Biologia.Moleculas; // Namespace file-scoped (C# 10.0) termina en ; y aplica a todo el archivo, 
//sin llaves ni en un nivel mas de sangria.

public sealed record Adn(string Secuencia)
{
  public string Secuencia { get; } =  Bases.Validar(Secuencia);
  public int Longitud => Secuencia.Length;
  public static Adn Humano { get; } = new("ATGGTGCACCTGACTCCTGAGGAG");   // inicio del gen de la β-globina
  
  // crea una copia del record. Entre las llaves podrias cambiar valores, por ejemplo 
  // with { Secuencia = "ATGGTGCACCTGACTCCTGAGGAG" } crea un nuevo Adn con la secuencia de Humano.
  public Adn Replicar() => this with {};
}

// 'file' (C# 11): el tipo mas encapsulado que existe. Solo este archivo lo ve,
file static class Bases
{
    // Bases solo existe en Adn.cs. Ni Moleculas.cs ni la App pueden verla. Es una herramienta privada del archivo.
    private const string Validas = "ACGT";

    public static string Validar(string secuencia)
    {
        // La Biblioteca de Clases Base (BCL, por sus siglas en inglés Base Class Library) en .NET es el conjunto fundamental de clases, interfaces, tipos de datos y utilidades estándar que sirven como base para crear aplicaciones en el ecosistema .NET. 
        ArgumentException.ThrowIfNullOrWhiteSpace(secuencia);
        var normal = secuencia.ToUpperInvariant();

        // LINQ: comprueba que cada letra este en "ACGT". Validas.Contains se pasa como funcion.
        return normal.All(Validas.Contains) 
            ? normal 
            : throw new ArgumentException($"La secuencia de ADN contiene bases no válidas. Solo se permiten: {Validas}", nameof(secuencia));

    }
}