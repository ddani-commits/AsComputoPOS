using System.Collections.ObjectModel;
using System.Diagnostics;
using UiDesktopApp1.Data;
using UiDesktopApp1.Models;
using Wpf.Ui;
using UiDesktopApp1.Controls;

namespace UiDesktopApp1.ViewModels.Pages
{
    // Now every view model must inherit from ViewModel class
    public partial class EmployeesViewModel : ViewModel
    {
        // Employee list stays here, we will use it later 
        public ObservableCollection<Employee> EmployeesList { get; } = new();

        // this allows us to interact with the dialog 
        private readonly IContentDialogService _contentDialogService;

        // dialog serve is needed as an argument to get it from the service provider
        public EmployeesViewModel(IContentDialogService contentDialogService)
        {
            _contentDialogService = contentDialogService;
            LoadEmployees();
        }

        [RelayCommand]
        private async Task OnShowSignInContentDialog()
        {
            if (_contentDialogService.GetDialogHost() is not null)
            {
                //var termsOfUseContentDialog = new TermsOfUseContentDialog(contentDialogService.GetDialogHost());
                //_ = await termsOfUseContentDialog.ShowAsync();

                // Example of how to open a content dialog, a dialog must be created. examples are in Controls folder
                var newEmployeeContentDialog = new NewEmployeeContentDialog(_contentDialogService.GetDialogHost(), AddEmployee);
                _ = await newEmployeeContentDialog.ShowAsync();
            }
        }

        // RelaysCommands remain the same
        [RelayCommand]
        public void SaveEmployees()
        {
            using var db = new ApplicationDbContext();
            foreach (var employee in EmployeesList)
            {
                db.Employees.Update(employee);
            }
            db.SaveChanges();
            Debug.WriteLine("Saved from ViewModel");
        }

        private void LoadEmployees()
        {
            using var db = new ApplicationDbContext();
            foreach (var employee in db.Employees)
            {
                EmployeesList.Add(employee);
            }
        }

        [RelayCommand]
        public void AddEmployee(Employee CurrentEmployee)
        {
            using (var context = new ApplicationDbContext())
            {
                context.Employees.Add(CurrentEmployee);
                context.SaveChanges();
                EmployeesList.Add(CurrentEmployee);
            }
        }
    }
}
