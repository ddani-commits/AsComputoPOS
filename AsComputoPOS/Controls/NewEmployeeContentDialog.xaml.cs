using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using TamoPOS.Models;
using Wpf.Ui.Controls;

namespace TamoPOS.Controls
{
    public partial class NewEmployeeContentDialog : ContentDialog
    {
        // Declare form variables, there might be a better way to declare them, for now stays like this
        private string _name = "";
        private string _lastName = "";
        private string _email = "";
        private string _password = "";
        private string _confirmPassword = "";

        public string NameText
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string LastNameText
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        public string EmailText
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string PasswordText
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string ConfirmPasswordText
        {
            get => _confirmPassword;
            set { _confirmPassword = value; OnPropertyChanged(); }
        }

        // Declare a delegate to save employees, this will be used in the ViewModel
        // In javascript terms, this is a function that can be passed as an argument
        // The function takes an argument of type Employee
        private readonly Action<Employee>? _saveEmployees;
        public NewEmployeeContentDialog(ContentPresenter? contentPresenter, Action<Employee>? saveEmployees = null) : base(contentPresenter)
        {
            InitializeComponent();
            _saveEmployees = saveEmployees;
            DataContext = this; 
        }

        protected override void OnButtonClick(ContentDialogButton button)
        {
            if (button == ContentDialogButton.Primary)
            {
                // Perform save operation
                var employee = new Employee(NameText, LastNameText, EmailText);
                _saveEmployees?.Invoke(employee);
                base.OnButtonClick(button);
                Debug.WriteLine("primary button clicked");
            }
            else if (button == ContentDialogButton.Secondary)
            {
                // Secondary operation
                Debug.WriteLine("Secondary button clicked");
            }
            else if (button == ContentDialogButton.Close)
            {
                // Close dialog without saving
                base.OnButtonClick(button);
                Debug.WriteLine("Cancel button clicked");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        // Notify property changes for data binding
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
