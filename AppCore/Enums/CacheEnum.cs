using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Enums
{
    // Caching işlemleri için tanımlanmış enum değerleri
    public enum CacheEnum
    {
        TimeOutInMinutes = 1,
        TimeOutInSeconds = 2,
        AbsoluteExpiration = 3,
        SlidingExpiration = 4
    }
}
