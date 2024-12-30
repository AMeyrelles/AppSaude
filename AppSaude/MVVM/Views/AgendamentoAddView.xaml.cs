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




    //Verificar a permissão do usuario
    private async Task VerifyPermissionsAsync()
    {
        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Alerta", "Aceite para receber notificações!", "OK");
                status = await Permissions.RequestAsync<Permissions.PostNotifications>();
            }

            if (status == PermissionStatus.Granted)
            {
                Console.WriteLine("Permissão para notificações concedida.");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Alerta!", "Permissão para notificações negada." , "OK");
                await DisplayAlert("Alerta", "Permissão para notificações NEGADA!", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao verificar permissões: {ex.Message}");
        }
    }
    private async void btnCancelarAgendamento_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
      
}