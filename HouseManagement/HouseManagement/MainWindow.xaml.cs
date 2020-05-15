using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using System.Data;
using System.Reflection;

namespace HouseManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int counter = 1;

        DataTable dataTable = new DataTable();
        public string[]státuszok { get; set; }

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "9bgyN3qT9Qp5YdFzR2bszMi4qCfuiyWOo16fw936",
            BasePath = "https://flatify-f6e2b.firebaseio.com/"
        };

        IFirebaseClient client; 

        public MainWindow()
        {
            InitializeComponent();

            státuszok = new string[] { "Első kapcsolatfelvétel megtörtént, bemutatkozó anyag kiküldve", "Árajánlat kiküldve", "Személyes egyeztetés megtörtént", "Megválasztásra előkészítve", "Sikeres megválasztás, szerződés", "Sikertelen megválasztás"};

            DataContext = this;
            Data dd = new Data();
            foreach (PropertyInfo prop in dd.GetType().GetProperties())
            {
                dataTable.Columns.Add(prop.Name);
            }
            recordsTable.DataContext = dataTable.DefaultView;
        }


        private void mainPage_Loaded(object sender, RoutedEventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);

            if (client != null)
            {
                MessageBox.Show("Minden cool tesa");
            }
            else
            {
                MessageBox.Show("SHIT!!");
            }

        }

        private async void addButton_Click(object sender, EventArgs e)
        {
            var data1 = new Data
            {
                Id = id.Text,
                Státusz = státusz.SelectedValue,
                HonnanÉrkezett = honnanÉrkezett.Text ?? null,
                MikorÉrkezett = mikorÉrkezett.Text,
                KontaktSzemély = kontakt.Text,
                SzvbTag = szvb.Text,
                Telefonszáma = telefon.Text,
                Email = email.Text,
                JutalékrólTud = jutalékrólTud.Text,
                KiajánlottÁr = kiajánlottÁr.Text,
                HázNeve = házNeve.Text,
                HázTerülete = terület.Text,
                HázIrányítószáma = irányítószám.Text,
                Város = város.Text,
                Utca = utca.Text,
                Házszám = házszám.Text,
                AlbetétekSzáma = albetétek.Text,
                ElsőKontaktDátuma = elsőKontakt.Text,
                ElsőFlatifyKontakt = elsőFlatifyKontakt.Text,
                Egyéb = egyéb.Text,
                ReferenciaTelefonszám = referenciaTelefon.Text,
                MegválasztóKözgyűlésIdőpontja = megválasztóKözgyűlés.Text,
                Sikeres = sikeres.Text
            };

            SetResponse response = await client.SetTaskAsync<Data>("DataRecords/" + id.Text, data1);
            Data result = response.ResultAs<Data>();

            counter++;
            MessageBox.Show("Data was added " +  result.Id);
            Export();
        }

        private async void Export()
        {
            dataTable.Rows.Clear();
            for(int i = 1; i < counter; i++)
            {
                FirebaseResponse resp = await client.GetTaskAsync("DataRecords/" + i);
                Data responseObject = resp.ResultAs<Data>();

                DataRow dataRow = dataTable.NewRow();
                foreach (PropertyInfo prop in responseObject.GetType().GetProperties())
                {
                    dataRow[prop.Name] = prop.GetValue(responseObject);

                }
                dataTable.Rows.Add(dataRow);
            }
        }

    }
}
