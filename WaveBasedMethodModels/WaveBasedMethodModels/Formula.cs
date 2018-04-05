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
        public static Complex ComputeAcousticWaveNumber(Room room, double f, double c)
        {
            double ka = 2 * Math.PI * f / c;
            if (room.IsSource || room.IsReceiving)
            {
                return ka * new Complex(1.0, -2.2 / (2 * f * room.T));
            }
            else return new Complex(ka, 0.0);
        }

        /// <summary>
        /// Computes the b.
        /// Formula 2.31
        /// </summary>
        /// <returns>The b.</returns>
        /// <param name="E">E.</param>
        /// <param name="h">The height.</param>
        /// <param name="v">V.</param>
        public static double ComputePlateBendingStiffness(double E, double h, double v)
        {
            return E * Math.Pow(h, 3.0) / (12 - 12 * Math.Pow(v, 2.0));
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
            Complex ka = ComputeAcousticWaveNumber(room, f, c);
            Complex[][] kz = ComplexMatrix.Create2D(M, N);
            for (int m = 0; m < M; m++) 
            {
                for (int n = 0; n < N; n++)
                {
                    kz[m][n] = Complex.Sqrt(Complex.Pow(ka, 2.0) 
                                       - Complex.Pow(m * Math.PI / room.L_x, 2.0) 
                                       - Complex.Pow(n * Math.PI / room.L_y, 2.0));
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

        /// <summary>
        /// Computes the F.
        /// Formula 2.74
        /// </summary>
        /// <returns>The f.</returns>
        /// <param name="room">Room.</param>
        /// <param name="M">M.</param>
        /// <param name="N">N.</param>
        public static Complex[][] ComputeF(Room room, int M, int N)
        {
            Complex[][] result = ComplexMatrix.Create2D(M, N);
            if (room.IsSource)
            {
                for (int m = 0; m < M; m++)
                {
                    for (int n = 0; n < N; n++)
                    {
                        result[m][n] = ComputePhiRoom(room, m, n, room.X_s, room.Y_s);
                    }
                }
            }
            else throw new Exception("F does not apply to non-source room.");
            return result;
        }

        /// <summary>
        /// Computes the c1.
        /// Formula 2.84
        /// </summary>
        /// <returns>The c1.</returns>
        /// <param name="room">Room.</param>
        /// <param name="f">F.</param>
        /// <param name="c">C.</param>
        /// <param name="M">M.</param>
        /// <param name="N">N.</param>
        public static Complex[][] ComputeC1(Room room, double f, double c, int M, int N)
        {
            return ComputeC1OrC2(room, f, c, M, N, -1.0);
        }

        /// <summary>
        /// Computes the c2.
        /// Formula 2.85
        /// </summary>
        /// <returns>The c2.</returns>
        /// <param name="room">Room.</param>
        /// <param name="f">F.</param>
        /// <param name="c">C.</param>
        /// <param name="M">M.</param>
        /// <param name="N">N.</param>
        public static Complex[][] ComputeC2(Room room, double f, double c, int M, int N)
        {
            return ComputeC1OrC2(room, f, c, M, N, 1.0);
        }

        // Helper
        private static Complex[][] ComputeC1OrC2(Room room, double f, double c, int M, int N, double sign)
        {
            var result = ComplexMatrix.Create2D(M, N);
            var kz = ComputeAcousticWaveExpansion(room, f, c, M, N);
            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    Complex t = sign * kz[m][n] * room.L_z;
                    result[m][n] = Complex.Cos(t)
                                       + Complex.ImaginaryOne * Complex.Sin(t);
                }
            }
            return result;
        }

        /// <summary>
        /// Computes the norms of room wave functions.
        /// Formula 2.86
        /// </summary>
        /// <returns>The norms of room wave functions.</returns>
        /// <param name="room">Room.</param>
        /// <param name="M">M.</param>
        /// <param name="N">N.</param>
        public static Complex[][] ComputeNormsOfRoomWaveFunctions(Room room, int M, int N)
        {
            var result = ComplexMatrix.Create2D(M, N);
            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    result[m][n] = GaussLegendreRule.Integrate((x, y) =>
                    {
                        return Math.Pow(ComputePhiRoom(room, m, n, x, y), 2.0);
                    }, 0.0, room.L_x, 0.0, room.L_y, 5);
                }
            }
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
                    for (int p = 1; p <= P; p++)
                    {
                        for (int q = 1; q <= Q; q++)
                        {
                            result[m][n][p - 1][q - 1] = GaussLegendreRule.Integrate((x, y) => 
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

        /// <summary>
        /// Computes the eigenfrequencies.
        /// Formula 2.91
        /// </summary>
        /// <returns>The eigenfrequencies.</returns>
        /// <param name="plate">Plate.</param>
        /// <param name="P">P.</param>
        /// <param name="Q">Q.</param>
        public static Complex[][] ComputeSimplySupportedPlateEigenfrequencies(Plate plate, int P, int Q)
        {
            Complex[][] wp = ComplexMatrix.Create2D(P, Q);
            for (int p = 1; p <= P; p++)
            {
                for (int q = 1; q <= Q; q++)
                {
                    double multiplier = Math.Sqrt((Math.Pow(Math.PI, 4) * plate.B) / (plate.H * plate.Rho));
                    wp[p - 1][q - 1] = multiplier * (Math.Pow(Math.Pow(p / plate.L_x, 2) + Math.Pow(q / plate.L_y, 2), 2));
                }
            }
            return wp;
        }

        /// <summary>
        /// Computes the C3.
        /// Formula 2.96 and 2.98
        /// </summary>
        /// <returns>The c3.</returns>
        /// <param name="room">Room.</param>
        /// <param name="f">F.</param>
        /// <param name="c">C.</param>
        /// <param name="M">M.</param>
        /// <param name="N">N.</param>
        public static Complex[][] ComputeC3(Room room, double f, double c, double rhoAir, int M, int N)
        {
            Complex[][] result = ComplexMatrix.Create2D(M, N);
            Complex[][] kz = ComputeAcousticWaveExpansion(room, f, c, M, N);
            if (room.IsSource)
            {
                // Formula 2.96
                Complex[][] N1 = ComputeNormsOfRoomWaveFunctions(room, M, N);
                Complex[][] F = ComputeF(room, M, N);
                for (int m = 0; m < M; m++)
                {
                    for (int n = 0; n < N; n++)
                    {
                        Complex t = kz[m][n] * Math.Abs(room.Z_s - room.L_z);
                        Complex firstTerm = Complex.Cos(t) + Complex.ImaginaryOne * Complex.Sin(t);
                        Complex secondTerm = 2 * Math.PI * f * rhoAir / (kz[m][n] * N1[m][n]);
                        result[m][n] = 0.5 * firstTerm * secondTerm * F[m][n];
                    }
                }
            }
            else if (room.IsReceiving)
            {
                throw new Exception("Receiving room does not need C3");
            }
            else
            {
                // Formula 2.98
                Complex[][] Ni = ComputeNormsOfRoomWaveFunctions(room, M, N);
                for (int m = 0; m < M; m++)
                {
                    for (int n = 0; n < N; n++)
                    {
                        Complex firstTerm = -1.0 * Complex.Pow(2 * Math.PI * f, 2.0) / (kz[m][n] * Ni[m][n]);
                        Complex secondTerm = 1.0 / Complex.Sin(kz[m][n] * room.L_z);
                        result[m][n] = firstTerm * secondTerm;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Computes the norms of the plate modes.
        /// Formula 2.102
        /// </summary>
        /// <returns>The norms of the plate modes.</returns>
        /// <param name="room">Room.</param>
        /// <param name="M">M.</param>
        /// <param name="N">N.</param>
        public static Complex[][] ComputeNormsOfThePlateModes(Plate plate, int P, int Q,
                                                              Func<Plate, int, int, double, double, double> func)
        {
            var result = ComplexMatrix.Create2D(P, Q);
            for (int p = 1; p <= P; p++)
            {
                for (int q = 1; q <= Q; q++)
                {
                    result[p - 1][q - 1] = GaussLegendreRule.Integrate((x, y) =>
                    {
                        return Math.Pow(func(plate, p, q, x, y), 2.0);
                    }, 0.0, plate.L_x, 0.0, plate.L_y, 5);
                }
            }
            return result;
        }
    }
}
