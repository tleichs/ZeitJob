using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ZeitJob
{
    public partial class MainPage : ContentPage
    {
        private AuthenticationResult authenticationResult;

        public MainPage()
        {
            InitializeComponent();

            this.BindingContext = this;
            this.IsBusy = false;
            btnsignup.Clicked += OnSignIn;
            btnsignup.Clicked += Btnsignup_Indicator;
            btntodo.Clicked += todo;
        }

        private void Btnsignup_Indicator(object sender, EventArgs e)
        {
            this.IsBusy = true;
        }

        public async void todo(object sender, EventArgs e)
        {
            this.IsBusy = true;
            await Navigation.PushAsync(new ProfilPage1());


        }

        protected override async void OnAppearing()
        {
            UpdateSignInState(false);

            // Check to see if we have a User
            // in the cache already.
            try
            {
                AuthenticationResult ar = await App.PCA.AcquireTokenSilentAsync(App.Scopes, GetUserByPolicy(App.PCA.Users, App.SignInPolicy), App.Authority, false);
                UpdateUserInfo(ar);
                UpdateSignInState(true);
            }

            catch (Exception ex)
            {
                // Uncomment for debugging purposes
                //await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");

                // Doesn't matter, we go in interactive mode
                UpdateSignInState(false);
            }


        }
        private IUser GetUserByPolicy(IEnumerable<IUser> users, string policy)
        {
            foreach (var user in users)
            {
                string userIdentifier = Base64UrlDecode(user.Identifier.Split('.')[0]);
                if (userIdentifier.EndsWith(policy.ToLower())) return user;
            }

            return null;
        }
        private string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());
            return decoded;
        }
        public void UpdateUserInfo(AuthenticationResult ar)
        {
            JObject user = ParseIdToken(ar.IdToken);
            lblName.Text = user["name"]?.ToString();
        }

        JObject ParseIdToken(string idToken)
        {
            // Get the piece with actual user info
            idToken = idToken.Split('.')[1];
            idToken = Base64UrlDecode(idToken);
            return JObject.Parse(idToken);
        }
        void UpdateSignInState(bool isSignedIn)
        {
            slUser.IsVisible = isSignedIn;
            btnsignup.Text = isSignedIn ? "Sign out" : "Sign in";
            this.IsBusy = false; 
        }

        async void OnSignIn(object sender, EventArgs e)
        {
          

            try
            {
                if (btnsignup.Text == "Sign in")
                {
                    AuthenticationResult ar = await App.PCA.AcquireTokenAsync(App.Scopes, GetUserByPolicy(App.PCA.Users, App.SignInPolicy), App.UiParent);
                    UpdateUserInfo(ar);
                    UpdateSignInState(true);

                }
                else
                {
                    foreach (var user in App.PCA.Users)
                    {
                        App.PCA.Remove(user);
                    }
                    UpdateSignInState(false);
                }
            }
            catch (Exception ex)
            {
                //await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
                // Checking the exception message 
                // should ONLY be done for B2C
                // reset and not any other error.
                //if (ex.Message.Contains("AADB2C90118"))
                //    OnPasswordReset();
                //// Alert if any exception excludig user cancelling sign-in dialog
                //else if (((ex as MsalException)?.ErrorCode != "authentication_canceled"))
                //    await DisplayAlert($"Exception:", ex.ToString(), "Dismiss");
            }

        }
    }

}
