using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace DrugsInfo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
            List<string> genericgroup = new List<string>()
            {
                "--Select--",
                "Aceclofenac",
                "Albendazole",
                "Ambraxol",
                "Antacid",
                "Azithromycin",
                "Diclofenac",
                "Baclofen",
                "Bilastine",
                "Cefixime",
                "Loratadine",
                "Menthol",
                "Metronidazole",
                "Omeprazole",
                "Paracetamol",
                "Pantoprazole",
                "Rabeprazole",
                "Zinc Oxide"

            };
            this.cmbGroup.ItemsSource = genericgroup;
            cmbGroup.SelectedItem = "--Select--";

            List<string> shelfs = new List<string>()
            {
                "--Select--",
                "A-001",
                "A-002",
                "B-001",
                "C-001",
                "D-001",
                "E-001",
                "F-001",
                "G-001",
                "H-001",
                "J-001",
                "K-001",
                "L-001",
                "M-001",
                "Q-001",
                "R-001"
            };
            this.cmbShelf.ItemsSource = shelfs;
            cmbShelf.SelectedItem = "--Select--";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Drug drugs = new Drug();
                drugs.DrugID = int.Parse(txtID.Text);
                drugs.DrugName = txtName.Text;
                drugs.GenericGroup = cmbGroup.Text;
                drugs.ShelfNo = cmbShelf.Text;
                drugs.UnitPrice = double.Parse(txtUnitPrice.Text);

                var newDrugsDetails = "{'DrugID':'" + drugs.DrugID + "','DrugName':'" + drugs.DrugName + "','GenericGroup':'" + drugs.GenericGroup + "','ShelfNo':'" + drugs.ShelfNo + "','UnitPrice':'" + drugs.UnitPrice + "'}";

                var drugsData = File.ReadAllText(@"DrugInformation.json");
                var drugsObj = JObject.Parse(drugsData);
                var drugsArray = drugsObj.GetValue("Drug") as JArray;
                var newDrugs = JObject.Parse(newDrugsDetails);
                drugsArray.Add(newDrugs);

                drugsObj["Drug"] = drugsArray;
                string saveData = JsonConvert.SerializeObject(drugsObj, Formatting.Indented);
                File.WriteAllText(@"DrugInformation.json", saveData);
                MessageBox.Show("Data saved successfully");
                Reset();
				ViewData();
                listDrugs.Visibility = Visibility.Visible;

            }
            catch (Exception)
            {

                MessageBox.Show("Data doesn't save!!!");
                Reset();
            }
            
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            listDrugs.Visibility = Visibility.Visible;
            ViewData();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var drugsData = File.ReadAllText(@"DrugInformation.json");
                var drugsObj = JObject.Parse(drugsData);
                JArray drugUpdateArray = (JArray)drugsObj["Drug"];

                var DrugID = Convert.ToInt32(txtID.Text);
                var DrugName = txtName.Text;
                var GenericGroup = cmbGroup.Text;
                var ShelfNo = cmbShelf.Text;
                var UnitPrice = Convert.ToDouble(txtUnitPrice.Text);

                foreach (var drugs in drugUpdateArray.Where(drg => drg["DrugID"].Value<int>() == DrugID))
                {
                    drugs["DrugName"] = !string.IsNullOrEmpty(DrugName) ? DrugName : "";
                    drugs["GenericGroup"] = !string.IsNullOrEmpty(GenericGroup) ? GenericGroup : "";
                    drugs["ShelfNo"] = !string.IsNullOrEmpty(ShelfNo) ? ShelfNo : "";
                    drugs["UnitPrice"] = !double.IsNaN(UnitPrice) ? UnitPrice : 0;
                }

                drugsObj["Drug"] = drugUpdateArray;
                string updateData = JsonConvert.SerializeObject(drugsObj, Formatting.Indented);
                File.WriteAllText(@"DrugInformation.json", updateData);
                MessageBox.Show("Data updated successfully!!!");
                Reset();
                ViewData();
            }
            catch (Exception)
            {

                MessageBox.Show("Data doesn't update!!!");
                Reset();
            }
        }
        

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            BtnSave.IsEnabled = false;
            BtnUpdate.Visibility = Visibility.Visible;
            Button b = sender as Button;
            Drug drugBtn = b.CommandParameter as Drug;
            int drugID = drugBtn.DrugID;
            txtID.Text = drugID.ToString();
            txtName.Text = drugBtn.DrugName.ToString();
            cmbGroup.Text = drugBtn.GenericGroup.ToString();
            cmbShelf.Text = drugBtn.ShelfNo.ToString();
            txtUnitPrice.Text = drugBtn.UnitPrice.ToString();
            txtID.IsEnabled = false;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var drugsData = File.ReadAllText(@"DrugInformation.json");
            var drugsObj = JObject.Parse(drugsData);
            JArray drugArray = (JArray)drugsObj["Drug"];
            Button b = sender as Button;
            Drug drugBtn = b.CommandParameter as Drug;
            int drugID = drugBtn.DrugID;

            try
            {
                if (drugID > 0)
                {
                    var drugsDelete = drugArray.FirstOrDefault(drg => drg["DrugID"].Value<int>() == drugID);
                    drugArray.Remove(drugsDelete);
                    string newResult = JsonConvert.SerializeObject(drugsObj, Formatting.Indented);
                    File.WriteAllText(@"DrugInformation.json", newResult);
                    MessageBox.Show("Data deleted successfully!!!");
                    ViewData();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Data can't be deleted or invalid input!!!");
            }
            
        }
        private void ViewData()
        {
            var drugsData = File.ReadAllText(@"DrugInformation.json");
            var drugsObj = JObject.Parse(drugsData);
            if (drugsObj != null)
            {
                JArray drugArray = (JArray)drugsObj["Drug"];
                List<Drug> drugs = new List<Drug>();
                foreach (var drg in drugArray)
                {
                    drugs.Add(new Drug() { DrugID = Convert.ToInt32(drg["DrugID"]), DrugName = drg["DrugName"].ToString(), GenericGroup = drg["GenericGroup"].ToString(), ShelfNo = drg["ShelfNo"].ToString(), UnitPrice = Convert.ToDouble(drg["UnitPrice"]) });
                }
                listDrugs.ItemsSource = drugs;

            }
        }
        private void Reset()
        {
            txtID.Clear();
            txtName.Clear();
            txtUnitPrice.Clear();
            cmbGroup.Text = "--Select--";
            cmbShelf.Text = "--Select--";
            BtnSave.IsEnabled = true;
            BtnUpdate.Visibility = Visibility.Collapsed;
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }
    }
}
