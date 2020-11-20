
using System.Threading.Tasks;

namespace Reports
{
    class DataBase
    {
        public System.Collections.Generic.List<MyTree> GetTree(string query)
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

        public void GetRecords(string query, System.Windows.Forms.DataGridView dataGridView)
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

        public async Task<System.Data.DataTable> GetRecords(string query)
        {
            using (var dataTable = new System.Data.DataTable())
            {
                using (var conn = new Npgsql.NpgsqlConnection(Helper.CnnVal("DBConnection")))
                {
                    await conn.OpenAsync();
                    using (var cmd = new Npgsql.NpgsqlCommand(query, conn))
                    {
                        var reader = await cmd.ExecuteReaderAsync();
                        dataTable.Load(reader);
                        return dataTable;
                    }                        
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
                            MyControls.Employee employee = new MyControls.Employee
                            {
                                //employee.Dock = System.Windows.Forms.DockStyle.Fill;
                                FIO = reader[1].ToString() + " " + reader[2].ToString() + " " + reader[3].ToString(),
                                UserID = reader[0].ToString(),
                                MyImage = System.Drawing.Image.FromStream(new System.IO.MemoryStream((byte[])reader[4])),
                                Otdel = reader[5].ToString(),
                                Lavozim = reader[6].ToString()
                            };
                            employees.Add(employee);
                        }
                    }
                }
            }
            return employees;
        }

        public void InsertData(string query)
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

        public bool CheckRow(string query)
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
                        var deviceInfo = new EmployeeListbox
                        {
                            ID = System.Convert.ToInt32(reader[0]),
                            Familiya = reader[1].ToString(),
                            Ism = reader[2].ToString()
                        };
                        mySmenas.Add(deviceInfo);
                    }
                }
            }
            return mySmenas;
        }

        public string GetString(string query)
        {
            var str = "";
            using (var connection = new Npgsql.NpgsqlConnection(Helper.CnnVal("DBConnection")))
            {
                using (var cmd = new Npgsql.NpgsqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            str = reader[0].ToString();
                    }
                }
            }
            return str;
        }
    }

    public class EmployeeListbox
    {
        public int ID { get; set; }

        public string Familiya { get; set; }

        public string Ism { get; set; }
    }
}
