using MauiApp1_2026.Models;
using MauiApp1_2026.PageModels;

namespace MauiApp1_2026.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
    }
}