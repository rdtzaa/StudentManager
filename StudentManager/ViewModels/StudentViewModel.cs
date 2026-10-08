using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using StudentManager.Data;
using StudentManager.Models;

namespace StudentManager.ViewModels;

public class StudentViewModel : INotifyPropertyChanged
{
    private readonly StudentRepository _repository;

    public ObservableCollection<Student> Students { get; } = new();
    public string[] DaftarJurusan { get; } = { "Informatika", "Sistem Informasi", "Manajemen", "Akuntansi" };
    public string[] DaftarGender { get; } = { "Laki-laki", "Perempuan" };
    public string SearchText { get; set; } = "";

    private Student? _selectedStudent;
    public Student? SelectedStudent
    {
        get => _selectedStudent;
        set
        {
            _selectedStudent = value;
            OnPropertyChanged();
        }
    }

    public int TotalStudents => Students.Count;
    public int TotalInformatika => Students.Count(x => x.Jurusan == "Informatika");
    public int TotalSistemInformasi => Students.Count(x => x.Jurusan == "Sistem Informasi");
    public int TotalLakiLaki => Students.Count(x => x.Gender == "Laki-laki");
    public int TotalPerempuan => Students.Count(x => x.Gender == "Perempuan");

    public ICommand SaveCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand SearchCommand { get; }

    public StudentViewModel()
    {
        _repository = new StudentRepository();
        SaveCommand = new RelayCommand(Save);
        DeleteCommand = new RelayCommand(Delete);
        ResetCommand = new RelayCommand(Reset);
        SearchCommand = new RelayCommand(Search);
        LoadData();
        Reset();
    }

    private void LoadData()
    {
        try
        {
            var students = _repository.GetAll();
            Students.Clear();
            foreach (var student in students)
                Students.Add(student);

            RefreshStatistics();
        }
        catch (SqlException ex)
        {
            MessageBox.Show($"Data gagal dimuat: {ex.Message}", "Database",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Save()
    {
        if (SelectedStudent == null) return;

        if (string.IsNullOrWhiteSpace(SelectedStudent.NIM) ||
            string.IsNullOrWhiteSpace(SelectedStudent.Nama) ||
            string.IsNullOrWhiteSpace(SelectedStudent.Jurusan) ||
            string.IsNullOrWhiteSpace(SelectedStudent.Gender))
        {
            MessageBox.Show("NIM, Nama, Jurusan, dan Gender wajib diisi.", "Validasi",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            if (SelectedStudent.Id == 0)
                _repository.Insert(SelectedStudent);
            else
                _repository.Update(SelectedStudent);
        }
        catch (SqlException ex)
        {
            MessageBox.Show($"Data gagal disimpan: {ex.Message}", "Simpan",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        LoadData();
        Reset();
    }

    private void Delete()
    {
        if (SelectedStudent == null || SelectedStudent.Id == 0) return;

        try
        {
            _repository.Delete(SelectedStudent.Id);
        }
        catch (SqlException ex)
        {
            MessageBox.Show($"Data gagal dihapus: {ex.Message}", "Hapus",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        LoadData();
        Reset();
    }

    private void Search()
    {
        try
        {
            var result = string.IsNullOrWhiteSpace(SearchText)
                ? _repository.GetAll()
                : _repository.Search(SearchText);

            Students.Clear();
            foreach (var student in result)
                Students.Add(student);

            RefreshStatistics();
        }
        catch (SqlException ex)
        {
            MessageBox.Show($"Pencarian gagal: {ex.Message}", "Cari",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Reset()
    {
        SelectedStudent = new Student();
    }

    private void RefreshStatistics()
    {
        OnPropertyChanged(nameof(TotalStudents));
        OnPropertyChanged(nameof(TotalInformatika));
        OnPropertyChanged(nameof(TotalSistemInformasi));
        OnPropertyChanged(nameof(TotalLakiLaki));
        OnPropertyChanged(nameof(TotalPerempuan));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
