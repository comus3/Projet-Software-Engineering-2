using DAL;
using Org.BouncyCastle.Utilities;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using DevTools;
using AppServices;



namespace KitBox.Views
{
    public partial class Form : ContentPage
    {
        private Object armoirePk;
        private Connection con;
        private List<CasierData> casiersData = new List<CasierData>();

        int count = 0; // Compteur pour les casiers

        string[] options = { "marron", "white" };
        List<string> doorOptions = new List<string> { "marron", "white" }; // Supprimer l'option "glass" du picker de couleur de la porte

        List<int> heigth = new List<int> { 32, 42, 52 };

        public Form(Object armoirePk)
        {
            this.armoirePk = armoirePk;
            InitializeComponent();
            BindingContext = this;
            Connection.TestConnection();
            con = new Connection();
            
        }

        private void OnCreateNewLockerClicked(object sender, EventArgs e)
        {
            count++;
            Label newLabel = new Label();
            newLabel.Text = "Locker number " + count.ToString();
            newLabel.FontAttributes = FontAttributes.Bold;
            newLabel.HorizontalOptions = LayoutOptions.Center;

            Label heigthLabel = new Label();
            heigthLabel.Text = "Height " + count;
            Picker heightPicker = new Picker();
            foreach (int elem in heigth)
            {
                heightPicker.Items.Add(elem.ToString());
            }

            Label colorLabel = new Label();
            colorLabel.Text = "Choose a color : ";

            Picker colorPicker = new Picker();
            foreach (string option in options)
            {
                colorPicker.Items.Add(option);
            }

            Label doorLabel = new Label();
            doorLabel.Text = "Include a door";
            doorLabel.VerticalOptions = LayoutOptions.Center;
            CheckBox checkBox = new CheckBox();

            // Nouvelle checkbox pour la porte en verre
            Label glassDoorLabel = new Label();
            glassDoorLabel.Text = "Glass door";
            glassDoorLabel.IsVisible = false; 
            glassDoorLabel.VerticalOptions = LayoutOptions.Center;
            CheckBox glassCheckBox = new CheckBox();
            glassCheckBox.IsVisible = false; // Caché par défaut

            // Logique pour afficher la checkbox de porte en verre lorsque la checkbox de porte est cochée
            checkBox.CheckedChanged += (s, args) =>
            {
                bool isChecked = checkBox.IsChecked;
                glassCheckBox.IsVisible = isChecked; // Rend la checkbox de porte en verre visible si la checkbox de porte est cochée
                glassDoorLabel.IsVisible = isChecked; // Rend le label de porte en verre visible si la checkbox de porte est cochée
            };

            HorizontalStackLayout stackPanel = new HorizontalStackLayout();
            stackPanel.Children.Add(doorLabel);
            stackPanel.Children.Add(checkBox);
            stackPanel.Children.Add(glassDoorLabel);
            stackPanel.Children.Add(glassCheckBox);

            Label colorDoorLabel = new Label();
            colorDoorLabel.Text = "Choose a color for the door number " + count.ToString() + " : ";
            colorDoorLabel.VerticalOptions = LayoutOptions.Center;
            Picker colorDoorPicker = new Picker();
            foreach (string option in doorOptions)
            {
                colorDoorPicker.Items.Add(option);
            }

            checkBox.CheckedChanged += (sender, args) =>
            {
                bool isChecked = checkBox.IsChecked;
                if (isChecked)
                {
                    stackPanel.Children.Add(colorDoorLabel);
                    stackPanel.Children.Add(colorDoorPicker);
                }
                else
                {
                    stackPanel.Children.Remove(colorDoorLabel);
                    stackPanel.Children.Remove(colorDoorPicker);
                }
            };

            // Ajoutez également une logique pour gérer la sélection de la checkbox pour la porte en verre
            glassCheckBox.CheckedChanged += (sender, args) =>
            {
                bool isChecked = glassCheckBox.IsChecked;
                if (isChecked)
                {
                    // Si la checkbox pour la porte en verre est cochée, supprimez le picker de couleur de la porte
                    stackPanel.Children.Remove(colorDoorLabel);
                    stackPanel.Children.Remove(colorDoorPicker);
                }
                else
                {
                    // Sinon, si la checkbox pour la porte en verre est décochée, ajoutez le picker de couleur de la porte
                    if (checkBox.IsChecked)
                    {
                        stackPanel.Children.Add(colorDoorLabel);
                        stackPanel.Children.Add(colorDoorPicker);
                    }
                }
            };

            labelContainer.Children.Add(newLabel);
            labelContainer.Children.Add(heigthLabel);
            labelContainer.Children.Add(heightPicker);
            labelContainer.Children.Add(colorLabel);
            labelContainer.Children.Add(colorPicker);
            labelContainer.Children.Add(stackPanel);

            // Ajouter les données du casier actuel à la liste casiersData
            casiersData.Add(new CasierData
            {
                Color = colorPicker,
                DoorColor = colorDoorPicker,
                Height = heightPicker,
                CheckBox = checkBox,
                GlassCheckBox = glassCheckBox // Enregistrez la checkbox pour la porte en verre
            });
           
        }

