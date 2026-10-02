using System.Text.Json.Serialization;

namespace GradilApp.Models;

/// <summary>
/// Representa uma cerca de gradil.
/// </summary>
public class Gradil
{
	#region Propriedades

	/// <summary>
	/// Obtém ou define o comprimento total da cerca em metros.
	/// </summary>
	public double Comprimento { get; set; }

	/// <summary>
	/// Obtém ou define a altura da cerca.
	/// </summary>
	public AlturaGradil Altura { get; set; }

	/// <summary>
	/// Obtém ou define a cor da cerca.
	/// </summary>
	public CorGradil Cor { get; set; }

	/// <summary>
	/// Obtém a descrição da altura da cerca.
	/// </summary>
	[JsonIgnore]
	public string AlturaDescricao
	{
		get
		{
			return Altura switch
			{
				AlturaGradil.UmMetroTres => "1,03 m",
				AlturaGradil.UmMetroCinquentaETres => "1,53 m",
				AlturaGradil.DoisMetrosTres => "2,03 m",
				_ => string.Empty
			};
		}
	}

	/// <summary>
	/// Obtém a descrição da cor da cerca.
	/// </summary>
	[JsonIgnore]
	public string CorDescricao
	{
		get
		{
			return Cor switch
			{
				CorGradil.SemPintura => "Sem pintura",
				CorGradil.Branca => "Branca",
				CorGradil.Preta => "Preta",
				CorGradil.Verde => "Verde",
				_ => string.Empty
			};
		}
	}

	#endregion
}