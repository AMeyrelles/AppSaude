using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using Plugin.LocalNotification;
using Plugin.Maui.Audio;
using System.Timers;

namespace AppSaude.MVVM.Views
{
    public partial class AlarmeAddView : ContentPage
    {
        private readonly IService _service;    

        IServiceAndroid _servicesAndroid;
        public string MessageToast { get; set; }

        private readonly List<DateTime> _alarmList = new();

        public AlarmeAddView(IService servicos, IServiceAndroid serviceAndroid)
        {
            InitializeComponent();
           
            _service = servicos ?? throw new ArgumentNullException(nameof(servicos), "O servi�o de alarme n�o foi fornecido.");

            _servicesAndroid = serviceAndroid ?? throw new ArgumentNullException(nameof(servicos), "O servi�o n�o foi fornecido.");

            _servicesAndroid = serviceAndroid;

            var viewModel = new AlarmeViewModel(_service);

            BindingContext = viewModel;            
        }

        //Verifica permiss�o para exibir notificacao
        private async Task VerifyPermissionsAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Alerta", "Aceite para receber notifica��es!", "OK");
                    status = await Permissions.RequestAsync<Permissions.PostNotifications>();
                }

                if (status == PermissionStatus.Granted)
                {
                    Console.WriteLine("Permiss�o para notifica��es concedida.");
                }
                else
                {
                    Console.WriteLine("Permiss�o para notifica��es negada.");
                    await DisplayAlert("Alerta", "Permiss�o para notifica��es NEGADA!", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar permiss�es: {ex.Message}");
            }
        }

        //Bot�o para salvar o alarme
        private async void btnAddAlarme_Clicked(object sender, EventArgs e)
        {
            TimeSpan selectedTime = TimePickerControl.Time;

            DateTime now = DateTime.Now;

            DateTime alarmDateTime = new DateTime(now.Year, now.Month, now.Day, selectedTime.Hours, selectedTime.Minutes, 0);

            if (alarmDateTime <= now)
            {
                alarmDateTime = alarmDateTime.AddDays(1);
            }

            _alarmList.Add(alarmDateTime);

            _servicesAndroid.Start();

            await VerifyPermissionsAsync();            
        }

        //Bot�o de voltar
        private async void btnCancelarAlarme_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
      
    }
}
