namespace ExchangeRates.Shared
{
    public class ApplicationResult<T>
    {
        public bool Success { get; set; }
        public IEnumerable<Error> Errors { get; set; } = default!;
        public T Data { get; set; } = default!;


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
}