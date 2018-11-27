using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Functional.Option
{
    public class Option<T> : IEquatable<Option<T>>
    {

        private readonly T _value;
        private readonly bool _isSome;
        private bool _isNone => !_isSome;

        private Option(T value)
        {
            _value = value;
            _isSome = true;
        }

        public bool IsSome => _isSome;
        public bool IsNone => _isNone;

        public Option()
        {
            _isSome = false;
        }

        public R Match<R>(Func<R> None, Func<T, R> Some)
        {
            return _isSome ? Some(_value) : None();
        }

        public async Task<R> Match<R>(Func<Task<R>> None, Func<T, Task<R>> Some)
        {
            return _isSome ? await Some(_value) : await None();
        }

        public void Match(Action None, Action<T> Some)
        {
            if (_isSome)
            {
                Some(_value);
                return;
            }

            None();
        }

        public async Task Match(Func<Task> None, Func<T, Task> Some)
        {
            if (_isSome)
            {
                await Some(_value);
                return;
            }

            await None();
        }

        public Option<R> Map<R>(Func<T, R> func)
        {
            return _isSome ? F.Some(func(_value)) : F.None;
        }

        public static implicit operator Option<T>(Some<T> some) => new Option<T>(some.Value);
        public static implicit operator Option<T>(None _) => new Option<T>();

        public bool Equals(Option<T> other)
        {
            return _isSome == other._isSome &&
                   (_isNone || _value.Equals(other._value));
        }

        public bool Equals(None none) => _isNone;

        public override bool Equals(object obj)
        {
            return Equals((Option<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<T>.Default.GetHashCode(_value) * 397) ^ _isSome.GetHashCode();
            }
        }


        public static bool operator ==(Option<T> @this, Option<T> other) => @this.Equals(other);
        public static bool operator !=(Option<T> @this, Option<T> other) => !@this.Equals(other);

        public override string ToString() => _isSome ? $"Some({_value})" : "None";

    }
}
