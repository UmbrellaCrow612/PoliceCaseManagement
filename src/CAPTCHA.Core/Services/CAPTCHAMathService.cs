﻿using CAPTCHA.Core.Models;
namespace CAPTCHA.Core.Services
{
    public class CAPTCHAMathService
    {
        public static (byte[] imgQuestionBytes, CAPTCHAMathQuestion question) CreateQuestion()
        {
            var random = new Random();
            int num1 = random.Next(1, 50); 
            int num2 = random.Next(1, 50); 
            char[] operations = { '+', '-', '*', '/' };
            char operation = operations[random.Next(operations.Length)];

            string expression = $"{num1} {operation} {num2}";

            double answer = operation switch
            {
                '+' => num1 + num2,
                '-' => num1 - num2,
                '*' => num1 * num2,
                '/' => Math.Round((double)num1 / num2, 2),
                _ => throw new InvalidOperationException("Unsupported operation")
            };

            var question = new CAPTCHAMathQuestion
            {
                Answer = answer.ToString(),
                ExpiresAt = DateTime.UtcNow.AddMinutes(2)
            };

            return ([], question);
        }


        public bool VaidateQuestion(CAPTCHAMathQuestion question)
        {
            throw new NotImplementedException();
        }
    }
}
