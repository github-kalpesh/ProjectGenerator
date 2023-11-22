using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectGenerator.Data.Model
{
    public class TableInfo
    {
        public string TableName { get; set; }
        public string DisplayTableName { get; set; }
        public List<TableColumnInfo> Columns { get; set; }
    }
    public class TableColumnInfo
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool IsAllowNull { get; set; }
        public string DefaultValue { get; set; }
        public int OrdinalPosition { get; set; }
        public int MaxLength { get; set; }
    }
}
