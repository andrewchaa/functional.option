using System.Threading.Tasks;
using Functional.Option;


namespace Functional
{
    public static partial class F
    {
        public static Option<T> Some<T>(T value) => new Some<T>(value);

        public static None None => None.Default;
    }
}
