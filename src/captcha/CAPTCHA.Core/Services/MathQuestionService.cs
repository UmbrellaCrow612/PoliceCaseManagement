﻿using CAPTCHA.Core.Models;
namespace CAPTCHA.Core.Services
{
    public class MathQuestionService
    {
        public static (byte[] imgQuestionBytes, CAPTCHAMathQuestion question) CreateQuestion()
        {
            var random = new Random();
            int num1 = random.Next(1, 20); 
            int num2 = random.Next(1, 20);
            char[] operations = { '+', '-' };
            char operation = operations[random.Next(operations.Length)];
            string expression = $"{num1} {operation} {num2}";
            double answer = operation switch
            {
                '+' => num1 + num2,
                '-' => num1 - num2,
                _ => throw new InvalidOperationException("Unsupported operation")
            };

            var question = new CAPTCHAMathQuestion
            {
                Answer = answer,
                ExpiresAt = DateTime.UtcNow.AddMinutes(2)
            };

            var imgService = new ImgService();
            var bytes = imgService.CreateImg(question, expression);

            return (bytes, question);
        }

    }
}
