namespace LogixHealth.EnterpriseLibrary.ModelAttributes
{
    [System.AttributeUsage(validOn: System.AttributeTargets.Class, AllowMultiple =false, Inherited =false)]
    public sealed class MapTableAttribute : System.Attribute
    {
        public MapTableAttribute(string tableName)
        {
            TableNameMapped = tableName;
            SchemaNameMapped = "dbo";
        }

        public MapTableAttribute(string tableName, string schemaName)
        {
            TableNameMapped = tableName;
            SchemaNameMapped = schemaName;
        }

        public string TableNameMapped { get; }

        public string SchemaNameMapped { get; }
    }

    [System.AttributeUsage(validOn: System.AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class MapColumnAttribute : System.Attribute
    {
        public MapColumnAttribute(string columnName)
        {
            ColumnNameMapped = columnName;
        }

        public string ColumnNameMapped { get; }
    }

    //[System.AttributeUsage(validOn: System.AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    //public class MapForeignKey : System.Attribute
    //{
    //    public MapForeignKey(string primaryTableName, string key)
    //    {
    //        PrimaryTableNameMapped = primaryTableName;
    //        KeyColumnNameMapped = key;
    //    }

    //    public string PrimaryTableNameMapped { get; }

    //    public string KeyColumnNameMapped { get; }
    //}

    [System.AttributeUsage(validOn: System.AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class IgnoreAttribute : System.Attribute
    {
        public IgnoreAttribute()
        {
            IsNotMapped = true;
        }

        public bool IsNotMapped { get; }
    }
}
