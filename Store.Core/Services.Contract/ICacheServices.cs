using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services.Contract
{
    public interface ICacheServices
    {

        Task SetCacheAsync(string key, object value,TimeSpan expireTime);

        Task<string> GetCacheAsync(string key);

    }
}
