namespace Eonup.EF.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class temp_tab1
    {
        public int id { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }
    }
}
