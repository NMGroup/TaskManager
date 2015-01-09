using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Mephi.Cybernetics.Nm.TaskManager;
using System.Collections.ObjectModel;


namespace Chm_lab_1
{
    
    
    public class Matrix
    {
        private double[,] _matrix;
        private int n, m;
        
        public int N
        {
            get
            {
                return n;
            }
            set
            {
                if ((value > -1) && (value < 999999))
                {
                    n = value;
                }
            }
        }
        public int M
        {
            get
            {
                return m;
            }
            set
            {
                if ((value > -1) && (value < 999999))
                {
                    m = value;
                }
            }
        }

        public double getValueAt(int y, int x)
        {
            if ((x > -1) && (y > -1) && (x < M) && (y < N))
            {
                return this._matrix[y, x];
            }
            else
            {
                throw new Exception("Out of range");
            }
        }

        public void setValueAt(int y, int x, double value)
        {
            if (( x > -1 ) && ( y > -1 ) && ( x < M ) && ( y < N )) 
                _matrix[y,x] = value;
        }
        
    /*---Getter-Setter---*/

    /*---Ctor---*/

        public Matrix(int n)
        {
            if (n <= 0) return;
            _matrix = new double[n,n];
            N = n;
            M = n;
        }

        public Matrix(int n,int m)
        {
            if ((n <= 0) && (m <= 0)) return;
            _matrix = new double[n,m];
            N = n;
            M = m;
        }

        public Matrix(double[,] inpMatrix)
        {
            N = inpMatrix.GetLength(0);
            M = inpMatrix.GetLength(1);
            _matrix = new double[N,M];

            for ( int i = 0; i < N; ++i )
                for ( int j = 0; j <M; ++j )
                    _matrix[i,j] = inpMatrix[i,j];
        }
        /*---Ctor---*/

        public Matrix makeSimetricMatrix()
        {
            if (!isMatrix() || (!isSquare())) throw new IOException("Invalid type of Matrix.");
            Matrix returnedMatrix = new Matrix(N,M);
            for (int i = 0; i < N; ++i)
            {
                for (int j = i; j < M; ++j)
                {
                    returnedMatrix._matrix[j, i] = _matrix[i, j];
                    if (i != j)
                        returnedMatrix._matrix[i, j] = _matrix[i, j];
                }
            }
            return returnedMatrix;
        }

        public override String ToString()
        {
            string retString = null;
            for(int i=0; i<N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    retString += _matrix[i,j] + " ";
                }
                retString += String.Format("\n");
            }
            return retString;
        }

