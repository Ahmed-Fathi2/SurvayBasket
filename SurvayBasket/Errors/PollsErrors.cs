namespace SurvayBasket.Errors
{
    public static class PollsErrors
    {
        public static readonly Error PollNotFound = new Error(" Poll Not Found ", "poll with given id dosent Exist ");
        public static readonly Error PollDuplications = new Error(" Dublication operation " , "poll title is already exist and dublication Title is avoided");
    }
}
