using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using StudentManager.Models;
using System.Security.Cryptography.Xml;

namespace StudentManager.Data
{
    public class StudentRepository
    {
        private string connectionString =
            @"Server=(localdb)\MSSQLLocalDB;
                Database=StudentDB
                Trusted_Connection=True;";
        public List<Student> GetAll()
        {
            var students = new List<Student>();
            string sql = "Select Id, NIM, Nama, Jurusan, Email " + "FROM Students ORDER BY NIM";
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(sql, connection);
            connection.Open();

            using var reader = command.ExecuteReader();
            while(reader.Read())
            {
                students.Add(new Student
                {
                    Id = (int)reader["Id"],
                    NIM = reader["NIM"].ToString()!,
                    Nama = reader["Nama"].ToString()!,
                    Jurusan = reader["Reader"].ToString()!,
                    Email = reader["Email"].ToString()!,
                });
            }
            return students;
        }
    }

}


