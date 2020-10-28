
namespace Reports
{
    class DataBase
    {
        public System.Collections.Generic.List<MyTree> getTree(string query)
        {
            System.Collections.Generic.List<MyTree> myTrees = new System.Collections.Generic.List<MyTree>();
            using (var connection = new Npgsql.NpgsqlConnection(Helper.CnnVal("DBConnection")))
            {
                connection.Open();
                using (var cmd = new Npgsql.NpgsqlCommand(query, connection))
                {
                    cmd.AllResultTypesAreUnknown = true;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MyTree myTree = new MyTree();
                            myTree.Ttext = reader[0].ToString();
                            myTree.Tname = reader[1].ToString();
                            myTrees.Add(myTree);
                        }
                    }
                }
            }
            return myTrees;
        }

        public void getRecords(string query, System.Windows.Forms.DataGridView dataGridView)
        {
            using (var conn = new Npgsql.NpgsqlConnection(Helper.CnnVal("DBConnection")))
            {
                conn.Open();
                using (var adapter = new Npgsql.NpgsqlDataAdapter(query, conn))
                {
                    var dataTable = new System.Data.DataTable();
                    adapter.Fill(dataTable);
                    dataGridView.DataSource = dataTable;
                    //GridHeaders();
                }
            }
        }
        
        public System.Collections.Generic.List<MyControls.Employee> GetEmployees(string query)
        {
            System.Collections.Generic.List<MyControls.Employee> employees = new System.Collections.Generic.List<MyControls.Employee>();
            using (var connection = new Npgsql.NpgsqlConnection(Helper.CnnVal("DBConnection")))
            {
                connection.Open();               
                using (var cmd = new Npgsql.NpgsqlCommand(query, connection))
                {
                    //cmd.AllResultTypesAreUnknown = true;
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MyControls.Employee employee = new MyControls.Employee();
                            //employee.Dock = System.Windows.Forms.DockStyle.Fill;
                            employee.FIO     = reader[1].ToString() + " " + reader[2].ToString() + " " + reader[3].ToString();
                            employee.UserID  = reader[0].ToString();                           
                            employee.MyImage = System.Drawing.Image.FromStream(new System.IO.MemoryStream((byte[])reader[4]));
                            employee.Otdel   = reader[5].ToString();
                            employee.Lavozim = reader[6].ToString();
                            employees.Add(employee);
                        }
                    }
                }
            }
            return employees;
        }

        public void insertData(string query)
        {
            using (var connection = new Npgsql.NpgsqlConnection(Helper.CnnVal("DBConnection")))
            {
                connection.Open();
                using (var cmd = new Npgsql.NpgsqlCommand(query, connection))
                {
                    cmd.AllResultTypesAreUnknown = true;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool checkRow(string query)
        {
            using (var connection = new Npgsql.NpgsqlConnection(Helper.CnnVal("DBConnection")))
            {                
                using (var cmd = new Npgsql.NpgsqlCommand(query, connection))
                {
                    //cmd.AllResultTypesAreUnknown = true;
                    connection.Open();
                    return System.Convert.ToBoolean(cmd.ExecuteScalar());                    
                }
            }
        }

        public System.Collections.Generic.List<EmployeeListbox> GetEmployeeListbox(string query)
        {
            System.Collections.Generic.List<EmployeeListbox> mySmenas = new System.Collections.Generic.List<EmployeeListbox>();
            using (var connection = new Npgsql.NpgsqlConnection(Helper.CnnVal("DBConnection")))
            {
                using (var cmd = new Npgsql.NpgsqlCommand(query, connection))
                {
                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        EmployeeListbox deviceInfo = new EmployeeListbox
                        {
                            ID = System.Convert.ToInt32(reader[0]),
                            Familiya = reader[1].ToString()
                        };
                        mySmenas.Add(deviceInfo);
                    }
                }
            }
            return mySmenas;
        }        
    }

    public class EmployeeListbox
    {
        public int ID { get; set; }

        public string Familiya { get; set; }
    }
}
