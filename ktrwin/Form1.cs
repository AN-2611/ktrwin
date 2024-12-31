using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApp13
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadLoaiSach();
            LoadDanhSachSach();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void LoadLoaiSach()
        {
            using (var db = new ModelSach())
            {
                var list = db.LoaiSaches.ToList();
                cmbTheLoai.DataSource = list;
                cmbTheLoai.DisplayMember = "TenLoai";
                cmbTheLoai.ValueMember = "MaLoai";
            }
        }
        private void LoadDanhSachSach()
        {
            using (var context = new ModelSach())
            {
                var list = context.Saches.ToList();
                dtgSach.DataSource = list;
            }
        }

        private void dtgSach_SelectionChanged(object sender, EventArgs e)
        {
            if (dtgSach.SelectedRows.Count > 0)
            {
                var selectedRow = dtgSach.SelectedRows[0];
                var selectedBook = (Sach)selectedRow.DataBoundItem;
                txtMaSach.Text = selectedBook.MaSach;
                txtTenSach.Text = selectedBook.TenSach;
                txtNamXB.Text = selectedBook.NamXB.ToString();
                cmbTheLoai.SelectedValue = selectedBook.MaLoai;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dtgSach.SelectedRows.Count > 0)
            {
                var selectedRow = dtgSach.SelectedRows[0];
                var selectedBook = (Sach)selectedRow.DataBoundItem;

                using (var context = new ModelSach())
                {
                    var bookToDelete = context.Saches.Find(selectedBook.MaSach);
                    if (bookToDelete != null)
                    {
                        var result = MessageBox.Show("Bạn có muốn xóa không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            context.Saches.Remove(bookToDelete);
                            context.SaveChanges();
                            LoadDanhSachSach();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Sách cần xóa không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSach.Text) || string.IsNullOrWhiteSpace(txtTenSach.Text) || string.IsNullOrWhiteSpace(txtNamXB.Text) || cmbTheLoai.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtMaSach.Text.Length != 6)
            {
                MessageBox.Show("Mã sách phải có 6 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var context = new ModelSach())
            {
                var newBook = new Sach
                {
                    MaSach = txtMaSach.Text,
                    TenSach = txtTenSach.Text,
                    NamXB = int.Parse(txtNamXB.Text),
                    MaLoai = (int)cmbTheLoai.SelectedValue
                };

                context.Saches.Add(newBook);
                context.SaveChanges();
                MessageBox.Show("Thêm mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDanhSachSach();
                ResetInputControls();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSach.Text) || string.IsNullOrWhiteSpace(txtTenSach.Text) || string.IsNullOrWhiteSpace(txtNamXB.Text) || cmbTheLoai.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtMaSach.Text.Length != 6)
            {
                MessageBox.Show("Mã sách phải có 6 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var context = new ModelSach())
            {
                var bookToUpdate = context.Saches.Find(txtMaSach.Text);
                if (bookToUpdate != null)
                {
                    bookToUpdate.TenSach = txtTenSach.Text;
                    bookToUpdate.NamXB = int.Parse(txtNamXB.Text);
                    bookToUpdate.MaLoai = (int)cmbTheLoai.SelectedValue;

                    context.SaveChanges();
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDanhSachSach();
                    ResetInputControls();
                }
                else
                {
                    MessageBox.Show("Sách cần cập nhật không tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void ResetInputControls()
        {
            txtMaSach.Clear();
            txtTenSach.Clear();
            txtNamXB.Clear();