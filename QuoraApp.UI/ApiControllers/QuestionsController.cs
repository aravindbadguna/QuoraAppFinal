using QuoraApp.ServiceLayer;
using System.Web.Http;

namespace QuoraApp.UI.ApiControllers
{
    public class QuestionsController : ApiController
    {
        IQuestionsService qs;
        IAnswersService asr;
        public QuestionsController(IQuestionsService qs,IAnswersService asr)
        {
            this.qs = qs;
            this.asr = asr;
        }
        public void Post(int AnswerID,int UserID,int value)
        {
            this.asr.UpdateAnswerVotesCount(AnswerID, UserID, value);
        }
    }
}
