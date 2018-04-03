using System;
using System.Numerics;

namespace WaveBasedMethodModel
{
    public class ComplexMatrix
    {
        public static Complex[][] Create2D(int X, int Y)
        {
            Complex[][] result = new Complex[X][];
            for (int i = 0; i < X; ++i)
                result[i] = new Complex[Y];
            return result;
        }

        public static Complex[][][] Create3D(int X, int Y, int Z)
        {
            Complex[][][] result = new Complex[X][][];
            for (int i = 0; i < X; ++i)
                result[i] = Create2D(Y, Z);
            return result;
        }

        public static Complex[][][][] Create4D(int X1, int X2, int X3, int X4) {
            Complex[][][][] result = new Complex[X1][][][];
            for (int i = 0; i < X1; ++i)
                result[i] = Create3D(X2, X3, X4);
            return result;
        }

        /// <summary>
        /// Copy the specified matrix.
        /// </summary>
        /// <returns>The copy.</returns>
        /// <param name="matrix">Matrix.</param>
        public static Complex[][] Copy(Complex[][] matrix)
        {
            int n = matrix.Length;
            Complex[][] result = Create2D(n, n);
            for (int i = 0; i < n; ++i)
                for (int j = 0; j < n; ++j)
                    result[i][j] = matrix[i][j];
            return result;
        }

        /// <summary>
        /// Matrix as string.
        /// </summary>
        /// <returns>The as string.</returns>
        /// <param name="matrix">Matrix.</param>
        public static string MatrixAsString(Complex[][] matrix)
        {
            string s = "";
            for (int i = 0; i < matrix.Length; ++i)
            {
                for (int j = 0; j < matrix[i].Length; ++j)
                    s += matrix[i][j].ToString("F3").PadLeft(8) + " ";
                s += Environment.NewLine;
            }
            return s;
        }

        /// <summary>
        /// 2D matrix multiplucation.
        /// </summary>
        /// <returns>The product.</returns>
        /// <param name="matrixA">Matrix a.</param>
        /// <param name="matrixB">Matrix b.</param>
        public static Complex[][] MatrixProduct(Complex[][] matrixA,
                                                Complex[][] matrixB)
        {
            int aRows = matrixA.Length;
            int aCols = matrixA[0].Length;
            int bRows = matrixB.Length;
            int bCols = matrixB[0].Length;
            if (aCols != bRows)
                throw new Exception("Non-conformable matrices");

            Complex[][] result = Create2D(aRows, bCols);
            for (int i = 0; i < aRows; ++i) // each row of A
                for (int j = 0; j < bCols; ++j) // each col of B
                    for (int k = 0; k < aCols; ++k) // could use k < bRows
                        result[i][j] += matrixA[i][k] * matrixB[k][j];

            return result;
        }

        /// <summary>
        /// Matrixs the product.
        /// </summary>
        /// <returns>The product.</returns>
        /// <param name="matrixA">X1 x X2.</param>
        /// <param name="matrixB">Y1 x Y2 x Y3 x Y4.</param>
        public static Complex[][] MatrixProduct(Complex[][] matrixA,
                                                Complex[][][][] matrixB)
        {
            int X1 = matrixA.Length;
            int X2 = matrixA[0].Length;
            int Y1 = matrixB.Length;
            int Y2 = matrixB[0].Length;
            if (X1 != Y1 || X2 != Y2)
                throw new Exception("Non-conformable matrices");

            int Y3 = matrixB[0][0].Length;
            int Y4 = matrixB[0][0][0].Length;
            Complex[][] result = Create2D(Y3, Y4);
            for (int y1 = 0; y1 < Y1; y1++)
            {
                for (int y2 = 0; y2 < Y2; y2++)
                {
                    for (int y3 = 0; y3 < Y3; y3++)
                    {
                        for (int y4 = 0; y4 < Y3; y4++)
                        {
                            result[y3][y4] += matrixA[y1][y2] * matrixB[y1][y2][y3][y4];
                        }
                    }
                }
            }
            return result;
        }
    }
}
