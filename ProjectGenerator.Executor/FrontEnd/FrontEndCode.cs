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
                _Tables = tables.Where(x => _configuration.Pages.Any(y => y.TableName == x.TableName && y.IsGenerateFrontEnd)).ToList();
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
                GenerateScriptAddEdit();
                GenerateMVCControler();
                GenerateListPage();
                GenerateAddEditPage();
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
            var templateText = File.ReadAllText(TemplateUtill.ScriptListTemplatePath);
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
                var fileText = templateText.Replace("#DATACOLUMN#", dataColumn)
                    .Replace("#ENTITY#", table.DisplayTableName);
                File.WriteAllText(Path.Combine(controllerDir, table.DisplayTableName + "List.js"), fileText);
            }
        }

        public void GenerateScriptAddEdit()
        {
            if (!Directory.Exists(_configuration.ScriptDirectory))
            {
                Directory.CreateDirectory(_configuration.ScriptDirectory);
            }
            var templateText = File.ReadAllText(TemplateUtill.ScriptAddEditTemplatePath);
            foreach (var table in _Tables)
            {
                var controllerDir = Path.Combine(_configuration.ScriptDirectory, table.DisplayTableName);
                if (!Directory.Exists(controllerDir))
                {
                    Directory.CreateDirectory(controllerDir);
                }
                var fileText = templateText.Replace("#ENTITY#", table.DisplayTableName);
                File.WriteAllText(Path.Combine(controllerDir, table.DisplayTableName + "AddEdit.js"), fileText);
            }
        }

        public void GenerateMVCControler()
        {
            if (!Directory.Exists(_configuration.MVCControllerDirectory))
            {
                Directory.CreateDirectory(_configuration.MVCControllerDirectory);
            }
            var templateText = File.ReadAllText(TemplateUtill.MVCTemplatePath);
            foreach (var table in _Tables)
            {
                var fileText = templateText.Replace("#PROJECTNAME#", _configuration.ProjectName)
                    .Replace("#ENTITY#", table.DisplayTableName)
                    .Replace("#TABLENAME#", table.TableName);
                File.WriteAllText(Path.Combine(_configuration.MVCControllerDirectory, table.DisplayTableName + "Controller.cs"), fileText);
            }
        }

        public void GenerateListPage()
        {
            if (!Directory.Exists(_configuration.ViewDirectory))
            {
                Directory.CreateDirectory(_configuration.ViewDirectory);
            }
            var templateText = File.ReadAllText(TemplateUtill.ListPageTemplatePath);
            foreach (var table in _Tables)
            {
                var viewDir = Path.Combine(_configuration.ViewDirectory, table.DisplayTableName);
                var dataColumn = "";
                var columnInfo = _dbConnection.GetTableInfo(table.TableName);
                if (columnInfo != null && columnInfo.Count > 0)
                {
                    foreach (var column in columnInfo)
                    {
                        dataColumn += "<th>"+ column.ColumnName + "</th>\n";
                    }
                    dataColumn += "<th class=\"text-center\">Action</th>";
                }
                if (!Directory.Exists(viewDir))
                {
                    Directory.CreateDirectory(viewDir);
                }
                var fileText = templateText.Replace("#DATACOLUMN#", dataColumn)
                    .Replace("#ENTITY#", table.DisplayTableName);
                File.WriteAllText(Path.Combine(viewDir, table.DisplayTableName + "List.cshtml"), fileText);
            }
        }

        public void GenerateAddEditPage()
        {
            if (!Directory.Exists(_configuration.ViewDirectory))
            {
                Directory.CreateDirectory(_configuration.ViewDirectory);
            }
            var templateText = File.ReadAllText(TemplateUtill.AddEditPageTemplatePath);
            foreach (var table in _Tables)
            {
                var viewDir = Path.Combine(_configuration.ViewDirectory, table.DisplayTableName);
                var dataColumn = "";
                var columnInfo = _dbConnection.GetTableInfo(table.TableName);
                if (columnInfo != null && columnInfo.Count > 0)
                {
                    var col = 0;
                    
                    foreach (var column in columnInfo)
                    {
                        var type = TemplateUtill.SqlTypeToCSharp(column.DataType);
                        var requiredHtml = column.IsAllowNull ? "" : "<span class='symbol required'></span>";
                        var requiredTag = column.IsAllowNull ? "" : "required";
                        if (col >= 12 || col == 0)
                        {
                            dataColumn += "<div class='row'>";
                            col = 0;
                        }
                        
                        if (column.ColumnName == "Id")
                        {
                            dataColumn += "<input type='hidden' id='hdnId' data-column='Id' value='@ViewBag.Id'>\n";
                        }
                        if(column.ColumnName == "Created" || column.ColumnName == "Updated")
                        {
                            dataColumn += "<input type='hidden' id='hdn"+ column.ColumnName + "' data-column='"+ column.ColumnName + "'>\n";
                        }
                        if (type == "bool")
                        {
                            var columnText = @"<div class='col-md-6 form-group'>
                                                <label class='control-label'>{0} {2}</label>
                                                <div class='checkbox'><label><input type='checkbox' class='grey' value='' id='chk{1}' data-column='{1}' data-error='Enter {0}' {3} checked> {0}</label></div>
                                            </div>";
                            dataColumn += string.Format(columnText, column.ColumnName.ToDisplayCase(), column.ColumnName, requiredHtml, requiredTag);
                            col += 6;
                        }
                        else if (type == "DateTime")
                        {
                            var columnText = @"<div class='col-md-6 form-group'>
                                                    <label class='control-label'>{0} {2}</label>
                                                    <input type='date' placeholder='Select {0}' class='form-control' id='txt{1}' data-column='{1}' {3} data-error='Select {0}'>
                                               </div>";
                            dataColumn += string.Format(columnText, column.ColumnName.ToDisplayCase(), column.ColumnName, requiredHtml, requiredTag);
                            col += 6;
                        }
                        else if (type == "string" && column.MaxLength >= 512 )
                        {
                            var columnText = @"<div class='col-md-6 form-group'>
                                                    <label class='control-label'>{0} {2}</label>
                                                    <textarea placeholder='Enter {0}' class='form-control' id='txt{1}' data-column='{1}' {3} data-error='Enter {0}'></textarea>
                                                </div>";
                            dataColumn += string.Format(columnText, column.ColumnName.ToDisplayCase(), column.ColumnName, requiredHtml, requiredTag);
                            col += 6;
                        }
                        else if (type == "decimal" || type == "double" || type == "int")
                        {
                            var columnText = @"<div class='col-md-6 form-group'>
                                                    <label class='control-label'>{0} {2}</label>
                                                    <input type='number' placeholder='Enter {0}' class='form-control' id='txt{1}' data-column='{1}' {3} data-error='Enter {0}'>
                                                </div>";
                            dataColumn += string.Format(columnText, column.ColumnName.ToDisplayCase(), column.ColumnName, requiredHtml, requiredTag);
                            col += 6;
                        }
                        else if(column.ColumnName != "Id")
                        {
                            var columnText = @"<div class='col-md-6 form-group'>
                                                    <label class='control-label'>{0} {2}</label>
                                                    <input type='text' placeholder='Enter {0}' class='form-control' id='txt{1}' data-column='{1}' {3} data-error='Enter {0}'>
                                               </div>";
                            dataColumn += string.Format(columnText, column.ColumnName.ToDisplayCase(), column.ColumnName, requiredHtml, requiredTag);
                            col += 6;
                        }

                        if(col >= 12)
                        {
                            dataColumn += "</div>\n";
                        }
                    }
                }
                if (!Directory.Exists(viewDir))
                {
                    Directory.CreateDirectory(viewDir);
                }
                var fileText = templateText.Replace("#FORMCONTROL#", dataColumn)
                    .Replace("#ENTITY#", table.DisplayTableName);
                File.WriteAllText(Path.Combine(viewDir, "AddUpdate" +table.DisplayTableName + ".cshtml"), fileText);
            }
        }
    }
}
