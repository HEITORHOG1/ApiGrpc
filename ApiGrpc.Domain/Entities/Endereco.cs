using ApiGrpc.Domain.Entities.Base;
using ApiGrpc.Domain.Exceptions;

public class Endereco : Entity
{
    public string Logradouro { get; private set; }
    public string Numero { get; private set; }
    public string Complemento { get; private set; }
    public string Bairro { get; private set; }
    public string Cidade { get; private set; }
    public string Estado { get; private set; }
    public string Cep { get; private set; }
    public bool IsEstabelecimento { get; private set; }
    public Guid UsuarioId { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public double? RaioEntregaKm { get; private set; }
    public bool Status { get; private set; }

    protected Endereco()
    { } // EF Constructor

    public Endereco(
        string logradouro,
        string numero,
        string complemento,
        string bairro,
        string cidade,
        string estado,
        string cep,
        bool isEstabelecimento,
        Guid usuarioId,
        double latitude,
        double longitude,
        double? raioEntregaKm = null)
    {
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Cep = cep;
        IsEstabelecimento = isEstabelecimento;
        UsuarioId = usuarioId;
        Latitude = latitude;
        Longitude = longitude;
        RaioEntregaKm = isEstabelecimento ? raioEntregaKm ?? 5 : null;
        Status = true;
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
        Logradouro = logradouro;
        Numero = numero;
        Complemento = complemento;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Cep = cep;
        Latitude = latitude;
        Longitude = longitude;
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

        RaioEntregaKm = raioEntregaKm;
        UpdatedAt = DateTime.UtcNow;
    }
}