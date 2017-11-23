﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;

namespace ThiTracNghiem.Forms
{
    public partial class FrmTeacher : DevExpress.XtraEditors.XtraForm
    {
        public FrmTeacher()
        {
            InitializeComponent();
        }

        private void gIAOVIENBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsGV.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS_SERVER1);

        }

        private void FrmTeacher_Load(object sender, EventArgs e)
        {
            dS_SERVER1.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS_SERVER1.GIAOVIEN' table. You can move, or remove it, as needed.
            this.gIAOVIENTableAdapter.Connection.ConnectionString = Program.connstr;
            this.gIAOVIENTableAdapter.Fill(this.dS_SERVER1.GIAOVIEN);
            if (Program.nhom == "COSO")
            {
                String dkien = "";
                String strLenh = "Select * from V_MAKH ";
                SqlDataReader reader = Program.ExecSqlDataReader(strLenh);
                Boolean rd = reader.Read();
                while (rd)
                {
                    dkien += "MAKH = '" + reader["MAKH"] + "'";
                    rd = reader.Read();
                    if (rd != false)
                        dkien += " OR ";
                }

                Program.conn.Close();
                reader.Close();
                bdsGV.Filter = dkien;
            }
            // TODO: This line of code loads data into the 'dS_SERVER1.BODE' table. You can move, or remove it, as needed.
            this.bODETableAdapter.Connection.ConnectionString = Program.connstr;
            this.bODETableAdapter.Fill(this.dS_SERVER1.BODE);
            // TODO: This line of code loads data into the 'dS_SERVER1.GIAOVIEN_DANGKY' table. You can move, or remove it, as needed.
            this.gIAOVIEN_DANGKYTableAdapter.Connection.ConnectionString = Program.connstr;
            this.gIAOVIEN_DANGKYTableAdapter.Fill(this.dS_SERVER1.GIAOVIEN_DANGKY);
           
            gridView1.OptionsBehavior.Editable = false;
            if (Program.nhom == "TRUONG")
            {
                
                btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReload.Enabled = btnPrint.Enabled = btnGhi.Enabled = btnUndo.Enabled = false;
            }
            groupBox1.Enabled = false;
        }
    }
}