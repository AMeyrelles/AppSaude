using AppSaude.MVVM.ViewModels;
using AppSaude.Services;

namespace AppSaude.MVVM.Views;

public partial class HomePageView : ContentPage
{
	private readonly IAlarmeService _service;
	public HomePageView(IAlarmeService alarmeService)
	{
		InitializeComponent();
        _service = alarmeService;

		_service = new AlarmeService();

        var viewModel = new AlarmeViewModel(alarmeService);

        BindingContext = viewModel;
    }

    private async void btnAlarme_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AlarmesView(_service));
    }
}