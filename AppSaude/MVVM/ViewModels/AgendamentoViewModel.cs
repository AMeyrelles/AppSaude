using AppSaude.MVVM.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public ICommand SaveCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand DisplayCommand { get; set; }


        public AgendamentoViewModel()
        {
            AppointmentDateTime = new TimeSpan(8, 0, 0); // 08:00
        }






        public async Task Refresh()
        {
            try
            {
                //Inicia a coleção sempre que chamado
                Agendamentos = new ObservableCollection<Agendamento>();

                // Recupera os alarmes do serviço e atualiza a coleção ObservableCollection
                var agendamentoList = await _servicos.GetAlarmes();

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
