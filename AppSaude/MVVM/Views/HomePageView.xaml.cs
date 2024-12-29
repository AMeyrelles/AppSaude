using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Threading.Channels;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;

namespace AppSaude.MVVM.Views;

public partial class HomePageView : ContentPage
{
	private readonly IService _services;  

    private readonly IAudioManager _audioManager;
   
    public HomePageView(IService services, IAudioManager audioManager)
    {
        InitializeComponent();

        _services = services;
        _audioManager = audioManager;

        var viewModel = new MainViewModel(services);

        BindingContext = viewModel;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // O BindingContext é da ViewModel que contém o DisplayCommand
            var viewModel = BindingContext as MainViewModel;

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
        await Navigation.PushAsync(new AlarmesView(_services, _audioManager));
    }

    private async void btnAgendamentos_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AgendamentosView(_services, _audioManager));
    }
}