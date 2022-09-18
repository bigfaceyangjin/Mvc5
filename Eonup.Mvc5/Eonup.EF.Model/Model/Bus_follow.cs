namespace Eonup.EF.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Bus_follow
    {
        public int id { get; set; }

        public int? sourceId { get; set; }

        public int? targetId { get; set; }

        public bool? isRelation { get; set; }
    }
}
