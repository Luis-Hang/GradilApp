using System;
using GradilApp.Models;

namespace GradilApp.Services;

/// <summary>
/// Realiza os cálculos necessários para determinar os componentes de uma cerca.
/// </summary>
public class CalculadoraGradilService
{
	#region Constantes

	/// <summary>
	/// Comprimento da tela em metros.
	/// </summary>
	private const double ComprimentoTela = 2.5;

	/// <summary>
	/// Quantidade de parafusos por poste.
	/// </summary>
	private const int ParafusosPorPoste = 4;

	#endregion

	#region Métodos

	/// <summary>
	/// Calcula os componentes necessários para construir a cerca.
	/// </summary>
	/// <param name="comprimentoDesejado">Comprimento desejado pelo cliente em metros.</param>
	/// <param name="altura">Altura selecionada para a cerca.</param>
	/// <returns>Quantidades calculadas para cada componente.</returns>
	public ComponentesGradil Calcular(double comprimentoDesejado, AlturaGradil altura)
	{
		int quantidadeTelas = CalcularQuantidadeTelas(comprimentoDesejado);
		int quantidadePostes = quantidadeTelas + 1;
		int quantidadeFixadores = quantidadePostes * ObterFixadoresPorTela(altura);
		int quantidadeParafusos = quantidadePostes * ParafusosPorPoste;

		// Retorna os componentes referentes ao comprimento desejado.
		return new ComponentesGradil
		{
			Telas = quantidadeTelas,
			Postes = quantidadePostes,
			Fixadores = quantidadeFixadores,
			Parafusos = quantidadeParafusos,
			ComprimentoVendido = quantidadeTelas * ComprimentoTela,
			DiferencaComprimento = quantidadeTelas * ComprimentoTela - comprimentoDesejado
		};
	}

	/// <summary>
	/// Calcula a quantidade de telas necessárias para atender o comprimento solicitado.
	/// </summary>
	private static int CalcularQuantidadeTelas(double comprimentoDesejado)
	{
		return (int)Math.Ceiling(comprimentoDesejado / ComprimentoTela);
	}

	/// <summary>
	/// Obtém a quantidade de fixadores necessária pela altura.
	/// </summary>
	private static int ObterFixadoresPorTela(AlturaGradil altura)
	{
		return altura switch
		{
			AlturaGradil.UmMetroTres => 3,
			AlturaGradil.UmMetroCinquentaETres => 4,
			AlturaGradil.DoisMetrosTres => 6,
			_ => throw new ArgumentOutOfRangeException(nameof(altura))
		};
	}

	#endregion
}