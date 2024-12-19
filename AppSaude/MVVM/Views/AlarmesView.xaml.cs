
using AppSaude.MVVM.ViewModels;
using AppSaude.Services;

namespace AppSaude.MVVM.Views;

public partial class AlarmesView : ContentPage
{
	private readonly IAlarmeService _service;
	public AlarmesView(IAlarmeService alarmeService)
	{
        InitializeComponent();

        // Criação da ViewModel com a injeção do serviço
        var viewModel = new AlarmeViewModel(alarmeService);
        BindingContext = viewModel;
    }

    // Chama DisplayCommand automaticamente quando a página aparecer

    public AlarmeViewModel DisparoAutoViewModel { get; }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // Dispara o DisplayCommand para carregar os dados automaticamente
            DisparoAutoViewModel.DisplayCommand.Execute(null);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    // Botão para navergar AlarmeAddView
    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AlarmeAddView(_service));
    }
}