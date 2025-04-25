using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseEnum
{
    public enum FilterOperator
    {
        Equal = 1,
        NotEqual = 2, 
        LessThan = 3, 
        LessThanOrEqual = 4,
        GreaterThan = 5,
        GreaterThanOrEqual = 6,
        Contains = 7,
        StartWith = 8,
        EndsWith = 9
    }
}
