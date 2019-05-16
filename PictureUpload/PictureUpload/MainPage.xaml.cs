using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PictureUpload
{
    public partial class MainPage : ContentPage
    {
        private static ImageSource emailPic;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void TakePic_Pressed(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            await DisplayAlert("File Location", file.Path, "OK");

            emailPic = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            imgCurrentPic.Source = emailPic;

            try
            {
                string EmailUsername = "YOUR EMAIL";
                string EmailPassword = "YOUR PASSWORD";

                MailMessage message = new MailMessage
                {
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8,
                    From = new MailAddress(EmailUsername),
                    Sender = new MailAddress(EmailUsername),
                    Subject = "Business Trip: " + DateTime.Today.ToString(),
                    Body = "Whatever"
                };
                message.To.Add(EmailUsername);

                System.IO.Stream imageStream = file.GetStream();

                var image = new Attachment(imageStream, DateTime.Today.ToString() + ".jpg");

                message.Attachments.Add(image);

                NetworkCredential myCreds = new NetworkCredential(EmailUsername, EmailPassword);
                SmtpClient smtp = new SmtpClient("smtp.gmail.com")
                {
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    Credentials = myCreds,
                    Port = 587
                };
                smtp.Send(message);


                await DisplayAlert("Done", null, "qwewqewq");
            }
            catch(Exception ex)
            {
                await DisplayAlert("ERROR:", ex.ToString(), "OK");
            }

        }

        private void BtnSendPic_Pressed(object sender, EventArgs e)
        {



            
        }
    }
}
