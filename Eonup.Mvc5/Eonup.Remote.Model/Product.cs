using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eonup.Remote.Model
{
	public class Product
	{
		public int Id { get; set; }
		public long ProductId { get; set; }
		public int CategoryId { get; set; }
		public string Title { get; set; }
		public decimal Price { get; set; }
		public string Url { get; set; }
		public string ImageUrl { get; set; }
	}
}
