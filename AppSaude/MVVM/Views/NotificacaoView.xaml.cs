using AppSaude.MVVM.Models;
using AppSaude.MVVM.ViewModels;
using AppSaude.Services;


namespace AppSaude.MVVM.Views;

public partial class NotificacaoView : ContentPage
{
    private readonly IServicesTeste _services;
    public NotificacaoView(IServicesTeste services)
	{
		InitializeComponent();

        _services = services ?? throw new ArgumentNullException(nameof(services));

        var viewModel = new MainViewModel(_services);
        BindingContext = viewModel;

    }
   
    protected override async void OnAppearing()
    {
        base.OnAppearing();        
        try
        {
            // O BindingContext é da ViewModel que contém o DisplayCommand            
            if (BindingContext is MainViewModel viewModel)
            {
                // Dispara o DisplayCommand para carregar os dados automaticamente
                viewModel.DisplayCommand.Execute(null);
            }
            await LoadNotificacaoFromDatabaseAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private async Task<List<NotificacaoAlarme>> LoadNotificacaoFromDatabaseAsync()
    {
        try
        {
            // Busca todos os alarmes do banco de dados usando o serviço
            var notificacao = await _services.GetNotAlarmes();

            // Verifica se a lista retornada não é nula
            if (notificacao == null)
            {
                Console.WriteLine("NOTIFICACAOVIEW : Nenhum notificacao de alarme encontrado no banco de dados.");
                return new List<NotificacaoAlarme>();
            }

            Console.WriteLine($"NOTIFICACAOVIEW : Total de notificação de alarmes encontrados: {notificacao.Count}");
            return notificacao;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"NOTIFICACAOVIEW : Erro ao carregar alarmes: {ex.Message}");
            return new List<NotificacaoAlarme>();
        }
    }
}