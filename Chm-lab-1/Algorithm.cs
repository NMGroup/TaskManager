using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Instrumentation;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Chm_lab_1
{
    class Algorithm
    {
        public static Matrix yakobiSpinAlgorithm(Matrix matrix, out int timeCount, out int iterationCount)
        {
            
            timeCount = 0;
            iterationCount = 0;

            int width = matrix.M;
            int height = matrix.N;

            double eps = 0.00000000001;
            double fi = 0, ownNumber = 1;
            int maxI = 0, maxJ = 0;
            Matrix U = new Matrix(height, width);
            Matrix Ut = new Matrix(height, width);
            Matrix temp = new Matrix(height, width);
            Matrix matrixK = new Matrix(height, width);
            Matrix ownNumberMatrix = new Matrix(height,1);

            double max;

            long time = Environment.TickCount;

            while (Math.Abs(ownNumber) > eps)
            {
                max = -1;

                for (int i = 0; i < height; ++i)
                {
                    for (int j = 0; j < width; ++j)
                    {
                        if ((i != j) && (Math.Abs(matrix.getValueAt(i, j)) > max))
                        {
                            max = Math.Abs(matrix.getValueAt(i, j));
                            maxI = i; 
                            maxJ = j;
                        }
                        max = Math.Abs(matrix.getValueAt(maxI, maxJ));
                    }
                }
                max = matrix.getValueAt(maxI, maxJ);
                /*Console.WriteLine(max);*/
                
                if ((matrix.getValueAt(maxI, maxI) != matrix.getValueAt(maxJ, maxJ))) 
                {
                    fi = Math.Atan(2 * max / (matrix.getValueAt(maxI, maxI) - matrix.getValueAt(maxJ, maxJ))) / 2;
                }
                else
                {
                    fi = Math.PI / 4;
                }


                for (int i = 0; i < height; ++i)
                {
                    for (int j = 0; j < width; ++j)
                    {
                        if (i != j)
                        {
                            U.setValueAt(i, j, 0);
                            Ut.setValueAt(i, j, 0);
                        }
                        else
                        {
                            U.setValueAt(i, j, 1);
                            Ut.setValueAt(i, j, 1);
                        }
                    }
                }

                U.setValueAt(maxI, maxI, Math.Cos(fi)); U.setValueAt(maxI, maxJ, -(Math.Sin(fi)));
                U.setValueAt(maxJ, maxI, Math.Sin(fi)); U.setValueAt(maxJ, maxJ, Math.Cos(fi));
                Ut = U.transparent();

                temp = Matrix.multiply(Ut, matrix);
                matrixK = Matrix.multiply(temp, U);

                ownNumber = 0;
                for (int i = 0; i < height; ++i)
                {
                    for (int j = 0; j < width; ++j)
                    {
                        matrix.setValueAt(i, j, matrixK.getValueAt(i, j));
                        if (i != j)
                        {
                            ownNumber += matrixK.getValueAt(i, j) * matrixK.getValueAt(i, j);
                        }
                    }
                }
                ++iterationCount;
                /*Console.WriteLine("Iteration {0} \n\n", iterationCount);
                for (int i = 0; i < height; ++i)
                {
                    for (int j = 0; j < width; ++j)
                    {
                        Console.Write("{0} ", matrixK.getValueAt(i, j));
                    }
                    Console.WriteLine("\n");
                }
                Console.Write("\n\nOwn number {0} ", ownNumber);*/
            }


            System.Threading.Thread.Sleep(0);
            time -= Environment.TickCount;
            timeCount = (int)time;
            //Console.WriteLine(timeCount);
            //Console.WriteLine("\n");
            for (int i = 0; i < height; i++)
            {
                /*Console.WriteLine("L{0} = {1}\n", i + 1, matrixK.getValueAt(i, i));*/
                ownNumberMatrix.setValueAt(i, 0, matrixK.getValueAt(i, i));
            }
            return ownNumberMatrix;
        }

        public static Matrix QRAlgorithm(Matrix matrix, out int timeCount, out int iterationCount)
        {
            timeCount = 0;
            iterationCount = 0;

            int width = matrix.M;
            int height = matrix.N;
            double eps = 0.00000000001;
            int k = 0;
            int k1 = 0;//simply count number
            int widthCount = width;
            int heightCount = height;
            int outOfWhile = height + 6;
            bool readyToWork = true;

            Matrix ownNumberColumn = new Matrix(height,1);

            Matrix vectorNu = new Matrix(height, 1);

            Matrix HouseholderMatrix = new Matrix(height, width);

            Matrix secondMatrixInSumm = new Matrix(height, width);

            Matrix tempUP = new Matrix(height, width);

            Matrix tempDOWN = new Matrix(1, 1);

            Matrix elementaryMatrix = new Matrix(height, width);

            Matrix Q = new Matrix(height,width);
            Q.FillOnesDiag();

            int columnCount = 0;

            double columnSumm;

            long time = Environment.TickCount;

            while (columnCount < width - 1)
            {
                columnSumm = 0;

                vectorNu = new Matrix(height, 1);

                HouseholderMatrix = new Matrix(height, width);

                secondMatrixInSumm = new Matrix(height, width);

                tempUP = new Matrix(height, width);

                elementaryMatrix = new Matrix(height, width);
                elementaryMatrix.FillOnesDiag();

                for (int i = columnCount; i < height; ++i)
                {
                    columnSumm += matrix.getValueAt(i, columnCount)*matrix.getValueAt(i, columnCount);
                }

                for (int i = 0; i < height; ++i)
                {
                    if (i < columnCount)
                    {
                        vectorNu.setValueAt(i, 0, 0);
                    }
                    else if (i == columnCount)
                    {
                        vectorNu.setValueAt(i, 0,
                            matrix.getValueAt(i, columnCount) +
                            Math.Sign(matrix.getValueAt(i, columnCount))*Math.Pow(columnSumm, 0.5));
                    }
                    else
                    {
                        vectorNu.setValueAt(i, 0, matrix.getValueAt(i, columnCount));
                    }
                }
                /*Console.WriteLine(vectorNu);*/

                tempUP = Matrix.multiply(vectorNu, vectorNu.transparent());
                /*Console.WriteLine(tempUP);*/
                tempDOWN = Matrix.multiply(vectorNu.transparent(), vectorNu);
                /*Console.WriteLine(tempDOWN);*/
                for (int i = 0; i < height; ++i)
                {
                    for (int j = 0; j < width; ++j)
                    {
                        secondMatrixInSumm.setValueAt(i, j, (-2)*tempUP.getValueAt(i, j)/tempDOWN.getValueAt(0, 0));
                    }
                }
                //Console.WriteLine(secondMatrixInSumm);
                HouseholderMatrix = elementaryMatrix + secondMatrixInSumm;
                //Console.WriteLine(HouseholderMatrix);
                matrix = Matrix.multiply(HouseholderMatrix, matrix);
                /*Console.WriteLine(Q + "qwewq");*/
                Q = Matrix.multiply(Q, HouseholderMatrix);
                /*Console.WriteLine(Q);*/
                ++columnCount;
                if (columnCount == (width - 1))
                {
                    /*Console.WriteLine(matrix+" kk");*/
                    matrix = Matrix.multiply(matrix, Q);
                    /* Console.WriteLine(matrix);
                    Console.ReadLine();
                    Console.WriteLine(width + " " + height);*/
                    if ((width == 2) && (height == 2))
                    {
                        double D;
                        D = Math.Pow((-(matrix.getValueAt(0, 0) + matrix.getValueAt(1, 1))), 2) -
                            4*
                            (matrix.getValueAt(0, 0)*matrix.getValueAt(1, 1) -
                             matrix.getValueAt(0, 1)*matrix.getValueAt(1, 0));
                        /*Console.WriteLine(D);*/
                        if (D < 0)
                        {
                            Complex lyambdaN;
                            Complex lyambdaNMinusOne;
                            lyambdaN = new Complex(
                                -(-(matrix.getValueAt(0, 0) + matrix.getValueAt(1, 1)))/2,
                                (Math.Sqrt(Math.Abs(D)))/(2));
                            lyambdaNMinusOne =
                                new Complex(-(-(matrix.getValueAt(0, 0) + matrix.getValueAt(1, 1)))/2,
                                    -(Math.Sqrt(Math.Abs(D)))/(2));
                            //Console.WriteLine("x1= {0}\n x2= {1}", lyambdaN, lyambdaNMinusOne);
                            //Console.WriteLine(ownNumberColumn);
                            columnCount = outOfWhile;
                            //ownNumberColumn.setValueAt(height-2, width-2, (double)1+"i");
                            /*Console.ReadLine();*/
                            break;
                        }
                        else if (D >= 0)
                        {
                            //Console.WriteLine(ownNumberColumn.N + " " + ownNumberColumn.M);
                            ownNumberColumn.setValueAt(ownNumberColumn.N - 2, 0,
                                (((-(-matrix.getValueAt(0, 0) - matrix.getValueAt(1, 1) + Math.Sqrt(D))/2))));
                            ownNumberColumn.setValueAt(ownNumberColumn.N - 1, 0,
                                (((-(-matrix.getValueAt(0, 0) - matrix.getValueAt(1, 1) - Math.Sqrt(D))/2))));
                            columnCount = outOfWhile;
                            break;
                        }

                    }
                    else
                    {
                        /*Console.WriteLine(matrix+"jjjjj");*/
                        for (int i = 0; i < widthCount - 1; ++i)
                        {
                            if (!readyToWork)
                            {
                                break;
                            }
                            for (int j = i + 1; j < heightCount; ++j)
                            {
                                /*Console.WriteLine("\n"+i+" "+j+"\n");
                                Console.WriteLine(matrix.getValueAt(j, i));*/
                                if (Math.Abs(matrix.getValueAt(j, i)) < eps)
                                {
                                    ++k;
                                    if (k == (heightCount - i - 1))
                                    {
                                        ownNumberColumn.setValueAt(k1, 0, matrix.getValueAt(i, i));
                                        //Console.WriteLine(i+"\n"+matrix);
                                        matrix = matrix.createSubMatrix(i, i);
                                        ++k1;
                                        if (matrix.N == 1)
                                        {
                                            ownNumberColumn.setValueAt(k1, 0, matrix.getValueAt(0, 0));
                                        }
                                        --heightCount;
                                        --widthCount;
                                        i = 0;
                                        j = 1;
                                        width = matrix.M;
                                        height = matrix.N;
                                        k = 0;
                                        readyToWork = false;
                                        break;
                                    }
                                }
                                
                            }
                            k = 0;

                        }

                    }
                    if (columnCount != outOfWhile)
                    {
                        /*Console.ReadLine();*/
                        readyToWork = true;
                        ++iterationCount;
                        Q = new Matrix(height, width);
                        Q.FillOnesDiag();
                        columnCount = 0;
                    }
                    
                    /*Console.ReadLine();*/
                }
            }

            time -= Environment.TickCount;
            timeCount = (int) time;
            //Console.WriteLine(timeCount);
            
            return ownNumberColumn;
        }

        public static Matrix withCHeboshyvskySetOfParametrAlgorithm(Matrix inpA, Matrix inpb, out int timeCount, out int iterationCount)
        {
            var sw = Stopwatch.StartNew();
            timeCount = 0;
            iterationCount = 0;

            Matrix matrixA = inpA.CreateCopy();
            Matrix vectorb = inpb.CreateCopy();

            Matrix lyambdaVector = QRAlgorithm(inpA, out timeCount, out iterationCount);
            double maxLyambda = -1;
            double minLyambda = 1;
            double Tk;
            int psevdoCount = 0;
            double eps;
            double rou0;
            double rou1;
            double n;
            double tk;

            for (int i = 0; i < lyambdaVector.N; ++i)
            {
                if (maxLyambda < lyambdaVector.getValueAt(i,0))
                {
                    maxLyambda = lyambdaVector.getValueAt(i, 0);
                }
                if (minLyambda > lyambdaVector.getValueAt(i,0))
                {
                    minLyambda = lyambdaVector.getValueAt(i, 0);
                }
            }

            double t0 = 2/(minLyambda + maxLyambda);
            eps = minLyambda/maxLyambda;
            rou0 = (1 - eps)/(1 + eps);
            rou1 = (1 - Math.Pow(eps, 0.5))/(1 + Math.Pow(eps, 0.5));
            //тут наинается скорее всего итерационный процесс

            ++psevdoCount;

            n = Math.Abs(Math.Log(2/eps, Math.E)/Math.Log(1/rou1, Math.E));
            Tk = -Math.Cos((2*psevdoCount - 1)*Math.PI/(2*n));
            tk = t0/(1 + rou0*Tk);

            matrixA = Matrix.multiply(matrixA.transparent(), matrixA);
            vectorb = Matrix.multiply(matrixA.transparent(), vectorb);
            Matrix vectorX = vectorb.CreateCopy();
            



            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalMilliseconds);
            return vectorX;
        }

        public static void CompareAlgorithms()
        {
            
        }
    }
}
