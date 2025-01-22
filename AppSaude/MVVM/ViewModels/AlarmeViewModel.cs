using AppSaude.MVVM.Models;
using AppSaude.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace AppSaude.MVVM.ViewModels
{
    public partial class AlarmeViewModel : ObservableObject
    {
        private Alarme _alarmeAtual;
        public Alarme AlarmeAtual
        {
            get => _alarmeAtual;
            set
            {
                if (_alarmeAtual != value)
                {
                    _alarmeAtual = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<Alarme> _alarmes;
        public ObservableCollection<Alarme> Alarmes
        {
            get => _alarmes;
            set
            {
                if (_alarmes != value)
                {
                    _alarmes = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _medicationName;
        public string MedicationName
        {
            get => _medicationName;
            set
            {
                if (_medicationName != value)
                {
                    _medicationName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _patientName;
        public string PatientName
        {
            get => _patientName;
            set
            {
                if (_patientName != value)
                {
                    _patientName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }


        private TimeSpan _reminderTime;

        public TimeSpan ReminderTime
        {
            get => _reminderTime;
            set
            {
                if (_reminderTime != value)
                {
                    _reminderTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private DateTime _lastNotifiedDate;
        public DateTime LastNotifiedDate
        {
            get => _lastNotifiedDate;
            set => SetProperty(ref _lastNotifiedDate, value);
        }

        public ICommand SaveCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand DisplayCommand { get; set; }


        public AlarmeViewModel()
        {
            ReminderTime = new TimeSpan(8, 0, 0); // 08:00
        }
        public AlarmeViewModel(IServicesTeste alarmeRepository)
        {

            // Inicializa o serviço de alarme
            _ = alarmeRepository.InitializeAsync();

            if (alarmeRepository == null)
            {
                throw new ArgumentNullException(nameof(alarmeRepository), "O serviço de alarme não foi fornecido.");
            }        
            
            AlarmeAtual = new Alarme(); // Inicializa o objeto

   
            SaveCommand = new Command(async () =>
            {
                try
                {
                    await alarmeRepository.AddAlarme(AlarmeAtual);
                    await Refresh(alarmeRepository);  // Atualiza a lista após salvar
                    await App.Current.MainPage.DisplayAlert("Alerta", "Salvo com sucesso!", "OK");
                    await App.Current.MainPage.Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
                }
            });

            UpdateCommand = new Command(async () =>
            {
                try
                {
                    await alarmeRepository.UpdateAlarme(AlarmeAtual);
                    await Refresh(alarmeRepository);  // Atualiza a lista após atualizar
                    await App.Current.MainPage.DisplayAlert("Alerta", "Atualizado com sucesso!", "OK");
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
                }
            });


            DeleteCommand = new Command(async () =>
            {
                if (AlarmeAtual == null)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", "Nenhum alarme selecionado.", "OK");
                    return;
                }

                try
                {
                    var resposta = await App.Current.MainPage.DisplayAlert("Alerta", "Excluir alarme???", "SIM", "NÃO");
                    if (resposta)
                    {
                        await alarmeRepository.DeleteAlarme(AlarmeAtual);
                        await Refresh(alarmeRepository);
                    }
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
                }
            });

            DisplayCommand = new Command(async () =>
            {

                try
                {
                    await alarmeRepository.InitializeAsync();
                    await Refresh(alarmeRepository);  // Atualiza a lista quando a página é carregada
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
                }
            });
        }

        public async Task Refresh(IServicesTeste alarmeService)
        {
            try
            {
                //Inicia a coleção sempre que chamado
                Alarmes = new ObservableCollection<Alarme>();

                // Recupera os alarmes do serviço e atualiza a coleção ObservableCollection
                var alarmesList = await alarmeService.GetAlarmes();

                //Alarmes.Clear(); // Limpa a coleção antes de adicionar novos dados
                foreach (var alarme in alarmesList)
                {
                    Alarmes.Add(alarme);  // Adiciona os alarmes retornados à coleção
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
