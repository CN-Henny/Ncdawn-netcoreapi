using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using QRCoder;
using Microsoft.Extensions.Configuration;

namespace Ncdawn.Service.Account
{

    public interface IQRCodeService : IBaseService
    {
        string Create_ImgCode(string codeNumber);
    }

    public class QRCodeService : IBaseService, IQRCodeService
    {
        private IHostingEnvironment hostingEnv;
        public string filePathConfig;

        public QRCodeService(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            hostingEnv = hostingEnvironment;
            filePathConfig = configuration.GetSection("File")["Attach"];
        }

        #region 二维码生成
        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="codeNumber">要生成二维码的字符串</param>     
        /// <param name="size">大小尺寸</param>
        /// <returns>二维码图片</returns>
        public string Create_ImgCode(string codeNumber)
        {
            QRCodeGenerator generator = new QRCodeGenerator();

            QRCodeData codeData = generator.CreateQrCode(codeNumber, QRCodeGenerator.ECCLevel.M, true);

            QRCode qrcode = new QRCode(codeData);

            Bitmap qrImage = qrcode.GetGraphic(4, Color.Black, Color.White, true);

            //return qrImage;

            string suffix = ".png"; //文件的后缀名根据实际情况
            string fileName = GetTimeStamp() + suffix;
            string ymd = DateTime.Now.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            //获取目录
            string filePath = filePathConfig + ymd + "\\" + newFileName;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string filepath = filePath + "\\" + fileName;

            qrImage.Save(filepath);
            //获取文件储存路径    返回路径
            var strPath = $"/upload/{ymd}/{newFileName}/{fileName}";
            return strPath;
        }
        #endregion

        #region 获取当前时间段额时间戳
        /// <summary>
        /// 获取当前时间段额时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }
        #endregion
    }
}
