using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Threading.Channels;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;

namespace AppSaude.MVVM.Views;

public partial class HomePageView : ContentPage
{
	private readonly IAlarmeService _service;

    private readonly IAudioManager _audioManager;
    public HomePageView(IAlarmeService alarmeService, IAudioManager audioManager)
    {
        InitializeComponent();

        _service = alarmeService;

        _service = new AlarmeService();

        var viewModel = new AlarmeViewModel(alarmeService);

        BindingContext = viewModel;
        _audioManager = audioManager;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // O BindingContext é da ViewModel que contém o DisplayCommand
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

  
    //Button de navegacao para AlarmesView
    private async void btnAlarme_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AlarmesView(_service, _audioManager));
    }
}