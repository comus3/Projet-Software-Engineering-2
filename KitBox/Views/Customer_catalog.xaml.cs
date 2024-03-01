using Microsoft.Maui.Controls;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using DevTools;
using KitBox.AppServices ; 

namespace KitBox.Views
{
    public partial class Customer_catalog : ContentPage
    {
        private Casier casier;
        private Connection con;
        private Commande com;
        private object comPk;
        private List<Object> listArmoirePk;

        public Customer_catalog()
        {
            InitializeComponent();
            BindingContext = this;
            Connection.TestConnection();

            con = new Connection();

         
            com = new Commande(con);
            Dictionary<string, object> infoClient = new Dictionary<string, object>();
            infoClient["date"] = 240224;
            com.Update(infoClient);
            DataTable data = com.Insert();
            Displayer.DisplayData(data);
            FetchingServices.CurrentCommand = data.Rows[0].ItemArray[0].ToString();
            FetchingServices.CurrentCommand = data.Rows[0].ItemArray[0].ToString();
            listArmoirePk = new List<Object>();
        }

        private void OnCreateLockerClicked(object sender, EventArgs e)
    {
    if (lengthPicker.SelectedItem == null || widthPicker.SelectedItem == null || Locker_Color_Picker.SelectedItem == null)
    {
        
        DisplayAlert("Error", "Make sure to choose the both dimensions and a color.", "OK");
        return;
    }

    double width, length;
    string couleur = Locker_Color_Picker.SelectedItem.ToString();  


    if (!double.TryParse(lengthPicker.SelectedItem.ToString(), out length) || 
        !double.TryParse(widthPicker.SelectedItem.ToString(), out width) )
    {
       
        DisplayAlert("Error", "Please enter valid dimensions and color.", "OK");
        return;
    }
    
   
    if (width <= length && couleur != null)
    {
      
        Armoire armoire = new Armoire(con);
        Dictionary<string, object> values = new Dictionary<string, object>();
        values["longueur"] = length;
        values["largeur"] = width;
        values["couleur"] = couleur; 
        values["commande"] = FetchingServices.CurrentCommand;
        armoire.Update(values);

        listArmoirePk.Add(armoire.Insert().Rows[0].ItemArray[0]);
        Logger.WriteToFile(listArmoirePk);

        
        DisplayAlert("Success", $"Your Cabinet is\nLength: {length}, Width: {width}, Color: {couleur}\n\nThe cabinet has been created successfully!", "OK");

        object lastArmoireCree = listArmoirePk[listArmoirePk.Count - 1];
        Navigation.PushAsync(new Form(lastArmoireCree));
    }
    else
    {
   
        DisplayAlert("Error", "Width cannot be greater than length.", "OK");
    }
}


    }
}
