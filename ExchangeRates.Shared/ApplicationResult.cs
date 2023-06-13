namespace ExchangeRates.Shared
{
    public class ApplicationResult<T>
    {
        public bool Success { get; set; }
        public IEnumerable<Error> Errors { get; set; }
        public T Data { get; set; }


        public static ApplicationResult<T> Default()
        {
            return new ApplicationResult<T>();
        }
    }

    public class ApplicationResult : ApplicationResult<dynamic>
    {
        public new static ApplicationResult Default()
        {
            return new ApplicationResult();
        }
    }


    public class ApplicationOKResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
    }

    public class ApplicationErrorResult
    {
        public bool Success { get; set; }
        public IEnumerable<Error> Errors { get; set; }
    }

    public class ApplicationResultEitherOr<TLeft, TRight>
        where TRight : ApplicationErrorResult
    {
        private readonly TLeft _left;
        private readonly TRight _right;
        private readonly bool _isLeft;

        public ApplicationResultEitherOr(TLeft left)
        {
            _left = left;
            _isLeft = true;
        }

        public ApplicationResultEitherOr(TRight right)
        {
            _right = right;
            _isLeft = false;
        }

        public T Match<T>(Func<TLeft, T> left, Func<TRight, T> right)
        {
            return _isLeft ? left(_left) : right(_right);
        }

        public async Task<T> MatchAsync<T>(Func<TLeft, Task<T>> left, Func<TRight, Task<T>> right)
        {
            return _isLeft ? await left(_left) : await right(_right);
        }
    }
}