using CommunityToolkit.Mvvm.Input;
using MauiApp1_2026.Models;

namespace MauiApp1_2026.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}