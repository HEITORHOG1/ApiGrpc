using ApiGrpc.Domain.Entities.Base;

namespace ApiGrpc.Domain.Entities
{
    public class HorarioFuncionamento : Entity
    {
        public int EstabelecimentoId { get; private set; }
        public DayOfWeek DiaSemana { get; private set; }
        public TimeSpan HoraAbertura { get; private set; }
        public TimeSpan HoraFechamento { get; private set; }

        protected HorarioFuncionamento()
        { } // EF Constructor

        public HorarioFuncionamento(int estabelecimentoId, DayOfWeek diaSemana, TimeSpan horaAbertura, TimeSpan horaFechamento)
        {
            EstabelecimentoId = estabelecimentoId;
            DiaSemana = diaSemana;
            HoraAbertura = horaAbertura;
            HoraFechamento = horaFechamento;
        }

        public void Update(DayOfWeek diaSemana, TimeSpan horaAbertura, TimeSpan horaFechamento)
        {
            DiaSemana = diaSemana;
            HoraAbertura = horaAbertura;
            HoraFechamento = horaFechamento;
        }
    }
}