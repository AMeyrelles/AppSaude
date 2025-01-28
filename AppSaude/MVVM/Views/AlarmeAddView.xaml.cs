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

            // Atribuir dependências injetadas
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _serviceAndroid = servicesAndroid ?? throw new ArgumentNullException(nameof(servicesAndroid));


            var viewModel = new AlarmeViewModel(_services);

            BindingContext = viewModel;            
        }

        //Verifica permissão para exibir notificacao
        private async Task VerifyPermissionsAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Alerta", "Aceite para receber as notificações!", "OK");
                    status = await Permissions.RequestAsync<Permissions.PostNotifications>();
                }

                if (status == PermissionStatus.Granted)
                {
                    Console.WriteLine("Permissão para notificações concedida.");
                }
                else
                {
                    Console.WriteLine("Permissão para notificações negada.");
                    await DisplayAlert("Alerta", "Permissão para notificações NEGADA!", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar permissões: {ex.Message}");
            }
        }

        //Botão para salvar o alarme
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

        //Botão de voltar
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
                Console.WriteLine("Erro: BindingContext do Switch é nulo.");
            }
        }

        //Insere nas Entrys apenas letras e espaço
        private void OnLetterEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            // Remove todos os caracteres que não sejam letras
            var entry = (Entry)sender;
            entry.Text = string.Concat(e.NewTextValue.Where(c => char.IsLetter(c) || char.IsWhiteSpace(c)));
        }

        //Insere nas Entrys letras, números, espaços e traços
        private void OnCustomEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            string pattern = @"^[a-zA-Z0-9\s\-]*$"; // Permite letras, números, espaços e traços
            if (!Regex.IsMatch(e.NewTextValue, pattern))
            {
                // Reverte para o último valor válido
                entry.Text = e.OldTextValue;
            }
        }

        public void StartService()
        {
            if (!_serviceAndroid.IsRunning)
            {
                _serviceAndroid.Start(); // Inicia o serviço
            }
            else
            {
                Console.WriteLine("HOMEPAGE: O serviço já está em execução.");
            }

        }

        //FIM
    }
}

