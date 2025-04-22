using System.Globalization;

namespace SurvayBasket.Abstractions
{
    public class Result
    {

        public bool IsSuccess { get; } // ----> 1
        public bool IsFailue => !IsSuccess;
        public Error Error { get; }   // ----> 2

        public Result(bool isSuccess, Error error)
        {
            if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
                throw new InvalidOperationException();

            IsSuccess=isSuccess;
            Error=error;
        }

        public static Result Success () => new Result(true, Error.None);
        public static Result Falire (Error error) => new Result(false, error);

        public static Result<TValue> Success<TValue>(TValue value ) => new Result<TValue>(value,true, Error.None);
        public static Result<TValue> Falire<TValue>(Error error) => new Result<TValue>(default,false, error);


    }

    /*****************************/

    public class Result<TValue> : Result
    {

        private readonly TValue? value;   // ----> 3

        public Result(TValue _value, bool isSuccess, Error error):base(isSuccess,error)
        {
            value = _value;
            
        }

        public TValue? Value()
        {

            return IsSuccess ? value : throw new InvalidOperationException("faliure operation");
        }





    }

}
