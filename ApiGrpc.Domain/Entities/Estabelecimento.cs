using ApiGrpc.Domain.Entities.Base;
using ApiGrpc.Domain.Exceptions;

namespace ApiGrpc.Domain.Entities
{
    public class Estabelecimento : Entity
    {
        // Propriedades básicas
        public Guid UsuarioId { get; private set; }
        public string RazaoSocial { get; private set; }
        public string NomeFantasia { get; private set; }
        public string CNPJ { get; private set; }
        public string Telefone { get; private set; }
        public string Email { get; private set; }
        public bool Status { get; private set; }
        public string? UrlImagem { get; private set; }
        public string Descricao { get; private set; }
        public string? InscricaoEstadual { get; private set; }
        public string? InscricaoMunicipal { get; private set; }
        public string? Website { get; private set; }
        public string? RedeSocial { get; private set; }

        // Chaves estrangeiras
        public Guid CategoriaId { get; private set; }

        public Guid EnderecoId { get; private set; }

        // Propriedades de navegação
        public virtual Categoria Categoria { get; private set; }

        public virtual Endereco Endereco { get; private set; }
        public virtual ICollection<HorarioFuncionamento> HorariosFuncionamento { get; private set; } = new List<HorarioFuncionamento>();

        protected Estabelecimento()
        { } // EF Constructor

        // Construtor
        public Estabelecimento(
            Guid usuarioId,
            string razaoSocial,
            string nomeFantasia,
            string cnpj,
            string telefone,
            string email,
            string urlImagem,
            string descricao,
            string inscricaoEstadual,
            string inscricaoMunicipal,
            string website,
            string redeSocial,
            Guid categoriaId,
            Endereco endereco)
        {
            // Validações
            if (string.IsNullOrWhiteSpace(cnpj)) throw new DomainException("CNPJ é obrigatório");
            if (!ValidarCNPJ(cnpj)) throw new DomainException("CNPJ inválido");
            if (string.IsNullOrWhiteSpace(email)) throw new DomainException("E-mail é obrigatório");

            // Atribuições
            UsuarioId = usuarioId;
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            CNPJ = cnpj;
            Telefone = telefone;
            Email = email;
            Status = true;
            UrlImagem = urlImagem;
            Descricao = descricao;
            InscricaoEstadual = inscricaoEstadual;
            InscricaoMunicipal = inscricaoMunicipal;
            Website = website;
            RedeSocial = redeSocial;
            CategoriaId = categoriaId;
            Endereco = endereco;
            EnderecoId = endereco.Id;
        }

        public void Update(
            string razaoSocial,
            string nomeFantasia,
            string cnpj,
            string telefone,
            string email,
            string urlImagem,
            string descricao,
            string inscricaoEstadual,
            string inscricaoMunicipal,
            string website,
            string redeSocial,
            Guid categoriaId)
        {
            RazaoSocial = razaoSocial;
            NomeFantasia = nomeFantasia;
            CNPJ = cnpj;
            Telefone = telefone;
            Email = email;
            UrlImagem = urlImagem;
            Descricao = descricao;
            InscricaoEstadual = inscricaoEstadual;
            InscricaoMunicipal = inscricaoMunicipal;
            Website = website;
            RedeSocial = redeSocial;
            CategoriaId = categoriaId;
        }

        // Métodos para adicionar horários
        public void AdicionarHorario(HorarioFuncionamento horario)
        {
            HorariosFuncionamento.Add(horario);
        }
        private static bool ValidarCNPJ(string cnpj)
        {
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14)
                return false;

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCnpj;
            string digito;
            int soma;
            int resto;

            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }

        public void UpdateStatus(bool status)
        {
            Status = status;
        }
    }
}