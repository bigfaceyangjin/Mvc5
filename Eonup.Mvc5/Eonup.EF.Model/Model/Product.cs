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
		[Display(Name ="��Ʒ��")]
        public string ProductName { get; set; }
		[Display(Name ="��Ӧ��")]
		public int? SupplierID { get; set; }
		[Display(Name = "��Ʒ����")]
		public int? CategoryID { get; set; }

        [StringLength(20)]
        [Required]
		[Display(Name ="��Ʒ���")]
		public string QuantityPerUnit { get; set; }

        [Column(TypeName = "money")]
        [Required]
		[Display(Name ="����")]
		public decimal UnitPrice { get; set; }

        [Required]
		[Display(Name ="�����")]
		public short? UnitsInStock { get; set; }

        [Required]
		[Display(Name ="������")]
		public short? UnitsOnOrder { get; set; }

        [Required]
		[Display(Name ="���ۼ���")]
		public short? ReorderLevel { get; set; }

        [Required]
		[Display(Name ="�Ƿ�ͣ��")]
		public bool Discontinued { get; set; }

		public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Detail> Order_Details { get; set; }

        public virtual Supplier Supplier { get; set; }
    }
}
