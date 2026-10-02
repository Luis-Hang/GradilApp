namespace GradilApp.Models;

/// <summary>
/// Representa as quantidades de componentes necessárias para construir uma cerca.
/// </summary>
public class ComponentesGradil
{
    /// <summary>
	/// Obtém ou define a quantidade de postes necessários.
	/// </summary>
	public int Postes { get; set; }

	/// <summary>
	/// Obtém ou define a quantidade de telas necessárias.
	/// </summary>
	public int Telas { get; set; }

	/// <summary>
	/// Obtém ou define a quantidade de fixadores necessários.
	/// </summary>
	public int Fixadores { get; set; }

	/// <summary>
	/// Obtém ou define a quantidade de parafusos necessários.
	/// </summary>
	public int Parafusos { get; set; }

    /// <summary>
	/// Obtém ou define o comprimento total da cerca que será vendida em metros.
	/// </summary>
	public double ComprimentoVendido { get; set; }

	/// <summary>
	/// Obtém ou define a diferença entre o comprimento vendido e o solicitado.
	/// </summary>
	public double DiferencaComprimento { get; set; }
}