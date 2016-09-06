using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var select = new SelectBuilder();

            var command = 
            select.Column("ID")
                  .Column("Name")
                  .Column("Age")
                  .From("Table", "Schema")
                  .Where("ID").Equals(1).If(() => true)
                  .And().Where("Age").GreaterThan(30)
                  .Or().Where("Name").Like("%Angelo%")
                  .GetSQLCommand();

            Console.WriteLine(command);
            Console.ReadKey();
        }
    }

    public class SelectBuilder : WhereBuilder<SelectBuilder>
    {
        private string _schema;
        private string _table;

        private List<string> _columns = new List<string>();

        public SelectBuilder()
        {
            this.Instance = this;
        }

        public SelectBuilder Column(string column)
        {
            this._columns.Add(column);
            return this;
        }

        public SelectBuilder From(string table, string schema = "")
        {
            this._table = table;
            this._schema = schema;
            return this;
        }

        public string GetSQLCommand()
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT");
            var columns = "";
            foreach (var column in this._columns)
                columns += column + ",\n";

            sb.AppendLine(columns);
            sb.AppendLine($"FROM {this._schema}.{this._table}");

            var where = this.Build();
            sb.AppendLine(where);

            return sb.ToString();
        }
    }

    public class WhereBuilder<T>
    {
        private string _condition;
        private string _column;
        private string _operation;
        private object _value;
        private Func<bool> _function;

        private List<Filter> _filters = new List<Filter>();

        public T Instance { get; protected set; }

        public T Where(string column)
        {
            this._column = column;
            return this.Instance;
        }

        public T And()
        {
            this.RestartWhere();
            this._condition = "AND";
            return this.Instance;
        }

        public T Or()
        {
            this.RestartWhere();
            this._condition = "OR";
            return this.Instance;
        }

        public new T Different(object value)
        {
            this._operation = "<>";
            this._value = value;
            return this.Instance;
        }

        public new T Equals(object value)
        {
            this._operation = "=";
            this._value = value;
            return this.Instance;
        }

        public T Like(object value)
        {
            this._operation = "LIKE";
            this._value = value;
            return this.Instance;
        }

        public T GreaterThan(object value)
        {
            this._operation = ">";
            this._value = value;
            return this.Instance;
        }

        public T LowerThan(object value)
        {
            this._operation = "<";
            this._value = value;
            return this.Instance;
        }

        public T GreaterThanOrEquals(object value)
        {
            this._operation = ">=";
            this._value = value;
            return this.Instance;
        }

        public T LowerThanOrEquals(object value)
        {
            this._operation = "<=";
            this._value = value;
            return this.Instance;
        }

        public T If(Func<bool> function)
        {
            this._function = function;
            return this.Instance;
        }

        private void RestartWhere()
        {
            if (!string.IsNullOrWhiteSpace(this._column))
                if (this._function == null || (this._function != null && this._function()))
                    this._filters.Add(new Filter
                    {
                        Column = this._column,
                        Condition = this._condition,
                        Operation = this._operation,
                        Value = this._value
                    });

            this._condition = string.Empty;
            this._column = string.Empty;
            this._operation = string.Empty;
            this._value = string.Empty;
            this._function = null;
        }

        protected string Build()
        {
            this.RestartWhere();

            var sb = new StringBuilder();
            if (this._filters.Count > 0)
                sb.Append("WHERE ");

            foreach (var filter in this._filters)
                sb.AppendLine($"{filter.Condition} {filter.Column} {filter.Operation} @{filter.Column}");

            return sb.ToString();
        }

        private class Filter
        {
            public string Condition { get; set; }
            public string Column { get; set; }
            public string Operation { get; set; }
            public object Value { get; set; }
        }
    }
}
