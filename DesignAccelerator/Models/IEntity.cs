using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DesignAccelerator.Models
{
    public interface IEntity
    {
        EntityState EntityState { get; set; }
    }

    public enum EntityState
    {
        Unchanged,
        Added,
        Modified,
        Deleted
    }
}