using DevExpress.XtraEditors.Repository;
using PMS.Business;
using PMS.Business.Models;
using PMS.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanLyNangSuat
{
    public partial class frmSize : Form
    {
        public frmSize()
        {
            InitializeComponent();
        }

        private void frmSize_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void LoadGrid()
        {
            try
            {
                gridSize.DataSource = null;
                var items = new List<SizeModel>();
                items.Add(new SizeModel() { Id = 0, Name = "" });
                items.AddRange(BLLSize.Instance.Gets());
                gridSize.DataSource = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy danh sách mã hàng.\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int Id = 0;
                int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString(), out Id);
                if (Id != 0)
                    SaveSize();
            }
            catch (Exception ex)
            {
            }
        }

        private void SaveSize()
        {
            int Id = 0;
            int.TryParse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString(), out Id);
            if (string.IsNullOrEmpty(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Name").ToString()))
                MessageBox.Show("Vui lòng nhập tên Size.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                var obj = new P_Size();
                obj.Id = Id;
                obj.Name = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Name").ToString();
                obj.Note = gridView.GetRowCellValue(gridView.FocusedRowHandle, "Note") != null ? gridView.GetRowCellValue(gridView.FocusedRowHandle, "Note").ToString() : "";

                var rs = BLLSize.Instance.InsertOrUpdate(obj);
                if (rs.IsSuccess)
                {
                    LoadGrid();
                }
                else
                    MessageBox.Show(rs.Messages[0].msg, rs.Messages[0].Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (Convert.ToInt32(gridView.GetRowCellValue(e.RowHandle, "Id")) == 0 && e.Column.Caption == "")
            {
                //  e.RepositoryItem.Appearance.Image = 
                RepositoryItemButtonEdit ritem = new RepositoryItemButtonEdit();
                ritem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
                ritem.ReadOnly = true;
                ritem.Buttons[0].Image = global::QuanLyNangSuat.Properties.Resources.if_plus_sign_173078;
                ritem.Buttons[0].ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
                ritem.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
                ritem.Buttons[0].Enabled = true;

                ritem.Click += ritem_Click;
                e.RepositoryItem = ritem;
            }
        }

        private void ritem_Click(object sender, EventArgs e)
        {
            SaveSize();
        }

        private void repbtnDelete_Click(object sender, EventArgs e)
        {
            int Id = int.Parse(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Id").ToString());
            if (Id != 0)
            {
                if (MessageBox.Show("Bạn có muốn xoá không?", "Xoá đối tượng", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var result = BLLSize.Instance.Delete(Id);
                    if (result.IsSuccess)
                        LoadGrid();
                    else
                        MessageBox.Show(result.Messages[0].msg, result.Messages[0].Title);
                }
            }
        }
    }
}
