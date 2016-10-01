namespace SqlBuilder
{
    internal class Constants
    {
        public const string SELECT = "SELECT";
        public const string SELECT_FROM = "FROM";
        public const string SELECT_DISTINCT = "DISTINCT";
        public const string SELECT_TOP = "TOP";
        public const string SELECT_BREAK_LINE = ",\n";
        public const string SELECT_SELECT_SPACES = "       ";

        public const string INSERT = "INSERT INTO";
        public const string INSERT_VALUES = "VALUES";
        public const string INSERT_BREAK_LINE = "\n";

        public const string UPDATE = "UPDATE TABLE";
        public const string UPDATE_SET = "SET";

        public const string DELETE = "DELETE TABLE";

        public const string WHERE = "WHERE";
        public const string WHERE_CONDITION_AND = "AND";
        public const string WHERE_CONDITION_OR = "OR";
        public const string WHERE_OPERATION_BETWEEN = "BETWEEN";
        public const string WHERE_OPERATION_BETWEEN_VALUE_A = "_BETWEEN_A";
        public const string WHERE_OPERATION_BETWEEN_VALUE_B = "_BETWEEN_B";
        public const string WHERE_OPERATION_DIFF = "<>";
        public const string WHERE_OPERATION_EQ = "=";
        public const string WHERE_OPERATION_LIKE = "LIKE";
        public const string WHERE_OPERATION_GT = ">";
        public const string WHERE_OPERATION_LT = "<";
        public const string WHERE_OPERATION_GT_EQ = ">=";
        public const string WHERE_OPERATION_LT_EQ = "<=";
        public const string WHERE_OPERATION_IN = "IN";
        public const string WHERE_OPERATION_NOT_IN = "NOT IN";
    }
}
