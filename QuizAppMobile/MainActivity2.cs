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
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity2 : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private SwipeRefreshLayout refreshLayout;

        HttpClient client;
        List<quiz> listQuestions;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main2);
            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            //Appel de l'API
            var uri = "https://10.0.2.2:7032/api/Quiz";

            var handler = GetInsecureHandler();
            client = new HttpClient(handler);
            client.BaseAddress = new Uri(uri);
            recupAPI(uri);


            //button pour ajouter une question
            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            //Menu
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);


        }





        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            // Nombre total de questions
            var countQ = listQuestions.Count;

            Intent nextActivity = new Intent(this, typeof(AddQuestion));
            nextActivity.PutExtra("nb_questions", countQ.ToString());
            StartActivity(nextActivity);
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


        public async void recupAPI(string uri)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);


                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();


                    ListView lst1 = FindViewById<ListView>(Resource.Id.listView1);
                    listQuestions = JsonConvert.DeserializeObject<List<quiz>>(content);
                    List<string> items = new List<String>();
                    foreach (var en in listQuestions)
                    {
                        items.Add(en.id_quiz.ToString() + ". " + en.question);
                    }
                    var ListAdapter = new Android.Widget.ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);
                    lst1.SetAdapter(ListAdapter);

                    lst1.ItemClick += lst1_ItemClick;

                    

                    //Rafraichissement de la page 
                    refreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);
                    refreshLayout.SetColorSchemeColors(Color.Red, Color.Green, Color.Blue, Color.Yellow);
                    refreshLayout.Refresh += RefreshLayout_Refresh;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Problème lors de la récupération des données", e);
            }

            
        }

       

        private void lst1_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            
            var position = e.Position;
            var item = listQuestions[position] as quiz;
            var id = item.Id;
            //Toast.MakeText(this, "Id = "+id, ToastLength.Short).Show();

            Intent nextActivity = new Intent(this, typeof(DetailsQuestion));
            nextActivity.PutExtra("id_Question", id);
            StartActivity(nextActivity);
        }





        //Rafraichissement de la page 
        private void RefreshLayout_Refresh(object sender, EventArgs e)
        {
            //Data Refresh Place  
            BackgroundWorker work = new BackgroundWorker();
            work.DoWork += Work_DoWork;
            work.RunWorkerCompleted += Work_RunWorkerCompleted;
            work.RunWorkerAsync();
        }
        private void Work_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            refreshLayout.Refreshing = false;
        }
        private void Work_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(1000);
        }


        public HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { if (cert.Issuer.Equals("CN=localhost")) return true; return errors == System.Net.Security.SslPolicyErrors.None; }; return handler;
        }



        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

