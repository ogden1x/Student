using Student.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
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



        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
           
            // Инициализация тестовых данных
            InitializeTestData();
        }
        private void InitializeTestData()
        {
            Students = new ObservableCollection<Students>
        {
            new Students { LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Group = "ИТ-101" },
            new Students { LastName = "Петров", FirstName = "Петр", MiddleName = "Петрович", Group = "ИТ-101" },
            new Students { LastName = "Сидорова", FirstName = "Анна", MiddleName = "Сергеевна", Group = "ИТ-102" },
            new Students { LastName = "Кузнецов", FirstName = "Алексей", MiddleName = "Владимирович", Group = "ИТ-101" },
            new Students { LastName = "Смирнова", FirstName = "Елена", MiddleName = "Александровна", Group = "ИТ-103" },
            new Students { LastName = "Васильев", FirstName = "Дмитрий", MiddleName = "Олегович", Group = "ИТ-102" },
            new Students { LastName = "Павлова", FirstName = "Ольга", MiddleName = "Игоревна", Group = "ИТ-103" },
            new Students { LastName = "Николаев", FirstName = "Сергей", MiddleName = "Михайлович", Group = "ИТ-101" },
            new Students { LastName = "Федорова", FirstName = "Мария", MiddleName = "Дмитриевна", Group = "ИТ-102" },
            new Students { LastName = "Морозов", FirstName = "Андрей", MiddleName = "Викторович", Group = "ИТ-103" },
            new Students { LastName = "Волкова", FirstName = "Наталья", MiddleName = "Анатольевна", Group = "ИТ-101" },
            new Students { LastName = "Алексеев", FirstName = "Владимир", MiddleName = "Сергеевич", Group = "ИТ-102" },
            new Students { LastName = "Лебедева", FirstName = "Татьяна", MiddleName = "Владимировна", Group = "ИТ-103" },
            new Students { LastName = "Семенов", FirstName = "Артем", MiddleName = "Ильич", Group = "ИТ-101" },
            new Students { LastName = "Егорова", FirstName = "Кристина", MiddleName = "Павловна", Group = "ИТ-102" }
        };
        }
        // Реализация INotifyPropertyChanged
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

        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
