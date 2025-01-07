using System.Text;

namespace CAPTCHA.Core.Services
{
    internal class RandomStringService
    {
        private static readonly string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string GenerateRandomString(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentException("Size must be greater than zero.", nameof(size));
            }

            var random = new Random();
            var stringBuilder = new StringBuilder(size);

            for (int i = 0; i < size; i++)
            {
                int index = random.Next(Characters.Length);
                stringBuilder.Append(Characters[index]);
            }

            return stringBuilder.ToString();
        }
    }
}
