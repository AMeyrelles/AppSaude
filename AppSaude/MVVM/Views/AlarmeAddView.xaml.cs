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

        // Opcional: defina o BindingContext, caso necess�rio para usar com MVVM.
        BindingContext = new AlarmeViewModel(alarmeService);
    }

    //private async void btnAddAlarme_Clicked(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        // Exemplo de chamada ao servi�o para adicionar o alarme
    //        var alarmeAtual = new Alarme(); // Obtenha os dados do alarme de algum controle ou vari�vel
    //        await _alarmeService.AddAlarme(alarmeAtual);

    //        // Atualize algo na interface, se necess�rio
    //        await DisplayAlert("Sucesso", "Alarme adicionado com sucesso!", "OK");
    //    }
    //    catch (Exception ex)
    //    {
    //        // Exibi��o de erros
    //        await DisplayAlert("Erro", $"Falha ao adicionar o alarme: {ex.Message}", "OK");
    //    }
    //}
}
