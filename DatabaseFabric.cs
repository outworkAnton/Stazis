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
        public static IDatabase CreateDataBaseInstance(string FilePath)
        {
            IDatabase dataBase = null;
            switch (Path.GetExtension(FilePath))
            {
                case ".xls":
                case ".xlsx":
                    dataBase = new ExcelDatabase();
                    break;
                case ".csv":
                    dataBase = new CSVDatabase();
                    break;
                case ".db":
                case ".cdb":
                case ".sqlite3":
                    dataBase = new SQLiteDatabase();
                    break;
            }
            dataBase.ConnectToDatabase(FilePath);
            dataBase.DatabasePath = FilePath;
            return dataBase;
        }
    }
}
