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

        _alarmeService = alarmeService ?? throw new ArgumentNullException(nameof(alarmeService), "O servi�o de alarme n�o foi fornecido.");

        // Defina o BindingContext, caso necess�rio para usar com MVVM.
        BindingContext = new AlarmeViewModel(_alarmeService);
    }

    private async void btnCancelarAlarme_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
 

    private async void btnAddAlarme_Clicked(object sender, EventArgs e)
    {
         // Obt�m o valor do TimePicker
             TimeSpan selectedTime = TimePickerControl.Time;

        // Extrai as horas e minutos
        int hours = selectedTime.Hours;
        int minutes = selectedTime.Minutes;

        // Exibe o hor�rio selecionado        
        await DisplayAlert("Alerta", $"Hor�rio selecionado: {hours:D2}:{minutes:D2}", "OK");
    }
}
