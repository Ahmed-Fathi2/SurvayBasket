namespace SurvayBasket.Errors
{
    public static class QuestionsErrors
    {
        public static readonly Error DublicatedQuestionInTheSamePoll = new Error(" Dublicated Question In This Poll ", " Question is Already Exist in this Poll ");
        public static readonly Error QuestionNotFound = new Error(" Question Not Found In This Poll ", " this question is not found in this poll ");
        public static readonly Error IvalidQuestion = new Error(" IvalidQuestion ", " There are unanswered questions, please vote on them.");
    }
}
