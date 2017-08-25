using System.Collections.Generic;
using System.Data;

namespace Stazis
{
	public interface IDatabase
	{
        #region Properties
        List<string> NamesOfTables { get; set; }
        DataSet DatabaseSet { get; set; }
        string DatabasePath { get; set; }
        int SelectedTableIndex { get; set; }
        DataTable CurrentDataTable { get; set; }
        #endregion

        #region Methods
        void ConnectToDatabase(string FilePath);
        void Reload();
        void DisconnectFromDatabase();
        string GetTypeNameOfDatabaseFile();
        void AddRecord(DataRow Record);
        int ChangeRecordsInColumn(int Column, IList<string> InputElements, string OutputElement);
        #endregion
    }
}
