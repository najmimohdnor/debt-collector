using Microsoft.Maui.ApplicationModel.Communication;

namespace Debt_Collector;

public partial class About : ContentPage
{
    public About()
    {
        InitializeComponent();
    }

    private void OnEmailClicked(object sender, EventArgs e)
    {
        try
        {
            var email = new EmailMessage
            {
                Subject = "Inquiry",
                Body = "",
                To = new List<string> { "justnamilol@gmail.com" }
            };
            Email.ComposeAsync(email);
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", "Unable to send email: " + ex.Message, "OK");
        }
    }

    private void OnPhoneClicked(object sender, EventArgs e)
    {
        try
        {
            PhoneDialer.Open("0196053192");
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", "Unable to make a call: " + ex.Message, "OK");
        }
    }
}