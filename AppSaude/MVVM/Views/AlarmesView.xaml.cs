using AppSaude.MVVM.ViewModels;
using AppSaude.Services;

namespace AppSaude.MVVM.Views;

public partial class AlarmesView : ContentPage
{
    private readonly IAlarmeService _service;

    public AlarmesView(IAlarmeService alarmeService)
    {
        InitializeComponent();

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
            // Verifique se o BindingContext � da ViewModel que cont�m o DisplayCommand
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
        await Navigation.PushAsync(new AlarmeAddView(_service));
    }
}
