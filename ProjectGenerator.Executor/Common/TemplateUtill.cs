using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGenerator.Executor.Common
{
    public static class TemplateUtill
    {
        public static string RepositoryTemplatePath {
            get {
                var templateFiles = Directory.GetFiles(Path.Combine(TemplateProjectPath + "/Template/Repository/"));
                if (templateFiles != null && templateFiles.Count() > 0) 
                {
                    return templateFiles.Where(x => x.IndexOf("Repository.cs.pg") != -1).FirstOrDefault();
                }
                return "";
            }
        }
        public static string APITemplatePath
        {
            get
            {
                var templateFiles = Directory.GetFiles(Path.Combine(TemplateProjectPath + "/Template/APIController/"));
                if (templateFiles != null && templateFiles.Count() > 0)
                {
                    return templateFiles.Where(x => x.IndexOf("APIController.cs.pg") != -1).FirstOrDefault();
                }
                return "";
            }
        }
        public static string ServicesTemplatePath
        {
            get
            {
                var templateFiles = Directory.GetFiles(Path.Combine(TemplateProjectPath + "/Template/Service/"));
                if (templateFiles != null && templateFiles.Count() > 0)
                {
                    return templateFiles.Where(x => x.IndexOf("Services.cs.pg") != -1).FirstOrDefault();
                }
                return "";
            }
        }
        public static string ModelTemplatePath
        {
            get
            {
                var templateFiles = Directory.GetFiles(Path.Combine(TemplateProjectPath + "/Template/Model/"));
                if (templateFiles != null && templateFiles.Count() > 0)
                {
                    return templateFiles.Where(x => x.IndexOf("Model.cs.pg") != -1).FirstOrDefault();
                }
                return "";
            }
        }
        public static string MappingTemplatePath
        {
            get
            {
                var templateFiles = Directory.GetFiles(Path.Combine(TemplateProjectPath + "/Template/Mapping/"));
                if (templateFiles != null && templateFiles.Count() > 0)
                {
                    return templateFiles.Where(x => x.IndexOf("Mapping.cs.pg") != -1).FirstOrDefault();
                }
                return "";
            }
        }

        public static string MVCTemplatePath
        {
            get
            {
                var templateFiles = Directory.GetFiles(Path.Combine(TemplateProjectPath + "/Template/Controller/"));
                if (templateFiles != null && templateFiles.Count() > 0)
                {
                    return templateFiles.Where(x => x.IndexOf("MVCController.cs.pg") != -1).FirstOrDefault();
                }
                return "";
            }
        }
        public static string ScriptListTemplatePath
        {
            get
            {
                var templateFiles = Directory.GetFiles(Path.Combine(TemplateProjectPath + "/Template/Script/"));
                if (templateFiles != null && templateFiles.Count() > 0)
                {
                    return templateFiles.Where(x => x.IndexOf("ListPage.js.pg") != -1).FirstOrDefault();
                }
                return "";
            }
        }
        public static string ScriptAddEditTemplatePath
        {
            get
            {
                var templateFiles = Directory.GetFiles(Path.Combine(TemplateProjectPath + "/Template/Script/"));
                if (templateFiles != null && templateFiles.Count() > 0)
                {
                    return templateFiles.Where(x => x.IndexOf("AddUpdatePage.js.pg") != -1).FirstOrDefault();
                }
                return "";
            }
        }

        public static string TemplateProjectPath
        {
            get
            {
                var SolutionDir = Directory.GetParent(Directory.GetCurrentDirectory());
                if (SolutionDir != null)
                {
                    var templateDir = SolutionDir.GetDirectories("ProjectGenerator.Template")[0];
                    return templateDir.FullName;

                }
                return "";
            }
        }


        private static string[] SqlServerTypes = { "bigint", "binary", "bit", "char", "date", "datetime", "datetime2", "datetimeoffset", "decimal", "filestream", "float", "geography", "geometry", "hierarchyid", "image", "int", "money", "nchar", "ntext", "numeric", "nvarchar", "real", "rowversion", "smalldatetime", "smallint", "smallmoney", "sql_variant", "text", "time", "timestamp", "tinyint", "uniqueidentifier", "varbinary", "varchar", "xml" };
        private static string[] CSharpTypes = { "long", "byte[]", "bool", "char", "DateTime", "DateTime", "DateTime", "DateTimeOffset", "decimal", "byte[]", "double", "Microsoft.SqlServer.Types.SqlGeography", "Microsoft.SqlServer.Types.SqlGeometry", "Microsoft.SqlServer.Types.SqlHierarchyId", "byte[]", "int", "decimal", "string", "string", "decimal", "string", "Single", "byte[]", "DateTime", "short", "decimal", "object", "string", "TimeSpan", "byte[]", "byte", "Guid", "byte[]", "string", "string" };

        public static string SqlTypeToCSharp(string typeName)
        {
            var index = Array.IndexOf(SqlServerTypes, typeName);

            return index > -1
                ? CSharpTypes[index]
                : "object";
        }

        public static string ConvertCSharpFormatToSqlServer(string typeName)
        {
            var index = Array.IndexOf(CSharpTypes, typeName);

            return index > -1
                ? SqlServerTypes[index]
                : null;
        }
    }
}
