using CRS.ChannelManager.Domain.Dtos;
using CRS.ChannelManager.Domain.Entities;
using CRS.ChannelManager.Infrastructure.Repositories;
using CRS.ChannelManager.Library.Base;
using CRS.ChannelManager.Library.BaseInterface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Infrastructure.Services
{
    public interface IProductService : IExtendEntityServiceAsync<ProductEntity, ProductDto.ProductRequestDto, ProductDto.ProductResponseDto, ProductDto.ProductSearchDto.ProductFilter>
    {

    }

    public class ProductService : ExtendedEntityServiceBase<ProductEntity, ProductDto.ProductRequestDto, ProductDto.ProductResponseDto, ProductDto.ProductSearchDto.ProductFilter>, IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository, ILogger<ProductService> logger) : base(repository, logger)
        {
            _logger = logger;
            _repository = repository;
        }

    }
}
