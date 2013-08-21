﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms; 
using System.Data.OleDb;
using System.Data.Common;
using System.Collections;
using TRPO.Controller;
using TRPO.Structures;
using System.IO;

namespace TRPO.View
{
    public partial class ChiefForm : Form, IOrderViewable, IDishManagable
    {
        OrderCookController ordCookContr;
        DishesManagementController dishesManagementContr;

        public ChiefForm(OrderCookController occ, DishesManagementController dmc)
        {
            InitializeComponent();
            ordCookContr = occ;
            dishesManagementContr = dmc;
            setDishInfo(new Dish());

        }

        public void updateOrderList(List<ChiefListEntry> list)
        {//TODO: отладить изменение количества элементов в заказе
            int selectedItem = 0;
            if (listView1.SelectedIndices.Count > 0)
            {
                selectedItem = listView1.SelectedIndices[0];
            }

            listView1.Items.Clear();
            String[] s;


            foreach (ChiefListEntry entry in list)
            {
                s = new String[] { entry.name, entry.need.ToString(), entry.ready.ToString(), entry.left.ToString() };
                ListViewItem tmp = new ListViewItem(s);
                listView1.Items.Add(tmp);
            }

            if ((listView1.Items.Count > 0) &&(!listView1.Focused || listView1.SelectedItems.Count <= 0))
            {
                if (listView1.Items.Count <= selectedItem)
                {
                    selectedItem = listView1.Items.Count - 1;
                }
                this.listView1.Focus();
                this.listView1.Items[selectedItem].Selected = true;
            }
            dishesManagementContr.updateDishInfo();
        }

        public void showMsg(String msg, GlobalObj.ErrorLevels el)
        {
            switch (el)
            {
                case GlobalObj.ErrorLevels.Critical:
                    MessageBox.Show(msg);
                    break;
                case GlobalObj.ErrorLevels.Info:
                    toolStripStatusLabel.Text = msg;
                    toolStripStatusLabel.Visible = true;
                    break;
            }
            
        }

        public void showMsg(String msg, String header)
        {
            MessageBox.Show(msg, header);
        }

        public String getSelectedDishName()
        {
            String result = "";
            switch (mainTab.SelectedIndex)
            {
                case 0:
                    result = listView1.SelectedItems.Count > 0 ? listView1.SelectedItems[0].Text : "";
                    break;
                case 1:
                    result = dishesDataGrid.SelectedRows.Count > 0 ? dishesDataGrid.SelectedRows[0].Cells[0].Value.ToString() : "";
                    break;
                case 2:
                    result = "";
                    break;
            }
            return result;
        }

        public void setDishInfo(Dish d)
        {
          
            dishName.Text = d.Name;
            dishTypeLabel.Text = d.DishType;
            receipeText.Text = d.Recipe;
            if (d.LinkToPhoto != "" && File.Exists(Properties.Settings.Default.dishesImagesFolderPath + d.LinkToPhoto))
            {

                dishPicture.Image = Image.FromFile(Properties.Settings.Default.dishesImagesFolderPath + d.LinkToPhoto);
            }
            else
            {
                dishPicture.Image = null;
            }

            dishConsistanceDataGrid.Rows.Clear();
            foreach (KeyValuePair<String, Double> p in d.Consistance)
            {
                dishConsistanceDataGrid.Rows.Add(p.Key, p.Value);
            }

        }

        public void setCreateDishInfo(Dish d)
        {

            createDishName.Text = d.Name;
            createDishType.Text = d.DishType;
            createDishRecipe.Text = d.Recipe;
            if (d.LinkToPhoto != "" && File.Exists(Properties.Settings.Default.dishesImagesFolderPath + d.LinkToPhoto))
            {

                createDishImage.Image = Image.FromFile(Properties.Settings.Default.dishesImagesFolderPath + d.LinkToPhoto);
            }
            else
            {
                createDishImage.Image = null;
            }

            createDishContentsDataGrid.Rows.Clear();
            foreach (KeyValuePair<String, Double> p in d.Consistance)
            {
                createDishContentsDataGrid.Rows.Add(p.Key, p.Value);
            }
        }

        public int getReadyDishesAmount()
        {
            return Convert.ToInt32(readyDishesAmount.Value);
        }

        public void setDishesList(Dictionary<String, String> dishesTypes)
        {
            dishesDataGrid.Rows.Clear();
            
            if (dishesTypes != null && dishesTypes.Count != 0)
            {
                foreach (KeyValuePair<String, String> p in dishesTypes)
                {
                    dishesDataGrid.Rows.Add(p.Key, p.Value);
                }
            }
        }

        public void setProductsList(List<String> pList)
        {
            productsDataGrid.Rows.Clear();
            if (pList != null && pList.Count != 0)
            {
                foreach (String s in pList)
                {
                    productsDataGrid.Rows.Add(s);
                }
            }
        }

        public void updateContents(Dictionary<String, Double> cont)
        {
            createDishContentsDataGrid.Rows.Clear();
            foreach(KeyValuePair<String, Double> p in cont)
            {
                createDishContentsDataGrid.Rows.Add(p.Key, p.Value);
            }
        }


        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            dishesManagementContr.updateDishInfo();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void readyButton_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel.Visible = false;
            dishesManagementContr.addReadyDishes();
            ordCookContr.updateOrderList();
        }

        private void mainTab_Selected(object sender, TabControlEventArgs e)
        {
            toolStripStatusLabel.Visible = false;
            dishesManagementContr.fillDishProd();
        }

        private void readyDishesAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                readyButton_Click(sender, new EventArgs());
                readyDishesAmount.Focus();
            }
        }

        private void dishesDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            //dishesManagementContr.updateContents();
            dishesManagementContr.updateCreationDishInfo();
        }

        private void createDishCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (createDishCheckBox.Checked)
            {
                tableLayoutPanel5.Enabled = false;
                createDishButton.Text = "Создать";

                createDishName.Text = "";
                createDishType.Text = "";
                createDishRecipe.Text = "";
                createDishImage.Image = null;
                createDishContentsDataGrid.Rows.Clear();

                dishesDataGrid.DefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                dishesDataGrid.DefaultCellStyle.ForeColor = Color.FromArgb(200, 200, 200);
            }
            else
            {
                tableLayoutPanel5.Enabled = true;
                createDishButton.Text = "Обновить";
                dishesManagementContr.updateCreationDishInfo();

                dishesDataGrid.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                dishesDataGrid.DefaultCellStyle.ForeColor = Color.FromArgb(0, 0, 0);
            }
        }

        private void createDishButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
