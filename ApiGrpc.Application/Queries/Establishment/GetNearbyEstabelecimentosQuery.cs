using ApiGrpc.Application.DTOs.Address;
using ApiGrpc.Domain.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGrpc.Application.Queries.Establishment
{
    public record GetNearbyEstabelecimentosQuery(double Latitude, double Longitude, double Raio)
   : IRequest<IEnumerable<EnderecoDto>>;

    public class GetNearbyEstabelecimentosQueryHandler
       : IRequestHandler<GetNearbyEstabelecimentosQuery, IEnumerable<EnderecoDto>>
    {
        private readonly IEnderecoRepository _repository;
        private readonly IMapper _mapper;

        public GetNearbyEstabelecimentosQueryHandler(IEnderecoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EnderecoDto>> Handle(GetNearbyEstabelecimentosQuery request, CancellationToken cancellationToken)
        {
            var enderecos = await _repository.GetEstabelecimentosProximosAsync(
                request.Latitude,
                request.Longitude,
                request.Raio);

            return _mapper.Map<IEnumerable<EnderecoDto>>(enderecos);
        }
    }
}
