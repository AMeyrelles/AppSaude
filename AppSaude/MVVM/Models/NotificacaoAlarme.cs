using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSaude.MVVM.Models
{      
        [Table("NotificacaoAlarmes")]
        public class NotificacaoAlarme
        {
            [PrimaryKey, AutoIncrement]
            public int IdNA { get; set; }

            // Nome do medicamento
            [NotNull, MaxLength(100)]
            public string MedicationNameNA { get; set; }

            // Nome de quem irá tomar o medicamento
            [NotNull, MaxLength(50)]
            public string PatientNameNA { get; set; }

            // Descrição do lembrete
            [NotNull, MaxLength(300)]
            public string DescriptionNA { get; set; }

            // Horário em que o medicamento será tomado 
            public TimeSpan ReminderTimeNA { get; set; }
            public bool IsEnabledNA { get; set; }
            public bool IsNotifiedNA { get; set; } // Verifica se o alarme ja foi chamado
            public DateTime? LastNotifiedDateNA { get; set; } // Nova propriedade
        }
    }

