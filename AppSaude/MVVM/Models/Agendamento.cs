using SQLite;

namespace AppSaude.MVVM.Models
{
    [Table("Agendamentos")]
    public class Agendamento
    {
        [PrimaryKey, AutoIncrement]
        public int AppointmentsId { get; set; }

        // Nome do especialista
        [MaxLength(100)]
        public string SpecialistName { get; set; }

        // Nome da especialidade
        [NotNull, MaxLength(100)]
        public string Specialty { get; set; }

        // Codigo postal
        [MaxLength(8)]
        public string PostalCode { get; set; }

        // Rua
        [MaxLength(100)]
        public string Street { get; set; }

        //Bairro
        [MaxLength(100)]
        public string Neighborhood { get; set; }

        // Cidade
        [MaxLength(100)]
        public string City { get; set; }

        // Horário do agendamento
        [NotNull]
        public TimeSpan AppointmentDateTime { get; set; }

        [NotNull]
        public DateTime SelectedDate { get; set; }

        public DateTime MinDate { get; set; }

        [MaxLength(100)]
        public string DescriptionAppointments { get; set; }
        public bool IsEnabled { get; set; } // Verifica se o agendamento esta habilitado
        public bool IsNotified { get; set; } // Verifica se o agendamento ja foi notificado
        public int NotificationCount { get; set; } = 0;
        public int NotificationDailyCount { get; set; } = 0;
    }
}

