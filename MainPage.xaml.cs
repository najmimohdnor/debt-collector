using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.IO;

namespace Debt_Collector;

public partial class MainPage : ContentPage
{
    public ObservableCollection<Record> Records { get; set; } = new();
    public ObservableCollection<ActivityLog> ActivityLogs { get; set; } = new();
    private string fileName;

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
        Records.CollectionChanged += Records_CollectionChanged;
        UpdatePlaceholderVisibility();

        // Set the file path
        fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DebtRecords.txt");
    }

    private void Records_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdatePlaceholderVisibility();
    }

    private void UpdatePlaceholderVisibility()
    {
        PlaceholderFrame.IsVisible = Records.Count == 0;
    }

    private void OnAddRecord(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(PersonNameEntry.Text) ||
            DebtTypePicker.SelectedItem == null ||
            string.IsNullOrWhiteSpace(PhoneNumberEntry.Text) ||
            !decimal.TryParse(TotalDebtAmountEntry.Text, out var amount))
        {
            DisplayAlert("Error", "Please provide valid details.", "OK");
            return;
        }

        var record = new Record
        {
            Name = PersonNameEntry.Text,
            DebtType = DebtTypePicker.SelectedItem?.ToString() ?? string.Empty,
            PhoneNumber = PhoneNumberEntry.Text,
            DebtAmount = amount,
            Balance = amount,
            CreationDate = DateTime.Now.ToUniversalTime().AddHours(8)
        };

        Records.Insert(0, record);
        ActivityLogs.Insert(0, new ActivityLog
        {
            LogType = "Record Added",
            Name = record.Name,
            DebtType = record.DebtType,
            PhoneNumber = record.PhoneNumber,
            DebtAmount = amount,
            AmountBorrowed = amount,
            RemainingBalance = amount,
            ActionDate = DateTime.Now.ToUniversalTime().AddHours(8)
        });

        // Write the record to the text file
        var writerRecord = $"Name: {record.Name}\nDebt Type: {record.DebtType}\nPhone Number: {record.PhoneNumber}\nDebt Amount: {record.DebtAmount}\nBalance: {record.Balance}\nCreation Date: {record.CreationDate}\n";
        File.AppendAllText(fileName, writerRecord + Environment.NewLine);

        PersonNameEntry.Text = string.Empty;
        DebtTypePicker.SelectedItem = null;
        PhoneNumberEntry.Text = string.Empty;
        TotalDebtAmountEntry.Text = string.Empty;

        UpdatePlaceholderVisibility();
    }

    private void OnDelete(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Record record)
        {
            Records.Remove(record);
            ActivityLogs.Insert(0, new ActivityLog
            {
                LogType = "Record Deleted",
                Name = record.Name,
                DebtType = record.DebtType,
                PhoneNumber = record.PhoneNumber,
                DebtAmount = record.DebtAmount,
                RemainingBalance = 0,
                ActionDate = DateTime.Now.ToUniversalTime().AddHours(8)
            });

            UpdatePlaceholderVisibility();
        }
    }

    private async void OnPaid(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Record record)
        {
            string result = await DisplayPromptAsync("Payment",
                $"Enter payment amount (RM) for {record.Name}:",
                "OK", "Cancel",
                keyboard: Keyboard.Numeric);

            if (decimal.TryParse(result, out var payment) && payment > 0)
            {
                record.Balance -= payment;
                if (record.Balance < 0) record.Balance = 0;

                ActivityLogs.Insert(0, new ActivityLog
                {
                    LogType = "Payment Made",
                    Name = record.Name,
                    DebtType = record.DebtType,
                    PhoneNumber = record.PhoneNumber,
                    DebtAmount = record.DebtAmount,
                    AmountPaid = payment,
                    RemainingBalance = record.Balance,
                    ActionDate = DateTime.Now.ToUniversalTime().AddHours(8)
                });

                UpdatePlaceholderVisibility();
            }
            else
            {
                await DisplayAlert("Invalid Input", "Please enter a valid payment amount.", "OK");
            }
        }
    }

    private async void OnAddDebt(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is Record record)
        {
            string result = await DisplayPromptAsync("Add Debt",
                $"Enter additional debt amount (RM) for {record.Name}:",
                "OK", "Cancel",
                keyboard: Keyboard.Numeric);

            if (decimal.TryParse(result, out var additionalDebt) && additionalDebt > 0)
            {
                record.DebtAmount += additionalDebt;
                record.Balance += additionalDebt;

                ActivityLogs.Insert(0, new ActivityLog
                {
                    LogType = "Amount Borrowed",
                    Name = record.Name,
                    DebtType = record.DebtType,
                    PhoneNumber = record.PhoneNumber,
                    DebtAmount = record.DebtAmount,
                    AmountBorrowed = additionalDebt,
                    RemainingBalance = record.Balance,
                    ActionDate = DateTime.Now.ToUniversalTime().AddHours(8)
                });

                UpdatePlaceholderVisibility();
            }
            else
            {
                await DisplayAlert("Invalid Input", "Please enter a valid debt amount (RM).", "OK");
            }
        }
    }
    private async void OnEditProfileName(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("Edit Profile Name",
                                                 "Enter your Profile Name:",
                                                 "OK", "Cancel",
                                                 initialValue: ProfileNameLabel.Text);
        if (!string.IsNullOrWhiteSpace(result))
        {
            ProfileNameLabel.Text = result;
        }
    }
    private async void OnViewActivityLog(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ActivityLogPage(ActivityLogs));
    }
}