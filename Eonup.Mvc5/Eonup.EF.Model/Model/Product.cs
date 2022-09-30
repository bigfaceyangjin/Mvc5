namespace Eonup.EF.Model.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Order_Details = new HashSet<Order_Detail>();
        }

        public int ProductID { get; set; }

        [Required]
        [StringLength(40)]
		[Display(Name ="商品名")]
        public string ProductName { get; set; }
		[Display(Name ="供应商")]
		public int? SupplierID { get; set; }
		[Display(Name = "商品类型")]
		public int? CategoryID { get; set; }

        [StringLength(20)]
        [Required]
		[Display(Name ="产品规格")]
		public string QuantityPerUnit { get; set; }

        [Column(TypeName = "money")]
        [Required]
		[Display(Name ="单价")]
		public decimal UnitPrice { get; set; }

        [Required]
		[Display(Name ="库存量")]
		public short? UnitsInStock { get; set; }

        [Required]
		[Display(Name ="订单量")]
		public short? UnitsOnOrder { get; set; }

        [Required]
		[Display(Name ="销售级别")]
		public short? ReorderLevel { get; set; }

        [Required]
		[Display(Name ="是否停产")]
		public bool Discontinued { get; set; }

		public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Detail> Order_Details { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
