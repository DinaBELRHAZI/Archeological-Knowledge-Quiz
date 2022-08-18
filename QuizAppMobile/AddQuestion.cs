using System;
using System.Collections.Generic;
using System.Net.Http;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using Android.Widget;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Navigation;
using Google.Android.Material.Snackbar;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AndroidX.SwipeRefreshLayout.Widget;
using Android.Graphics;
using System.ComponentModel;
using System.Threading;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Android.Content;

namespace QuizAppMobile
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class AddQuestion : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {

        HttpClient client;
        string nbQ;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main_add);

            Button btn_Submit = FindViewById<Button>(Resource.Id.btnSubmit);
            btn_Submit.Click += btnSubmit_ItemClick;

            //Menu
            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            //Récupération du paramètre transmis
            nbQ = Intent.GetStringExtra("nb_questions");
        }

        private void btnSubmit_ItemClick(object sender, EventArgs e)
        {

            //Appel de l'API
            var uri = "https://10.0.2.2:7032/api/Quiz";

            var handler = GetInsecureHandler();
            client = new HttpClient(handler);
            client.BaseAddress = new Uri(uri);
            sendAPI(uri, nbQ);
        }

        public async void sendAPI(string uri, string nbQ)
        {

            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);


                EditText txtQuest = FindViewById<EditText>(Resource.Id.textViewQuestion);
                EditText txtRep1 = FindViewById<EditText>(Resource.Id.textViewRep1);
                EditText txtRep2 = FindViewById<EditText>(Resource.Id.textViewRep2);
                EditText txtRep3 = FindViewById<EditText>(Resource.Id.textViewRep3);
                EditText txtRep4 = FindViewById<EditText>(Resource.Id.textViewRep4);
                EditText txtBonneRep = FindViewById<EditText>(Resource.Id.textViewBonneRep);
                EditText txtDesc = FindViewById<EditText>(Resource.Id.textViewDescription);

                int nbQuest = Convert.ToInt32(nbQ) + 1;

                quiz addQuestion = new quiz
                {
                    id_quiz = nbQuest,
                    question = (string)txtQuest.Text,
                    reponse_1 = (string)txtRep1.Text,
                    reponse_2 = (string)txtRep2.Text,
                    reponse_3 = (string)txtRep3.Text,
                    reponse_4 = (string)txtRep4.Text,
                    bonne_reponse = Convert.ToInt32(txtBonneRep.Text),
                    description = (string)txtDesc.Text
                };

                var json = JsonConvert.SerializeObject(addQuestion);
                HttpContent httpContent = new StringContent(json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var resp = await client.PostAsync(new Uri(uri), httpContent);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("La question a été ajouté avec succès !");
                    Intent nextActivity = new Intent(this, typeof(MainActivity2));
                    StartActivity(nextActivity);
                }
                else
                {
                    Toast.MakeText(this, "L'ajout d'une question a échoué !", ToastLength.Short).Show();
                    Console.WriteLine("L'ajout d'une question a échoué !");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Problème lors de la récupération des données", e);
            }


        }

     


        public HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { if (cert.Issuer.Equals("CN=localhost")) return true; return errors == System.Net.Security.SslPolicyErrors.None; }; return handler;
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_list_questions)
            {
                Intent nextActivity = new Intent(this, typeof(MainActivity2));
                StartActivity(nextActivity);

            }
            if (id == Resource.Id.nav_add_question)
            {
                Intent nextActivity = new Intent(this, typeof(AddQuestion));
                StartActivity(nextActivity);

            }


            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }


    }
}

