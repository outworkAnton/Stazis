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
        public static DataBaseModel CreateDataBaseInstance(string FilePath)
        {
            DataBaseModel dataBase = null;
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
            switch (dataBase)
            {
                case ITable t:
                    {
                        (dataBase as ITable).LoadTablesToMemory(FilePath);
                        break;
                    }
                case IDatabase d:
                    {
                        (dataBase as IDatabase).ConnectToDatabase(FilePath);
                        break;
                    }
            }
            return dataBase;
        }
    }
}
