using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace BookDb.Models
{
    class StateModel
    {
        private readonly string ConnectionString = "User=SYSDBA;Password=masterkey;Database=D:\\fbdata\\BOOKSDB.fdb;DataSource=localhost;Port=3050;Charset=UTF8;";

        public List<State> ReadingStates { get; set; }
        public List<State> OwnershipStates { get; set; }

        public StateModel()
        {
            ReadingStates = new List<State>();
            OwnershipStates = new List<State>();
            FetchStates();
        }

        public void FetchStates()
        {
            using (FbConnection connection = new FbConnection(ConnectionString))
            {
                connection.Open();

                string query = @"SELECT * FROM ReadingState;";

                FbCommand command = new FbCommand(query, connection);
                FbDataAdapter adapter = new FbDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    State state = new()
                    {
                        Id = row["id"] as int?,
                        Name = row["name"] as string,
                        Color = row["color"] as string
                    };

                    ReadingStates.Add(state);
                }
                
                query = @"SELECT * FROM OwnershipState;";

                command = new FbCommand(query, connection);
                adapter = new FbDataAdapter(command);
                dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    State state = new()
                    {
                        Id = row["id"] as int?,
                        Name = row["name"] as string,
                        Color = row["color"] as string
                    };

                    OwnershipStates.Add(state);
                }
            }
        }
    }
}
