using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using Model;
using Common;

namespace Web
{
    public partial class ReaderManager_UI : Form
    {
        public ReaderManager_UI()
        {
            InitializeComponent();
        }
        public BookInfoManager_UI book = null;
        Com autocoumns = new Com();
        public Reader_BLL reader_bll = new Reader_BLL();
        ReaderType_BLL readerType_bll = new ReaderType_BLL();
        BorrowReturn_BLL borrowReturn_bll = new BorrowReturn_BLL();

        //树状图的绑定
        public void TreeViewBand()
        {
            //图书类型的集合
            List<ReaderType> list = readerType_bll.selectReaderType();
            TreeNode node = new TreeNode();
            node.Text = "读者类型";
            node.ImageIndex = 0;
            treeView1.Nodes.Add(node);

            foreach (ReaderType k in list)
            {
                TreeNode n = new TreeNode();
                n.Text = k.ReaderTypeName;
                n.Tag = k.ReaderTypeId;
                n.ImageIndex = 0;
                node.Nodes.Add(n);
            }
            treeView1.ExpandAll();
        }
        //读者管理窗体的加载事件
        private void ReaderManager_UI_Load(object sender, EventArgs e)
        {
            //需要添加列的列标题字符串
            string arraysHeaderText = @"读者编号,读者名称,登记时间,有效时间,读者类型,所在院系,所在班级,省份证号,性别,电话,手机,Email,联系地址,描述信息";
            //需要绑定数据库列名称的字符串
            string arraysName = @"ReaderId,ReaderName,TimeIn,TimeOut,ReaderTypeName,DepartmentName,ClassName,IdentityCard,Gender,Special,Phone,Email,Address,ReaderRemark";

            //自动生成columns
            autocoumns.AutoColumn(arraysHeaderText, arraysName, dgvHeaderInfo);
            autocoumns.AddColumn("修改", dgvHeaderInfo);
            autocoumns.AddColumn("删除", dgvHeaderInfo);

            //dgvHeaderInfo数据集绑定
            this.dgvHeaderInfo.DataSource = reader_bll.selectReader().Tables[0];
            this.dgvHeaderInfo.Columns[13].Visible = false;
            this.dgvHeaderInfo.Columns[12].Visible = false;
            this.dgvHeaderInfo.Columns[11].Visible = false;
            this.dgvHeaderInfo.Columns[10].Visible = false;

            #region 下拉框绑定

            DataGridViewColumnCollection columns = dgvHeaderInfo.Columns;

            for (int i = 0; i < columns.Count - 2; i++)
            {
                comboBox1.Items.Add(columns[i].HeaderText);
            }
            comboBox1.Items.Insert(0, "全部");
            comboBox1.SelectedIndex = 0;

            #endregion


            //树状图的绑定

            TreeViewBand();

            #region DataGridView3的绑定 已借阅的图书信息表

            //dataGridView3.AutoGenerateColumns = false;
            //需要添加列的列标题字符串
            string arraysHeaderText1 = @"读者编号,读者名称,图书编号,图书名称,借出时间,书应归还时间,实际归还时间,应付罚金,续借次数,借还描述";
            //需要绑定数据库列名称的字符串
            string arraysName1 = @"ReaderId,ReaderName,BookId,BookName,BorrowTime,ReturnTime,FactReturnTime,Fine,RenewCount,BorrowRemark";

            //自动生成columns
            autocoumns.AutoColumn(arraysHeaderText1, arraysName1, dgvReaderBorrowBook);

            //string index = dgvHeaderInfo.Rows[0].Cells[0].Value.ToString();
            //DataGridView1数据集绑定
            //this.dgvReaderBorrowBook.DataSource = borrowReturn_bll.ReaderBorrowReturn(index).Tables[0];

            #endregion

            #region DataGridView2的绑定 历史借阅记录表

            //dataGridView2.AutoGenerateColumns = false;
            //需要添加列的列标题字符串
            string arraysHeaderText2 = @"读者编号,读者名称,图书编号,图书名称,借出时间,书应归还时间,实际归还时间,应付罚金,续借次数,借还描述";
            //需要绑定数据库列名称的字符串
            string arraysName2 = @"ReaderId,ReaderName,BookId,BookName,BorrowTime,ReturnTime,FactReturnTime,Fine,RenewCount,BorrowRemark";

            //自动生成columns
            autocoumns.AutoColumn(arraysHeaderText2, arraysName2, dgvReaderBorrowBookInfoed);

            string index1 = dgvHeaderInfo.Rows[0].Cells[0].Value.ToString();
            //DataGridView1数据集绑定
            this.dgvReaderBorrowBookInfoed.DataSource = borrowReturn_bll.ReaderBorrowReturn1(index1).Tables[0];
            dgvReaderBorrowBookInfoed.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            #endregion
        }
        //TreeView节点选项改变事件
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode.Level == 0)
            {
                dgvHeaderInfo.DataSource = reader_bll.selectReader().Tables[0];
            }
            else if (treeView1.SelectedNode.Level == 1)
            {
                int index = (int)treeView1.SelectedNode.Tag;
                dgvHeaderInfo.DataSource = reader_bll.selectReader(index).Tables[0];
            }

