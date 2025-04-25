using CRS.ChannelManager.Infrastructure.Repositories;
using CRS.ChannelManager.Library.Base;
using CRS.ChannelManager.Library.BaseInterface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Infrastructure
{
    public class CRSChannelManagerUnitOfWork : UnitOfWorkBase
    {
        private IDbContext _dbContext;
        private IHttpContextAccessor _httpContext;

        public CRSChannelManagerUnitOfWork(IDbContext dbContext, IHttpContextAccessor httpContext) : base(dbContext, httpContext)
        {
            _dbContext = dbContext;
            _httpContext = httpContext;
        }
        private ICountryRepository _iCountryRepository;
        public ICountryRepository iCountryRepository
        {
            get
            {
                if (_iCountryRepository == null)
                {
                    _iCountryRepository = new CountryRepository(_dbContext, _httpContext);
                }
                return _iCountryRepository;
            }
        }

        private IAccountRepository _iAccountRepository;
        public IAccountRepository iAccountRepository
        {
            get
            {
                if (_iAccountRepository == null)
                {
                    _iAccountRepository = new AccountRepository(_dbContext, _httpContext);
                }
                return _iAccountRepository;
            }
        }

        private IChannelRepository _iChannelRepository;
        public IChannelRepository iChannelRepository
        {
            get
            {
                if (_iChannelRepository == null)
                {
                    _iChannelRepository = new ChannelRepository(_dbContext, _httpContext);
                }
                return _iChannelRepository;
            }
        }

        private IHotelRepository _iHotelRepository;
        public IHotelRepository iHotelRepository
        {
            get
            {
                if (_iHotelRepository == null)
                {
                    _iHotelRepository = new HotelRepository(_dbContext, _httpContext);
                }
                return _iHotelRepository;
            }
        }

        private IProductRepository _iProductRepository;
        public IProductRepository iProductRepository
        {
            get
            {
                if (_iProductRepository == null)
                {
                    _iProductRepository = new ProductRepository(_dbContext, _httpContext);
                }
                return _iProductRepository;
            }
        }

        private IPackageRepository _iPackageRepository;
        public IPackageRepository iPackageRepository
        {
            get
            {
                if (_iPackageRepository == null)
                {
                    _iPackageRepository = new PackageRepository(_dbContext, _httpContext);
                }
                return _iPackageRepository;
            }
        }

        private IPackagePlanRepository _iPackagePlanRepository;
        public IPackagePlanRepository iPackagePlanRepository
        {
            get
            {
                if (_iPackagePlanRepository == null)
                {
                    _iPackagePlanRepository = new PackagePlanRepository(_dbContext, _httpContext);
                }
                return _iPackagePlanRepository;
            }
        }

        private IRoomTypeRepository _iRoomTypeRepository;
        public IRoomTypeRepository iRoomTypeRepository
        {
            get
            {
                if (_iRoomTypeRepository == null)
                {
                    _iRoomTypeRepository = new RoomTypeRepository(_dbContext, _httpContext);
                }
                return _iRoomTypeRepository;
            }
        }

        private IMarketSegmentRepository _iMarketSegmentRepository;
        public IMarketSegmentRepository iMarketSegmentRepository
        {
            get
            {
                if (_iMarketSegmentRepository == null)
                {
                    _iMarketSegmentRepository = new MarketSegmentRepository(_dbContext, _httpContext);
                }
                return _iMarketSegmentRepository;
            }
        }

        private ISubSegmentRepository _iSubSegmentRepository;
        public ISubSegmentRepository iSubSegmentRepository
        {
            get
            {
                if (_iSubSegmentRepository == null)
                {
                    _iSubSegmentRepository = new SubSegmentRepository(_dbContext, _httpContext);
                }
                return _iSubSegmentRepository;
            }
        }

        private IChannelRoomTypeRepository _iChannelRoomTypeRepository;
        public IChannelRoomTypeRepository iChannelRoomTypeRepository
        {
            get
            {
                if (_iChannelRoomTypeRepository == null)
                {
                    _iChannelRoomTypeRepository = new ChannelRoomTypeRepository(_dbContext, _httpContext);
                }
                return _iChannelRoomTypeRepository;
            }
        }

        private IChannelMappingRoomTypeRepository _iChannelMappingRoomTypeRepository;
        public IChannelMappingRoomTypeRepository iChannelMappingRoomTypeRepository
        {
            get
            {
                if (_iChannelMappingRoomTypeRepository == null)
                {
                    _iChannelMappingRoomTypeRepository = new ChannelMappingRoomTypeRepository(_dbContext, _httpContext);
                }
                return _iChannelMappingRoomTypeRepository;
            }
        }

    }
}
