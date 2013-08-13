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

namespace TRPO.View
{
    public partial class CourierForm : Form, IClientManagable, IOrderManagable
    {
        ClientManagementConroller clientManagementController;
        OrdersConroller ordersController;
        public CourierForm(ClientManagementConroller cmc, OrdersConroller oc)
        {
            InitializeComponent();
            clientManagementController = cmc;
            clientManagementController.addForm(this);
            ordersController = oc;
            ordersController.addForm(this);
            clientManagementController.fillCompanyList();
            
            //включаем автозаполнение и подключаем источник данных
            headerList1.DataSource = clientManagementController.companyList;  
            headerList1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            headerList1.AutoCompleteSource = AutoCompleteSource.ListItems;

            headerList2.DataSource = clientManagementController.employList;
            headerList2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            headerList2.AutoCompleteSource = AutoCompleteSource.ListItems;

            
        
            //headerList1.Items.Add("Кот котофей");
        }

        public void showMsg(String msg, GlobalObj.ErrorLevels el)
        {
            //throw ("не умею");
        }

        public void showMsg(String msg, String header)
        {
            MessageBox.Show(msg, header);
        }




        public int getIndexSelectedCompany()
        {
            return headerList1.SelectedIndex;
        }

        public void showMenuList(){}
        public void chowCurMenu(){}

        private void headerSearchButton2_Click(object sender, EventArgs e)
        {
            clientManagementController.fillEmployList();
        }



        private void headerList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            clientManagementController.fillEmployList();
            
        }
        public void updateEmployList()
        {
            headerList2.DataSource = null;
            headerList2.DataSource = clientManagementController.employList;
        }
        public void updateCompanyList()
        {
            headerList1.DataSource = null;
            headerList1.DataSource = clientManagementController.employList;
        }


        private void s(object sender, EventArgs e)
        {}
        private void headerList2_EnabledChanged(object sender, EventArgs e)
        {}
        private void headerList2_MouseClick(object sender, MouseEventArgs e)
        {}

    }

    public struct ClientData
    {
        public int id;
        public String data;
        public ClientData(int id_, String data_)
        {
            id = id_;
            data = data_;

        }
    }
}
