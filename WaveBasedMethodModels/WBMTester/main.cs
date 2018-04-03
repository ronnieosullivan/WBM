using System;
using WaveBasedMethodModel;
//using NLapack;
//using NLapack.Matrices;
using System.Numerics;

namespace WBMTester
{
    class Tester
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing parameters ... ");
            var input = new Input();
            var room1 = new Room()
            {
                IsSource = true,
                L_x = 5.09, 
                L_y = 4.12, 
                L_z = 4.15
            };
            var room2 = new Room()
            {
                IsReceiving = true,
                L_x = 5.09,
                L_y = 4.12,
                L_z = 4.15
            };
            input.Add(room1);
            input.Add(room2);
            var plate = new Plate()
            {
                L_x = 3.25,
                L_y = 2.95,
                DeltaX = 0.5,
                DeltaY = 0.5
            };
            input.Add(plate);
            Console.WriteLine(input);

            // ILNumerics isn't free!
            // ILArray<double> a = ILMath.zeros<double>(3);
            // ILArray<double> A = ILMath.zeros<double>(3, 3);
            //A[0, 0] = 2; A[0, 1] = 5; A[0, 2] = 3;
            //A[1, 0] = 1; A[1, 1] = 5; A[1, 2] = 7;
            //A[2, 0] = 8; A[2, 1] = 2; A[2, 2] = 3;

            //ILArray<double> B = ILMath.zeros<double>(3, 3);

            ////generate upper triangular matrix
            //for (int i = 0; i < 3; i++) {
            //    for (int j = 0; j < 3; j++) {
            //        B[i, j] = i <= j ? i + j + 1 : 0;
            //    }
            //}
            //Console.WriteLine(A);
            //Console.WriteLine(B);

            //var res = ILMath.multiply(A ,B);
            //Console.WriteLine(res);

            //Console.WriteLine("\nBegin matrix inverse using Crout LU decomp demo \n");

            //double[][] m = RealMatrix.MatrixCreate(4, 4);
            //m[0][0] = 3.0; m[0][1] = 7.0; m[0][2] = 2.0; m[0][3] = 5.0;
            //m[1][0] = 1.0; m[1][1] = 8.0; m[1][2] = 4.0; m[1][3] = 2.0;
            //m[2][0] = 2.0; m[2][1] = 1.0; m[2][2] = 9.0; m[2][3] = 3.0;
            //m[3][0] = 5.0; m[3][1] = 4.0; m[3][2] = 7.0; m[3][3] = 1.0;

            //Console.WriteLine("Original matrix m is ");
            //Console.WriteLine(RealMatrix.MatrixAsString(m));

            //double d = RealMatrix.MatrixDeterminant(m);
            //if (Math.Abs(d) < 1.0e-5)
            //    Console.WriteLine("RealMatrix has no inverse");

            //double[][] inv = RealMatrix.Inverse(m);

            //Console.WriteLine("Inverse matrix inv is ");
            //Console.WriteLine(RealMatrix.MatrixAsString(inv));

            //double[][] prod = RealMatrix.MatrixProduct(m, inv);
            //Console.WriteLine("The product of m * inv is ");
            //Console.WriteLine(RealMatrix.MatrixAsString(prod));

            //Console.WriteLine("========== \n");

            //double[][] lum;
            //int[] perm;
            //int toggle = RealMatrix.Decompose(m, out lum, out perm);
            //Console.WriteLine("The combined lower-upper decomposition of m is");
            //Console.WriteLine(RealMatrix.MatrixAsString(lum));

            //double[][] lower = RealMatrix.ExtractLower(lum);
            //double[][] upper = RealMatrix.ExtractUpper(lum);

            //Console.WriteLine("The lower part of LUM is");
            //Console.WriteLine(RealMatrix.MatrixAsString(lower));

            //Console.WriteLine("The upper part of LUM is");
            //Console.WriteLine(RealMatrix.MatrixAsString(upper));

            //Console.WriteLine("The perm[] array is");
            //RealMatrix.ShowVector(perm);

            //double[][] lowTimesUp = RealMatrix.MatrixProduct(lower, upper);
            //Console.WriteLine("The product of lower * upper is ");
            //Console.WriteLine(RealMatrix.MatrixAsString(lowTimesUp));


            //Console.WriteLine("\nEnd matrix inverse demo \n");
            //Console.ReadLine();

            //var a = ComplexMatrix.Create2D(2, 2);
            //a[0][0] = new Complex(1.0, 2.0); a[0][1] = new Complex(2.0 ,3.0);
            //a[1][0] = new Complex(4.0, 5.0); a[1][1] = new Complex(3.0, 4.0);
            //Console.WriteLine(ComplexMatrix.MatrixAsString(a));
            //var b = ComplexMatrix.Create2D(2, 2);
            //b[0][0] = new Complex(1.0, 2.0); b[0][1] = new Complex(2.0, 3.0);
            //b[1][0] = new Complex(4.0, 5.0); b[1][1] = new Complex(3.0, 4.0);
            //Console.WriteLine(ComplexMatrix.MatrixAsString(b));
            //var c = ComplexMatrix.MatrixProduct(a, b);
            //Console.WriteLine(ComplexMatrix.MatrixAsString(c));

            //var d = ComplexMatrix.Create3D(2, 2, 2);
            //d[1][1][1] = new Complex(3, 3);
            //Console.WriteLine(ComplexMatrix.MatrixAsString(d[1]));

            var kz = Formula.ComputeAcousticWaveExpansion(room1, 400, 340, 5, 5);
            Console.WriteLine(ComplexMatrix.MatrixAsString(kz));

            var P = Formula.ComputePlateRoomProjectionCoefficients(plate, room1, 5, 5, 5, 5);
            Console.WriteLine("plate room projection coefficients: ");
            Console.WriteLine(ComplexMatrix.MatrixAsString(P[0][0]));
        }
    }
}
