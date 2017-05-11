using System;

namespace Citrine.Core
{
    public static class Alg
    {
        public static T Let<T>(Func<T> get) => get();

        public static U Let<T, U>(this T original, Func<T, U> transformed) => transformed(original);

        public static void Let<T>(this T original, Action<T> run) => run(original);

        public static U Let<I0, I1, U>(this Tuple<I0, I1> original, Func<I0, I1, U> transformed) => transformed(original.Item1, original.Item2);

        public static void Let<I0, I1>(this Tuple<I0, I1> original, Action<I0, I1> run) => run(original.Item1, original.Item2);

        public static U Let<I0, I1, I2, U>(this Tuple<I0, I1, I2> original, Func<I0, I1, I2, U> transformed) => transformed(original.Item1, original.Item2, original.Item3);

        public static void Let<I0, I1, I2>(this Tuple<I0, I1, I2> original, Action<I0, I1, I2> run) => run(original.Item1, original.Item2, original.Item3);
    }
}
