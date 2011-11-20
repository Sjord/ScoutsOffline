using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScoutsOffline.Model
{
    public interface IListContainer<T>
    {
        List<T> ToList();
    }
}
