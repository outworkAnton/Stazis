using System.Collections.Generic;
using System.Data;

namespace StazisExtensibilityInterface
{
    public interface IExtensibility
    {
        #region Properties
        IList<string> NamesOfTables { get; set; }
        DataSet DatabaseSet { get; set; }
        string DatabasePath { get; set; }
        int SelectedTableIndex { get; set; }
        DataTable CurrentDataTable { get; }
        #endregion

        #region Methods
        string GetTypeNameOfDatabaseFile();
        IList<string> GetDatabaseFileExtension();
        bool ConnectToDatabase(string filePath);
        string GetDatabaseConnectionStatus();
        bool Reload();
        bool AddRecord(IList<dynamic> valuesOfRecord);
        bool DeleteRecord(int index);
        bool UpdateRecord(int index, IList<string> valuesOfRecord);
        int ChangeRecordsInColumn(int column, IList<string> valuesToModifyList, string changeValue);
        bool DisconnectFromDatabase();
        #endregion
    }
}
