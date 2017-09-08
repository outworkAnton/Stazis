using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.ComponentModel.Composition;
using StazisExtensibilityInterface;

namespace SQLiteDatabasePlugin
{
    [Export(typeof(IDatabaseExtensibility))]
    public class SqLiteDatabase : IDatabaseExtensibility
    {
        public IList<string> NamesOfTables { get; set; }
        public DataSet DatabaseSet { get; set; }
        public string DatabasePath { get; set; }
        public int SelectedTableIndex { get; set; }
        public DataTable CurrentDataTable => DatabaseSet.Tables[SelectedTableIndex];
        readonly SQLiteConnection _connection;
        

        public SqLiteDatabase()
        {
            _connection = (SQLiteConnection)SQLiteFactory.Instance.CreateConnection();
            NamesOfTables = new List<string>();
            DatabaseSet = new DataSet();
        }

        public string GetTypeNameOfDatabaseFile()
        {
            return "База данных SQLite";
        }

        public bool ConnectToDatabase(string filePath)
        {
            try
            {
                DatabasePath = filePath;
                DatabaseSet.DataSetName = Path.GetFileNameWithoutExtension(DatabasePath);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter();
                _connection.ConnectionString = "Data Source = " + DatabasePath;
                _connection.Open();
                var tmpadapter = new SQLiteDataAdapter("SELECT name FROM sqlite_master WHERE type = 'table' AND name NOT LIKE '%sqlite%'", _connection);
                DataTable tmpDt = new DataTable();
                tmpadapter.Fill(tmpDt);
                foreach (DataRow row in tmpDt.Rows)
                {
                    var tableName = row.ItemArray.GetValue(0).ToString();
                    NamesOfTables.Add(tableName);
                    var tempAdapter = new SQLiteDataAdapter("select * from " + tableName, _connection);
                    DataTable tmpdt = new DataTable(tableName);
                    tempAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    tempAdapter.MissingMappingAction = MissingMappingAction.Passthrough;
                    tempAdapter.Fill(tmpdt);
                    DatabaseSet.Tables.Add(tmpdt);
                }
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public string GetDatabaseConnectionStatus()
        {
            return _connection.State.ToString();
        }

        public bool Reload()
        {
            throw new System.NotImplementedException();
        }

        public bool Export(IDatabaseExtensibility exportFrom, string filePath, bool onlyCurrentTable = false)
        {
            throw new NotImplementedException();
        }

        public bool DisconnectFromDatabase()
        {
            try
            {
                _connection.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool AddRecord(IList<dynamic> valuesOfRecord)
        {
            try
            {
                DataRow newRow = CurrentDataTable.NewRow();
                for (var i = 0; i < CurrentDataTable.Columns.Count; i++)
                {
                    if (!CurrentDataTable.Columns[i].AutoIncrement)
                    {
                        newRow[i] = valuesOfRecord[i];
                    }
                }
                CurrentDataTable.Rows.Add(newRow);
                var tempAdapter = new SQLiteDataAdapter("select * from " + CurrentDataTable.TableName, _connection)
                    {
                        AcceptChangesDuringUpdate = true
                    };
                SaveChangesToDatabase(tempAdapter);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        void SaveChangesToDatabase(SQLiteDataAdapter tempAdapter)
        {
            using (SQLiteTransaction transaction = _connection.BeginTransaction())
            {
                SQLiteCommandBuilder commandBuilder = new SQLiteCommandBuilder(tempAdapter);
                tempAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                tempAdapter.Update(CurrentDataTable);
                transaction.Commit();
            }
        }

        public int ChangeRecordsInColumn(int column, IList<string> valuesToModifyList, string changeValue)
        {
            throw new System.NotImplementedException();
        }

        public IList<string> GetDatabaseFileExtension()
        {
            return new List<string> { ".sqlite3", ".db", ".cdb" };
        }

        public bool DeleteRecord(int index)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateRecord(int index, IList<string> valuesOfRecord)
        {
            throw new System.NotImplementedException();
        }
    }
}

