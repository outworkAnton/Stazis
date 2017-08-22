using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stazis
{
    static class DatabaseFabric
    {
        public static bool CreationCompleted;

        public static DataBaseModel CreateDataBaseInstance(string FilePath)
        {
            DataBaseModel dataBase = null;
            Task databaseLoad = Task.Factory.StartNew(() =>
            {
                switch (Path.GetExtension(FilePath))
                {
                    case ".xls":
                    case ".xlsx":
                        dataBase = new ExcelDatabase(FilePath);
                        break;
                    case ".csv":
                        dataBase = new CSVDatabase(FilePath);
                        break;
                    case ".db":
                    case ".cdb":
                    case ".sqlite3":
                        dataBase = new SQLiteDatabase(FilePath);
                        break;
                }
            });
            while (!databaseLoad.IsCompleted)
            {
                CreationCompleted = false;
            }
            CreationCompleted = true;
            return dataBase;
        }
    }
}
