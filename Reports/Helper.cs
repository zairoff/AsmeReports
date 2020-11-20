namespace Reports
{
    class Helper
    {
        public static string CnnVal(string name)
        {
            return "Server = localhost; Port = 5432; User Id = postgres; Password = postgres; Database = '" +
                System.Configuration.ConfigurationManager.AppSettings["db"] + "'";
        }
    }
}
