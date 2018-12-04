using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Functional.Option
{
    public struct Option<T> : IEquatable<None>, IEquatable<Option<T>>
    {

        private readonly T _value;

        private Option(T value)
        {
            if (value == null) 
                throw new ArgumentNullException();

            _value = value;
            IsSome = true;
        }

        public static implicit operator Option<T>(Some<T> some) => new Option<T>(some.Value);
        public static implicit operator Option<T>(None _) => new Option<T>();
        public static implicit operator Option<T>(T value) => value == null ? F.None : F.Some(value);

        public bool IsSome { get; }
        public bool IsNone => !IsSome;

        public R Match<R>(Func<R> None, Func<T, R> Some)
        {
            return IsSome ? Some(_value) : None();
        }

        public async Task<R> Match<R>(Func<Task<R>> None, Func<T, Task<R>> Some)
        {
            return IsSome ? await Some(_value) : await None();
        }

        public void Match(Action None, Action<T> Some)
        {
            if (IsSome)
            {
                Some(_value);
                return;
            }

            None();
        }

        public async Task Match(Func<Task> None, Func<T, Task> Some)
        {
            if (IsSome)
            {
                await Some(_value);
                return;
            }

            await None();
        }

        public bool Equals(Option<T> other)
            => IsSome == other.IsSome && (IsNone || _value.Equals(other._value));
        public bool Equals(None _) => IsNone;
        public static bool operator ==(Option<T> @this, Option<T> other) => @this.Equals(other);
        public static bool operator !=(Option<T> @this, Option<T> other) => !@this.Equals(other);

        public override string ToString() => IsSome ? $"Some({_value})" : "None";

        public Option<R> Map<R>(Func<T, R> func)
        {
            return IsSome ? F.Some(func(_value)) : F.None;
        }

        public IEnumerable<T> AsEnumerable()
        {
            if (IsSome) yield return _value;
        }
    }
}
