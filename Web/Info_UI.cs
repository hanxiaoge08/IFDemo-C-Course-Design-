using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using BLL;
using Model;

namespace Web
{
    public partial class Info_UI : Form
    {
        public Info_UI()
        {
            InitializeComponent();
        }
        public BorrowHostory_UI BorrowHostory = null;
        public string txtName = "";
        public BorrowManager_UI borrowManager = null;
        Com com = new Com();
        Reader_BLL reader_bll = new Reader_BLL();
        BookInfo_BLL bookInfo_bll = new BookInfo_BLL();

        private void Info_UI_Load(object sender, EventArgs e)
        {
            if (txtName.Equals("读者信息"))
            {
                this.Text = "读者信息类表";
                //需要添加列的列标题字符串
                string arraysHeaderText = @"读者编号,读者名称,登记时间,有效时间,图书类型,所在院系,所在班级,省份证号,性别,电话,手机,Email,联系地址,描述信息";
                //需要绑定数据库列名称的字符串
                string arraysName = @"ReaderId,ReaderName,TimeIn,TimeOut,ReaderTypeName,DepartmentName,ClassName,IdentityCard,Gender,Special,Phone,Email,Address,ReaderRemark";

                //自动生成columns
                com.AutoColumn(arraysHeaderText, arraysName, dgvInfo);
                for (int i = 0; i < dgvInfo.Columns.Count; i++)
                {
                    if (i > 5)
                        dgvInfo.Columns[i].Visible = false;
                }
                dgvInfo.DataSource = reader_bll.selectReader().Tables[0];
                dgvInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            else if (txtName == "图书信息")
            {
                this.Text = "图书信系列表";
                //需要添加列的列标题字符串
                string arraysHeaderText = @"图书编号,图书名称,登记时间,图书类型,作者,拼音码,翻译,语言,页数,价格,印刷版面,存放位置,ISBS码,版本,描述";
                //需要绑定数据库列名称的字符串
                string arraysName = @"BookId,BookName,TimeIn,BookTypeName,Author,PinYinCode,Translator,Language,BookNumber,Price,Layout,Address,ISBS,Versions,BookRemark";

                //自动生成columns
                com.AutoColumn(arraysHeaderText, arraysName, dgvInfo);
                for (int i = 0; i < dgvInfo.Columns.Count; i++)
                {
                    if (i > 5)
                        dgvInfo.Columns[i].Visible = false;
                }
                if (BorrowHostory != null)
                {
                    dgvInfo.DataSource = bookInfo_bll.selectBookInfo1().Tables[0];
                }
                else if (borrowManager != null)
                {
                    dgvInfo.DataSource = bookInfo_bll.selectBookInfo2("").Tables[0];
                }

                dgvInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

            com.AddColumn("选取", dgvInfo);
        }

        private void dgvInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string name = "";
            try
            {
                //选中行的编号
                name = dgvInfo.Rows[e.RowIndex].Cells[0].Value.ToString();

            }
            catch (Exception) { }


            if (e.ColumnIndex == dgvInfo.Columns.Count - 1)
            {
                if (borrowManager != null)
                {
                    if (txtName == "读者信息")
                    {
                        borrowManager.txtReaderId.Text = name;
                    }
                    else if (txtName == "图书信息")
                    {
                        borrowManager.txtBookId.Text = name;
                    }
                }
                else if (BorrowHostory != null)
                {
                    if (txtName == "读者信息")
                    {
                        BorrowHostory.txtReaderId.Text = name;
                    }
                    else if (txtName == "图书信息")
                    {
                        BorrowHostory.txtBookId.Text = name;
                    }
                }
                this.Close();
            }
        }

        //当编辑绑定完 DataGridView所有单元格之后，执行绘制引发的事件
        private void dgvInfo_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dgvInfo.RowHeadersWidth - 4,
               e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgvInfo.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgvInfo.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }
}
