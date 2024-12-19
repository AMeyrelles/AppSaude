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

        // Opcional: defina o BindingContext, caso necessário para usar com MVVM.
        BindingContext = new AlarmeViewModel(alarmeService);
    }

    //private async void btnAddAlarme_Clicked(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        // Exemplo de chamada ao serviço para adicionar o alarme
    //        var alarmeAtual = new Alarme(); // Obtenha os dados do alarme de algum controle ou variável
    //        await _alarmeService.AddAlarme(alarmeAtual);

    //        // Atualize algo na interface, se necessário
    //        await DisplayAlert("Sucesso", "Alarme adicionado com sucesso!", "OK");
    //    }
    //    catch (Exception ex)
    //    {
    //        // Exibição de erros
    //        await DisplayAlert("Erro", $"Falha ao adicionar o alarme: {ex.Message}", "OK");
    //    }
    //}
}
