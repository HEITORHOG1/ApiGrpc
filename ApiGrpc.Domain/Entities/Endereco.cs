using ApiGrpc.Domain.Entities;
using ApiGrpc.Domain.Entities.Base;
using ApiGrpc.Domain.Exceptions;

public class Endereco : Entity
{
    public Guid? EstabelecimentoId { get; private set; }
    public Guid? UsuarioId { get; private set; }
    public bool IsEstabelecimento { get; private set; }
    public virtual Estabelecimento? Estabelecimento { get; private set; }
    public virtual ApplicationUser? Usuario { get; private set; }
    public string Logradouro { get; private set; }
    public string Numero { get; private set; }
    public string Complemento { get; private set; }
    public string Bairro { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }
    public string Cep { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public double? RaioEntregaKm { get; private set; }
    public bool Status { get; private set; }

    protected Endereco() { }

    public Endereco(
        string logradouro,
        string numero,
        string complemento,
        string bairro,
        string cidade,
        string estado,
        string cep,
        bool isEstabelecimento,
        Guid? usuarioId,
        Guid? estabelecimentoId,
        double latitude,
        double longitude,
        double? raioEntregaKm = null)
    {
        ValidateConstruction(isEstabelecimento, usuarioId, estabelecimentoId);
        SetProperties(logradouro, numero, complemento, bairro, cidade, estado, cep,
            isEstabelecimento, usuarioId, estabelecimentoId, latitude, longitude, raioEntregaKm);
    }

    private void ValidateConstruction(bool isEstabelecimento, Guid? usuarioId, Guid? estabelecimentoId)
    {
        if (isEstabelecimento && estabelecimentoId == null)
            throw new DomainException("EstabelecimentoId é obrigatório para endereços de estabelecimento");
        if (!isEstabelecimento && usuarioId == null)
            throw new DomainException("UsuarioId é obrigatório para endereços de usuário");
        if (isEstabelecimento && usuarioId != null)
            throw new DomainException("Endereço de estabelecimento não pode ter UsuarioId");
        if (!isEstabelecimento && estabelecimentoId != null)
            throw new DomainException("Endereço de usuário não pode ter EstabelecimentoId");
    }

    private void SetProperties(
        string logradouro,
        string numero,
        string complemento,
        string bairro,
        string cidade,
        string estado,
        string cep,
        bool isEstabelecimento,
        Guid? usuarioId,
        Guid? estabelecimentoId,
        double latitude,
        double longitude,
        double? raioEntregaKm)
    {
        ValidateProperties(latitude, longitude);

        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Cep = cep.Replace("-", "");
        IsEstabelecimento = isEstabelecimento;
        UsuarioId = usuarioId;
        EstabelecimentoId = estabelecimentoId;
        Latitude = latitude;
        Longitude = longitude;
        RaioEntregaKm = isEstabelecimento ? raioEntregaKm ?? 5 : null;
        Status = true;
    }

    private void ValidateProperties(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90)
            throw new DomainException("Latitude deve estar entre -90 e 90");
        if (longitude < -180 || longitude > 180)
            throw new DomainException("Longitude deve estar entre -180 e 180");
    }

    public void Update(
        string logradouro,
        string numero,
        string complemento,
        string bairro,
        string cidade,
        string estado,
        string cep,
        double latitude,
        double longitude)
    {
        ValidateProperties(latitude, longitude);
        SetProperties(logradouro, numero, complemento, bairro, cidade, estado, cep,
            IsEstabelecimento, UsuarioId, EstabelecimentoId, latitude, longitude, RaioEntregaKm);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(bool status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateRaioEntrega(double raioEntregaKm)
    {
        if (!IsEstabelecimento)
            throw new DomainException("Apenas estabelecimentos podem ter raio de entrega");
        if (raioEntregaKm <= 0)
            throw new DomainException("Raio de entrega deve ser maior que zero");

        RaioEntregaKm = raioEntregaKm;
        UpdatedAt = DateTime.UtcNow;
    }
}