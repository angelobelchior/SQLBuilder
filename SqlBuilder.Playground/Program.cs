using System;

namespace SqlBuilder.Playground
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var select = new SelectBuilder();
            var update = new UpdateBuilder();
            var delete = new DeleteBuilder();

            int? age = null;

            int? teste = null;

            var selectBuilder =
            select.Top(10)
                  .Distinct()
                  .Column("ID")
                  .Column("Name")
                  .Column("Age")
                  .From("Table")
                  .Where("ID").Eq(1).If(() => true)
                    .And.Where("Age").Gt(age).If(age)
                    .And.Where("Age").In(1, 2, 3, 4, 5)
                    .Or.Where("Age").NotIn(1, 2, 5, 6)
                    .Or.Where("Age").Between(1, 3).If(true)
                    .Or.Where("Name").Like("%Angelo%")
                    .And.Where("CreatedDate").Eq(teste)
                  .Build();
            Console.WriteLine(selectBuilder.SQLCommand);

            var updateBuilder =
            update.Table("Table")
                  .Set("ID", 1)
                  .Set("Age", 1)
                  .Set("Name", "José")
                  .Where("ID").Eq(1).If(() => true)
                    .And.Where("Age").Gt(age).If(age)
                    .And.Where("Age").In(1, 2, 3, 4, 5)
                    .Or.Where("Age").NotIn(1, 2, 5, 6)
                    .Or.Where("Age").Between(1, 3).If(true)
                    .Or.Where("Name").Like("%Angelo%")
                    .And.Where("CreatedDate").Eq(teste)
                  .Build();
            Console.WriteLine(updateBuilder.SQLCommand);

            var deleteBuilder =
            delete.Table("Table")
                  .Where("ID").Eq(1).If(() => true)
                    .And.Where("Age").Gt(age).If(age)
                    .And.Where("Age").In(1, 2, 3, 4, 5)
                    .Or.Where("Age").NotIn(1, 2, 5, 6)
                    .Or.Where("Age").Between(1, 3).If(true)
                    .Or.Where("Name").Like("%Angelo%")
                    .And.Where("CreatedDate").Eq(teste)
                  .Build();
            Console.WriteLine(deleteBuilder.SQLCommand);

            Console.ReadKey();
        }
    }
}
