using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using Plugin.Maui.Audio;

namespace AppSaude.MVVM.Views;

public partial class AlarmesView : ContentPage
{
    private readonly IService _service;

    private readonly IAudioManager _audioManager;

    public AlarmesView(IService servico, IAudioManager audioManager)
    {
        InitializeComponent();

        _audioManager = audioManager;

        _service = servico;

        var viewModel = new MainViewModel(servico);

        BindingContext = viewModel;
    }

    // Chama DisplayCommand automaticamente quando a página aparecer
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

    // Botão para navegar para AlarmeAddView
    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AlarmeAddView(_service, _audioManager));
    }
}
