using AppSaude.MVVM.Models;
using AppSaude.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AppSaude.MVVM.ViewModels
{
    public partial class AlarmeViewModel : INotifyPropertyChanged
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

        public ICommand SaveCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand DisplayCommand { get; }

        public AlarmeViewModel(IAlarmeService alarmeRepository)
        {
            AlarmeAtual = new Alarme(); // Inicialize o objeto
            Alarmes = new ObservableCollection<Alarme>(); // Inicialize a coleção

            SaveCommand = new Command(async () =>
            {
                try
                {
                    await alarmeRepository.InitializeAsync();
                    await alarmeRepository.AddAlarme(AlarmeAtual);
                    await Refresh(alarmeRepository);
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
                    await alarmeRepository.InitializeAsync();
                    await alarmeRepository.UpdateAlarme(AlarmeAtual);
                    await Refresh(alarmeRepository);
                    await App.Current.MainPage.DisplayAlert("Alerta", "Atualizado com sucesso!", "OK");
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
                }
            });

            DeleteCommand = new Command(async () =>
            {
                try
                {
                    await alarmeRepository.InitializeAsync();
                    var resposta = await App.Current.MainPage.DisplayAlert("Alerta", "Deseja deletar o alarme?", "SIM", "NÃO");
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
                    await Refresh(alarmeRepository);
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
                }
            });
        }

        public async Task Refresh(IAlarmeService alarmeRepository)
        {
            var alarmesAtualizados = await alarmeRepository.GetAlarmes();
            Alarmes.Clear();
            foreach (var alarme in alarmesAtualizados)
            {
                Alarmes.Add(alarme);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
