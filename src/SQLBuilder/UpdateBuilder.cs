using SQLBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQLBuilder
{

    public class UpdateBuilder : SQLBuilder<IUpdateBuilder>, IUpdateBuilder
    {
        public IUpdateBuilder Table(string table, string schema = "")
        {
            this.SetTableSchema(table, schema);
            return this;
        }

        public IUpdateBuilder Set()
        {
            return this;
        }
    }
}
