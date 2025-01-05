using CAPTCHA.Core.Models;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;

namespace CAPTCHA.Core.Services
{
    public class CAPTCHAImgService : ICAPTCHAImgService
    {

        public byte[] CreateImg(CAPTCHAMathQuestion question, string expression)
        {
            // Image dimensions
            int width = 200;
            int height = 80;

            using var bitmap = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(bitmap);
            using var memoryStream = new MemoryStream();
            // Set up graphics quality
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Fill background with light color
            using (var brush = new SolidBrush(Color.White))
            {
                graphics.FillRectangle(brush, 0, 0, width, height);
            }

            // Add noise/pattern to background
            AddNoiseToImage(graphics, width, height);

            // Draw the expression
            using (var font = new Font("Arial", 20, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.Black))
            {
                // Measure string to center it
                var stringSize = graphics.MeasureString(expression, font);
                var x = (width - stringSize.Width) / 2;
                var y = (height - stringSize.Height) / 2;

                // Apply slight rotation to each character
                DrawDistortedText(graphics, expression, font, brush, x, y);
            }

            // Add some random lines for additional security
            AddRandomLines(graphics, width, height);

            // Save to memory stream as PNG
            bitmap.Save(memoryStream, ImageFormat.Png);
            return memoryStream.ToArray();
        }

        private static void AddNoiseToImage(Graphics graphics, int width, int height)
        {
            var random = new Random();
            using (var brush = new SolidBrush(Color.LightGray))
            {
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(0, width);
                    int y = random.Next(0, height);
                    graphics.FillEllipse(brush, x, y, 2, 2);
                }
            }
        }

        private static void AddRandomLines(Graphics graphics, int width, int height)
        {
            var random = new Random();
            using (var pen = new Pen(Color.Gray, 1))
            {
                for (int i = 0; i < 5; i++)
                {
                    int x1 = random.Next(0, width);
                    int y1 = random.Next(0, height);
                    int x2 = random.Next(0, width);
                    int y2 = random.Next(0, height);
                    graphics.DrawLine(pen, x1, y1, x2, y2);
                }
            }
        }

        private static void DrawDistortedText(Graphics graphics, string text, Font font, Brush brush, float startX, float startY)
        {
            var random = new Random();
            float currentX = startX;

            foreach (char c in text)
            {
                // Apply random rotation between -15 and 15 degrees
                float rotation = random.Next(-15, 15);

                // Save the current graphics state
                var state = graphics.Save();

                // Rotate around the current character position
                graphics.TranslateTransform(currentX, startY);
                graphics.RotateTransform(rotation);
                graphics.DrawString(c.ToString(), font, brush, 0, 0);

                // Restore graphics state
                graphics.Restore(state);

                // Move to next character position
                currentX += graphics.MeasureString(c.ToString(), font).Width;
            }
        }
    }
}
