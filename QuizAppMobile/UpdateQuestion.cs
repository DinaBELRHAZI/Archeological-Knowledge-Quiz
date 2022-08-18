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
    public class UpdateQuestion : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {

        HttpClient client;
        quiz question;
        string uri;

        string idQ;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            //Appel de la view
            SetContentView(Resource.Layout.activity_main_update);

            //Récupération del'id de la question transmise 
            idQ = Intent.GetStringExtra("id_Question");


            //Appel de l'API
            uri = "https://10.0.2.2:7032/api/Quiz/" + idQ;

            var handler = GetInsecureHandler();
            client = new HttpClient(handler);
            client.BaseAddress = new Uri(uri);

            //Méthode pour afficher les données
            showDataAPI(uri);


            //Click sur le bouton pour enregistrer les modifications
            Button btn_Submit = FindViewById<Button>(Resource.Id.btnSubmit);
            btn_Submit.Click += btnSubmit_Click;

            //Menu
            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);


        }


        public async void showDataAPI(string uri)
        {

            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);

                string content = await response.Content.ReadAsStringAsync();
                question = JsonConvert.DeserializeObject<quiz>(content);

                TextView txtId = FindViewById<TextView>(Resource.Id.textViewId);
                EditText txtQuest = FindViewById<EditText>(Resource.Id.textViewQuestion);
                EditText txtRep1 = FindViewById<EditText>(Resource.Id.textViewRep1);
                EditText txtRep2 = FindViewById<EditText>(Resource.Id.textViewRep2);
                EditText txtRep3 = FindViewById<EditText>(Resource.Id.textViewRep3);
                EditText txtRep4 = FindViewById<EditText>(Resource.Id.textViewRep4);
                EditText txtBonneRep = FindViewById<EditText>(Resource.Id.textViewBonneRep);
                EditText txtDesc = FindViewById<EditText>(Resource.Id.textViewDescription);

                //Affichage des données à chaque champs approprié
                txtId.Text = "ID question : " + question.Id;
                txtQuest.Text = question.question;
                txtRep1.Text = question.reponse_1;
                txtRep2.Text = question.reponse_2;
                txtRep3.Text = question.reponse_3;
                txtRep4.Text = question.reponse_4;
                txtBonneRep.Text = question.bonne_reponse.ToString();
                txtDesc.Text = question.description;

            }
            catch (Exception e)
            {
                Console.WriteLine("Problème lors de la récupération des données", e);
            }

        }


        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // Popup pour confirmation de l'enregistrement des modifications
            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
            Android.App.AlertDialog alert = dialog.Create();
            alert.SetTitle("Confirmation");
            alert.SetMessage("Confirmez-vous l'enregistrement des modifications ?");
            alert.SetIcon(Resource.Drawable.ic_dialog_alert);
            alert.SetButton("Oui", (c, ev) =>
            {
                var handler = GetInsecureHandler();
                client = new HttpClient(handler);
                client.BaseAddress = new Uri(uri);
                //Appel de la méthode pour l'update
                sendAPI(uri);
            });
            alert.SetButton2("Annuler", (c, ev) => { });
            alert.Show();

            
        }


        public async void sendAPI(string uri)
        {

            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);



                string content = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(content);
                var finaltest = json.ToString();

                quiz quizToUpdate = JsonConvert.DeserializeObject<quiz>(content);


                EditText txtQuest = FindViewById<EditText>(Resource.Id.textViewQuestion);
                EditText txtRep1 = FindViewById<EditText>(Resource.Id.textViewRep1);
                EditText txtRep2 = FindViewById<EditText>(Resource.Id.textViewRep2);
                EditText txtRep3 = FindViewById<EditText>(Resource.Id.textViewRep3);
                EditText txtRep4 = FindViewById<EditText>(Resource.Id.textViewRep4);
                EditText txtBonneRep = FindViewById<EditText>(Resource.Id.textViewBonneRep);
                EditText txtDesc = FindViewById<EditText>(Resource.Id.textViewDescription);

                quizToUpdate.id_quiz = quizToUpdate.id_quiz; 
                quizToUpdate.question = (string)txtQuest.Text;
                quizToUpdate.reponse_1 = (string)txtRep1.Text;
                quizToUpdate.reponse_2 = (string)txtRep2.Text;
                quizToUpdate.reponse_3 = (string)txtRep3.Text;
                quizToUpdate.reponse_4 = (string)txtRep4.Text;
                quizToUpdate.bonne_reponse = Convert.ToInt32(txtBonneRep.Text);
                quizToUpdate.description = (string)txtDesc.Text;



                var jsonConv = JsonConvert.SerializeObject(quizToUpdate);
                HttpContent httpContent = new StringContent(jsonConv);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var resp = await client.PutAsync(new Uri(uri), httpContent);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("La question a été ajouté avec succès !");
                    Intent nextActivity = new Intent(this, typeof(MainActivity2));
                    StartActivity(nextActivity);
                }
                else
                {
                    Toast.MakeText(this, "La modification d'une question a échoué !", ToastLength.Short).Show();
                    Console.WriteLine("La modification de la question a échoué !");
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

