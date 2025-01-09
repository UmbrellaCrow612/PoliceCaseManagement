using CAPTCHA.Core.Models;

namespace CAPTCHA.Core.Services
{
    public class CarouselChoiceGameQuestionService
    {
        public static (CAPTCHACarouselChoiceGameQuestion question, ICollection<CarouselChoiceChild> choices, ICollection<SubCarouselChoiceChild> answerSubChildren) CreateQuestion()
        {
            var imgService = new ImgService();
            var question = new CAPTCHACarouselChoiceGameQuestion();

            var choiceCount = 3;

            ICollection<CarouselChoiceChild> carouselChoiceChildren = [];
            ICollection<SubCarouselChoiceChild> answerSubCarouselChoiceChildren = [];

            for (int i = 0; i < choiceCount; i++)
            {
                var child = new CarouselChoiceChild
                {
                    CaptchaCarouselChoiceGameQuestionId = question.Id
                };
                carouselChoiceChildren.Add(child);

                var childRandomString = RandomStringService.GenerateRandomString(4);
                var childAnswerString = childRandomString[..2];

                child.SetTempAnswer(childAnswerString.ToString());

                foreach (var component in childRandomString)
                {
                    var subChild = new SubCarouselChoiceChild { CarouselChoiceChildId = child.Id };

                    if (childAnswerString.Contains(component))
                    {
                        answerSubCarouselChoiceChildren.Add(subChild);
                    }

                    var bytes = imgService.CreateImg(question, component.ToString());
                    subChild.SetTempBytes(bytes);

                    child.SetTempFullSubChoice(subChild);
                }
            }

            return (question, carouselChoiceChildren, answerSubCarouselChoiceChildren);

        }
    }
}