            //如果读者信息表中查不到一条数据，相关表的数据也清空
            if (dgvHeaderInfo.Rows.Count == 0)
            {
                dgvReaderBorrowBookInfoed.DataSource = null;
                dgvReaderBorrowBook.DataSource = null;
                return;
            }
        }

        
        private void btnExitColumn_Click(object sender, EventArgs e)
        {
            BookInfoExitRows_UI b = new BookInfoExitRows_UI();
            b.Reader = this;
            b.ShowDialog();
        }

        //当编辑绑定完 DataGridView所有单元格之后，执行绘制引发的事件
        //private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        //{

        //}
        //当编辑绑定完 DataGridView所有单元格之后，执行绘制引发的事件
        private void dgvHeaderInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string ReaderId = "";
            try
            {
                //选中行的图书编号
                ReaderId = dgvHeaderInfo.Rows[e.RowIndex].Cells[0].Value.ToString();

            }
            catch (Exception) { }

            //单击某行 读者借阅表就显示相关的数据
            dgvReaderBorrowBook.DataSource = borrowReturn_bll.ReaderBorrowReturn(ReaderId).Tables[0];

            //单击某行 读者历史借阅表就显示相关的数据
            dgvReaderBorrowBookInfoed.DataSource = borrowReturn_bll.ReaderBorrowReturn1(ReaderId).Tables[0];

            //DataGridView的总列数
            int rows = dgvHeaderInfo.Columns.Count;

            if (e.ColumnIndex == rows - 2)//修改
            {
                ReaderExit_UI b = new ReaderExit_UI();
                b.reader = this;
                b.ReaderId = ReaderId;
                b.ShowDialog();
            }
            else if (e.ColumnIndex == rows - 1)//删除
            {
                DialogResult result = MessageBox.Show("确定删除吗？", "提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    if (reader_bll.deleteReader(ReaderId) > 0)
                    {
                        btnSelect_Click(null, null);
                        //历史记录表中的数据引用着读者信息表的数据，删除读者信息应该刷新历史记录表
                        book.dgvHostory.DataSource = book.borrowReturn_bll.selectBorrowReturn(book.BookId).Tables[0];
                    }
                    else
                    {
                        MessageBox.Show("删除失败！");
                    }
                }
            }
        }

        //当编辑绑定完 DataGridView所有单元格之后，执行绘制引发的事件
        private void dgvHeaderInfo_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X,
               e.RowBounds.Location.Y,
               dgvHeaderInfo.RowHeadersWidth - 4,
               e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgvHeaderInfo.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgvHeaderInfo.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        //单击查询按钮
        public void btnSelect_Click(object sender, EventArgs e)
        {
            DataGridViewColumnCollection columns = dgvHeaderInfo.Columns;
            if (comboBox1.SelectedIndex == 0)//全部
            {
                List<string> list = new List<string>();
                for (int i = 0; i < columns.Count - 2; i++)
                {
                    list.Add(columns[i].DataPropertyName);
                }
                string B = textBox1.Text.Trim();
                dgvHeaderInfo.DataSource = reader_bll.selectReader(list, B).Tables[0];
            }
            else
            {

                for (int i = 0; i < columns.Count - 2; i++)
                {
                    if (comboBox1.SelectedItem.ToString() == columns[i].HeaderText)
                    {
                        string A = columns[i].DataPropertyName;
                        string B = textBox1.Text.Trim();
                        dgvHeaderInfo.DataSource = reader_bll.selectReader(A, B).Tables[0];
                        break;
                    }
                }
            }

            //如果读者信息表中查不到一条数据，相关表的数据也清空
            if (dgvHeaderInfo.Rows.Count == 0)
            {
                dgvReaderBorrowBookInfoed.DataSource = null;
                dgvReaderBorrowBook.DataSource = null;
                return;
            }
        }


        public ReaderAdd_UI readerAdd;
        public void btnAddReaderInfo_Click(object sender, EventArgs e)
        {
            readerAdd = new ReaderAdd_UI();
            readerAdd.reader = this;
            readerAdd.ShowDialog();
        }
    }
}
