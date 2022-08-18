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
using Android.Content;

namespace QuizAppMobile
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class DetailsQuestion : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {

        HttpClient client;
        quiz question;
        string uri;

        TextView txtId; 
        TextView txtIdQuestion; 
        TextView txtQuest;
        TextView txtRep1;
        TextView txtRep2;
        TextView txtRep3;
        TextView txtRep4;
        TextView txtBonneRep;
        TextView txtDesc;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main_details);

            //Récupération de l'Id de la question transmis 
            string id = Intent.GetStringExtra("id_Question");
            

            //Appel de l'API
            uri = "https://10.0.2.2:7032/api/Quiz/"+id;

            var handler = GetInsecureHandler();
            client = new HttpClient(handler);
            client.BaseAddress = new Uri(uri);
            recupAPI(uri);

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


        public async void recupAPI(string uri)
        {

            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);


                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    question = JsonConvert.DeserializeObject<quiz>(content);

                    txtId = FindViewById<TextView>(Resource.Id.textViewId);
                    txtIdQuestion = FindViewById<TextView>(Resource.Id.textViewIdQuestion);
                    txtQuest = FindViewById<TextView>(Resource.Id.textViewQuestion);
                    txtRep1 = FindViewById<TextView>(Resource.Id.textViewRep1);
                    txtRep2 = FindViewById<TextView>(Resource.Id.textViewRep2);
                    txtRep3 = FindViewById<TextView>(Resource.Id.textViewRep3);
                    txtRep4 = FindViewById<TextView>(Resource.Id.textViewRep4);
                    txtBonneRep = FindViewById<TextView>(Resource.Id.textViewBonneRep);
                    txtDesc = FindViewById<TextView>(Resource.Id.textViewDescription);

                    //Affichage des données à chaque champs approprié
                    txtId.Text = "ID question : " + question.Id;
                    txtIdQuestion.Text = "Numéro question : " + question.id_quiz;
                    txtQuest.Text = "Question : " + question.question;
                    txtRep1.Text = "Réponse 1 : " + question.reponse_1;
                    txtRep2.Text = "Réponse 2 : " + question.reponse_2;
                    txtRep3.Text = "Réponse 3 : " + question.reponse_3;
                    txtRep4.Text = "Réponse 4 : " + question.reponse_4;
                    txtBonneRep.Text = "Bonne réponse : " + question.bonne_reponse;
                    txtDesc.Text = "Description : " + question.description;


                    
                    Button btnModif = FindViewById<Button>(Resource.Id.btnUpdate);
                    Button btnSupprimer = FindViewById<Button>(Resource.Id.btnDelete);

                    btnModif.Click += BtnModif_Click;
                    btnSupprimer.Click += BtnSupprimer_Click;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Problème lors de la récupération des données", e);
            }


        }

        private void BtnSupprimer_Click(object sender, EventArgs e)
        {
            
            // Popup pour confirmation de suppression de la question
            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
            Android.App.AlertDialog alert = dialog.Create();
            alert.SetTitle("Confirmation");
            alert.SetMessage("Confirmez-vous la suppression ?");
            alert.SetIcon(Resource.Drawable.ic_dialog_alert);
            alert.SetButton("Oui", (c, ev) =>
            {
                //Méthode de suppression
                deleteQuestion();
            });
            alert.SetButton2("Annuler", (c, ev) => { });
            alert.Show();

        }

        //Appel activity UpdateQuestion
        private void BtnModif_Click(object sender, EventArgs e)
        {
            
            Intent nextActivity = new Intent(this, typeof(UpdateQuestion));
            nextActivity.PutExtra("id_Question", question.Id);
            StartActivity(nextActivity);
        }

        


        public HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { if (cert.Issuer.Equals("CN=localhost")) return true; return errors == System.Net.Security.SslPolicyErrors.None; }; return handler;
        }


        //Suppresion d'une question
        public async void deleteQuestion()
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    Intent nextActivity = new Intent(this, typeof(MainActivity2));
                    StartActivity(nextActivity);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
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

