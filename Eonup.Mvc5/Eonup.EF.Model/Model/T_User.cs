namespace Eonup.EF.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class T_User
    {
        public int ID { get; set; }

        [StringLength(20)]
        public string UserName { get; set; }

        [StringLength(100)]
        public string Remark { get; set; }

        public int Age { get; set; }

        [StringLength(20)]
        public string Pwd { get; set; }

        [StringLength(30)]
        public string Email { get; set; }
    }
}
