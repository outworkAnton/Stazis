﻿using StazisExtensibilityInterface;
using System.IO;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DatabaseFactoryCore
{
    public class DatabaseFactory
    {
        #region Public Methods
        public DatabaseFactory(string directoryOfPlugins)
        {
            GetAllPlugins(directoryOfPlugins);
        }

        public IExtensibility CreateDataBaseInstance(string filePath)
        {
            var extensionOfFile = Path.GetExtension(filePath);
            foreach (var plugin in Plugins)
            {
                if (!plugin.GetDatabaseFileExtension().Contains(extensionOfFile)) continue;
                plugin.ConnectToDatabase(filePath);
                plugin.DatabasePath = filePath;
                return plugin;
            }
            throw new Exception($"Plugin for {extensionOfFile} not found");
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
        [ImportMany(typeof(IExtensibility))]
        List<IExtensibility> Plugins { get; set; }

        void GetAllPlugins(string pluginsDirectory)
        {
            var catalog = new DirectoryCatalog(pluginsDirectory, "*.dll");
            using (var container = new CompositionContainer(catalog))
            {
                try
                {
                    Plugins = new List<IExtensibility>();
                    container.ComposeParts(this);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Plugins not found. Cannot to load any file types. Message: {ex}");
                }
            }
        } 
        #endregion
    }
}
