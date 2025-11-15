using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace Debt_Collector;

public partial class ActivityLogPage : ContentPage
{
    private ObservableCollection<ActivityLog> _logs;

    public ActivityLogPage(ObservableCollection<ActivityLog> logs)
    {
        InitializeComponent();
        _logs = logs;
        BindingContext = this;
        ActivityLogList.ItemsSource = _logs;
        UpdateButtonsVisibility();
    }

    private void OnDeleteLog(object sender, EventArgs e)
    {
        if (sender is ImageButton button && button.CommandParameter is ActivityLog log)
        {
            _logs.Remove(log);
            UpdateButtonsVisibility();
        }
    }

    private void OnClearAll(object sender, EventArgs e)
    {
        _logs.Clear();
        UpdateButtonsVisibility();
    }

    private void UpdateButtonsVisibility()
    {
        ClearAllButton.IsVisible = _logs.Count > 0;
        PlaceholderFrame.IsVisible = _logs.Count == 0;
    }
}