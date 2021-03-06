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
        private int position = 0;
        private bool editMode = false;
        int viTri = 0;
        List<String> srcROLE;
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
            // TODO: This line of code loads data into the 'dS_SERVER1.V_DS_KHOA' table. You can move, or remove it, as needed.
            
            dS_SERVER1.EnforceConstraints = false;
            // TODO: This line of code loads data into the 'dS_SERVER1.GIAOVIEN' table. You can move, or remove it, as needed.
            this.gIAOVIENTableAdapter.Connection.ConnectionString = Program.connstr;
            this.gIAOVIENTableAdapter.Fill(this.dS_SERVER1.GIAOVIEN);
            if (Program.nhom == "COSO")
            {
                String dkien = "";
                String strLenh = "Select * from V_DS_KHOA ";
                SqlDataReader reader = Program.ExecSqlDataReader(strLenh);
                try
                {
                    if (reader != null && reader.HasRows)
                    {
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

                    Program.conn.Close();
                    reader.Close();
                } catch (Exception ex)
                {
                    DlgOk.Show(ex.ToString());
                }
                bdsGV.Filter = dkien;
                gbTAOLOGIN.Enabled = false;


               
            }
            // TODO: This line of code loads data into the 'dS_SERVER1.BODE' table. You can move, or remove it, as needed.
            this.v_DS_KHOATableAdapter.Connection.ConnectionString = this.bODETableAdapter.Connection.ConnectionString = Program.connstr;
            this.v_DS_KHOATableAdapter.Fill(this.dS_SERVER1.V_DS_KHOA);
            this.bODETableAdapter.Fill(this.dS_SERVER1.BODE);
            // TODO: This line of code loads data into the 'dS_SERVER1.GIAOVIEN_DANGKY' table. You can move, or remove it, as needed.
            this.gIAOVIEN_DANGKYTableAdapter.Connection.ConnectionString = Program.connstr;
            this.gIAOVIEN_DANGKYTableAdapter.Fill(this.dS_SERVER1.GIAOVIEN_DANGKY);
           
            gridView1.OptionsBehavior.Editable = false;
            if (Program.nhom == "TRUONG")
            {
                srcROLE = new List<String> { "TRUONG" };
                cmbROLE.DataSource = srcROLE;
                
                //cmbROLE.SelectedIndex = 0;
                cmbROLE.Enabled = false;
                btnTAOLOGIN.Enabled = true;
                btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReload.Enabled = btnPrint.Enabled = btnGhi.Enabled = btnUndo.Enabled = false;
            }
            else
            {
                srcROLE = new List<String> { "COSO","GIAOVIEN" };

                cmbROLE.DataSource = srcROLE;
                
                btnTAOLOGIN.Enabled = true;

            }
            groupBox1.Enabled = false;
            gbTAOLOGIN.Enabled = false;
            lbMAGV.Text = "";
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            viTri = bdsGV.Position;
            editMode = false;
            groupBox1.Enabled = true;
            cmbMaKH.DataSource = v_DS_KHOABindingSource;
            bdsGV.AddNew();
            cmbMaKH.SelectedIndex = 0;
            txtMAGV.Text = "";
            txtHO.Text = "";
            txtTEN.Text = "";
            txtDIACHI.Text = "";
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReload.Enabled = btnPrint.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = true;
            gcGV.Enabled = false;
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            editMode = true;
            cmbMaKH.DataSource = v_DS_KHOABindingSource;
            viTri = bdsGV.Position;
            groupBox1.Enabled = true;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReload.Enabled = btnPrint.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = true;
            gcGV.Enabled = false;
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtMAGV.Text.Trim() == "")
            {
                MessageBox.Show("Mã giáo viên không được phép rỗng", "", MessageBoxButtons.OK);
                txtMAGV.Focus();
                return;
            }
            if (txtHO.Text.Trim() == "")
            {
                MessageBox.Show("Họ không được phép rỗng", "", MessageBoxButtons.OK);
                txtHO.Focus();
                return;
            }
            if (txtTEN.Text.Trim() == "")
            {
                MessageBox.Show("Tên không được phép rỗng", "", MessageBoxButtons.OK);
                txtTEN.Focus();
                return;
            }
            if (txtDIACHI.Text.Trim() == "")
            {
                MessageBox.Show("Địa chỉ không được phép rỗng", "", MessageBoxButtons.OK);
                txtDIACHI.Focus();
                return;
            }
            
            //((DataRowView)bdsSV[0])["MALOP"] = cmbMALOP.SelectedText.ToString().Trim();
            if (!isExits())
            {
                try
                {
                    bdsGV.EndEdit();
                    bdsGV.ResetCurrentItem();
                    this.gIAOVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.gIAOVIENTableAdapter.Update(this.dS_SERVER1.GIAOVIEN);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi ghi giáo viên.\n" + ex.Message, "", MessageBoxButtons.OK);
                    return;
                }
                gcGV.Enabled = true;
                btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReload.Enabled = btnPrint.Enabled = true;
                btnGhi.Enabled = btnUndo.Enabled = false;

                groupBox1.Enabled = false;
            }
            
        }
        public static String timLoginName(String magv)
        {
            String LGNAME="";
            String strLenh = "EXEC SP_TIMLOGIN '" + magv + "'";
            SqlDataReader reader = Program.ExecSqlDataReader(strLenh);
            reader.Read();
            LGNAME = reader[0].ToString().Trim();
            Program.conn.Close();
            reader.Close();
            return LGNAME;
        }
        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String magv = "";


            if (bdsGVDK.Count > 0)
            {
                MessageBox.Show("Không thể xóa giáo viên này vì có đăng kí lớp thi", "",
                       MessageBoxButtons.OK);
                return;
            }
            if (bdsBODE.Count > 0)
            {
                MessageBox.Show("Không thể xóa giáo viên này vì có bộ đề", "",
                       MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show("Bạn có thật sự muốn xóa sinh viên này ?? ", "Xác nhận",
                       MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                   
                    magv = ((DataRowView)bdsGV[bdsGV.Position])["MAGV"].ToString(); // giữ lại để khi xóa bij lỗi thì ta sẽ quay về lại

                    

                    bdsGV.RemoveCurrent();
                    this.gIAOVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.gIAOVIENTableAdapter.Update(this.dS_SERVER1.GIAOVIEN);

                    String LGNAME = "";
                    LGNAME = timLoginName(magv);

                    String strLenh = "EXEC SP_XOALOGIN '" + LGNAME + "','"+magv+"'";
                    SqlDataReader reader = Program.ExecSqlDataReader(strLenh);
                    if(LGNAME=="TRUONG")
                    {
                        Program.servername1 = Program.servername;
                        for (int i = 0; i < Program.bds_dspm.Count; i++)
                        {
                            if (Program.servername1.ToString().Trim() != ((DataRowView)Program.bds_dspm[i])["TENSERVER"].ToString().Trim())
                            {
                                Program.mlogin = Program.remotelogin;
                                Program.password = Program.remotepassword;
                                Program.servername = ((DataRowView)Program.bds_dspm[i])["TENSERVER"].ToString().Trim();
                                if (Program.KetNoi() != 0)
                                {
                                    reader = Program.ExecSqlDataReader(strLenh);

                                    Program.conn.Close();
                                    reader.Close();
                                }
                            }
                        }
                        Program.mlogin = Program.mloginDB;
                        Program.password = Program.mpasswordDB;
                        Program.servername = Program.servername1;
                        this.gIAOVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                        //this.gIAOVIENTableAdapter.Fill(this.dS_SERVER1.GIAOVIEN);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa giáo viên. Bạn hãy xóa lại\n" + ex.Message, "",
                        MessageBoxButtons.OK);
                    this.gIAOVIENTableAdapter.Fill(this.dS_SERVER1.GIAOVIEN);
                    bdsGV.Position = bdsGV.Find("MAGV", magv);
                    return;
                }
            }

            if (bdsGV.Count == 0) btnXoa.Enabled = false;
        }

        private void btnUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bdsGV.CancelEdit();
            if (btnThem.Enabled == false) bdsGV.Position = viTri;
            gcGV.Enabled = true;
            groupBox1.Enabled = false;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReload.Enabled = btnPrint.Enabled = true;
            btnGhi.Enabled = btnUndo.Enabled = false;
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            try
            {
                this.gIAOVIENTableAdapter.Fill(this.dS_SERVER1.GIAOVIEN);
                if (Program.nhom == "COSO")
                {
                    String dkien = "";
                    String strLenh = "Select * from V_DS_KHOA ";
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

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Reload :" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
        }
        public static Boolean kiemTraLogin(String magv)
        {
            String strLenh = "EXEC SP_TIMUSER '" + magv + "'";
            SqlDataReader reader = Program.ExecSqlDataReader(strLenh);
            Boolean hasLogin = reader.Read();
            Program.conn.Close();
            reader.Close();
            return hasLogin;
        }
        private void btnTAOLOGIN_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            lbMAGV.Text = ""+((DataRowView)bdsGV[bdsGV.Position])["MAGV"].ToString();
            if (kiemTraLogin(lbMAGV.Text.ToString().Trim()))
                MessageBox.Show("USER ĐÃ TỒN TẠI", "ERROR", MessageBoxButtons.OK);
            else
            {
                gcGV.Enabled = false;
                btnTAOLOGIN.Enabled = false;
                gbTAOLOGIN.Enabled = true;

                btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnPrint.Enabled = btnGhi.Enabled = btnUndo.Enabled = false;
                btnReload.Enabled = false;
            }
        }

        private void btnCANCLEL_Click(object sender, EventArgs e)
        {

            gbTAOLOGIN.Enabled = false;
            gcGV.Enabled = true;
            lbMAGV.Text = "";
            if (Program.nhom == "TRUONG")
            {
                
                btnTAOLOGIN.Enabled = true;
                btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled =  btnPrint.Enabled = btnGhi.Enabled = btnUndo.Enabled = false;
                btnReload.Enabled = true;
            }
            else
            {
                btnTAOLOGIN.Enabled = true;
                btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnPrint.Enabled = btnGhi.Enabled = btnUndo.Enabled = true;
                btnReload.Enabled = true;
            }
        }

        private void btnDANGKY_Click(object sender, EventArgs e)
        {
            String LGNAME = txtLOGIN.Text.ToString().Trim();
            String PASS = txtPASSWORD.Text.ToString().Trim();
            String USERNAME = lbMAGV.Text.ToString();
            String ROLE = cmbROLE.SelectedValue.ToString().Trim();
            String strLenh = "EXEC SP_TAOLOGIN '" + LGNAME + "','" + PASS + "','" + USERNAME + "','" + ROLE + "'";

            SqlDataReader reader = Program.ExecSqlDataReader(strLenh);
            if (ROLE == "TRUONG")
            {
                Program.ExecSqlDataReaderAllSite(strLenh);
                this.gIAOVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                //this.gIAOVIENTableAdapter.Fill(this.dS_SERVER1.GIAOVIEN);
            }
            btnCANCLEL_Click(sender, e);
        }
        private bool isExits()
        {
            if (editMode && txtMAGV.Text.Trim() == ((DataRowView)bdsGV[position])["MAGV"].ToString().Trim())
            {
                return false;
            }

            string cmd = "SELECT * FROM GIAOVIEN WHERE MAGV = '" + txtMAGV.Text.Trim() + "'";
            SqlDataReader reader = Program.ExecSqlDataReader(cmd);
            if (reader != null && reader.HasRows)
            {
                DlgOk.Show("Mã giáo viên đã tồn tại", "Đóng");
                reader.Close();
                return true;
            }
            reader.Close();
            return false;
        }

        private void gcGV_MouseClick(object sender, MouseEventArgs e)
        {
            this.position = bdsGV.Position;
        }
    }
}