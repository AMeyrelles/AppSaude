using AppSaude.MVVM.Models;
using AppSaude.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppSaude.MVVM.ViewModels
{
    public partial class NotificacaoAlarmeViewModel : ObservableObject
    {

        private NotificacaoAlarme _notAlarmeAtual;
        public NotificacaoAlarme NotAlarmeAtual
        {
            get => _notAlarmeAtual;
            set
            {
                if (_notAlarmeAtual != value)
                {
                    _notAlarmeAtual = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<NotificacaoAlarme> _notAlarmes;
        public ObservableCollection<NotificacaoAlarme> NotificacaoAlarme
        {
            get => _notAlarmes;
            set
            {
                if (_notAlarmes != value)
                {
                    _notAlarmes = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _idNA;
        public int IdNA
        {
            get => _idNA;
            set
            {
                if (_idNA != value)
                {
                    _idNA = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _medicationNameNA;
        public string MedicationNameNA
        {
            get => _medicationNameNA;
            set
            {
                if (_medicationNameNA != value)
                {
                    _medicationNameNA = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _patientNameNA;
        public string PatientNameNA
        {
            get => _patientNameNA;
            set
            {
                if (_patientNameNA != value)
                {
                    _patientNameNA = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _descriptionNA;
        public string DescriptionNA
        {
            get => _descriptionNA;
            set
            {
                if (_descriptionNA != value)
                {
                    _descriptionNA = value;
                    OnPropertyChanged();
                }
            }
        }

        private TimeSpan _reminderTimeNA;
        public TimeSpan ReminderTimeNA
        {
            get => _reminderTimeNA;
            set
            {
                if (_reminderTimeNA != value)
                {
                    _reminderTimeNA = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isEnabledNA;
        public bool IsEnabledNA
        {
            get => _isEnabledNA;
            set => SetProperty(ref _isEnabledNA, value);
        }

        private bool _isNotifiedNA;
        public bool IsNotifiedNA
        {
            get => _isNotifiedNA;
            set => SetProperty(ref _isNotifiedNA, value);
        }

        private DateTime _lastNotifiedDateNA;
        public DateTime LastNotifiedDateNA
        {
            get => _lastNotifiedDateNA;
            set => SetProperty(ref _lastNotifiedDateNA, value);
        }

        public ICommand SaveCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand DisplayCommand { get; set; }


        public NotificacaoAlarmeViewModel(IServicesTeste notificacaoRepository) {

            // Inicializa o serviço de alarme
            _ = notificacaoRepository.InitializeAsync();

            if (notificacaoRepository == null)
            {
                throw new ArgumentNullException(nameof(notificacaoRepository), "O serviço de alarme não foi fornecido.");
            }

            NotAlarmeAtual = new NotificacaoAlarme(); // Inicializa o objeto


            SaveCommand = new Command(async () =>
            {
                try
                {
                    await notificacaoRepository.AddNotAlarme(NotAlarmeAtual);
                    await Refresh(notificacaoRepository);  // Atualiza a lista após salvar
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
                    await notificacaoRepository.UpdateNotAlarme(NotAlarmeAtual);
                    await Refresh(notificacaoRepository);  // Atualiza a lista após atualizar
                    await App.Current.MainPage.DisplayAlert("Alerta", "Atualizado com sucesso!", "OK");
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
                }
            });


            DeleteCommand = new Command(async () =>
            {
                if (NotAlarmeAtual == null)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", "Nenhum alarme selecionado.", "OK");
                    return;
                }

                try
                {
                    var resposta = await App.Current.MainPage.DisplayAlert("Alerta", "Excluir alarme???", "SIM", "NÃO");
                    if (resposta)
                    {
                        await notificacaoRepository.DeleteNotAlarme(NotAlarmeAtual);
                        await Refresh(notificacaoRepository);
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
                    await notificacaoRepository.InitializeAsync();
                    await Refresh(notificacaoRepository);  // Atualiza a lista quando a página é carregada
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
                }
            });

        }

        public async Task Refresh(IServicesTeste notService)
        {
            try
            {
                //Inicia a coleção sempre que chamado
                NotificacaoAlarme = new ObservableCollection<NotificacaoAlarme>();

                // Recupera os alarmes do serviço e atualiza a coleção ObservableCollection
                var notList = await notService.GetNotAlarmes();

                //Alarmes.Clear(); // Limpa a coleção antes de adicionar novos dados
                foreach (var notificacao in notList)
                {
                    NotificacaoAlarme.Add(notificacao);  // Adiciona os alarmes retornados à coleção
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }
        //FIM
    }
}
