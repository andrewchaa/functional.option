using System.Threading.Tasks;
using Unit = System.ValueTuple;


namespace Functional.Option
{
    public static class F
    {
        // Options
        public static Option<T> Some<T>(T value) => new Some<T>(value);
        public static None None { get; set; }

        public static Unit Unit() => default(Unit);
        public static async Task<Unit> UnitAsync() => default(Unit);
    }
}
