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
    public interface IAccountService : IExtendEntityServiceAsync<AccountEntity, AccountDto.AccountRequestDto, AccountDto.AccountResponseDto, AccountDto.AccountSearchDto.AccountFilter>
    {

    }

    public class AccountService : ExtendedEntityServiceBase<AccountEntity, AccountDto.AccountRequestDto, AccountDto.AccountResponseDto, AccountDto.AccountSearchDto.AccountFilter>, IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository, ILogger<AccountService> logger) : base(repository, logger)
        {
            _logger = logger;
            _repository = repository;
        }

    }
}
