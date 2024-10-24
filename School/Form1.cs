using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using School.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace School
{
    public partial class Form1 : Form
    {
        private readonly SchoolContextDB context = new SchoolContextDB();

        public Form1()
        {
            InitializeComponent();
        }
        
        private void StudentLoad(List<Student> students)
        {
            dtgv_ThongTin.Rows.Clear();

            foreach (var item in students)
            {
                int index = dtgv_ThongTin.Rows.Add();

                dtgv_ThongTin.Rows[index].Cells[0].Value = item.StudentID;
                dtgv_ThongTin.Rows[index].Cells[1].Value = item.FullName;
                dtgv_ThongTin.Rows[index].Cells[3].Value = item.Age;
                dtgv_ThongTin.Rows[index].Cells[2].Value = item.Major;
            }

            cmbChuyenNganh.DataSource = students;
            cmbChuyenNganh.DisplayMember = "Major";
            cmbChuyenNganh.ValueMember = "Major";
        }

        private void ResetControl()
        {
            txtID.Clear();
            txtHoTen.Clear();            
            txtTuoi.Clear();
            cmbChuyenNganh.SelectedIndex = 1;
            dtgv_ThongTin.ClearSelection();
        }

        private void ThongBaoTrong()
        {
            MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private bool KiemTraRangBuoc()
        {
            if (String.IsNullOrWhiteSpace(txtID.Text))
            {
                ThongBaoTrong();
                txtID.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                ThongBaoTrong();
                txtHoTen.Focus();
                return false;
            }

            if (String.IsNullOrWhiteSpace(txtTuoi.Text))
            {
                ThongBaoTrong();
                txtTuoi.Focus();
                return false;
            }

            return true;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var school = context.Students.ToList();
            StudentLoad(school);

            ResetControl();
        }

        private void btbThem_Click(object sender, EventArgs e)
        {
            if (!KiemTraRangBuoc()) { return; }

            int maSV = int.Parse(txtID.Text);

            var timSV = context.Students.Find(maSV);

            //Hiển thị lỗi khi nhân viên đã tốn tại
            if (timSV != null)
            {
                MessageBox.Show("Mã sinh viên đã tồn tại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; //Thoát luôn hàm, không thực hiện chức năng thêm
            }

            var student = new Student
            {
                StudentID = int.Parse(txtID.Text.ToString()),
                FullName = txtHoTen.Text,
                Age = int.Parse(txtTuoi.Text.ToString()),
                Major = cmbChuyenNganh.Text
            };

            // Thêm vào cơ sở dữ liệu
            context.Students.Add(student);
            context.SaveChanges();

            // Thông báo thành công
            MessageBox.Show("Thêm mới dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Cập nhật lại danh sách nhân viên và reset controls
            StudentLoad(context.Students.ToList());
            ResetControl();
        }



        private void dtgv_ThongTin_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= -1)
            {
                var row = dtgv_ThongTin.Rows[e.RowIndex];

                txtID.Text = row.Cells[0].Value.ToString();

                txtHoTen.Text = row.Cells[1].Value.ToString();

                txtTuoi.Text = row.Cells[2].Value.ToString();

                cmbChuyenNganh.Text = row.Cells[3].Value.ToString();

            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                if (!KiemTraRangBuoc()) return;

                int maSV = int.Parse(txtID.Text);

                var timSV = context.Students.Find(maSV);

                if (timSV != null)
                {
                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        // Thực hiện xóa dữ liệu 
                        context.Students.Remove(timSV);
                        MessageBox.Show("Dữ liệu đã được xóa thành công!");
                    }
                    else
                    {
                        // Không làm gì cả nếu người dùng chọn No
                        MessageBox.Show("Hành động xóa đã bị hủy bỏ.");
                        return;
                    }
                }

                context.SaveChanges();

                StudentLoad(context.Students.ToList());
                ResetControl();
            }
            catch
            {
                MessageBox.Show("Không tìm thấy MSSV cần xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (!KiemTraRangBuoc()) return;

                int maSV = int.Parse(txtID.Text);

                var timSV = context.Students.Find(maSV);

                if (timSV != null)
                {

                    timSV.StudentID = int.Parse(txtID.Text.ToString());
                    timSV.FullName = txtHoTen.Text;
                    timSV.Age = int.Parse(txtTuoi.Text.ToString());
                    timSV.Major = cmbChuyenNganh.Text;

                }

                context.SaveChanges();

                MessageBox.Show("Cập nhật dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                StudentLoad(context.Students.ToList());
                ResetControl();
            }
            catch
            {
                MessageBox.Show("Không tìm thấy MSSV cần sửa", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
