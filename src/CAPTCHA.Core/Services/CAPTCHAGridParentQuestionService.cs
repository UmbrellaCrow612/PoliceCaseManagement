using CAPTCHA.Core.Models;

namespace CAPTCHA.Core.Services
{
    public class CAPTCHAGridParentQuestionService
    {
        public static (ICollection<byte[]> childrenAsBytes, CAPTCHAGridParentQuestion question, ICollection<CAPTCHAGridChild> parentsChildren) CreateQuestion()
        {
            var imgService = new CAPTCHAImgService();
            var rn = RandomStringService.GenerateRandomString(12);

            var question = new CAPTCHAGridParentQuestion() { ValueInPlainText = rn[..6]};

            var componentsOfValue = rn.ToCharArray();

            List<CAPTCHAGridChild> parentsChildren = [];
            List<byte[]> childrenAsBytes = [];

            foreach (var component in componentsOfValue)
            {
                parentsChildren.Add(new CAPTCHAGridChild 
                { 
                    CAPTCHAGridParentQuestionId = question.Id, 
                    ValueInPlainText = component.ToString() 
                });
            }

            foreach (var child in parentsChildren)
            {
                var bytes = imgService.CreateImg(question, child.ValueInPlainText);
                childrenAsBytes.Add(bytes);
            }

            return (childrenAsBytes, question, parentsChildren);
        }
    }
}
