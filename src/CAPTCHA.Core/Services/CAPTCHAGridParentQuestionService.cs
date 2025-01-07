using CAPTCHA.Core.Models;

namespace CAPTCHA.Core.Services
{
    public class CAPTCHAGridParentQuestionService
    {
        public static (CAPTCHAGridParentQuestion question, ICollection<CAPTCHAGridChild> parentsChildren) CreateQuestion()
        {
            var imgService = new CAPTCHAImgService();
            var rn = RandomStringService.GenerateRandomString(12);

            var question = new CAPTCHAGridParentQuestion() { ValueInPlainText = rn[..6]};

            var componentsOfValue = rn.ToCharArray();

            List<CAPTCHAGridChild> parentsChildren = [];

            foreach (var component in componentsOfValue)
            {
                var child = new CAPTCHAGridChild
                {
                    CAPTCHAGridParentQuestionId = question.Id,
                    ValueInPlainText = component.ToString()
                };

                parentsChildren.Add(child);

                var bytes = imgService.CreateImg(question, child.ValueInPlainText);
                
                child.SetTempBytes(bytes);
            }

            return (question, parentsChildren);
        }
    }
}
