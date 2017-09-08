using System.IO;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Collections.Generic;
using System;
using System.Linq;
using StazisExtensibilityInterface;

namespace DatabaseFactoryCore
{
    public class DatabaseFactory
    {
        private static DatabaseFactory _factory;
        private static readonly object SyncRoot = new Object();
        
        private DatabaseFactory(string directoryOfPlugins)
        {
            GetAllPlugins(directoryOfPlugins);
        }
        #region Public Methods

        public static DatabaseFactory GetFactory(string directoryOfPlugins)
        {
            if (_factory == null)
            {
                lock (SyncRoot)
                {
                    if (_factory == null)
                    {
                        _factory = new DatabaseFactory(directoryOfPlugins);
                    }
                }
            }
            return _factory;
        }

        public IDatabaseExtensibility CreateDataBaseInstance(string filePath)
        {
            var extensionOfFile = Path.GetExtension(filePath);
            foreach (var plugin in Plugins)
            {
                if (!plugin.GetDatabaseFileExtension().Contains(extensionOfFile)) continue;
                plugin.ConnectToDatabase(filePath);
                plugin.DatabasePath = filePath;
                return plugin;
            }
            throw new FileNotFoundException($"Plugin for {extensionOfFile} not found");
        }

        public bool Export(IDatabaseExtensibility exportFrom, string filePath, bool onlyCurrentTable = false)
        {
            var extensionOfFile = Path.GetExtension(filePath);
            foreach (var plugin in Plugins)
            {
                if (!plugin.GetDatabaseFileExtension().Contains(extensionOfFile)) continue;
                plugin.Export(exportFrom, filePath, onlyCurrentTable);
                return true;
            }
            throw new IOException($"Can't export to {extensionOfFile} file format");
        }

        public IList<string> GetSupportedFormats()
        {
            return (Plugins.Select(plugin => new {plugin, name = plugin.GetTypeNameOfDatabaseFile()})
                .Select(@t => new
                {
                    @t,
                    extensions = string.Join(";", @t.plugin.GetDatabaseFileExtension().Select(ext => "*" + ext))
                })
                .Select(@t => @t.@t.name + "|" + @t.extensions)).ToList();
        } 
        #endregion

        #region PrivateMembers
        [ImportMany(typeof(IDatabaseExtensibility))]
        List<IDatabaseExtensibility> Plugins { get; set; }

        void GetAllPlugins(string pluginsDirectory)
        {
            var catalog = new DirectoryCatalog(pluginsDirectory, "*.dll");
            using (var container = new CompositionContainer(catalog))
            {
                try
                {
                    Plugins = new List<IDatabaseExtensibility>();
                    container.ComposeParts(this);
                }
                catch (Exception ex)
                {
                    throw new FileNotFoundException($"Plugins not found. Cannot to load any file types. Message: {ex}");
                }
            }
        } 
        #endregion
    }
}
