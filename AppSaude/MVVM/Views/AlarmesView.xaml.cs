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

    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AlarmeAddView(_service));
    }
}