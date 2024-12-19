using AppSaude.MVVM.Models;
using AppSaude.MVVM.ViewModels;
using AppSaude.Services;

namespace AppSaude.MVVM.Views;

public partial class AlarmeAddView : ContentPage
{
    private readonly IAlarmeService _alarmeService;

    public AlarmeAddView(IAlarmeService alarmeService)
    {
        InitializeComponent();
        _alarmeService = alarmeService;

        _alarmeService = alarmeService ?? throw new ArgumentNullException(nameof(alarmeService), "O serviço de alarme não foi fornecido.");

        // Defina o BindingContext, caso necessário para usar com MVVM.
        BindingContext = new AlarmeViewModel(_alarmeService);
    }

    private async void btnCancelarAlarme_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
