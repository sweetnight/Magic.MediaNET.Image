using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net;

namespace Magic.MediaNET
{
    public class Image
    {

        System.Drawing.Image imageSource { get; set; }

        public Image(string imagePath)
        {
            imageSource = System.Drawing.Image.FromFile(imagePath);
        } // end of method

        public Image(System.Drawing.Image image)
        {
            imageSource = image;
        } // end of method

        public int Height()
        {
            return imageSource.Height;
        } // end of method

        public int Width()
        {
            return imageSource.Width;
        } // end of method

        public System.Drawing.Image FitToSquare(int size)
        {
            Bitmap res = new Bitmap(size, size);
            Graphics g = Graphics.FromImage(res);

            g.FillRectangle(new SolidBrush(Color.White), 0, 0, size, size);

            int x = 0, y = 0, t = size, l = size;

            if (imageSource.Height > imageSource.Width)
            {
                l = imageSource.Width * size / imageSource.Height;
                x = (size - l) / 2;
            }
            else
            {
                t = imageSource.Height * size / imageSource.Width;
                y = (size - t) / 2;
            }

            g.DrawImage(imageSource, new System.Drawing.Rectangle(x, y, l, t), new System.Drawing.Rectangle(0, 0, imageSource.Width, imageSource.Height), GraphicsUnit.Pixel);

            return (System.Drawing.Image)res;
        } // end of function

        public System.Drawing.Image AddFrame(System.Drawing.Image frame)
        {
            Bitmap bitmap1 = new Bitmap(frame.Width, frame.Height);
            Graphics baseImage = Graphics.FromImage(bitmap1);

            int farX = (frame.Width - imageSource.Width) / 2;
            int farY = (frame.Height - imageSource.Height) / 2;

            baseImage.DrawImage(imageSource, new System.Drawing.Point(farX, farY));
            baseImage.DrawImage(frame, new System.Drawing.Point(0, 0));

            return (System.Drawing.Image)bitmap1;
        } // end of function

        public System.Drawing.Image AddFrame(string framePath)
        {
            System.Drawing.Image frame = System.Drawing.Image.FromFile(framePath);

            Bitmap bitmap1 = new Bitmap(frame.Width, frame.Height);
            Graphics baseImage = Graphics.FromImage(bitmap1);

            int farX = (frame.Width - imageSource.Width) / 2;
            int farY = (frame.Height - imageSource.Height) / 2;

            baseImage.DrawImage(imageSource, new System.Drawing.Point(farX, farY));
            baseImage.DrawImage(frame, new System.Drawing.Point(0, 0));

            return (System.Drawing.Image)bitmap1;
        } // end of function

        public System.Drawing.Image CropToSquare(int size)
        {
            Bitmap res = new Bitmap(size, size);
            Graphics g = Graphics.FromImage(res);

            g.FillRectangle(new SolidBrush(Color.White), 0, 0, size, size);

            int t = 0, l = 0;

            if (imageSource.Height > imageSource.Width)
                t = (imageSource.Height - imageSource.Width) / 2;
            else
                l = (imageSource.Width - imageSource.Height) / 2;

            g.DrawImage(imageSource, new System.Drawing.Rectangle(0, 0, size, size), new System.Drawing.Rectangle(l, t, imageSource.Width - l * 2, imageSource.Height - t * 2), GraphicsUnit.Pixel);

            return (System.Drawing.Image)res;
        } // end of function

        public static string? SaveImageFromUrl(string imageUrl, string filename, ImageFormat format)
        {

            WebClient client = new WebClient();

            try
            {
                Stream stream = client.OpenRead(imageUrl);
                Bitmap bitmap;
                bitmap = new Bitmap(stream);

                if (bitmap != null)
                {
                    Size targetSize = TargetSize(bitmap);
                    System.Drawing.Image image = ResizeImage(bitmap, targetSize);

                    image.Save(filename, format);

                    stream.Flush();
                    stream.Close();
                    client.Dispose();

                    // Pastikan file benar-benar ada setelah penyimpanan
                    if (File.Exists(filename))
                    {
                        return filename; // Return the filename if the file was successfully saved
                    }
                }
            }
            catch
            {
            }

            return null;

        } // end of method

        // target size untuk ukuran 1280 x 720 (maximize / outer boundary)
        private static Size TargetSize(System.Drawing.Image imgToResize)
        {
            float x1 = (float)imgToResize.Width;
            float y1 = (float)imgToResize.Height;

            bool wide = (y1 / x1) < (float)0.5625 ? true : false; // 0.5625 adalah 720 / 1280

            int x2 = 0;
            int y2 = 0;

            if (wide)
            {
                x2 = 1280;
                y2 = (int)((float)1280 * y1 / x1);
            }
            else
            {
                y2 = 720;
                x2 = (int)((float)720 * x1 / y1);
            }

            Size size = new Size(x2, y2);

            return size;
        } // end of method

        private static System.Drawing.Image ResizeImage(System.Drawing.Image imgToResize, Size size)
        {
            //Get the image current width  
            float sourceWidth = (float)imgToResize.Width;

            //Get the image current height  
            float sourceHeight = (float)imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            //Calulate  width with new desired size  
            nPercentW = ((float)size.Width / (float)sourceWidth);

            //Calculate height with new desired size  
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            //New Width  
            int destWidth = (int)(sourceWidth * nPercent);

            //New Height  
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw image with new width and height  
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (System.Drawing.Image)b;
        } // end of method

        public void SaveAsJpg(string targetPath)
        {
            imageSource.Save(targetPath, ImageFormat.Jpeg);
        } // end of method

        public void SaveAsPng(string targetPath)
        {
            imageSource.Save(targetPath, ImageFormat.Png);
        } // end of method

        public void SaveAsBmp(string targetPath)
        {
            imageSource.Save(targetPath, ImageFormat.Bmp);
        } // end of method

        public void SaveAsGif(string targetPath)
        {
            imageSource.Save(targetPath, ImageFormat.Gif);
        } // end of method

        public void SaveAsTiff(string targetPath)
        {
            imageSource.Save(targetPath, ImageFormat.Tiff);
        } // end of method

        public string Recreate(string inputFilename)
        {
            string dir = Path.GetDirectoryName(inputFilename)!;
            string imageTargetFilename = string.Empty;
            string randomString = string.Empty;

            while (true)
            {
                randomString = Magic.HelperNET.CreateRandomString(5);

                imageTargetFilename = Path.Combine(dir, Path.GetFileNameWithoutExtension(inputFilename) + "_" + randomString + ".jpg");

                if (System.IO.File.Exists(imageTargetFilename)) continue;

                break;
            }

            // Ambil image dari file menggunakan System.Drawing.Image
            System.Drawing.Image A = System.Drawing.Image.FromFile(inputFilename);

            // Masukkan image A ke variabel B dengan tipe System.Drawing.Graphics
            Graphics B = Graphics.FromImage(A);

            // Ubah variabel B kembali ke tipe System.Drawing.Image dan masukkan ke variabel C
            System.Drawing.Image C = new Bitmap(A);
            B = Graphics.FromImage(C);

            // Save variabel C ke file jpg
            C.Save(imageTargetFilename, ImageFormat.Jpeg);

            return imageTargetFilename;
        } // end of method

    } // end of class
} // end of namespace
