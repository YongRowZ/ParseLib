using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseLib
{
    public static class parsCSV
    {       
        /// <summary>
        /// Возращает DataTable для DataDridView, полученную из файла типа .csv
        /// </summary>
        /// <param name="filePatch"> Путь к файлу типа .csv </param>
        /// <param name="delimiter"> Разделитель используемый между значениями </param>
        /// <param name="titleFirstRow"> Флаг указывающий является ли первая строка загаловками столбцов:
        ///                                 true -> ДА;
        ///                                 false -> НЕТ;
        /// </param>
        /// <returns></returns>
        public static DataTable get_DataTablefromCSV
            (   string filePatch,
                string delimiter,
                bool titleFirstRow = true  
            )
        {
            DataTable dataTable = new DataTable();
            DataRow row;

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

        /// <summary>
        /// Полученную DataTable сохраняет в формате .csv
        /// </summary>
        /// <param name="filePatch"> Путь по которому будет выполнено сохранение, файла типа .csv </param>
        /// <param name="delimiter"> Разделитель используемый между значениями </param>
        /// <param name="titleFirstRow"> Флаг указывающий необходимо ли загаловки столбцов сохронять в первую строку:
        ///                                 true -> ДА;
        ///                                 false -> НЕТ;
        /// </param>
        public static async void save_CSVfromDataTable
            (   string filePatch,
                string delimiter,
                DataTable dataTable,
                bool titleFirstRow = true
            )
        {
            using (StreamWriter writer = new StreamWriter(filePatch, false, Encoding.Default))
            {
                if (titleFirstRow == true)
                {
                    string line = string.Empty;

                    for (int indexColumn = 0; indexColumn < dataTable.Columns.Count; indexColumn++)
                    {
                        line += dataTable.Columns[indexColumn].ToString() + delimiter;
                    }

                    await writer.WriteLineAsync
                        (line.Substring(0, line.LastIndexOf(";")));
                }

                foreach (DataRow row in dataTable.Rows)
                {
                    string line = string.Empty;

                    for (int indexColumn = 0; indexColumn < dataTable.Columns.Count; indexColumn++)
                    {
                        line += row[indexColumn].ToString() + delimiter;
                    }

                    await writer.WriteLineAsync
                        (line.Substring(0, line.LastIndexOf(";")));
                }
            }
        }
    }
}
