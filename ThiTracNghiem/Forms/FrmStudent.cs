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
    public partial class FrmStudent : DevExpress.XtraEditors.XtraForm
    {
        bool editMode = false;
        int position = 0;
        int viTri = 0;
        public FrmStudent()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void sINHVIENBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsSV.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS_SERVER1);

        }

        private void FrmStudent_Load(object sender, EventArgs e)
        {
            dS_SERVER1.EnforceConstraints = false;
            this.sINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
            this.sINHVIENTableAdapter.Fill(this.dS_SERVER1.SINHVIEN);
            this.bANGDIEMTableAdapter.Connection.ConnectionString = Program.connstr;
            this.bANGDIEMTableAdapter.Fill(this.dS_SERVER1.BANGDIEM);
            this.v_DS_LOPTableAdapter.Connection.ConnectionString = Program.connstr;
            this.v_DS_LOPTableAdapter.Fill(this.dS_SERVER1.V_DS_LOP);
            gridView1.OptionsBehavior.Editable = false;

            
            if (Program.nhom == "TRUONG")
            {
                cmbCoSo.DataSource = Program.bds_dspm;
                cmbCoSo.DisplayMember = "TENCS";
                cmbCoSo.ValueMember = "TENSERVER";
                cmbCoSo.SelectedIndex = Program.mCoSo;
                cmbCoSo.Enabled = true;
                btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled =  btnGhi.Enabled = btnUndo.Enabled = false;
                btnReload.Enabled = btnPrint.Enabled = true;
            }
            else
            {
                cmbCoSo.Enabled = false;
                groupBox1.Enabled = false;
            }
        }

        private void cmbCoSo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCoSo.SelectedValue.ToString() == "System.Data.DataRowView")
                    return;
                Program.servername = cmbCoSo.SelectedValue.ToString();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            
            if (Program.KetNoi() == 0)
            {
                MessageBox.Show("Lỗi kết nối về chi nhánh mới", "", MessageBoxButtons.OK);
            }
            else
            {
                this.sINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                this.sINHVIENTableAdapter.Fill(this.dS_SERVER1.SINHVIEN);
                this.bANGDIEMTableAdapter.Connection.ConnectionString = Program.connstr;
                this.bANGDIEMTableAdapter.Fill(this.dS_SERVER1.BANGDIEM);
            }
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           // gridView1.OptionsBehavior.Editable = true;
            viTri = bdsSV.Position;
            groupBox1.Enabled = true;
            bdsSV.AddNew();
            cmbMALOP.DataSource = v_bdsMALOP;
            cmbMALOP.SelectedIndex = 0;
            dtpNGAYSINH.EditValue = "";
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReload.Enabled = btnPrint.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = true;
            gcSV.Enabled = false;
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            viTri = bdsSV.Position;
            groupBox1.Enabled = true;
            String maLop = cmbMALOP.SelectedValue.ToString();
            cmbMALOP.DataSource = v_bdsMALOP;
            cmbMALOP.SelectedValue = maLop;
            
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReload.Enabled = btnPrint.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = true;
            gcSV.Enabled = false;
        }
      
        private void btnUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bdsSV.CancelEdit();
            if (btnThem.Enabled == false) bdsSV.Position = viTri;
            gcSV.Enabled = true;
            groupBox1.Enabled = false;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReload.Enabled = btnPrint.Enabled = true;
            btnGhi.Enabled = btnUndo.Enabled = false;
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtMASV.Text.Trim() == "")
            {
                MessageBox.Show("Mã sinh viên không được phép rỗng", "", MessageBoxButtons.OK);
                txtMASV.Focus();
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
            if (dtpNGAYSINH.Text.Trim() == "")
            {
                MessageBox.Show("Ngày sinh không được phép rỗng", "", MessageBoxButtons.OK);
                dtpNGAYSINH.Focus();
                return;
            }
            
            //((DataRowView)bdsSV[0])["MALOP"] = cmbMALOP.SelectedText.ToString().Trim();
            if (!isExists())
            {
                try
                {
                    bdsSV.EndEdit();
                    bdsSV.ResetCurrentItem();
                    this.sINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.sINHVIENTableAdapter.Update(this.dS_SERVER1.SINHVIEN);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi ghi sinh viên.\n" + ex.Message, "", MessageBoxButtons.OK);
                    return;
                }
                gcSV.Enabled = true;
                btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = btnReload.Enabled = btnPrint.Enabled = true;
                btnGhi.Enabled = btnUndo.Enabled = false;

                groupBox1.Enabled = false;
            }
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.sINHVIENTableAdapter.Fill(this.dS_SERVER1.SINHVIEN);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Reload :" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            String masv = "";
            if (bdsBD.Count > 0)
            {
                MessageBox.Show("Không thể xóa sinh viên này vì có bảng điểm", "",
                       MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show("Bạn có thật sự muốn xóa sinh viên này ?? ", "Xác nhận",
                       MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    masv = ((DataRowView)bdsSV[bdsSV.Position])["MASV"].ToString(); // giữ lại để khi xóa bij lỗi thì ta sẽ quay về lại
                    bdsSV.RemoveCurrent();
                    this.sINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.sINHVIENTableAdapter.Update(this.dS_SERVER1.SINHVIEN);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa sinh viên. Bạn hãy xóa lại\n" + ex.Message, "",
                        MessageBoxButtons.OK);
                    this.sINHVIENTableAdapter.Fill(this.dS_SERVER1.SINHVIEN);
                    bdsSV.Position = bdsSV.Find("MASV", masv);
                    return;
                }
            }
            if (bdsSV.Count == 0) btnXoa.Enabled = false;
        }

        private bool isExists()
        {
            if (editMode && txtMASV.Text == ((DataRowView)bdsSV[position])["MASV"].ToString().Trim())
            {
                return false;
            }

            string cmd = "SELECT * FROM SINHVIEN WHERE MASV = '" + txtMASV.Text.Trim() + "'";
            SqlDataReader reader = Program.ExecSqlDataReader(cmd);
            if (reader != null && reader.HasRows)
            {
                DlgOk.Show("Mã sinh viên đã tồn tại", "Đóng");
                reader.Close();
                return true;
            }
            else if (Program.checkExistsAllSite(cmd))
            {
                reader.Close();
                DlgOk.Show("Mã sinh viên đã tồn tại trên cơ sở khác", "Đóng");
                return true;
            }
            reader.Close();
            return false;
        }

        private void gcSV_MouseClick(object sender, MouseEventArgs e)
        {
            this.position = bdsSV.Position;

        }
    }
}