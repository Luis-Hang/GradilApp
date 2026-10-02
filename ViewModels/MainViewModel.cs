using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GradilApp.Commands;
using GradilApp.Models;
using GradilApp.Services;

namespace GradilApp.ViewModels;

/// <summary>
/// Representa o estado e as operações da tela principal da aplicação.
/// </summary>
public class MainViewModel : ViewModelBase
{
	#region Constantes

	// Comprimento máximo a ser desenhado em metros.
	private const decimal ComprimentoMaximoDesenho = 250;

	// Comprimeto do modulo equivalente a da tela.
	private const double ComprimentoModulo = 2.5;

	// Largura da representação da tela em pixels.
	private const double LarguraModuloDesenho = 120;

	#endregion

	#region Atributos

	/// <summary>
	/// Serviço responsável pelos cálculos dos componentes do gradil.
	/// </summary>
	private readonly CalculadoraGradilService CalculadoraService;

	/// <summary>
	/// Serviço responsável pelo armazenamento e recuperação dos pedidos.
	/// </summary>
	private readonly PedidoService PedidoService;

	/// <summary>
	/// Comprimento desejado para a cerca.
	/// </summary>
	private decimal? ComprimentoDesejadoAttr;

	/// <summary>
	/// Altura selecionada para a cerca.
	/// </summary>
	private AlturaGradil AlturaSelecionadaAttr;

	/// <summary>
	/// Cor selecionada para a cerca.
	/// </summary>
	private CorGradil CorSelecionadaAttr;

	/// <summary>
	/// Pedido que está aguardando confirmação.
	/// </summary>
	private Pedido? PedidoEmConfirmacao;

	#endregion

	#region Comandos

	/// <summary>
	/// Comando responsável por excluir um pedido.
	/// </summary>
	public ICommand ExcluirPedidoCommand { get; }

	/// <summary>
	/// Comando responsável por excluir todos os pedidos.
	/// </summary>
	public ICommand ExcluirTodosPedidosCommand { get; }

	/// <summary>
	/// Comando responsável por calcular os componentes do gradil.
	/// </summary>
	public ICommand CalcularCommand { get; }

	/// <summary>
	/// Comando responsável por confirmar o pedido.
	/// </summary>
	public ICommand ConfirmarPedidoCommand { get; }

	/// <summary>
	/// Comando responsável por abrir a janela de pedidos confirmados.
	/// </summary>
	public ICommand PedidosCommand { get; }

	#endregion

	#region Propriedades

	/// <summary>
	/// Obtém ou define o comprimento desejado para a cerca.
	/// </summary>
	public decimal? ComprimentoDesejado
	{
		get => ComprimentoDesejadoAttr;
		set
		{
			if (SetProperty(ref ComprimentoDesejadoAttr, value))
			{
				Resultado = null;
				ModulosDesenho.Clear();
				OnPropertyChanged(nameof(Resultado));
				OnPropertyChanged(nameof(DiferencaComprimentoTexto));
				OnPropertyChanged(nameof(DesenhoLimitado));
			}
		}
	}

	/// <summary>
	/// Obtém as alturas disponíveis para seleção.
	/// </summary>
	public AlturaGradil[] Alturas { get; }

	/// <summary>
	/// Obtém ou define a altura selecionada.
	/// </summary>
	public AlturaGradil AlturaSelecionada
	{
		get => AlturaSelecionadaAttr;
		set
		{
			if (SetProperty(ref AlturaSelecionadaAttr, value))
			{
				OnPropertyChanged(nameof(Altura103Selecionada));
				OnPropertyChanged(nameof(Altura153Selecionada));
				OnPropertyChanged(nameof(Altura203Selecionada));
				RecalcularSeNecessario();
			}
		}
	}

	/// <summary>
	/// Obtém as opções de pintura disponíveis para seleção.
	/// </summary>
	public CorGradil[] Cores { get; }

	/// <summary>
	/// Obtém ou define a cor selecionada.
	/// </summary>
	public CorGradil CorSelecionada
	{
		get => CorSelecionadaAttr;
		set
		{
			if (SetProperty(ref CorSelecionadaAttr, value))
			{
				OnPropertyChanged(nameof(SemPinturaSelecionada));
				OnPropertyChanged(nameof(BrancaSelecionada));
				OnPropertyChanged(nameof(PretaSelecionada));
				OnPropertyChanged(nameof(VerdeSelecionada));
			}
		}
	}

