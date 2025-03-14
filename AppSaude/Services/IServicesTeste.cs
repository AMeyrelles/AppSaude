﻿using AppSaude.MVVM.Models;

namespace AppSaude.Services
{
    public interface IServicesTeste
    {
        Task InitializeAsync();
        Task<List<Alarme>> GetAlarmes();
        Task<Alarme> GetAlarme(int Id);
        Task<int> AddAlarme(Alarme alarme);
        Task<int> DeleteAlarme(Alarme alarme);
        Task<int> UpdateAlarme(Alarme alarme);

        Task<List<Agendamento>> GetAgendamentos();
        Task<Agendamento> GetAgendamento(int Id);
        Task<int> AddAgendamento(Agendamento agendamento);
        Task<int> DeleteAgendamento(Agendamento agendamento);
        Task<int> UpdateAgendamento(Agendamento agendamento);
  

        //Notificacao Getters e Setters               
        Task<List<NotificacaoAlarme>> GetNotAlarmes();
        Task<NotificacaoAlarme> GetNotAlarme(int Id);
        Task<int> AddNotAlarme(NotificacaoAlarme notAlarme);
        Task<int> DeleteNotAlarme(NotificacaoAlarme notAlarme);
        Task<int> UpdateNotAlarme(NotificacaoAlarme notAlarme);


        Task<List<NotificacaoAgendamento>> GetNotAgendamentos();
        Task<NotificacaoAgendamento> GetNotAgendamento(int Id);
        Task<int> AddNotAgendamento(NotificacaoAgendamento notAgendamento);
        Task<int> DeleteNotAgendamento(NotificacaoAgendamento notAgendamento);
        Task<int> UpdateNotAgendamento(NotificacaoAgendamento notAgendamento);

        //Banco de dados
        Task SetUpDb();
    }
}
