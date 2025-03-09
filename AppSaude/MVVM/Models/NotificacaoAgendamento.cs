using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSaude.MVVM.Models
{
   
        [Table("NotificacaoAgendamentos")]
        public class NotificacaoAgendamento
        {
            [PrimaryKey, AutoIncrement]
            public int IdNAg { get; set; }

            // Nome do especialista
            [NotNull, MaxLength(100)]
            public string SpecialistNameNAg  { get; set; }

            // Nome da especialidade
            [NotNull, MaxLength(50)]
            public string SpecialtyNAg { get; set; }

            // Codigo postal
            [MaxLength(30)]
            public string PostalCodeNAg { get; set; }

            // Rua
            [NotNull, MaxLength(100)]
            public string StreetNAg { get; set; }

            //Bairro
            [NotNull, MaxLength(100)]
            public string NeighborhoodNAg { get; set; }

            // Cidade
            [NotNull, MaxLength(50)]
            public string CityNAg { get; set; }

            // Horário do agendamento
            [NotNull]
            public TimeSpan AppointmentDateTimeNAg { get; set; }

            [NotNull]
            public DateTime SelectedDateNAg { get; set; }

            public DateTime MinDate { get; set; }

            [MaxLength(100)]
            public string DescriptionAppointmentsNAg { get; set; }
            public bool IsEnabledNAg { get; set; }
            public bool IsNotifiedNAg { get; set; } // Verifica se o agendamento ja foi chamado
        }
    }

