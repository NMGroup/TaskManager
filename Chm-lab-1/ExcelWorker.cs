using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;

namespace Chm_lab_1
{
    public class ExcelWorker
    {
        public static void ExcelCreator(Matrix matrix, string fileName)
        {
            Excel.Application xlApp = new Excel.Application();
            
            xlApp.Visible = false;

            Excel.Workbook wb = xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
            Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];
            ws.Name = matrix.N.ToString() + "x" + matrix.M.ToString();

            for (int i = 0; i < matrix.N; ++i)
            {
                for (int j = 0; j < matrix.M; ++j)
                {
                    ws.Cells[i + 1, j + 1] = matrix.getValueAt(i, j);
                }
            }
            xlApp.DisplayAlerts = false;
            try
            {
                wb.SaveAs("D:\\Mephi\\chm\\chm\\Chm-lab-1\\" + fileName + ".xls");
                wb.Close();
                xlApp.Quit();
            }
            catch (Exception)
            {
                wb.Close();
                xlApp.Quit();
                Console.WriteLine("File could not be saved here. Check this directory on file creation possibility");
            }
            

        }

        public static void writeToExcel(Matrix matrix, string fileName)
        {
            if (!System.IO.File.Exists(@"D:\\Mephi\\chm\\chm\\Chm-lab-1\\" + fileName + ".xls"))
            {
                Console.WriteLine("File doesn't exist.");
                return;
            }
            Excel.Application xlApp = new Excel.Application();
            xlApp.Visible = false;
            Excel.Workbook wb = xlApp.Workbooks.Open("D:\\Mephi\\chm\\chm\\Chm-lab-1\\" + fileName + ".xls");
            Excel.Worksheet ws = (Excel.Worksheet) wb.Worksheets[1];
            ws.Name = matrix.N.ToString() + "x" + matrix.M.ToString();

            for (int i = 0; i < matrix.N; ++i)
            {
                for (int j = 0; j < matrix.M; ++j)
                {
                    
                    ws.Cells[i + 1, j + 1] = matrix.getValueAt(i, j);
                }
            }
            xlApp.DisplayAlerts = false;
            wb.Save();
            wb.Close();
            xlApp.Quit();
        }

        public static Matrix ExcelExtractor(string fileName)
        {
            if (!File.Exists(@"D:\\Mephi\\chm\\chm\\Chm-lab-1\\" + fileName + ".xls"))
            {
                Console.WriteLine("File doesn't exist.");
                return null;
            }
            Excel.Application xlApp = new Excel.Application();
            xlApp.Visible = false;
            Excel.Workbook wb = xlApp.Workbooks.Open("D:\\Mephi\\chm\\chm\\Chm-lab-1\\" + fileName + ".xls");
            Excel.Worksheet ws = (Excel.Worksheet)wb.Worksheets[1];

            int[] sizes = getExcelSizes(ws.Name.Split('x'));
            Matrix returnedMatrix = new Matrix(sizes[0],sizes[1]);

            Excel.Range range = (Excel.Range)ws.UsedRange;
            for (int i = 0; i < sizes[0]; ++i)
            {
                for (int j = 0; j < sizes[1]; ++j)
                {
                    returnedMatrix.setValueAt(i, j, (range.Cells[i + 1, j + 1] as Excel.Range).Value2);    
                }
            }

            wb.Close();
            xlApp.Quit();
            return returnedMatrix;
        }

        private static int[] getExcelSizes(string[] stringOfSizes)
        {
            int[] sizes = new int[2];
            for (int i = 0; i < 2; ++i)
            {
                sizes[i] = System.Convert.ToInt32(stringOfSizes[i]);
            }
            return sizes;
        }

    }
}
