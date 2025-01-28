using AppSaude.MVVM.Models;
using AppSaude.MVVM.ViewModels;
using AppSaude.Services;
using System.Text.RegularExpressions;



namespace AppSaude.MVVM.Views
{
    public partial class AlarmeAddView : ContentPage
    {
        private readonly IServicesTeste _services;

        private readonly IServiceAndroid _serviceAndroid;

        private readonly List<DateTime> _alarmList = new();

        public AlarmeAddView(IServicesTeste services, IServiceAndroid servicesAndroid)
        {
            InitializeComponent();

            // Atribuir depend�ncias injetadas
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _serviceAndroid = servicesAndroid ?? throw new ArgumentNullException(nameof(servicesAndroid));


            var viewModel = new AlarmeViewModel(_services);

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
                    await DisplayAlert("Alerta", "Aceite para receber as notifica��es!", "OK");
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

            DateTime alarmDateTime = new(now.Year, now.Month, now.Day, selectedTime.Hours, selectedTime.Minutes, 0);

            if (alarmDateTime <= now)
            {
                alarmDateTime = alarmDateTime.AddDays(1);
            }

            _alarmList.Add(alarmDateTime);

            StartService();

            await VerifyPermissionsAsync();
        }

        //Bot�o de voltar
        private async void btnCancelarAlarme_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        //Adiciona o valor do Switch ao banco de dados
        private async void AlarmSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            var switchControl = sender as Switch;

            if (switchControl?.BindingContext is Alarme alarme)
            {
                alarme.IsEnabled = e.Value;
               
                await _services.AddAlarme(alarme);
            }
            else
            {
                Console.WriteLine("Erro: BindingContext do Switch � nulo.");
            }
        }

        //Insere nas Entrys apenas letras e espa�o
        private void OnLetterEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            // Remove todos os caracteres que n�o sejam letras
            var entry = (Entry)sender;
            entry.Text = string.Concat(e.NewTextValue.Where(c => char.IsLetter(c) || char.IsWhiteSpace(c)));
        }

        //Insere nas Entrys letras, n�meros, espa�os e tra�os
        private void OnCustomEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            string pattern = @"^[a-zA-Z0-9\s\-]*$"; // Permite letras, n�meros, espa�os e tra�os
            if (!Regex.IsMatch(e.NewTextValue, pattern))
            {
                // Reverte para o �ltimo valor v�lido
                entry.Text = e.OldTextValue;
            }
        }

        public void StartService()
        {
            if (!_serviceAndroid.IsRunning)
            {
                _serviceAndroid.Start(); // Inicia o servi�o
            }
            else
            {
                Console.WriteLine("HOMEPAGE: O servi�o j� est� em execu��o.");
            }

        }

        //FIM
    }
}

