using System;
using WaveBasedMethodModel;

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
                L_z = 4.15,
                X_s = 2.0,
                Y_s = 1.5,
                Z_s = 1.5,
                T = 2.68
            };
            var room2 = new Room()
            {
                L_x = 5.09,
                L_y = 4.12,
                L_z = 4.15,
                T = 2.68
            };
            var room3 = new Room()
            {
                IsReceiving = true,
                L_x = 5.09,
                L_y = 4.12,
                L_z = 4.15,
                T = 2.68
            };
            input.Add(room1);
            input.Add(room2);
            input.Add(room3);
            var plate = new Plate()
            {
                L_x = 3.25,
                L_y = 2.95,
                DeltaX = 0.5,
                DeltaY = 0.5,
                Rho = 2500,
                H = 0.01,
                B = Formula.ComputePlateBendingStiffness(62 * 10e9, 0.01, 0.24)
            };
            input.Add(plate);
            Console.WriteLine(input);

            // constants
            double f = 400;
            double c = 340;
            int M = 5;
            int N = 5;
            int P = 4;
            int Q = 4;
            double rhoAir = 1.217;

            // 2.36
            Console.WriteLine("Formula 2.36: acoustic wave expansion: ");

            var kz1 = Formula.ComputeAcousticWaveExpansion(room1, f, c, M, N);
            Console.WriteLine("Formula 2.36 - kz1");
            Console.WriteLine(ComplexMatrix.MatrixAsString(kz1));

            var kz2 = Formula.ComputeAcousticWaveExpansion(room2, f, c, M, N);
            Console.WriteLine("Formula 2.36 - kz2");
            Console.WriteLine(ComplexMatrix.MatrixAsString(kz2));

            var kz3 = Formula.ComputeAcousticWaveExpansion(room3, f, c, M, N);
            Console.WriteLine("Formula 2.36 - kz3");
            Console.WriteLine(ComplexMatrix.MatrixAsString(kz3));

            // 2.74
            var F = Formula.ComputeF(room1, M, N);
            Console.WriteLine("Formula 2.74 - F");
            Console.WriteLine(ComplexMatrix.MatrixAsString(F));

            // 2.84
            var C11 = Formula.ComputeC1(room1, f, c, M, N);
            Console.WriteLine("Formula 2.84 - C11");
            Console.WriteLine(ComplexMatrix.MatrixAsString(C11));

            // 2.85
            var C12 = Formula.ComputeC2(room1, f, c, M, N);
            Console.WriteLine("Formula 2.85 - C12");
            Console.WriteLine(ComplexMatrix.MatrixAsString(C12));

            // 2.86
            var N1 = Formula.ComputeNormsOfRoomWaveFunctions(room1, M, N);
            Console.WriteLine("Formula 2.86 - N1");
            Console.WriteLine(ComplexMatrix.MatrixAsString(N1));

            var N2 = Formula.ComputeNormsOfRoomWaveFunctions(room2, M, N);
            Console.WriteLine("Formula 2.86 - N2");
            Console.WriteLine(ComplexMatrix.MatrixAsString(N2));

            var N3 = Formula.ComputeNormsOfRoomWaveFunctions(room3, M, N);
            Console.WriteLine("Formula 2.86 - N3");
            Console.WriteLine(ComplexMatrix.MatrixAsString(N3));

            // 2.87
            var Prp = Formula.ComputePlateRoomProjectionCoefficients(plate, room1, M, N, P, Q);
            Console.WriteLine("Formula 2.87: plate room projection coefficients: ");
            Console.WriteLine(ComplexMatrix.MatrixAsString(Prp[0][0]));

            // 2.96
            var C13 = Formula.ComputeC3(room1, f, c, rhoAir, M, N);
            Console.WriteLine("Formula 2.96 - C13");
            Console.WriteLine(ComplexMatrix.MatrixAsString(C13));

            // 2.98
            var C23 = Formula.ComputeC3(room2, f, c, rhoAir, M, N);
            Console.WriteLine("Formula 2.98 - C23");
            Console.WriteLine(ComplexMatrix.MatrixAsString(C23));
        }
    }
}
