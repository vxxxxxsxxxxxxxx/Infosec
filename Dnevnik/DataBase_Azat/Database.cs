using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dnevnik
{
    class Database
    {
        private readonly string fileName;
        private readonly string connectionString;

        public Database(string fileName)
        {
            this.fileName = fileName;
            connectionString = $"Data Source={fileName}.sqlite;Version=3;";
        }

        public void CreateFile()
        {
            try
            {
                SQLiteConnection.CreateFile(fileName);
                ExecuteFirstQuery();
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ExecuteFirstQuery()
        {
            string query = "create table tablesInfo (name text primary key, annotationFields text not null)";
            ExecuteNonQuery(query);
        }

        public bool CreateNewEntity(string tableTitle, IEnumerable<string> fieldsNames, bool[] annotationFields)
        {
            string query = $"select exists (select 1 from tablesInfo where name=@name)";
            SQLiteParameter parameter1 = new SQLiteParameter("@name", tableTitle);

            object res = ExecuteScalar(query, parameter1);
            if (res == null || Convert.ToBoolean(res))
                return false;

            query = $"create table \"{tableTitle}\" (id integer primary key autoincrement, " + string.Join(", ", fieldsNames.Select(x => $"\"{x}\" text")) + ")";
            
            if (!ExecuteNonQuery(query))
                return false;

            string annotationFieldsMask = "";
            for (int i = 0; i < annotationFields.Length; i++)
            {
                annotationFieldsMask += annotationFields[i] ? '1' : '0';
            }
            query = $"insert into tablesInfo (name, annotationFields) values (@name, @annotationFields)";
            SQLiteParameter parameter2 = new SQLiteParameter("@annotationFields", annotationFieldsMask);

            ExecuteNonQuery(query, parameter1, parameter2);

            return true;
        }
        /// <summary>
        /// tried to get the list of columns names
        /// </summary>
        /// <param name="tableTitle"></param>
        /// <returns></returns>
        public IEnumerable<string> GetFieldNameListOfEntity(string tableTitle)
        {
            string query = $"select name from pragma_table_info('{tableTitle}')";
            return ReadRows(query).AsEnumerable().Select(row => row.Field<string>(0));
        }
        /// <summary>
        /// Get one exact Document from the table-Entity
        /// </summary>
        /// <param name="tableTitle"></param>
        /// <returns></returns>
        public OrderedDictionary GetDocumentByID(string tableTitle, int id)
        {
            string query = $"select * from \"{tableTitle}\" where id={id}";
            OrderedDictionary res = new OrderedDictionary();
            DataTable dt = ReadRows(query);
            foreach (DataColumn col in dt.Columns)
            {
                res.Add(col.ColumnName, dt.AsEnumerable().Select(row => row.Field<string>(col.ColumnName)).ToList());
            }

            return res;
        }

        public OrderedDictionary GetEntityAllFieldList(string tableTitle)
        {
            string query = $"select * from \"{tableTitle}\"";
            OrderedDictionary res = new OrderedDictionary();
            DataTable dt = ReadRows(query);
            foreach (DataColumn col in dt.Columns)
            {
                res.Add(col.ColumnName, dt.AsEnumerable().Select(row => row.Field<string>(col.ColumnName)).ToList());
            }

            return res;
        }

        public OrderedDictionary GetEntityAnnotationFieldList(string tableTitle)
        {
            string query = "select annotationFields from tablesInfo where name=@name";
            SQLiteParameter parameter = new SQLiteParameter("@name", tableTitle);
            string annotationFields = Convert.ToString(ExecuteScalar(query, parameter));

            query = $"select * from \"{tableTitle}\"";
            DataTable dt = ReadRows(query, annotationFields);

            OrderedDictionary res = new OrderedDictionary();
            foreach (DataColumn col in dt.Columns)
            {
                res.Add(col.ColumnName, dt.AsEnumerable().Select(row => row.Field<string>(col.ColumnName)).ToList());
            }

            return res;
        }

        public IEnumerable<string> GetEntities()
        {
            string query = "select name from tablesInfo";
            return ReadRows(query).AsEnumerable().Select(row => row.Field<string>(0));
        }

        private DataTable ReadRows(string query, string annotationFieldsMask = null)
        {
            DataTable table = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                try
                {
                    connection.Open();

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            table.Columns.Add(reader.GetName(0));

                            for (int i = 1; i < reader.FieldCount; i++)
                            {
                                if (annotationFieldsMask == null || annotationFieldsMask[i - 1] == '1')
                                {
                                    table.Columns.Add(reader.GetName(i));
                                }
                            }
                        }

                        object[] values = new object[reader.FieldCount];

                        while (reader.Read())
                        {
                            DataRow row = table.NewRow();
                            reader.GetValues(values);
                            row.ItemArray = values.Where((x, i) => i == 0 || annotationFieldsMask == null || (i > 0 && annotationFieldsMask[i - 1] == '1')).ToArray();
                            table.Rows.Add(row);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                return table;
            }
        }

        public bool AddDocument(string tableTitle, Document doc)
        {
            string query = $"insert into \"{tableTitle}\" (" + string.Join(", ", doc.Fields.Keys.Cast<string>().Select(x => $"\"{x}\"")) + ") values (" + string.Join(", ", Enumerable.Repeat("?", doc.Fields.Count)) + ")";
            SQLiteParameter[] parameters = new SQLiteParameter[doc.Fields.Count];

            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = new SQLiteParameter(DbType.String, doc.Fields[i]);
            }
            return ExecuteNonQuery(query, parameters);
        }

        public bool EditDocument(string tableTitle, int id, Document doc)
        {
            string query = $"update \"{tableTitle}\" set " + string.Join(", ", doc.Fields.Keys.Cast<string>().Select(x => $"\"{x}\"=?")) + " where id=@id";
            SQLiteParameter[] parameters = new SQLiteParameter[doc.Fields.Count + 1];

            parameters[0] = new SQLiteParameter("@id", id);
            for (int i = 1; i < parameters.Length; i++)
            {
                parameters[i] = new SQLiteParameter(DbType.String, doc.Fields[i - 1]);
            }
            return ExecuteNonQuery(query, parameters);
        }

        public bool DeleteDocument(string tableTitle, int id)
        {
            string query = $"delete from \"{tableTitle}\" where id=@id";
            SQLiteParameter parameter = new SQLiteParameter("@id", id);
            return ExecuteNonQuery(query, parameter);
        }

        private bool ExecuteNonQuery(string query, params SQLiteParameter[] parameters)
        {
            bool success = false;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    command.Parameters.AddRange(parameters);
                    //MessageBox.Show(command.CommandText.ToString());
                    command.ExecuteNonQuery();
                    success = true;
                }
                catch (SQLiteException ex)
                {
                    success = false;
                    MessageBox.Show(ex.Message);
                }
                return success;
            }
        }

        private object ExecuteScalar(string query, params SQLiteParameter[] parameters)
        {
            object res = null;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    command.Parameters.AddRange(parameters);
                    //MessageBox.Show(command.CommandText.ToString());
                    res = command.ExecuteScalar();
                }
                catch (SQLiteException ex)
                {
                    res = 0;
                    MessageBox.Show(ex.Message);
                }
                return res;
            }
        }
    }
}
