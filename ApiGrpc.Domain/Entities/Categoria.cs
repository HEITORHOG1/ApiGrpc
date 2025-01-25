using ApiGrpc.Domain.Entities.Base;

namespace ApiGrpc.Domain.Entities
{
    public class Categoria : Entity
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }

        protected Categoria() { }

        public Categoria(string nome, string descricao)
        {
            Nome = nome;
            Descricao = descricao;
        }

        public void Update(string nome, string descricao)
        {
            Nome = nome;
            Descricao = descricao;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}