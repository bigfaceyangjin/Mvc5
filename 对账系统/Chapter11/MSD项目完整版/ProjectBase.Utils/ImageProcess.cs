using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ProjectBase.Utils
{
    public static class ImageProcess
    {
        private static Image ImageSizeConvert(Image orignImage, Size standSize)
        {
            Size newsize = GetNewSize(orignImage.Size, standSize);
            Bitmap newimage = new Bitmap(orignImage, newsize);
            return newimage;
        }

        private static void SaveNewImage(Image newimage, ImageCodecInfo imagecodeinfo, string newfile)
        {
            using (EncoderParameters parms = new EncoderParameters(1))
            {

                switch (imagecodeinfo.MimeType)
                {
                    case "image/gif":
                        newimage.Save(newfile, ImageFormat.Jpeg);
                        break;
                    case "image/png":
                        parms.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L);
                        newimage.Save(newfile, imagecodeinfo, parms);
                        break;
                    default:
                        parms.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L);
                        newimage.Save(newfile, imagecodeinfo, parms);
                        break;
                }
                newimage.Dispose();
            }
        }

        private static Size GetNewSize(Size oldsize, Size stand)
        {
            Size newsize = oldsize;
            if (newsize.Width > stand.Width)
            {

                newsize.Height = stand.Width * newsize.Height / newsize.Width;
                newsize.Width = stand.Width;
            }
            if (newsize.Height > stand.Height)
            {

                newsize.Width = stand.Height * newsize.Width / newsize.Height;
                newsize.Height = stand.Height;
            }
            return newsize;
        }

        private static ImageCodecInfo GetEncoderInfo(Guid mimetype)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].FormatID == mimetype)
                    return encoders[j];
            }
            return null;
        }

        public static void Generate(Image oldImage, Size standSize, string fileName)
        {
            if (oldImage == null)
            {
                throw new ArgumentNullException("oldImage");
            }
            Image nimage = ImageSizeConvert(oldImage, standSize);
            SaveNewImage(nimage, GetEncoderInfo(oldImage.RawFormat.Guid), fileName);
        }


        public static void Generate(Image oldImage, int width, int height, string fileName)
        {
            Generate(oldImage, new Size(width, height), fileName);
        }

        /// <summary>
        /// 只限定宽度，高度自适应
        /// </summary>
        /// <param name="oldimage">要处理的原图</param>
        /// <param name="width">生成图片的宽度</param>
        /// <param name="filename">生成后的文件名</param>
        public static void Generate(Image oldImage, int width, string fileName)
        {
            if (oldImage == null)
            {
                throw new ArgumentNullException("oldImage");
            }
            int height = Convert.ToInt32((width / (double)oldImage.Width) * oldImage.Height);
            Generate(oldImage, new Size(width, height), fileName);
        }
    }
}
