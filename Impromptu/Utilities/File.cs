using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace Impromptu.Utilities
{
    public class File
    {
        public static DataView ReadExcel(string filepath)
        {
            var connection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;data source=" + filepath + ";Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1;\"");
            connection.Open();
            var theDataAdapter = new System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [Sheet1$]", connection);
            //var ds = new DataSet();
            var dt = new DataTable();
            theDataAdapter.Fill(dt);
            return dt.DefaultView;
        }

        public static void WriteCSV(IEnumerable<DataTable> dts, string filePath, string delimiter = ",")
        {
            using(var sw = new StreamWriter(filePath, false))
            {
                foreach(var dt in dts)
                {
                    var maxLengths = new int[dt.Columns.Count];
                    for(var i = 0; i < dt.Columns.Count; i++)
                    {
                        maxLengths[i] = dt.Columns[i].ColumnName.Length;

                        foreach(DataRow row in dt.Rows)
                        {
                            if(!row.IsNull(i))
                            {
                                var length = row[i].ToString().Length;
                                if(length > maxLengths[i])
                                    maxLengths[i] = length;
                            }
                        }
                    }


                    for(var i = 0; i < dt.Columns.Count; i++)
                    {
                        var temp = dt.Columns[i].ColumnName + delimiter;
                        sw.Write(temp.PadRight(maxLengths[i] + 2));
                    }

                    sw.WriteLine();

                    foreach(DataRow row in dt.Rows)
                    {
                        for(var i = 0; i < dt.Columns.Count; i++)
                        {
                            if(!row.IsNull(i))
                            {
                                var temp = row[i].ToString() + delimiter;
                                sw.Write(temp.PadRight(maxLengths[i] + 2));
                            }
                            else
                                sw.Write(new string(' ', maxLengths[i] + 2));
                        }
                        sw.WriteLine();
                    }
                    sw.WriteLine();
                }
                sw.Close();
            }
        }

        /// <summary>
        ///   Get text file by line
        /// </summary>
        /// <param name = "filepath"></param>
        /// <returns></returns>
        public static string[] GetTextFile(string filepath)
        {
            var input = new List<string>();

            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using(var sr = new StreamReader(filepath))
            {
                string line;
                // Read and display lines from the file until the end of 
                // the file is reached.
                while((line = sr.ReadLine()) != null)
                    input.Add(line);
            }

            var data = input.ToArray();
            return data;
        }

        public static void WriteLines(string filepath, string[] lines)
        {
            // Create an instance of StreamWriter to write text to a file.
            // The using statement also closes the StreamWriter.
            using(var sw = new StreamWriter(filepath, true))
            {
                foreach(var line in lines)
                    sw.WriteLine(line);
            }
        }

        /// <summary>
        ///   Open/close file to write single line
        /// </summary>
        /// <param name = "filepath"></param>
        /// <param name = "line"></param>
        public static void WriteLine(string filepath, string line)
        {
            // Create an instance of StreamWriter to write text to a file.
            // The using statement also closes the StreamWriter.
            using(var sw = new StreamWriter(filepath, true))
                sw.WriteLine(line);
        }

        /// <summary>
        ///   Open excel sheet into row and colume DataTable
        /// </summary>
        /// <param name = "fileName"></param>
        /// <returns></returns>
        public static DataTable OpenXSL(string fileName)
        {
            var connection =
                new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;data source=" + fileName + ";Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1;\"");
            connection.Open();
            var dataAdapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connection);

            var dt = new DataTable();
            dataAdapter.Fill(dt);

            return dt;
        }

        public static List<string[]> ReadCSV(string filepath)
        {
            char[] delimeter = { ',', '\n' };
            char[] remove = { ' ', '\t' };

            var read = new List<string>();
            using(var sr = new StreamReader(filepath))
            {
                var line = sr.ReadLine();
                while((line != null))
                {
                    if(line != string.Empty)
                    {
                        if((line[0] != '#') && (line[0] != '\n') && (line[0] != '\r')) //'#' is used for comments
                            read.Add(line);
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
            }

            var list = new List<string[]>();
            for(var i = 0; i < read.Count; i++)
            {
                var temp = read[i].Trim(remove);
                list.Add(temp.Split(delimeter));
            }

            return list;
        }
    }
}
