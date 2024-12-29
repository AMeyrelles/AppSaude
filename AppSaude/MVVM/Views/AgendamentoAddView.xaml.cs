using AppSaude.MVVM.ViewModels;
using AppSaude.Services;

namespace AppSaude.MVVM.Views;

public partial class AgendamentoAddView : ContentPage
{
	private readonly IService _services;
    public AgendamentoAddView(IService services)
	{
        InitializeComponent();
        _services = services;

        var viewModel = new AgendamentoViewModel(services);
        BindingContext = viewModel;
    }

    private async void btnCancelarAgendamento_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

}