using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using GradilApp.Models;

namespace GradilApp.Services;

/// <summary>
/// Responsável pelo armazenamento e recuperação dos pedidos confirmados.
/// </summary>
public class PedidoService
{
	#region Atributos

	/// <summary>
	/// Caminho onde ficará o arquivo com os pedidos.
	/// </summary>
	private readonly string caminhoArquivo;

	/// <summary>
	/// Define as opções utilizadas na serialização e desserialização dos pedidos em JSON.
	/// </summary>
	private readonly JsonSerializerOptions opcoesJson;

	#endregion

	#region Construtor

	/// <summary>
	/// Inicializa uma nova instância do serviço de pedidos.
	/// </summary>
	public PedidoService()
	{
		// Configura o diretório de armazenamento e as opções utilizadas para
		// serializar e desserializar os pedidos no arquivo JSON.
		string diretorioDados = Path.Combine(AppContext.BaseDirectory, "Data");
		Directory.CreateDirectory(diretorioDados);
		caminhoArquivo = Path.Combine(diretorioDados, "pedidos.json");
		opcoesJson = new JsonSerializerOptions
		{
			WriteIndented = true
		};
		opcoesJson.Converters.Add(new JsonStringEnumConverter());
	}

	#endregion

	#region Métodos

	/// <summary>
	/// Salva um pedido confirmado no arquivo de armazenamento.
	/// </summary>
	/// <param name="pedido">Pedido que será armazenado.</param>
	public void Salvar(Pedido pedido)
	{
		// Obtém a data da realização do pedido.
		pedido.DataConfirmacao = DateTime.Now;

		// Obtém todos os pedidos e adiciona o novo pedido.
		List<Pedido> pedidos = ObterTodos().ToList();
		pedidos.Add(pedido);
		string json = JsonSerializer.Serialize(pedidos, opcoesJson);
		File.WriteAllText(caminhoArquivo, json);
	}

	/// <summary>
	/// Excluí um pedido específico.
	/// </summary>
	/// <param name="pedido">Pedido que será excluído.</param>
	public void Excluir(Pedido pedido)
	{
		// Obtém todos os pedidos, exceto o pedido que será excluído e salva.
		List<Pedido> pedidos = ObterTodos()
			.Where(item => item.Id != pedido.Id)
			.ToList();
		string json = JsonSerializer.Serialize(pedidos, opcoesJson);
		File.WriteAllText(caminhoArquivo, json);
	}

	/// <summary>
	/// Exclui todos os pedidos.
	/// </summary>
	public void ExcluirTodos()
	{
		File.WriteAllText(caminhoArquivo, "[]");
	}

	/// <summary>
	/// Obtém todos os pedidos confirmados ordenados do mais recente para o mais antigo.
	/// </summary>
	/// <returns>Lista de pedidos confirmados ordenada pela data de confirmação.</returns>
	public IReadOnlyList<Pedido> ObterTodos()
	{
		if (!File.Exists(caminhoArquivo))
			return Array.Empty<Pedido>();

		string json = File.ReadAllText(caminhoArquivo);
		if (string.IsNullOrWhiteSpace(json))
			return Array.Empty<Pedido>();


		List<Pedido>? pedidos = JsonSerializer.Deserialize<List<Pedido>>(
			json, opcoesJson);
		return (pedidos ?? new List<Pedido>())
			.OrderByDescending(pedido => pedido.DataConfirmacao)
			.ToList();
	}

	#endregion
}