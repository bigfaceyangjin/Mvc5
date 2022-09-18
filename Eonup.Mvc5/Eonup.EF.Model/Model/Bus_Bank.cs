namespace Eonup.EF.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Bus_Bank
    {
        public int id { get; set; }

        [StringLength(50)]
        public string BankName { get; set; }

        public decimal? Deposit { get; set; }
    }
}
