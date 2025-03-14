﻿using AppSaude.MVVM.Models;
using AppSaude.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AppSaude.MVVM.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly IServicesTeste _service;      

        public AlarmeViewModel AlarmeViewModel { get; set; }
        public AgendamentoViewModel AgendamentoViewModel { get; set; }
        public NotificacaoAlarmeViewModel NotificacaoAlarmeViewModel { get; set; }
        public NotificacaoAlarmeViewModel NotificacaoAgendamentoViewModel { get; set; }

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

        //private int _id;
        //public int Id
        //{
        //    get => _id;
        //    set
        //    {
        //        if (_id != value)
        //        {
        //            _id = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private string _medicationName;
        //public string MedicationName
        //{
        //    get => _medicationName;
        //    set
        //    {
        //        if (_medicationName != value)
        //        {
        //            _medicationName = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private string _patientName;
        //public string PatientName
        //{
        //    get => _patientName;
        //    set
        //    {
        //        if (_patientName != value)
        //        {
        //            _patientName = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private string _description;
        //public string Description
        //{
        //    get => _description;
        //    set
        //    {
        //        if (_description != value)
        //        {
        //            _description = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        ////Hora
        //private TimeSpan _reminderTime;
        //public TimeSpan ReminderTime
        //{
        //    get => _reminderTime;
        //    set
        //    {
        //        if (_reminderTime != value)
        //        {
        //            _reminderTime = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        ////Data
        //private DateTime _selectedDate;
        //public DateTime SelectedDate
        //{
        //    get => _selectedDate;
        //    set
        //    {
        //        if (_selectedDate != value)
        //        {
        //            _selectedDate = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}


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


        private ObservableCollection<NotificacaoAlarme> notificacaoAlarmes;
        public ObservableCollection<NotificacaoAlarme> NotificacaoAlarmes
        {
            get => notificacaoAlarmes;
            set
            {
                if (notificacaoAlarmes != value)
                {
                    notificacaoAlarmes = value;
                    OnPropertyChanged();
                }
            }
        }

        private NotificacaoAlarme _notificacaoAlarmeAtual;
        public NotificacaoAlarme NotificacaoAlarmeAtual
        {
            get => _notificacaoAlarmeAtual;
            set
            {
                if (_notificacaoAlarmeAtual != value)
                {
                    _notificacaoAlarmeAtual = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<NotificacaoAgendamento> _notificacaoAgendamentos;
        public ObservableCollection<NotificacaoAgendamento> NotificacaoAgendamentos
        {
            get => _notificacaoAgendamentos;
            set
            {
                if (_notificacaoAgendamentos != value)
                {
                    _notificacaoAgendamentos = value;
                    OnPropertyChanged();
                }
            }
        }

        private NotificacaoAgendamento _notificacaoAgendamentoAtual;
        public NotificacaoAgendamento NotificacaoAgendamentoAtual
        {
            get => _notificacaoAgendamentoAtual;
            set
            {
                if (_notificacaoAgendamentoAtual != value)
                {
                    _notificacaoAgendamentoAtual = value;
                    OnPropertyChanged();
                }
            }
        }

        //private int _apointmentsId;
        //public int AppointmentsID
        //{
        //    get => _apointmentsId;
        //    set
        //    {
        //        if (_apointmentsId != value)
        //        {
        //            _apointmentsId = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private string _specialistName;
        //public string SpecialistName
        //{
        //    get => _specialistName;
        //    set
        //    {
        //        if (_specialistName != value)
        //        {
        //            _specialistName = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private string _specialty;
        //public string Specialty
        //{
        //    get => _specialty;
        //    set
        //    {
        //        if (_specialty != value)
        //        {
        //            _specialty = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private string _postalCode;
        //public string PostalCode
        //{
        //    get => _postalCode;
        //    set
        //    {
        //        if (_postalCode != value)
        //        {
        //            _postalCode = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private string _street;
        //public string Street
        //{
        //    get => _street;
        //    set
        //    {
        //        if (_street != value)
        //        {
        //            _street = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private string _neighborhood;
        //public string Neighborhood
        //{
        //    get => _neighborhood;
        //    set
        //    {
        //        if (_neighborhood != value)
        //        {
        //            _neighborhood = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private TimeSpan _appointmentDateTime;
        //public TimeSpan AppointmentDateTime
        //{
        //    get => _appointmentDateTime;
        //    set
        //    {
        //        if (_appointmentDateTime != value)
        //        {
        //            _appointmentDateTime = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private string _descriptionAppointments;
        //public string DescriptionAppointments
        //{
        //    get => _descriptionAppointments;
        //    set
        //    {
        //        if (_descriptionAppointments != value)
        //        {

        //            _descriptionAppointments = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private bool _isEnabled;
        //public bool IsEnabled
        //{
        //    get => _isEnabled;
        //    set => SetProperty(ref _isEnabled, value);
        //}

        public ICommand DeleteAlarmeCommand { get; set; }
        public ICommand DeleteAgendaCommand { get; set; }
        public ICommand DeleteNACommand { get; set; }
        public ICommand DeleteNAgCommand { get; set; }
        public ICommand DisplayCommand { get; set; }

        public MainViewModel(IServicesTeste servicesRepository)
        {
            // Inicializa o serviço
            _ = servicesRepository.InitializeAsync();

            _service = servicesRepository;
            AlarmeViewModel = new AlarmeViewModel(servicesRepository);
            AgendamentoViewModel = new AgendamentoViewModel(servicesRepository);

            Alarmes = new ObservableCollection<Alarme>();
            Agendamentos = new ObservableCollection<Agendamento>();

            //DELETE ALARME 
            DeleteAlarmeCommand = new Command(async () =>
            {
                if (AlarmeAtual == null)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", "Nenhuma alarme selecionada.", "OK");
                    return;
                }

                try
                {
                    var resposta = await App.Current.MainPage.DisplayAlert("Alerta", "Deseja exluir?", "SIM", "NÃO");
                    if (resposta)
                    {
                        await servicesRepository.DeleteAlarme(AlarmeAtual);
                        await Refresh(servicesRepository);
                    }
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
                }
            });

            //DELETE AGENDAMENTO
            DeleteAgendaCommand = new Command(async () =>
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
                        await servicesRepository.DeleteAgendamento(AgendamentoAtual);
                        await Refresh(servicesRepository);
                    }
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
                }
            });

            //DELETE NOTIFICACAO ALARME 
            DeleteNACommand = new Command(async () =>
            {
                if (NotificacaoAlarmeAtual == null)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", "Nenhuma notificação selecionada.", "OK");
                    return;
                }

                try
                {
                    var resposta = await App.Current.MainPage.DisplayAlert("Alerta", "Deseja exluir?", "SIM", "NÃO");
                    if (resposta)
                    {
                        await servicesRepository.DeleteNotAlarme(NotificacaoAlarmeAtual);
                        await Refresh(servicesRepository);
                    }
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
                }
            });

            //DELETE NOTIFICACAO AGENDAMENTO 
            DeleteNAgCommand = new Command(async () =>
            {
                if (NotificacaoAgendamentoAtual == null)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", "Nenhuma notificação selecionada.", "OK");
                    return;
                }

                try
                {
                    var resposta = await App.Current.MainPage.DisplayAlert("Alerta", "Deseja exluir?", "SIM", "NÃO");
                    if (resposta)
                    {
                        await servicesRepository.DeleteNotAgendamento(NotificacaoAgendamentoAtual);
                        await Refresh(servicesRepository);
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
                    await servicesRepository.InitializeAsync();
                    await Refresh(servicesRepository);  // Atualiza a lista quando a página é carregada
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
                }
            });
        }


        public async Task Refresh(IServicesTeste serviceServices)
        {
            try
            {
                //Inicia a coleção sempre que chamado
                Alarmes = new ObservableCollection<Alarme>();
                Agendamentos = new ObservableCollection<Agendamento>();
                NotificacaoAlarmes = new ObservableCollection<NotificacaoAlarme>();
                NotificacaoAgendamentos = new ObservableCollection<NotificacaoAgendamento>();             

                // Recupera os alarmes do serviço e atualiza a coleção ObservableCollection
                var alarmesList = await serviceServices.GetAlarmes();
                var agendamentosList = await serviceServices.GetAgendamentos();
                var alarmeNotificacaoList = await serviceServices.GetNotAlarmes();
                var agendamentoNotificacaoList = await serviceServices.GetNotAgendamentos();

                Alarmes.Clear(); // Limpa a coleção antes de adicionar novos dados
                foreach (var alarme in alarmesList)
                {
                    Alarmes.Add(alarme);  // Adiciona os alarmes retornados à coleção            
                }

                Agendamentos.Clear();
                foreach (var agendamento in agendamentosList)
                {
                    Agendamentos.Add(agendamento); // Adiciona os alarmes retornados à coleção   
                }

                NotificacaoAlarmes.Clear();
                foreach (var alarmeNot in alarmeNotificacaoList)
                {
                    NotificacaoAlarmes.Add(alarmeNot);  // Adiciona os notificação de alarmes retornados à coleção            
                }

                NotificacaoAgendamentos.Clear();
                foreach (var agendamentoNot in agendamentoNotificacaoList)
                {
                    NotificacaoAgendamentos.Add(agendamentoNot);  // Adiciona os notificação de alarmes retornados à coleção            
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
        }

    }

}
