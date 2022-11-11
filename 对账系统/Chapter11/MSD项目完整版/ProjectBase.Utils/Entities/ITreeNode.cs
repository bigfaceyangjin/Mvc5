using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBase.Utils.Entities
{
    public interface ITreeNode<T,TId> 
    {
        T Parent { get; set; }

        string Name { get; set; }

        TId Id { get; set; }
    }

}