        public Matrix createCopy()
        {
            Matrix copiedMatrix = new Matrix(N, M);
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    copiedMatrix._matrix[i, j] = _matrix[i, j];
                }   
            }
            return copiedMatrix;
        }

        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            if ((matrix1.N != matrix2.N) || (matrix1.M != matrix2.M))
                throw new IOException("Matrixs are not the same sizes.");
            Matrix returnedMatrix = new Matrix(matrix1.N,matrix2.M);
            for (int i = 0; i < matrix1.N; ++i)
            {
                for (int j = 0; j < matrix2.M; ++j)
                {
                    returnedMatrix._matrix[i, j] = matrix1._matrix[i,j] + matrix2._matrix[i, j];
                }
            }
            return returnedMatrix;
        }
        
        
        public Boolean isSquare() { return ( M == N ); }

        public Boolean isMatrix()
        {
            return (this != null)&&(N != 0);
        }

        public void fillOnesDiag()
        {
            if(!isSquare()) throw new IOException("Invalid action.");

            for (int i = 0; i < N; ++i)
            {
                _matrix[i, i] = 1;
            }
        }


        public void fillRandom()
        {
            fillRandom(5);
        }

        public void fillRandom(int absBorder)
        {
            if (!isMatrix())
            {
                throw new IOException("Rounded part and decimal part must be less than 100 000 000.");
            }

            double a;

            for( int i=0; i<N; ++i )
            {
                for ( int j = 0; j < M; ++j)
                {
                    a = RandomWIthBlackJackAndHoe.Next(absBorder); //+ RandomWIthBlackJackAndHoe.Next(99) / 100.0;
                    _matrix[i,j] = a;
                }
            }
        }

        public void round(int decimalNumber)
        {
            if (!isMatrix()) throw new IOException("Is not Matrix.");

            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    _matrix[i, j] = Math.Round(_matrix[i, j], decimalNumber);
                }
            }

        }

        public Matrix getRowMatrix(int numberOfRow)
        {
            Matrix returnedRow = new Matrix(1, M);
            for (int i = 0; i < M; ++i)
            {
                returnedRow._matrix[0, i] = _matrix[numberOfRow, i];
            }
            return returnedRow;
        }

        public void setRowMatrix(Matrix row, int numberOfRow)
        {
            for (int i = 0; i < this.M; i++)
            {
                this._matrix[numberOfRow, i] = row._matrix[0, i];
            }
        }


        public static Matrix multiply (Matrix matrix1, Matrix matrix2)
        {
            if ( !matrix1.isMatrix() || !matrix2.isMatrix() ) throw new IOException("Are not Matrix.");

            int width1 = matrix1.M;
            int width2 = matrix2.M;
            int height1 = matrix1.N;
            int height2 = matrix2.N;

            Matrix returnedMatrix = new Matrix(height1,width2);
            Matrix matrixTransparent = matrix2.transparent();

            if (( width1 > 0 ) && (height2 > 0) && (height2 == width1))
            {
                for (int i = 0; i < height1; ++i)
                {
                    for (int j = 0; j < width2; ++j)
                    {
                        for (int k = 0; k < height2; ++k)
                        {
                            returnedMatrix._matrix[i,j] += matrix1._matrix[i,k] * matrixTransparent._matrix[j,k];
                        }
                    }
                }
                return returnedMatrix;
            }
            return null;
        }

        public Matrix createSubMatrix(int delRow, int delColumn)
        {
            if (!isMatrix() || (delRow*delColumn < 0) || (delRow>N) || (delColumn>M) ) throw new IOException("Invalid action.");
        
            Matrix returnedMatrix = new Matrix(N-1,M-1);
            int x = -1;
            int y;

            for ( int i = 0; i < N; ++i )
            {
                if ( i == delRow ) continue;
                ++x;
                y = -1;
                for (int j = 0; j < M; ++j)
                {
                    if ( j == delColumn ) continue;
                    returnedMatrix._matrix[x, ++y] = _matrix[i, j];
                }
            }
            return returnedMatrix;
        }

        public Matrix transparent()
        {
            if ( !isMatrix() ) throw new IOException("Is not Matrix.");
        
            Matrix returnedMatrix = new Matrix(M,N);
            for ( int i = 0; i < N; ++i )
            {
                for ( int j = 0; j < M; ++j )
                {
                    returnedMatrix._matrix[j,i] = _matrix[i,j];
                }
            }
            return returnedMatrix;
        }

        public double determinant()
        {
            if (!isMatrix() || !isSquare()) throw new IOException("Invalid action.");
            if (M == 1)
            {
                return _matrix[0,0];
            }
            if (N == 2)
            {
                return (_matrix[0,0] * _matrix[1, 1]) - ( _matrix[0, 1] * _matrix[1, 0]);
            }
            int sign;

            double determinant = 0.0;

            for ( int i = 0; i < N; ++i)
            {
                sign = (i%2 == 0) ? 1 : -1;
                determinant += sign * _matrix[0, i] * createSubMatrix(0, i).determinant();
            }
            return determinant;
        }

        public Matrix cofactor()
        {
            if (!isMatrix()) throw new IOException("Is not Matrix.");

            Matrix returnedMatrix = new Matrix(N, M);
            int signI;
            int signJ;
            for ( int i = 0; i < N; ++i )
            {
                for ( int j = 0; j < M; ++j)
                {
                    signI = (i%2 == 0) ? 1 : -1;
                    signJ = (j%2 == 0) ? 1 : -1;
                    returnedMatrix._matrix[i, j] = signI * signJ * createSubMatrix(i, j).determinant();
                }
            }
            return returnedMatrix;
        }

        public Matrix inverse()
        {
            if (!isMatrix()) throw new IOException("Is not Matrix.");
            double myConst = (1.0/determinant());
            int width = M;
            int height = N;
            Matrix returnedMatrix = new Matrix(height,width);

            Matrix cofactorMatrix = this.cofactor();
            for ( int i = 0; i < height; ++i)
            {
                if (Math.Abs(determinant()) < 0.00000001) throw new IOException("Determinant is about null");
                if (( width == 1) && ( height == 1))
                {
                    returnedMatrix._matrix[0, 0] = myConst;
                    break;
                }
                for ( int j = 0; j < width; ++j)
                {
                    returnedMatrix._matrix[i, j] = myConst * cofactorMatrix._matrix[j, i];
                }
            }

            return returnedMatrix;
        }

        private double getNorm()
        {
            if (!isMatrix()) throw new IOException("Is not Matrix.");
        
            double returnedNorm;

            double[] arrayOfSum = new double[N];

            for ( int i = 0; i < N; ++i )
            {
                for ( int j = 0; j < M; ++j )
                {
                    arrayOfSum[i] += Math.Abs(_matrix[i, j]);
                }
            }

            returnedNorm = arrayOfSum[0];

            for ( int i = 1; i < arrayOfSum.Length; ++i )
            {
                if ( returnedNorm < arrayOfSum[i])
                    returnedNorm = arrayOfSum[i];
            }

            return returnedNorm;
        }

        public double matrixDependent()
        {
            if (!isMatrix()) throw new IOException("Is not Matrix");
            return getNorm() * this.inverse().getNorm();
        }


        public static Matrix ParralelMatrixMultiply(Matrix matrix1, Matrix matrix2)
        {
            var returnedMatrix = new Matrix(matrix2.N,matrix1.M);

            var tasklist = new List<TaskRE>();

            TaskRE task;
            TaskQueue tq = new TaskQueue();
            int i;
            for (i = 0; i < matrix1.N; ++i)
            {
                
                task = new TaskRE(
                    "ParralelMatrixMultiply", 
                    arguments: new Object[]{matrix1.getRowMatrix(i), matrix2}, 
                    dlgt: new Func<Matrix,Matrix,Matrix>((Matrix m1, Matrix m2) =>
                    {
                        var returnedRow = Matrix.multiply(m1, m2);
                        return returnedRow;
                    })
                );
                tq.AddTask(task);
                tasklist.Add(task);
            }

            foreach (TaskRE t in tasklist)
            {
                t.w81();
            }

            for(i=0; i<returnedMatrix.N; ++i)
            {
                returnedMatrix.setRowMatrix((Matrix) tasklist[i].ResultValue.Value, i);
            }

            return returnedMatrix;
        }

    }
}
