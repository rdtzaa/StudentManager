using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager.Models;

public class Student
{
    public int Id { get; set; }
    public string NIM { get; set; }
    public string Nama { get; set; }
    public string Jurusan { get; set; }
    public string Email { get; set; }
}
