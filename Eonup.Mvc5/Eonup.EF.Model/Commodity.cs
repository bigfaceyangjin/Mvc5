namespace Eonup.EF.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

	[Table("JD_Commodity_001")]
	public partial class Commodity
    {
		[Display(Name ="主键ID")]
        public int Id { get; set; }

        public long? ProductId { get; set; }
		[Display(Name ="商品类型")]
        public int CategoryId { get; set; }

		[Display(Name ="标题")]
        [StringLength(500)]
		public string Title { get; set; }
		[Required]
		[Display(Name ="价格")]
        public decimal Price { get; set; }

        [StringLength(1000)]
		[Display(Name ="链接地址")]
        public string Url { get; set; }

        [StringLength(1000)]
		[Display(Name = "图片地址")]
		public string ImageUrl { get; set; }
    }
}
