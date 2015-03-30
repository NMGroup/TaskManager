using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Chm_lab_1
{

    class Check
    {
        
        static void Main(string[] args)
        {
            Matrix m1 = new Matrix(2000,2000);
            m1.fillRandom();
            Matrix m2 = new Matrix(2000, 2000);
            m2.fillRandom();


/*            var sw = Stopwatch.StartNew();
            Matrix mult1 = Matrix.multiply(m1,m2);
            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalMilliseconds);*/

            var sw2 = Stopwatch.StartNew();
            Matrix mult2 = Matrix.ParralelMatrixMultiply(m1, m2);
            sw2.Stop();
            Console.WriteLine(sw2.Elapsed.TotalMilliseconds);

//            ExcelWorker.ExcelCreator(mult1, "mult1");
            ExcelWorker.ExcelCreator(mult2, "mult2");
            Console.ReadLine();

            /*Matrix m1 = new Matrix(10,10);
            m1.fillRandom(50);
            Console.WriteLine(m1);
            Matrix m2 = new Matrix(10,10);
            m2.fillRandom(50);
            Console.WriteLine(m2);
            Console.WriteLine("RESULT:\n");
            Console.WriteLine(Matrix.multiply(m1,m2));*/








            /*-------------- create 10 excel files with matrixs from 3 up to 12 range---------------*/
            /*Matrix m3x3 = new Matrix(3,3);
            m3x3.fillRandom();
            m3x3 = m3x3.makeSimetricMatrix();
            Matrix m4x4 = new Matrix(4, 4);
            m4x4.fillRandom();
            m4x4 = m4x4.makeSimetricMatrix();
            Matrix m5x5 = new Matrix(5, 5);
            m5x5.fillRandom();
            m5x5 = m5x5.makeSimetricMatrix();
            Matrix m6x6 = new Matrix(6, 6);
            m6x6.fillRandom();
            m6x6= m6x6.makeSimetricMatrix();
            Matrix m7x7 = new Matrix(7, 7);
            m7x7.fillRandom();
            m7x7 = m7x7.makeSimetricMatrix();
            Matrix m8x8 = new Matrix(8, 8);
            m8x8.fillRandom();
            m8x8 =m8x8.makeSimetricMatrix();
            Matrix m9x9 = new Matrix(9, 9);
            m9x9.fillRandom();
            m9x9 =m9x9.makeSimetricMatrix();
            Matrix m10x10 = new Matrix(10, 10);
            m10x10.fillRandom();
            m10x10 = m10x10.makeSimetricMatrix();
            Matrix m11x11 = new Matrix(11, 11);
            m11x11.fillRandom();
            m11x11 = m11x11.makeSimetricMatrix();
            Matrix m12x12 = new Matrix(12, 12);
            m12x12.fillRandom();
            m12x12 = m12x12.makeSimetricMatrix();
            
            for (int i = 3; i < 13; ++i)
            {
                ExcelWorker.writeToExcel(String.Format("m{0}x{1}",i,i)), String.Format("m{0}x{1}",i,i)));
                Console.WriteLine(timeCount1[i-3]+" "+iterationCount1[i-3]);
            }
            
            ExcelWorker.writeToExcel(m3x3, "m3x3");
            ExcelWorker.writeToExcel(m4x4, "m4x4");
            ExcelWorker.writeToExcel(m5x5, "m5x5");
            ExcelWorker.writeToExcel(m6x6, "m6x6");
            ExcelWorker.writeToExcel(m7x7, "m7x7");
            ExcelWorker.writeToExcel(m8x8, "m8x8");
            ExcelWorker.writeToExcel(m9x9, "m9x9");
            ExcelWorker.writeToExcel(m10x10, "m10x10");
            ExcelWorker.writeToExcel(m11x11, "m11x11");
            ExcelWorker.writeToExcel(m12x12, "m12x12");*/
            /*-------------- create 10 excel files with matrixs from 3 up to 12 range---------------*/

            /*-------------- extract 10 matrixs out of excel files---------------*/
            /*int[] timeCount1= new int[10];
            int[] iterationCount1 = new int[10];
            int[] timeCount2 = new int[10];
            int[] iterationCount2 = new int[10];

            for (int i = 3; i < 13; ++i)
            {
                Console.WriteLine(String.Format("m{0}x{1}", i, i));
                Algorithm.yakobiSpinAlgorithm(ExcelWorker.ExcelExtractor(String.Format("m{0}x{1}",i,i)), out timeCount1[i-3], out iterationCount1[i-3]);
                Console.WriteLine(timeCount1[i-3]+" "+iterationCount1[i-3]);
                Algorithm.QRAlgorithm(ExcelWorker.ExcelExtractor(String.Format("m{0}x{1}",i,i)), out timeCount2[i-3], out iterationCount2[i-3]);
                Console.WriteLine(timeCount2[i-3] + " " + iterationCount2[i-3]);
            }
            



            double[,] result1 = new double[10, 3];
            int county = 3;
            for (int i = 0; i < 10; i++)
            {
                result1[i,0] = Math.Abs(timeCount1[i]) + iterationCount1[i];
                result1[i,1] = Math.Abs(timeCount2[i]) + iterationCount2[i];
                result1[i,2] = county;
                ++county;
            }
            
            Matrix result = new Matrix(result1);

            ExcelWorker.writeToExcel(result, "result");





            //Console.WriteLine("time1"+iterationCount1+" "+timeCount1+"");
            //Console.WriteLine("time2"+iterationCount2 + " " + timeCount2 + "\n");
            Console.ReadLine();
            /*-------------- extract 10 matrixs out of excel files---------------#1#


*/

/*            Console.WriteLine(m1);
            long k = -1982883273432948L;
            Console.WriteLine(Math.Sign(k));*/
            /*double[,] matr = {{1, 3, 1, 2}, {1, 1, 4,7}, {4, 3, 1,4},{2,7,3,9}};*/
            /*double[,] matr = { { 1, -3, 4 }, { 4, -7, 8 }, { 6, -7, 7 } };*/
            /*double[,] matr = { { -1, -6 }, { 2, 6 } };
            Matrix m1 = new Matrix(matr);*/
            /*Console.WriteLine(m1.makeSimetricMatrix());
            Console.ReadLine();
            int k = 0;
            int z = 0;
            Matrix m3 =Algorithm.firstAlgorithm(m1.makeSimetricMatrix(), out k, out z);
            Console.WriteLine(k+" "+z);
            Console.ReadLine();

            Console.WriteLine(Algorithm.secondAlgorithm(m2.makeSimetricMatrix(), out k, out z));
            Console.WriteLine(k + " " + z);
            Console.ReadLine();*/

        }

        
    }
}
