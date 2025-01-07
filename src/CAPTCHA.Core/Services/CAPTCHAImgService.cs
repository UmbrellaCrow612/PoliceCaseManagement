using CAPTCHA.Core.Models;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;

namespace CAPTCHA.Core.Services
{
    public class CAPTCHAImgService : ICAPTCHAImgService
    {
        private readonly Random _random = new();
        private readonly string[] _fonts = { "Arial", "Verdana", "Georgia", "Tahoma", "Times New Roman" };

        public byte[] CreateImg(CAPTCHAMathQuestion question, string expression)
        {
            // Image dimensions with slight randomization
            int width = _random.Next(180, 220);
            int height = _random.Next(70, 90);

            using var bitmap = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(bitmap);
            using var memoryStream = new MemoryStream();

            // Enhanced quality settings
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Random background pattern
            CreateRandomBackground(graphics, width, height);

            // Add complex noise patterns
            AddComplexNoise(graphics, width, height);

            // Draw the expression with enhanced distortion
            DrawEnhancedText(graphics, expression, width, height);

            // Add overlapping lines and curves
            AddOverlappingDistortion(graphics, width, height);

            bitmap.Save(memoryStream, ImageFormat.Png);
            return memoryStream.ToArray();
        }

        private void CreateRandomBackground(Graphics graphics, int width, int height)
        {
            using var brush = new LinearGradientBrush(
                new Point(0, 0),
                new Point(width, height),
                Color.FromArgb(_random.Next(230, 255), _random.Next(230, 255), _random.Next(230, 255)),
                Color.FromArgb(_random.Next(230, 255), _random.Next(230, 255), _random.Next(230, 255))
            );
            graphics.FillRectangle(brush, 0, 0, width, height);
        }

        private void AddComplexNoise(Graphics graphics, int width, int height)
        {
            // Add multiple layers of noise with varying opacity
            for (int layer = 0; layer < 3; layer++)
            {
                using var brush = new SolidBrush(Color.FromArgb(
                    _random.Next(30, 90),
                    _random.Next(0, 255),
                    _random.Next(0, 255),
                    _random.Next(0, 255)
                ));

                for (int i = 0; i < 200; i++)
                {
                    float size = _random.Next(1, 4);
                    graphics.FillEllipse(brush,
                        _random.Next(-5, width + 5),
                        _random.Next(-5, height + 5),
                        size, size);
                }
            }
        }

        private void DrawEnhancedText(Graphics graphics, string text, int width, int height)
        {
            float startX = width * 0.1f;
            float startY = height * 0.3f;
            float charSpacing = (width * 0.8f) / text.Length;

            for (int i = 0; i < text.Length; i++)
            {
                using var font = new Font(
                    _fonts[_random.Next(_fonts.Length)],
                    _random.Next(18, 28),
                    FontStyle.Bold
                );

                using var brush = new SolidBrush(Color.FromArgb(
                    _random.Next(0, 80),
                    _random.Next(0, 80),
                    _random.Next(0, 80)
                ));

                var state = graphics.Save();

                // Enhanced distortion with variable rotation and position
                float x = startX + (i * charSpacing) + _random.Next(-5, 5);
                float y = startY + _random.Next(-5, 5);
                graphics.TranslateTransform(x, y);
                graphics.RotateTransform(_random.Next(-30, 30));
                graphics.ScaleTransform(
                    1.0f + (_random.Next(-10, 10) / 100.0f),
                    1.0f + (_random.Next(-10, 10) / 100.0f)
                );

                graphics.DrawString(text[i].ToString(), font, brush, 0, 0);
                graphics.Restore(state);
            }
        }

        private void AddOverlappingDistortion(Graphics graphics, int width, int height)
        {
            // Add curves and lines with varying thickness and opacity
            for (int i = 0; i < _random.Next(3, 6); i++)
            {
                using var pen = new Pen(
                    Color.FromArgb(_random.Next(30, 90), _random.Next(0, 100), _random.Next(0, 100), _random.Next(0, 100)),
                    _random.Next(1, 3)
                );

                var points = new Point[_random.Next(3, 6)];
                for (int j = 0; j < points.Length; j++)
                {
                    points[j] = new Point(
                        _random.Next(0, width),
                        _random.Next(0, height)
                    );
                }

                if (_random.Next(2) == 0)
                {
                    graphics.DrawCurve(pen, points);
                }
                else
                {
                    graphics.DrawLines(pen, points);
                }
            }
        }

        public byte[] CreateImg(CAPTCHAGridParentQuestion question, string component)
        {
            // Image dimensions with slight randomization
            int width = _random.Next(100, 150);
            int height = _random.Next(50, 70);

            using var bitmap = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(bitmap);
            using var memoryStream = new MemoryStream();

            // Enhanced quality settings
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Random background pattern
            CreateRandomBackground(graphics, width, height);

            // Add complex noise patterns
            AddComplexNoise(graphics, width, height);

            // Draw the component text with enhanced distortion
            DrawEnhancedText(graphics, component, width, height);

            // Add overlapping lines and curves
            AddOverlappingDistortion(graphics, width, height);

            bitmap.Save(memoryStream, ImageFormat.Png);
            return memoryStream.ToArray();
        }

    }
}