	/// <summary>
	/// Obtém ou define as quantidades calculadas para o gradil.
	/// </summary>
	public ComponentesGradil? Resultado { get; private set; }

	/// <summary>
	/// Obtém os pedidos confirmados armazenados.
	/// </summary>
	public ObservableCollection<Pedido> PedidosConfirmados { get; }

	/// <summary>
	/// Obtém os módulos de tela utilizados no desenho da cerca.
	/// </summary>
	public ObservableCollection<ModuloGradilViewModel> ModulosDesenho { get; } = new();

	/// <summary>
	/// Obtém ou define um valor que indica se a altura de 1,03 metro está selecionada.
	/// </summary>
	public bool Altura103Selecionada
	{
		get => AlturaSelecionada == AlturaGradil.UmMetroTres;
		set
		{
			if (value)
			{
				AlturaSelecionada = AlturaGradil.UmMetroTres;
			}
		}
	}

	/// <summary>
	/// Obtém ou define um valor que indica se a altura de 1,53 metro está selecionada.
	/// </summary>
	public bool Altura153Selecionada
	{
		get => AlturaSelecionada == AlturaGradil.UmMetroCinquentaETres;
		set
		{
			if (value)
			{
				AlturaSelecionada = AlturaGradil.UmMetroCinquentaETres;
			}
		}
	}

	/// <summary>
	/// Obtém ou define um valor que indica se a altura de 2,03 metros está selecionada.
	/// </summary>
	public bool Altura203Selecionada
	{
		get => AlturaSelecionada == AlturaGradil.DoisMetrosTres;
		set
		{
			if (value)
			{
				AlturaSelecionada = AlturaGradil.DoisMetrosTres;
			}
		}
	}

	/// <summary>
	/// Obtém ou define um valor que indica se o gradil sem pintura está selecionado.
	/// </summary>
	public bool SemPinturaSelecionada
	{
		get => CorSelecionada == CorGradil.SemPintura;
		set
		{
			if (value)
			{
				CorSelecionada = CorGradil.SemPintura;
			}
		}
	}

	/// <summary>
	/// Obtém ou define um valor que indica se a cor branca está selecionada.
	/// </summary>
	public bool BrancaSelecionada
	{
		get => CorSelecionada == CorGradil.Branca;
		set
		{
			if (value)
			{
				CorSelecionada = CorGradil.Branca;
			}
		}
	}

	/// <summary>
	/// Obtém ou define um valor que indica se a cor preta está selecionada.
	/// </summary>
	public bool PretaSelecionada
	{
		get => CorSelecionada == CorGradil.Preta;
		set
		{
			if (value)
			{
				CorSelecionada = CorGradil.Preta;
			}
		}
	}

	/// <summary>
	/// Obtém ou define um valor que indica se a cor verde está selecionada.
	/// </summary>
	public bool VerdeSelecionada
	{
		get => CorSelecionada == CorGradil.Verde;
		set
		{
			if (value)
			{
				CorSelecionada = CorGradil.Verde;
			}
		}
	}

	/// <summary>
	/// Obtém a diferença de comprimento formatada para apresentação na interface.
	/// </summary>
	public string DiferencaComprimentoTexto
	{
		get
		{
			if (Resultado is null || Resultado.DiferencaComprimento <= 0)
				return string.Empty;
			return $"Diferença: {Resultado.DiferencaComprimento:F2} m";
		}
	}

	/// <summary>
	/// Obtém um valor que indica se o desenho deve ser limitado.
	/// </summary>
	public bool DesenhoLimitado => ComprimentoDesejado > ComprimentoMaximoDesenho;

	#endregion

	#region Eventos

	/// <summary>
	/// Solicita a abertura da janela de pedidos confirmados.
	/// </summary>
	public event Action? OnAberturaPedidosSolicitada;

	/// <summary>
	/// Solicita à interface a abertura da janela de confirmação do pedido.
	/// </summary>
	public event Action<Pedido>? OnConfirmacaoPedidoSolicitada;

	#endregion

	#region Construtor

