using AppSaude.MVVM.Models;
using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using Plugin.Maui.Audio;

namespace AppSaude.MVVM.Views;

public partial class AlarmesView : ContentPage
{
    private readonly IServicesTeste _services;  
    private readonly IServiceAndroid _servicesAndroid;

    private readonly List<Alarme> _alarmeList = new();
   
    public AlarmesView(IServicesTeste services, IServiceAndroid servicesAndroid)
    {
        InitializeComponent();

        // Atribuir dependências injetadas
        _services = services ?? throw new ArgumentNullException(nameof(services));
        _servicesAndroid = servicesAndroid ?? throw new ArgumentNullException(nameof(servicesAndroid));

        var viewModel = new MainViewModel(services);
        BindingContext = viewModel;         
    }
        
    protected override async void OnAppearing()
    {
        base.OnAppearing();      


        // Exibe a quantidade de alarmes carregados
        Console.WriteLine($"Alarmes carregados: {_alarmeList.Count}");
        
        try
        {
            // O BindingContext é da ViewModel que contém o DisplayCommand
            if (BindingContext is MainViewModel viewModel)
            {
                // Dispara o DisplayCommand para carregar os dados automaticamente
                viewModel.DisplayCommand.Execute(null);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }

        //FIM
    }

    // Botão para navegar para AlarmeAddView
    private async void btnAdd_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AlarmeAddView(_services, _servicesAndroid));
    }

    //Switch para habilitar/desabilitar alarme
    private async void AlarmSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        var switchControl = sender as Switch;

        if (switchControl?.BindingContext is Alarme alarme)
        {
            alarme.IsEnabled = e.Value;
            try
            {
                if (alarme.IsEnabled)
                {                    
                    await _services.UpdateAlarme(alarme); // Atualiza o banco de dados
                    Console.WriteLine($"Alarme '{alarme.MedicationName}' habilitado: {alarme.IsEnabled}");
                    return;
                }
                else
                {
                    alarme.IsEnabled = false;
                    await _services.UpdateAlarme(alarme); // Atualiza o banco de dados
                    await DisplayAlert("Alerta!", "Você tem alarmes inativos, verifique se é isto mesmo!", "OK");
                    Console.WriteLine($"Alarme '{alarme.MedicationName}' desativado: {alarme.IsEnabled}");
                }          
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar o alarme: {ex.Message}");
            }
        }
        else
        {
            await DisplayAlert("Alerta!", "Alarme OFF", "OK");
            Console.WriteLine("Erro: BindingContext inválido para o Switch.");
        }
    }
}
