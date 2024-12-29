using AppSaude.MVVM.Models;
using AppSaude.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppSaude.MVVM.ViewModels
{
    public partial class AgendamentoViewModel : ObservableObject
    {
        private Agendamento _agendamentoAtual;
        public Agendamento AgendamentoAtual
        {
            get => _agendamentoAtual;
            set
            {
                if (_agendamentoAtual != value)
                {
                    _agendamentoAtual = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<Agendamento> _agendamentos;
        public ObservableCollection<Agendamento> Agendamentos
        {
            get => _agendamentos;
            set
            {
                if (_agendamentos != value)
                {
                    _agendamentos = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _appointmentsId;
        public int AppointmentsId
        {
            get => _appointmentsId;
            set
            {
                if (_appointmentsId != value)
                {
                    _appointmentsId = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _specialistName;
        public string SpecialistName
        {
            get => _specialistName;
            set
            {
                if (_specialistName != value)
                {
                    _specialistName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _specialty;
        public string Specialty
        {
            get => _specialty;
            set
            {
                if (_specialty != value)
                {
                    _specialty = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _postalCode;
        public string PostalCode
        {
            get => _postalCode;
            set
            {
                if (_postalCode != value)
                {
                    _postalCode = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _street;
        public string Street
        {
            get => _street;
            set
            {
                if (_street != value)
                {
                    _street = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _neighborhood;
        public string Neighborhood
        {
            get => _neighborhood;
            set
            {
                if (_neighborhood != value)
                {
                    _neighborhood = value;
                    OnPropertyChanged();
                }
            }
        }

        private TimeSpan _appointmentDateTime;
        public TimeSpan AppointmentDateTime
        {
            get => _appointmentDateTime;
            set
            {
                if (_appointmentDateTime != value)
                {
                    _appointmentDateTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _descriptionAppointments;     
        public string DescriptionAppointments
        {
            get => _descriptionAppointments;
            set
            {
                if(_descriptionAppointments != value)
                {

                    _descriptionAppointments = value;
                    OnPropertyChanged();
                        }
            }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand DisplayCommand { get; set; }


        public AgendamentoViewModel()
        {
            AppointmentDateTime = new TimeSpan(8, 0, 0); // 08:00
        }

        public AgendamentoViewModel(IService agendamentoRepository)
        {

            // Inicializa o serviço de alarme
            _ = agendamentoRepository.InitializeAsync();

            if (agendamentoRepository == null)
            {
                throw new ArgumentNullException(nameof(agendamentoRepository), "O serviço de agendamento não foi fornecido.");
            }

            AgendamentoAtual = new Agendamento();


            SaveCommand = new Command(async () =>
            {
                try
                {
                    await agendamentoRepository.AddAgendamento(AgendamentoAtual);
                    await Refresh(agendamentoRepository);  // Atualiza a lista após salvar
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
                    await agendamentoRepository.UpdateAgendamento(AgendamentoAtual);
                    await Refresh(agendamentoRepository);  // Atualiza a lista
                    await App.Current.MainPage.DisplayAlert("Alerta", "Atualizado com sucesso!", "OK");
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
                }
            });


            DeleteCommand = new Command(async () =>
            {
                if (AgendamentoAtual == null)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", "Nenhum agendamento selecionado.", "OK");
                    return;
                }

                try
                {
                    var resposta = await App.Current.MainPage.DisplayAlert("Alerta", "Excluir o agendamento???", "SIM", "NÃO");
                    if (resposta)
                    {
                        await agendamentoRepository.DeleteAgendamento(AgendamentoAtual);
                        await Refresh(agendamentoRepository);
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
                    await agendamentoRepository.InitializeAsync();
                    await Refresh(agendamentoRepository);  // Atualiza a lista quando a página é carregada
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
                }
            });

        }

        //Instância a lista e a atualiza
        public async Task Refresh(IService _servicos)
        {
            try
            {
                //Inicia a coleção sempre que chamado
                Agendamentos = new ObservableCollection<Agendamento>();

                // Recupera os alarmes do serviço e atualiza a coleção ObservableCollection
                var agendamentoList = await _servicos.GetAgendamentos();

                //Alarmes.Clear(); // Limpa a coleção antes de adicionar novos dados
                foreach (var agendamento in agendamentoList)
                {
                    Agendamentos.Add(agendamento);  // Adiciona os alarmes retornados à coleção
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }

}
