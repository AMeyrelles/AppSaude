using SQLite;

namespace AppSaude.MVVM.Models
{
    [Table("Agendamentos")]
    public class Agendamento
    {
        [PrimaryKey, AutoIncrement]
        public int AppointmentsId { get; set; }

        // Nome do especialista
        [NotNull, MaxLength(100)]
        public string SpecialistName { get; set; }

        // Nome da especialidade
        [NotNull, MaxLength(50)]
        public string Specialty { get; set; }

        // Codigo postal
        [MaxLength(30)]
        public string PostalCode { get; set; }

        // Rua
        [NotNull, MaxLength(100)]
        public string Street { get; set; }

        //Bairro
        [NotNull, MaxLength(100)]
        public string Neighborhood { get; set; }

        // Cidade
        [NotNull, MaxLength(50)]
        public string City { get; set; }

        // Horário do agendamento
        [NotNull]
        public TimeSpan AppointmentDateTime { get; set; }

        [NotNull]
        public DateTime SelectedDate { get; set; }

        [MaxLength(100)]
        public string DescriptionAppointments { get; set; }
    }
}
