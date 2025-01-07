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

            if (size > Characters.Length)
            {
                throw new ArgumentException($"Size cannot exceed the number of unique characters ({Characters.Length}).", nameof(size));
            }

            var random = new Random();
            var characterList = Characters.ToList();

            var stringBuilder = new StringBuilder(size);

            for (int i = 0; i < size; i++)
            {
                int index = random.Next(characterList.Count);
                stringBuilder.Append(characterList[index]);
                characterList.RemoveAt(index); // Remove the used character to avoid repetition
            }

            return stringBuilder.ToString();
        }
    }

}
