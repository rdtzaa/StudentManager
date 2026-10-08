using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StudentManager.ViewModels;

public class StudentViewModel
{
    public string NIM { get; set; }
    public string Nama { get; set; }
    public string Jurusan { get; set; }
    public string Email { get; set; }
}
