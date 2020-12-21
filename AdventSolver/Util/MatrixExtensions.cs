namespace AdventSolver.Util
{
    public static class MatrixExtensions
    {
        public static T[,] RotateCw<T>(this T[,] value)
        {
            var toReturn = new T[value.GetLength(1), value.GetLength(0)];

            for (int i = 0; i < toReturn.GetLength(0); ++i)
            for (int j = 0; j < toReturn.GetLength(1); ++j)
                toReturn[i, j] = value[value.GetLength(0) - j - 1, i];

            return toReturn;
        }

        public static T[,] FlipY<T>(this T[,] value)
        {
            int dim0Length = value.GetLength(0);
            int dim1Length = value.GetLength(1);
            var toReturn = new T[dim0Length, dim1Length];

            for (int i = 0; i < dim0Length; ++i)
            for (int j = 0; j < dim1Length; ++j)
                toReturn[i, j] = value[dim0Length - i - 1, j];

            return toReturn;
        }

        public static T[,] FlipX<T>(this T[,] value)
        {
            int dim0Length = value.GetLength(0);
            int dim1Length = value.GetLength(1);
            var toReturn = new T[dim0Length, dim1Length];

            for (int i = 0; i < dim0Length; ++i)
            for (int j = 0; j < dim1Length; ++j)
                toReturn[i, j] = value[i, dim1Length - j - 1];

            return toReturn;
        }
    }
}