using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eonup.Framework.ExtensionMethod
{
	public class VerifyCodeHelper
	{
		public static Bitmap CreateVerifyCode(out string code)
		{
			//建立画布
			Bitmap bitmap = new Bitmap(150, 40);
			//在画布上绘图
			Graphics graphic= Graphics.FromImage(bitmap);
			//初始化坐标 并指定宽高 和颜色
			graphic.FillRectangle(new SolidBrush(Color.Orange), 0, 0, 150, 40);

			Font font = new Font(FontFamily.GenericSerif, 33, FontStyle.Bold, GraphicsUnit.Pixel);
			string letters = "ABCDEFGHIJKlmnpQRSTUVWXYZ0123456789";
			Random r = new Random();
			//生成5个随机数
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < 5; i++)
			{
				string letter = letters.Substring(r.Next(0, letters.Length - 1), 1);
				sb.Append(letter);
				graphic.DrawString(letter,font,new SolidBrush(Color.Black), i * 27, r.Next(0, 6));
			}
			code = sb.ToString();
			//背景线
			Pen pen = new Pen(new SolidBrush(Color.Red), 2);
			for (int i = 0; i < 6; i++)
			{
				graphic.DrawLine(pen, new Point(r.Next(0,149), r.Next(0, 39)), new Point(r.Next(0, 149), r.Next(0, 49)));
			}
			return bitmap;
		}
	}
}
