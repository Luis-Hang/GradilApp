using System;

namespace GradilApp.Models;

/// <summary>
/// Representa um pedido de uma cerca de gradil.
/// </summary>
public class Pedido
{
	#region Propriedades

	/// <summary>
	/// Obtém ou define os dados da cerca solicitada.
	/// </summary>
	public Gradil Gradil { get; set; } = new();

	/// <summary>
	/// Identificador único do pedido.
	/// </summary>
	public Guid Id { get; set; } = Guid.NewGuid();

	/// <summary>
	/// Obtém ou define as quantidades de componentes calculadas para o pedido.
	/// </summary>
	public ComponentesGradil Componentes { get; set; } = new();

	/// <summary>
	/// Obtém ou define a data e hora em que o pedido foi confirmado.
	/// </summary>
	public DateTime? DataConfirmacao { get; set; }

	#endregion
}