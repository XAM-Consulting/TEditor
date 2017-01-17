using System;

using Xamarin.Forms;

namespace TEditor.Forms.Sample
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            this.MainPage = new NavigationPage(new ContentPage { Content = new TEditorHtmlView(), BackgroundColor = Color.White });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

