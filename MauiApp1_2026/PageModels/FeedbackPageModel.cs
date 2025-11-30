using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MauiApp1_2026.PageModels
{
    public partial class FeedbackPageModel : ObservableObject
    {
        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private string _content = string.Empty;

        public FeedbackPageModel()
        {
        }

        [RelayCommand]
        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(Title) && string.IsNullOrWhiteSpace(Content))
            {
                await AppShell.DisplayToastAsync("Please provide feedback before sending.");
                return;
            }

            // TODO: Persist or send feedback via service. For now, show confirmation.
            await Shell.Current.GoToAsync("..?refresh=true");
            await AppShell.DisplayToastAsync("Feedback submitted. Thank you!");
        }
    }
}
