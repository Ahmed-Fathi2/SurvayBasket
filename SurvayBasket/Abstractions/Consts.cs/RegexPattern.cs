namespace SurvayBasket.Abstractions.Consts.cs
{
    public static class RegexPattern
    {
        public const string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,16}$";


    }
}
