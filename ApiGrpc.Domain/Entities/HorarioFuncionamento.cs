using ApiGrpc.Domain.Entities.Base;

namespace ApiGrpc.Domain.Entities
{
    public class HorarioFuncionamento : Entity
    {
        // Propriedades
        public Guid EstabelecimentoId { get; private set; } // 👈 Tipo Guid

        public DayOfWeek DiaSemana { get; private set; }
        public TimeSpan HoraAbertura { get; private set; }
        public TimeSpan HoraFechamento { get; private set; }

        // Relacionamento N:1
        public virtual Estabelecimento Estabelecimento { get; private set; }

        protected HorarioFuncionamento()
        { } // EF Constructor

        // Construtor
        public HorarioFuncionamento(
            Guid estabelecimentoId,
            DayOfWeek diaSemana,
            TimeSpan horaAbertura,
            TimeSpan horaFechamento)
        {
            EstabelecimentoId = estabelecimentoId;
            DiaSemana = diaSemana;
            HoraAbertura = horaAbertura;
            HoraFechamento = horaFechamento;
        }

        // Método de atualização
        public void Update(DayOfWeek diaSemana, TimeSpan horaAbertura, TimeSpan horaFechamento)
        {
            DiaSemana = diaSemana;
            HoraAbertura = horaAbertura;
            HoraFechamento = horaFechamento;
        }
    }
}