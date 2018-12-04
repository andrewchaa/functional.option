using System;

namespace Functional.Option
{
    public class Some<T>
    {
        public T Value { get; }
        public Some(T value)
        {
            if (value == null)
                throw new ArgumentException();

            Value = value;
        }
    }
}