using ProjectGenerator.Data.Database;
using ProjectGenerator.Data.Model;
using ProjectGenerator.Executor.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGenerator.Executor.FrontEnd
{
    public class FrontEndCode
    {
        private readonly Configuration _configuration;
        public static List<TableInfo> _Tables;
        public DbConnection _dbConnection;
        public FrontEndCode(Configuration config)
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
            if (_Tables != null && _Tables.Count > 0)
            {
                GenerateScriptList();
                GenerateMVCControler();
                //GenerateAPIController();
                //GenerateModel();
            }
        }

        public void GenerateScriptList()
        {
            if (!Directory.Exists(_configuration.ScriptDirectory))
            {
                Directory.CreateDirectory(_configuration.ScriptDirectory);
            }
            var repositoryText = File.ReadAllText(TemplateUtill.ScriptListTemplatePath);
            foreach (var table in _Tables)
            {
                var controllerDir = Path.Combine(_configuration.ScriptDirectory, table.DisplayTableName);
                var dataColumn = "";
                var columnInfo = _dbConnection.GetTableInfo(table.TableName);
                if (columnInfo != null && columnInfo.Count > 0)
                {
                    foreach (var column in columnInfo)
                    {
                        var type = TemplateUtill.SqlTypeToCSharp(column.DataType);
                        if (column.ColumnName == "Id")
                        {
                            dataColumn += "{ data: 'Id', bVisible: false },\n";
                        }
                        if (type == "bool")
                        {
                            dataColumn += "{ \r\n            data: '" + column.ColumnName + "',\r\n            sClass: 'text-center',\r\n            render: function (value) {\r\n                if (value){ return 'Yes'}\r\n                else { return 'No' }\r\n        }},\n";
                        }
                        else if (type == "DateTime")
                        {
                            dataColumn += "{\r\n            data: '" + column.ColumnName + "',\r\n            sClass: 'text-center',\r\n            render: function (value) {\r\n                if (value === null) return '';\r\n                return moment(value).format('DD/MM/YYYY hh:mm:ss A');\r\n            }\r\n        },\n";
                        }
                        else if (column.ColumnName != "Id")
                        {
                            dataColumn += "{ data: '" + column.ColumnName + "', },\n";
                        }
                    }
                }
                if (!Directory.Exists(controllerDir))
                {
                    Directory.CreateDirectory(controllerDir);
                }
                var fileText = repositoryText.Replace("#DATACOLUMN#", dataColumn)
                    .Replace("#ENTITY#", table.DisplayTableName);
                File.WriteAllText(Path.Combine(controllerDir, table.DisplayTableName + ".js"), fileText);
            }
        }

        public void GenerateMVCControler()
        {
            if (!Directory.Exists(_configuration.MVCControllerDirectory))
            {
                Directory.CreateDirectory(_configuration.MVCControllerDirectory);
            }
            var repositoryText = File.ReadAllText(TemplateUtill.MVCTemplatePath);
            foreach (var table in _Tables)
            {
                var fileText = repositoryText.Replace("#PROJECTNAME#", _configuration.ProjectName)
                    .Replace("#ENTITY#", table.DisplayTableName)
                    .Replace("#TABLENAME#", table.TableName);
                File.WriteAllText(Path.Combine(_configuration.MVCControllerDirectory, table.DisplayTableName + "Controller.cs"), fileText);
            }
        }
    }
}
