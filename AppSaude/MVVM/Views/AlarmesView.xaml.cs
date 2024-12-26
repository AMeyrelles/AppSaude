using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using Plugin.Maui.Audio;

namespace AppSaude.MVVM.Views;

public partial class AlarmesView : ContentPage
{
    private readonly IAlarmeService _service;

    private readonly IAudioManager _audioManager;

    public AlarmesView(IAlarmeService alarmeService, IAudioManager audioManager)
    {
        InitializeComponent();

        _audioManager = audioManager;

        _service = alarmeService;

        // Cria��o da ViewModel com a inje��o do servi�o
        var viewModel = new AlarmeViewModel(alarmeService);

        // Definindo o BindingContext da p�gina para a ViewModel
        BindingContext = viewModel;
    }

    // Chama DisplayCommand automaticamente quando a p�gina aparecer
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // O BindingContext � da ViewModel que cont�m o DisplayCommand
            var viewModel = BindingContext as AlarmeViewModel;
            if (viewModel != null)
            {
                // Dispara o DisplayCommand para carregar os dados automaticamente
                viewModel.DisplayCommand.Execute(null);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    // Bot�o para navegar para AlarmeAddView
    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AlarmeAddView(_service, _audioManager));
    }
}
