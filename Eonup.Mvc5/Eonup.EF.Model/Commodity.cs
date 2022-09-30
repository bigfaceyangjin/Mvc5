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
		[Display(Name ="����ID")]
        public int Id { get; set; }

        public long? ProductId { get; set; }
		[Display(Name ="��Ʒ����")]
        public int CategoryId { get; set; }

		[Display(Name ="����")]
        [StringLength(500)]
		public string Title { get; set; }
		[Required]
		[Display(Name ="�۸�")]
        public decimal Price { get; set; }

        [StringLength(1000)]
		[Display(Name ="���ӵ�ַ")]
        public string Url { get; set; }

        [StringLength(1000)]
		[Display(Name = "ͼƬ��ַ")]
		public string ImageUrl { get; set; }
    }
}
