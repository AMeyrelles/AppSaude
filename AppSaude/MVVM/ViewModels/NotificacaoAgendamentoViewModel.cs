using AppSaude.MVVM.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSaude.MVVM.ViewModels
{
    public partial class NotificacaoAgendamentoViewModel : ObservableObject
    {
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

        private ObservableCollection<NotificacaoAgendamento> _notficacaoAgendamentos;
        public ObservableCollection<NotificacaoAgendamento> NotificacaoAgendamentos
        {
            get => _notficacaoAgendamentos;
            set
            {
                if (_notficacaoAgendamentos != value)
                {
                    _notficacaoAgendamentos = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _idNAg;
        public int IdNAg
        {
            get => _idNAg;
            set
            {
                if (_idNAg != value)
                {
                    _idNAg = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _specialistNameNAg;
        public string SpecialistNameNAg
        {
            get => _specialistNameNAg;
            set
            {
                if (_specialistNameNAg != value)
                {
                    _specialistNameNAg = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _specialtyNAg;
        public string SpecialtyNAg
        {
            get => _specialtyNAg;
            set
            {
                if (_specialtyNAg != value)
                {
                    _specialtyNAg = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _postalCodeNAg;
        public string PostalCodeNAg
        {
            get => _postalCodeNAg;
            set
            {
                if (_postalCodeNAg != value)
                {
                    _postalCodeNAg = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _streetNAg;
        public string StreetNAg
        {
            get => _streetNAg;
            set
            {
                if (_streetNAg != value)
                {
                    _streetNAg = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _neighborhoodNAg;
        public string NeighborhoodNAg
        {
            get => _neighborhoodNAg;
            set
            {
                if (_neighborhoodNAg != value)
                {
                    _neighborhoodNAg = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _countryNAg;
        public string CountryNAg
        {
            get => _countryNAg;
            set
            {
                if (_countryNAg != value)
                {
                    _countryNAg = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _cityNAg;
        public string CityNAg
        {
            get => _cityNAg;
            set
            {
                if (_cityNAg != value)
                {
                    _cityNAg = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _descriptionAppointmentsNAg;
        public string DescriptionAppointmentsNAg
        {
            get => _descriptionAppointmentsNAg;
            set
            {
                if (_descriptionAppointmentsNAg != value)
                {
                    _descriptionAppointmentsNAg = value;
                    OnPropertyChanged();
                }
            }
        }

        private TimeSpan _appointmentDateTimeNAg;
        public TimeSpan AppointmentDateTimeNAg
        {
            get => _appointmentDateTimeNAg;
            set
            {
                if (_appointmentDateTimeNAg != value)
                {
                    _appointmentDateTimeNAg = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _selectedDateNAg;
        public DateTime SelectedDateNAg
        {
            get => _selectedDateNAg;
            set
            {
                if (_selectedDateNAg != value)
                {
                    _selectedDateNAg = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime _minDate;
        public DateTime MinDate
        {
            get => _minDate;
            set
            {
                if (_minDate != value)
                {
                    _minDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isEnabledNAg;
        public bool IsEnabledNAg
        {
            get => _isEnabledNAg;
            set => SetProperty(ref _isEnabledNAg, value);
        }

        private bool _isNotifiedNAg;
        public bool IsNotifiedNAg
        {
            get => _isNotifiedNAg;
            set => SetProperty(ref _isNotifiedNAg, value);
        }

        //FIM
    }
}
