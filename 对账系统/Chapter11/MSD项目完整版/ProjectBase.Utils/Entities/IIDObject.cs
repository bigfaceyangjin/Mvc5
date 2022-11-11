using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBase.Utils.Entities
{
    public interface IIDObject<T>
    {
        T ID { get; set; }
    }
}
