using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TamoPOS.Controls;
using TamoPOS.Data;
using TamoPOS.Models;
namespace TamoPOS.ViewModels.Pages
{
    public partial class ProductDetailViewModel : ViewModel
    {
        private ApplicationDbContext _applicationDbContext = new ApplicationDbContext();
        [ObservableProperty]
        private string? _idText;
        [ObservableProperty]
        private Product? _currentProduct;

    }
}
