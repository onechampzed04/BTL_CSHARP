﻿using BTL_2.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTL_2.View
{
    public partial class SupplierForm : Form
    {
        public SupplierForm()
        {
            InitializeComponent();
            AddressController addressController = new AddressController(cbxProvince, cbxDistrict, cbxWard);
            //addressController.SetEvent();

            SupplierController supplierController = new SupplierController(this, SupplierDataGridView,lbSupplierId,btnDelete,btnUpdate,btnInsert,txtPhone,txtSupplierName,cbxProvince,cbxDistrict,cbxWard, txtEmail,cbxTieuchi,btnSearch,txtSearchContent);
            supplierController.SetEvent();
        }
    }
}
