namespace Functional.Option
{
    public class Some<T>
    {
        public T Value { get; }
        public Some(T value)
        {
            Value = value;
        }
    }
}