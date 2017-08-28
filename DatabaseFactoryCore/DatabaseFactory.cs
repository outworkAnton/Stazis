using ExtensibilityInterface;
using System.IO;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Collections.Generic;
using System;

namespace DatabaseFactoryCore
{
    public class DatabaseFactory
    {
        [ImportMany(typeof(IExtensibility))]
        List<IExtensibility> Plugins { get; set; }

        public IExtensibility CreateDataBaseInstance(string FilePath)
        {
            var extensionOfFile = Path.GetExtension(FilePath);
            var catalog = new DirectoryCatalog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"), "*.dll");
            using (var container = new CompositionContainer(catalog))
            {
                try
                {
                    Plugins = new List<IExtensibility>();
                    container.ComposeParts(this);
                    foreach (var plugin in Plugins)
                    {
                        if (plugin.GetDatabaseFileExtension().Contains(extensionOfFile))
                        {
                            plugin.ConnectToDatabase(FilePath);
                            plugin.DatabasePath = FilePath;
                            return plugin;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Plugins not found. Cannot to load any file types. Message: {ex}");
                }
                return null;
            }
        }
    }
}
