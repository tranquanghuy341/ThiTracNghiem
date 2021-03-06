﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using System.Data.SqlClient;

namespace ThiTracNghiem.Forms
{
    public partial class FrmLogin : DevExpress.XtraEditors.XtraForm
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'tRACNGHIEMDataSet.V_DS_PHANMANH' table. You can move, or remove it, as needed.
            this.branchesTableAdapter.Fill(this.ds.V_DS_PHANMANH);
           
        }

        private void cbbCoSo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbbCoSo.SelectedValue.ToString() != null)
                {
                    Program.servername = cbbCoSo.SelectedValue.ToString();
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private bool Login(string username)
        {
            Boolean sinhvien = false;
            if (Program.password == "")
            {
                sinhvien = true;
                Program.username = Program.mlogin;
                Program.mlogin = Program.mCoSo == 0 ? "001" : "VT001";
                Program.password = Program.pwdSV;
            }
            if (Program.KetNoi() == 0) return false;
            String strLenh = "EXEC SP_DangNhap '" + Program.mlogin + "'";
            SqlDataReader reader = Program.ExecSqlDataReader(strLenh);
            if (reader == null)
                return false;
            reader.Read();
            if (reader.HasRows)
            {
                Program.donVi = Program.HasColumn(reader, "donvi") ? reader["donvi"].ToString() : "";
                Program.tenDonVi = Program.HasColumn(reader, "tendonvi") ? reader["tendonvi"].ToString() : "";
                Program.hoTen = reader["hoten"].ToString();
                Program.nhom = reader["nhom"].ToString();
                Program.maUser = Program.HasColumn(reader, "maUser") ? reader["maUser"].ToString():"";
                Program.id = username;
                reader.Close();
                if (!Program.nhom.Equals("SINHVIEN") && sinhvien)
                {
                    return false;
                } else if (sinhvien)
                {
                    strLenh = "EXEC SP_TimSV '" + username + "'";
                    reader = Program.ExecSqlDataReader(strLenh);
                    if (reader != null)
                    {
                        reader.Read();
                        if (reader.HasRows)
                        {
                            Program.donVi = reader["donvi"].ToString();
                            Program.tenDonVi = reader["tendonvi"].ToString();
                            Program.hoTen = reader["hoten"].ToString();
                            Program.id = username;
                        } else
                        {
                            return false;
                        }
                    }
                }
                Program.conn.Close();
                reader.Close();
            }
            else
            {
                Program.conn.Close();
                reader.Close();
                return false;
            }
            return true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool result = false;
            if (txtUsername.Text.Trim() == "")
            {
                MessageBox.Show("Khong duoc rong", "Loi");
                txtUsername.Focus();
                return;
            }
            Program.servername = cbbCoSo.SelectedValue.ToString();
            Program.mlogin = txtUsername.Text;
            Program.password = txtPassword.Text;
            Program.bds_dspm = branchesBindingSource;
            Program.mCoSo = cbbCoSo.SelectedIndex;
            if (!Login(Program.mlogin))
                MessageBox.Show("Sai thông tin tài khoản hoặc mật khẩu");
            else
            {
                result = true;
                Console.WriteLine("Login successful");
                Program.mloginDB = Program.mlogin;
                Program.mpasswordDB = Program.password;
            }
            if (result)
            {
                this.Hide();
                FrmMain frmMain = new FrmMain();
                frmMain.Closed += (s, args) => this.Close();
                frmMain.Show();
            }
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.KeyCode);
            if (e.KeyCode == Keys.Enter)
            {
                this.btnLogin_Click(sender, e);
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(e.KeyCode);
            if (e.KeyCode == Keys.Enter)
            {
                this.btnLogin_Click(sender, e);
            }
        }

        private void mbarExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}