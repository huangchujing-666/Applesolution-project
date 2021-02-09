using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace Palmary.Loyalty.Web_backend.ObjectModels
{
    public class ThumbnailImage
    {
        private readonly string _savePath = string.Format("{0}{1}/{2}", HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings["storageFolder"], ConfigurationManager.AppSettings["photoFolder"]);
        public bool GetThumbnail(string sourceUrl, string saveUrl, int width = 60, int height = 60)
        {
            var originalImage = Image.FromFile(sourceUrl);
            int x, y, w, h;
            if (originalImage.Width / originalImage.Height > width / height)
            {
                h = Convert.ToInt32(originalImage.Height * (Convert.ToDouble(width) / Convert.ToDouble(originalImage.Width)));
                w = width;
                x = 0;
                y = (height - h) / 2;
            }
            else
            {
                h = height;
                w = Convert.ToInt32(originalImage.Width * (Convert.ToDouble(height) / Convert.ToDouble(originalImage.Height)));
                x = (width - w) / 2;
                y = 0;
            }
            var bm = new Bitmap(width, height);
            var g = Graphics.FromImage(bm);

            // 指定高质量、低速度呈现。
            g.SmoothingMode = SmoothingMode.HighQuality;
            // 指定高质量的双三次插值法。执行预筛选以确保高质量的收缩。此模式可产生质量最高的转换图像。
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.Clear(Color.White);
            g.DrawImage(originalImage, new Rectangle(x, y, w, h), 0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel);

            var quality = new long[1];
            quality[0] = 100;

            var encoderParams = new EncoderParameters();
            var encoderParam = new EncoderParameter(Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            var arrayICI = ImageCodecInfo.GetImageEncoders();//获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            var jpegICI = arrayICI.FirstOrDefault(t => t.FormatDescription.Equals("JPEG"));
            System.Diagnostics.Debug.WriteLine(_savePath, "_savePath");
            System.Diagnostics.Debug.WriteLine(saveUrl, "saveUrl");
            if (jpegICI != null)
            {
                bm.Save(string.Format("{0}/{1}", _savePath, saveUrl), jpegICI, encoderParams);
            }

            bm.Dispose();
            originalImage.Dispose();
            g.Dispose();

            return true;
        }
    }
}