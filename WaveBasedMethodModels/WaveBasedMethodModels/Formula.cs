using System;
using System.Numerics;
using MathNet.Numerics.Integration;

namespace WaveBasedMethodModel
{
    public static class Formula
    {
        /// <summary>
        /// Computes the acoustic wave number. 
        /// Formula 2.26
        /// </summary>
        /// <returns>The acoustic wave number.</returns>
        /// <param name="room">Room.</param>
        /// <param name="f">F.</param>
        /// <param name="c">C.</param>
        /// TODO find out if it is ok to ignore damping?
        public static double ComputeAcousticWaveNumber(Room room, double f, double c)
        {
            return 2 * Math.PI * f / c;
        }

        /// <summary>
        /// Computes the acoustic wave expansion.
        /// Formula 2.36
        /// </summary>
        /// <returns>The acoustic wave expansion.</returns>
        /// <param name="room">Room.</param>
        /// <param name="f">F.</param>
        /// <param name="c">C.</param>
        /// <param name="M">M.</param>
        /// <param name="N">N.</param>
        public static Complex[][] ComputeAcousticWaveExpansion(Room room, double f, double c, int M, int N)
        {
            double ka = ComputeAcousticWaveNumber(room, f, c);
            Complex[][] kz = ComplexMatrix.Create2D(M, N);
            for (int m = 0; m < M; m++) 
            {
                for (int n = 0; n < N; n++)
                {
                    kz[m][n] = Math.Sqrt(Math.Pow(ka, 2.0) 
                                         - Math.Pow(m * Math.PI / room.L_x, 2.0) 
                                         - Math.Pow(n * Math.PI / room.L_y, 2.0));
                }
            }
            return kz;
        }

        /// <summary>
        /// Computes the phi room.
        /// Formula 2.35
        /// </summary>
        /// <returns>The phi room.</returns>
        /// <param name="room">Room.</param>
        /// <param name="m">M.</param>
        /// <param name="n">N.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public static double ComputePhiRoom(Room room, int m, int n, double x, double y)
        {
            Func<double, int, double, double> beamFunc = (lx, mm, xx) =>
            {
                return Math.Cos(mm * Math.PI * xx / lx);
            };
            return beamFunc(room.L_x, m, x) * beamFunc(room.L_y, n, y);
        }

        /// <summary>
        /// Computes the eigenfrequencies.
        /// Formula 2.91
        /// </summary>
        /// <returns>The eigenfrequencies.</returns>
        /// <param name="plate">Plate.</param>
        /// <param name="P">P.</param>
        /// <param name="Q">Q.</param>
        public static Complex[][] ComputeEigenfrequencies(Plate plate, int P, int Q)
        {
            Complex[][] wp = ComplexMatrix.Create2D(P, Q);
            // TO BE IMPLEMENTED
            return wp;
        }

        /// <summary>
        /// Computes the phi: simply supported.
        /// Formula 2.55
        /// </summary>
        /// <returns>The phi symply supported.</returns>
        /// <param name="plate">Plate.</param>
        /// <param name="p">P.</param>
        /// <param name="q">Q.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public static double ComputePhiPlateSimplySupported(Plate plate, int p, int q, double x, double y)
        {
            Func<double, int, double, double> beamFunc = (lx, pp, xx) => 
            {
                return Math.Sin(pp * Math.PI * xx / lx);
            };
            return beamFunc(plate.L_x, p, x) * beamFunc(plate.L_y, q, y);
        }

        /// <summary>
        /// Computes the phi clamped.
        /// Formula 2.56
        /// </summary>
        /// <returns>The phi clamped.</returns>
        /// <param name="plate">Plate.</param>
        /// <param name="p">P.</param>
        /// <param name="q">Q.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public static double ComputePhiPlateClamped(Plate plate, int p, int q, double x, double y)
        {
            // TBD
            Func<double, int, double, double> beamFunc = (lx, pp, xx) =>
            {
                return 0.0;
            };
            return 0.0;
        }

        /// <summary>
        /// Computes the phi free.
        /// Formula 2.62 and 2.63
        /// </summary>
        /// <returns>The phi free.</returns>
        /// <param name="plate">Plate.</param>
        /// <param name="p">P.</param>
        /// <param name="q">Q.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public static double ComputePhiPlateFree(Plate plate, int p, int q, double x, double y)
        {
            // TBD
            Func<double, int, double, double> beamFunc = (lx, pp, xx) =>
            {
                return 0.0;
            };
            return 0.0;
        }

        public static Complex[][] ComputeNormsOfRoomWaveFunctions(Room room, int M, int N)
        {
            var result = ComplexMatrix.Create2D(M, N);
            // TBD
            return result;
        }

        /// <summary>
        /// Computes the plate room projection coefficients.
        /// Formula 2.87
        /// </summary>
        /// <returns>The plate room projection coefficients.</returns>
        /// <param name="plate">Plate.</param>
        /// <param name="room">Room.</param>
        /// <param name="M">M.</param>
        /// <param name="N">N.</param>
        /// <param name="P">P.</param>
        /// <param name="Q">Q.</param>
        public static Complex[][][][] ComputePlateRoomProjectionCoefficients(Plate plate, Room room, int M, int N, int P, int Q)
        {
            var result = ComplexMatrix.Create4D(M, N, P, Q);
            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    for (int p = 0; p < P; p++)
                    {
                        for (int q = 0; q < Q; q++)
                        {
                            result[m][n][p][q] = GaussLegendreRule.Integrate((x, y) => 
                            {
                                return ComputePhiRoom(room, m, n, x + plate.DeltaX, y + plate.DeltaY) 
                                    * ComputePhiPlateSimplySupported(plate, p, q, x, y);
                            }, 0.0, plate.L_x, 0.0, plate.L_y, 5);
                        }
                    }
                }
            }
            return result;
        }


    }
}
