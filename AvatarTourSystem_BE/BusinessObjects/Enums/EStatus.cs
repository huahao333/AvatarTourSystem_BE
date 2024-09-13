using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Enums
{
    public enum EStatus
    {
        
        Active = 1,
        Disabled = 0,
        IsDeleted = -1,
        IsCancelled = 2,
        IsPending = 3,
        InProgress = 9,
        IsCompleted = 4,
    }
}
