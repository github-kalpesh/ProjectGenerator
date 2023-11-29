using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectGenerator.Data.Database;
using ProjectGenerator.Data.Model;
using ProjectGenerator.Executor.Common;


namespace ProjectGenerator.Executor.BackEnd
{
    public class BackEndCode
    {
        private readonly Configuration _configuration;
        public static List<TableInfo> _Tables;
        public DbConnection _dbConnection;
        public BackEndCode(Configuration config) 
        {
            _configuration = config;
            _dbConnection = new DbConnection(_configuration.ConnectionString);
            var tables = _dbConnection.GetTables();
            if (_configuration.Pages != null && _configuration.Pages.Count > 0)
            {
                _Tables = tables.Where(x => _configuration.Pages.Any(y => y.TableName == x.TableName && y.IsGenerateBackEnd)).ToList();
            }
            else
            {
                _Tables = tables;
            }
        }

        public void Create()
        {
            if(_Tables != null && _Tables.Count > 0)
            {
                GenerateRepository();
                GenerateService();
                GenerateAPIController();
                GenerateModel();
            }
        }

        public void GenerateRepository() 
        {
            if (!Directory.Exists(_configuration.RepositoryDirectory))
            {
                Directory.CreateDirectory(_configuration.RepositoryDirectory);
            }
            var repositoryText = File.ReadAllText(TemplateUtill.RepositoryTemplatePath);
            foreach (var table in _Tables)
            {
                var fileText = repositoryText.Replace("#PROJECTNAME#", _configuration.ProjectName)
                    .Replace("#ENTITY#", table.DisplayTableName)
                    .Replace("#TABLENAME#", table.TableName);
                File.WriteAllText(Path.Combine(_configuration.RepositoryDirectory, table.DisplayTableName + "Repository.cs"), fileText);
            }
        }

        public void GenerateAPIController()
        {
            if (!Directory.Exists(_configuration.APIControllerDirectory))
            {
                Directory.CreateDirectory(_configuration.APIControllerDirectory);
            }
            var repositoryText = File.ReadAllText(TemplateUtill.APITemplatePath);
            foreach (var table in _Tables)
            {
                var fileText = repositoryText.Replace("#PROJECTNAME#", _configuration.ProjectName).Replace("#ENTITY#", table.DisplayTableName);
                File.WriteAllText(Path.Combine(_configuration.APIControllerDirectory, table.DisplayTableName + "Controller.cs"), fileText);
            }
        }
        public void GenerateService()
        {
            if (!Directory.Exists(_configuration.ServicesDirectory))
            {
                Directory.CreateDirectory(_configuration.ServicesDirectory);
            }
            var repositoryText = File.ReadAllText(TemplateUtill.ServicesTemplatePath);
            foreach (var table in _Tables)
            {
                var fileText = repositoryText.Replace("#PROJECTNAME#", _configuration.ProjectName).Replace("#ENTITY#", table.DisplayTableName);
                File.WriteAllText(Path.Combine(_configuration.ServicesDirectory, table.DisplayTableName + "Services.cs"), fileText);
            }
        }
        public void GenerateModel()
        {
            if (!Directory.Exists(_configuration.ModelDirectory))
            {
                Directory.CreateDirectory(_configuration.ModelDirectory);
            }
            var repositoryText = File.ReadAllText(TemplateUtill.ModelTemplatePath);
            foreach (var table in _Tables)
            {
                var propertices = "";
                var tableInfo = _dbConnection.GetTableInfo(table.TableName);
                if(tableInfo != null && tableInfo.Count > 0)
                {
                    foreach (var column in tableInfo)
                    {
                        var formate = "        public virtual {0} {1} { get; set; }\n";
                        propertices += formate.Replace("{0}",TemplateUtill.SqlTypeToCSharp(column.DataType)).Replace("{1}",column.ColumnName);
                    }
                }
                var fileText = repositoryText.Replace("#PROJECTNAME#", _configuration.ProjectName)
                    .Replace("#ENTITY#", table.DisplayTableName)
                    .Replace("#TABLENAME#", table.TableName)
                    .Replace("#PROPERTICES#", propertices);
                File.WriteAllText(Path.Combine(_configuration.ModelDirectory, table.DisplayTableName + ".cs"), fileText);
            }
        }
    }
}