	/// <summary>
	/// Inicializa uma nova instância do ViewModel principal.
	/// </summary>
	public MainViewModel()
	{
		CalculadoraService = new CalculadoraGradilService();
		PedidoService = new PedidoService();

		Alturas = Enum.GetValues<AlturaGradil>();
		Cores = Enum.GetValues<CorGradil>();

		PedidosConfirmados = new ObservableCollection<Pedido>(
			PedidoService.ObterTodos());

		CalcularCommand = new RelayCommand(Calcular);
		ConfirmarPedidoCommand = new RelayCommand(ConfirmarPedido);
		PedidosCommand = new RelayCommand(SolicitarAberturaPedidos);
		ExcluirPedidoCommand = new RelayCommand<Pedido>(ExcluirPedido);
		ExcluirTodosPedidosCommand = new RelayCommand(ExcluirTodosPedidos);

		AlturaSelecionada = Alturas.First();
		CorSelecionada = Cores.First();
	}

	#endregion

	#region Métodos

	private void RecalcularSeNecessario()
	{
		if (ComprimentoDesejado is null || ComprimentoDesejado <= 0)
			return;
		Calcular();
	}

	/// <summary>
	/// Solicita a abertura da janela de pedidos confirmados.
	/// </summary>
	private void SolicitarAberturaPedidos()
	{
		OnAberturaPedidosSolicitada?.Invoke();
	}

	/// <summary>
	/// Calcula os componentes necessários para o gradil.
	/// </summary>
	private void Calcular()
	{
		if (ComprimentoDesejado is null || ComprimentoDesejado <= 0)
		{
			Resultado = null;
			ModulosDesenho.Clear();
			OnPropertyChanged(nameof(Resultado));
			OnPropertyChanged(nameof(DiferencaComprimentoTexto));
			return;
		}

		Resultado = CalculadoraService.Calcular(
			(double)ComprimentoDesejado.Value,
			AlturaSelecionada);

		OnPropertyChanged(nameof(Resultado));
		OnPropertyChanged(nameof(DiferencaComprimentoTexto));

		GerarDesenho();
	}

	/// <summary>
	/// Gera os módulos utilizados no desenho da cerca.
	/// </summary>
	private void GerarDesenho()
	{
		ModulosDesenho.Clear();

		if (Resultado is null)
			return;

		double comprimentoRepresentado = Math.Min(
			(double)(ComprimentoDesejado ?? 0),
			(double)ComprimentoMaximoDesenho);

		int quantidadeTelasRepresentadas =
			(int)Math.Ceiling(comprimentoRepresentado / ComprimentoModulo);

		for (int indice = 0; indice < quantidadeTelasRepresentadas; indice++)
		{
			ModulosDesenho.Add(new ModuloGradilViewModel
			{
				Largura = LarguraModuloDesenho
			});
		}
	}

	/// <summary>
	/// Confirma e armazena o pedido atual.
	/// </summary>
	private void ConfirmarPedido()
	{
		if (ComprimentoDesejado is null || ComprimentoDesejado <= 0)
		{
			Resultado = null;
			ModulosDesenho.Clear();
			OnPropertyChanged(nameof(Resultado));
			OnPropertyChanged(nameof(DiferencaComprimentoTexto));

			return;
		}

		if (Resultado is null)
			Calcular();

		if (Resultado is null)
			return;

		Pedido pedido = new()
		{
			Gradil = new Gradil
			{
				Comprimento = (double)ComprimentoDesejado.Value,
				Altura = AlturaSelecionada,
				Cor = CorSelecionada
			},
			Componentes = Resultado,
		};

		PedidoEmConfirmacao = pedido;
		OnConfirmacaoPedidoSolicitada?.Invoke(pedido);
	}

	/// <summary>
	/// Confirma e armazena o pedido que está aguardando confirmação.
	/// </summary>
	public void RealizarPedido()
	{
		if (PedidoEmConfirmacao is null)
			return;

		PedidoService.Salvar(PedidoEmConfirmacao);
		PedidosConfirmados.Insert(0, PedidoEmConfirmacao);
		PedidoEmConfirmacao = null;
	}

	/// <summary>
	/// Cancela o pedido que está aguardando confirmação.
	/// </summary>
	public void CancelarPedido()
	{
		PedidoEmConfirmacao = null;
	}

	/// <summary>
	/// Exclui um pedido confirmado do armazenamento e da lista exibida na interface.
	/// </summary>
	/// <param name="pedido">Pedido que será excluído.</param>
	private void ExcluirPedido(Pedido pedido)
	{
		PedidoService.Excluir(pedido);
		PedidosConfirmados.Remove(pedido);
	}

	/// <summary>
	/// Exclui todos os pedidos confirmados do armazenamento e da lista exibida na interface.
	/// </summary>
	private void ExcluirTodosPedidos()
	{
		PedidoService.ExcluirTodos();
		PedidosConfirmados.Clear();
	}

	#endregion
}