using Student.DocumentGenerator;
using Student.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;


namespace Student
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Students> _students;
        public ObservableCollection<Students> Students
        {
            get => _students;
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }

        private ObservableCollection<Students> _bufferStudents;
        public ObservableCollection<Students> BufferStudents
        {
            get => _bufferStudents;
            set
            {
                _bufferStudents = value;
                OnPropertyChanged(nameof(BufferStudents));
            }
        }



        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = this;
            Dtp_DateSigning.SelectedDate = DateTime.Now;

            Students = new ObservableCollection<Students>
            {
                new Students { LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Group = "ИТ-101" },
                new Students { LastName = "Петров", FirstName = "Петр", MiddleName = "Петрович", Group = "ИТ-101" },
                new Students { LastName = "Сидорова", FirstName = "Анна", MiddleName = "Сергеевна", Group = "ИТ-102" },

            };

            BufferStudents = new ObservableCollection<Students>();




           
        }
     

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Вы уверены, что хотите закрыть приложение?",
                "Подтверждение закрытия",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void Btn_DownloadFile_Click(object sender, RoutedEventArgs e)
        {
            DateTime selectedDate;
            
            if (string.IsNullOrEmpty(Txb_Description.Text))
            {
                MessageBox.Show("Напиши чонить");
                return;
            }
            if (string.IsNullOrEmpty(Dtp_DateSigning.Text))
            {
                selectedDate = DateTime.Now;
            }
            else
            {
                selectedDate = Dtp_DateSigning.SelectedDate.GetValueOrDefault();
            }
            if (Ckb_Deminova.IsChecked != true && Ckb_Scvorcova.IsChecked != true)
            {
                MessageBox.Show("Выбери главную тётю");
                return;
            }


            string employeeFullname;
            string employeePost;
            if (Ckb_Deminova.IsChecked == true)
            {
                employeeFullname = "Деминова Имя Отчество";
                employeePost = "ЫЫЫЫЫЫЫЫЫЫЫ";
            }
            else
            {
                employeeFullname = "Скворцова Имя Отчество";
                employeePost = "ААААААААААААААААААААА";
            }
                





            DocumentWordGenerator.SaveAs(templatePath: "D:\\Coding\\ProjVS\\ogden1x\\Student\\Student\\Templates\\pattern.docx",
                $"{employeePost}",
                $"{employeeFullname}",
                $"{Txb_Description.Text}",
                $"{selectedDate.ToString("dd.mm.yyyy")}",
                _bufferStudents.Select(s => $"{s.LastName} {s.FirstName} {s.MiddleName} {s.Group}").ToList());
        }

        private void Btn_Dg_SelectStudent_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItem is Students selected)
            {
                _students.Remove(selected);
                _bufferStudents.Add(selected);
            }
        }

        private void Btn_Dg_UnSelectStudent_Click(object sender, RoutedEventArgs e)
        {
            if (Dg_Buffer.SelectedItem is Students selected)
            {
                _bufferStudents.Remove(selected);
                _students.Add(selected);
            }
        }

        private void Btn_ClearBuffer_Click(object sender, RoutedEventArgs e)
        {
            if (_bufferStudents == null || _bufferStudents.Count == 0)
                return;

            // Переносим всех студентов из буфера обратно
            foreach (var s in _bufferStudents.ToList())
            {
                _students.Add(s);
            }

            // Очищаем буфер
            _bufferStudents.Clear();
        }
    }
}
