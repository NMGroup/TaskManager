using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chm_lab_1;

namespace Chm_lab_1
{
    public class MatrixCreator
    {
        public static Matrix createRandomMatrix (int n, int m)
        {
            return createRandomMatrix(n,m,9999);
        }

        public static Matrix createRandomMatrix ( int n, int m, int absBorder)
        {
            Matrix returnedMatrix = new Matrix(n,m);
            returnedMatrix.fillRandom(absBorder);

            return returnedMatrix;
        }

        public static Matrix createPredeterminedConditionMatrix(int n, int m, int conditionNumber)
        {
            Matrix returnedMatrix = new Matrix(n,m);
            returnedMatrix.fillRandom();

            return returnedMatrix;
        }

        public static Matrix createSparseMatrix(int n, int m, int z)
        {
            Matrix returnedMatrix = new Matrix(n,m);
            returnedMatrix.fillRandom(50);
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < m; ++j)
                {
                    if (RandomWIthBlackJackAndHoe.Next(7) > 5)
                    {
                        returnedMatrix.setValueAt(i, j, 0);
                    }
                }
            }
            return returnedMatrix;
        }
    }
}
