using Fiap.Cursos.Mobile.Views;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModernHttpClient;

using Xamarin.Forms;

namespace Fiap.Cursos.Mobile
{
    public class App : Application
    {
        public static MobileServiceClient _client;

        public static MobileServiceClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = new MobileServiceClient("https://fiap01.azurewebsites.net/", new NativeMessageHandler());
                }

                return _client;
            }
        }

        public App()
        {
            MainPage = new NavigationPage(new CursosView());
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
