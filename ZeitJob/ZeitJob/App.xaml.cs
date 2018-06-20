using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ZeitJob
{
	public partial class App : Application
	{
        public static PublicClientApplication PCA { get; set; }
        public static object AuthenticationClient { get; internal set; }

        public static string Tenant = "tiagoleichs.onmicrosoft.com";
        public static string ClientId = "ba17d3c3-4fc5-4480-bc44-9e827acbead8";
        public static string SignInPolicy = "B2C_1_verbinden";

        public static string AuthorityBase = $"https://login.microsoftonline.com/tfp/{Tenant}/";
        public static string Authority = $"{AuthorityBase}{SignInPolicy}";
        public static string[] Scopes = { "https://tiagoleichs.onmicrosoft.com/ZeitArbeitapi/read" };

        public static UIParent UiParent = null;

        public App()
        {
            InitializeComponent();

            PCA = new PublicClientApplication(ClientId, Authority);
            PCA.RedirectUri = $"msal{ClientId}://auth";

            MainPage = new NavigationPage(new MainPage());
        }


        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
