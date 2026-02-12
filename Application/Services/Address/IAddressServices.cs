using Application.Common.Results;
using Application.Contracts.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Address;

public interface IAddressServices
{
    Result<IEnumerable<AddressResponse>> GetAll();
}
