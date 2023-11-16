using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseLib
{
    public class parsCSV
    {
        private DataTable dataTable = new DataTable();
        private DataRow row;

        /// <summary>
        /// Возращает DataTable для DataDridView
        /// </summary>
        /// <param name="filePatch"> Путь к файлу типа .csv </param>
        /// <param name="delimiter"> Разделитель используемый между значениями </param>
        /// <param name="titleFirstRow"> true -> если первая строка загаловки
        ///                              false -> если первая строка сразу с значениями
        /// </param>
        /// <returns></returns>
        public DataTable getDataTable
            (string filePatch,
                string delimiter,
                bool titleFirstRow)
        {
            using (StreamReader reader = new StreamReader(filePatch, Encoding.Default))
            {
                string line;
                string[] data;

                if (titleFirstRow == true)
                {
                    data =
                        reader.ReadLine().Split(delimiter.ToCharArray());

                    foreach (string s in data)
                    {
                        dataTable.Columns.Add(s);
                    }
                }

                while ((line = reader.ReadLine()) != null)
                {
                    data = line.Split(delimiter.ToCharArray());

                    if (titleFirstRow == false)
                    {
                        for (int i = 0; i < data.Length; i++)
                        {
                            dataTable.Columns.Add(new DataColumn());
                        }

                        titleFirstRow = true;
                    }

                    row = dataTable.NewRow();

                    int rowIndex = 0;
                    foreach (string s in data)
                    {
                        row[rowIndex] = s;
                        rowIndex++;
                    }

                    dataTable.Rows.Add(row);
                }

            }

            return dataTable;
        }
    }
}
