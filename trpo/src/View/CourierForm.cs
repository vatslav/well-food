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


namespace trpo.View
{
    public partial class CourierForm : Form
    {

        private OleDbConnection dbConnection = null;//соединение

        public CourierForm(OleDbConnection con)
        {
            InitializeComponent();
            dbConnection = con;
        }
        
    }
}