        private async void OnFinishClicked(object sender, EventArgs e)
        {
            // Check if no lockers have been created
            if (casiersData.Count == 0)
            {
                await DisplayAlert("Error", "You need to create at least one locker.", "OK");
                return;
            }

            // Vérifier si les données de chaque casier sont valides et les ajouter à la base de données
            foreach (var casierData in casiersData)
            {
                if (casierData.Color.SelectedItem == null || casierData.Height.SelectedItem.ToString() == null)
                {
                    await DisplayAlert("Error", "Make sure to choose a color and enter height", "OK");
                    return;
                }

                createCasier(casierData);
            }
            Armoire armoire = new Armoire(con);
            armoire.Load(Convert.ToInt32(this.armoirePk));
            LinkingServices.CreateAllArmoireLinks(con, armoire);

            await Navigation.PushAsync(new FinishPage());
        }


        private void createCasier(CasierData casierData)
        {
            Casier casier = new Casier(con);
            Dictionary<string, object> infoCasier = new Dictionary<string, object>();

            infoCasier["couleur"] = casierData.Color.SelectedItem.ToString();
            infoCasier["h"] = casierData.Height.SelectedItem.ToString();
            infoCasier["porte"] = casierData.CheckBox.IsChecked;
            if (casierData.CheckBox.IsChecked)
            {
                if (casierData.GlassCheckBox.IsChecked)
                {
                    infoCasier["couleur_porte"] = "glass";
                }
                else
                {
                    // Sinon, définissez la couleur de la porte à partir du picker de couleur de la porte
                    infoCasier["couleur_porte"] = casierData.DoorColor.SelectedItem.ToString();
                }
            }
            // Si la checkbox pour la porte en verre est cochée, définissez la couleur de la porte comme null

            infoCasier["armoire"] = this.armoirePk;
            casier.Update(infoCasier);
            casier.Insert();
        }

        private class CasierData
        {
            public Picker Color { get; set; }
            public Picker DoorColor { get; set; }
            public Picker Height { get; set; }
            public CheckBox CheckBox { get; set; }
            public CheckBox GlassCheckBox { get; set; } // Ajoutez la checkbox pour la porte en verre
        }
    }
}








// checkBox.CheckedChanged += (sender, e) => {
		// 	bool isChecked = ((CheckBox)sender).IsChecked;
		// 	if (isChecked)
		// 	{
		// 		StackLayout doorStack = new StackLayout();
		// 		doorStack.Children.Add(colorDoor);
		// 		doorStack.Children.Add(colorDoorPicker);
		// 		labelContainer.Children.Insert(labelContainer.Children.IndexOf(stackPanel) + 1, doorStack);
		// 	}
		// 	else
		// 	{
		// 		foreach (View item in labelContainer.Children)
		// 		{
		// 			if (item is StackLayout && ((StackLayout)item).Children.Contains(colorDoor) && ((StackLayout)item).Children.Contains(colorDoorPicker))
		// 			{
		// 				labelContainer.Children.Remove(item);
		// 				break;
		// 			}
		// 		}
		// 	}
		// };