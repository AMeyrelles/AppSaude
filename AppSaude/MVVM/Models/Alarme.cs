using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSaude.MVVM.Models
{
    [Table("Alames")]
    public class Alarme
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // Nome do medicamento
        [NotNull, MaxLength(100)]
        public string MedicationName { get; set; }

        // Nome de quem irá tomar o medicamento
        [NotNull, MaxLength(50)]
        public string PatientName { get; set; }

        // Descrição do lembrete
        [NotNull, MaxLength(300)]
        public string Description { get; set; }

        // Horário em que o medicamento será tomado
        [NotNull]
        public TimeSpan ReminderTime { get; set; }

        public bool IsEnabled { get; set; }

    }
}
