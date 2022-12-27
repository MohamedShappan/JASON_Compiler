using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASON_Compiler
{
    public partial class Form1 : Form
    {
        List<int> lineNumber = new List<int>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string[] sourceCode = textBox1.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            ScannerPhase scanner = new ScannerPhase();
            lineNumber = scanner.getlineNumberList();

            List<Token> tokens = new List<Token>();
            scanner.scanning(sourceCode, ref tokens);

            Node root = ParsingPhase.Parse(tokens);
            foreach (var value in tokens)
            {
                if (value.tokentype != Token_Class.Error)
                {
                    dataGridView1.RowCount++;
                    dataGridView1.Rows[dataGridView1.RowCount - 2].Cells[0].Value = value.lex;
                    dataGridView1.Rows[dataGridView1.RowCount - 2].Cells[1].Value = value.tokentype;
                }
                else
                {
                    listBox1.Items.Add(value.lex);
                    listBox1.Text+="\r\n";
                }
            }
            treeView1.Nodes.Add(ParsingPhase.PrintParseTree(root));
            for (int i = 0; i < ParsingPhase.ParserErrors.Count(); i++)
            {


                // dataGridView2.Rows.Insert(ParsingPhase.ParserErrors[i].Key.ToString(), ParsingPhase.ParserErrors[i].Value, ToString());
                listBox1.Text+=(ParsingPhase.ParserErrors[i].Key.ToString()+ParsingPhase.ParserErrors[i].Value.ToString());
                listBox1.Text += "\r\n";
            }
        }


     
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            listBox1.Text = "";
            JASON_Compiler.TokenStream.Clear();
            dataGridView1.Rows.Clear();
            treeView1.Nodes.Clear();
            Errors.Error_List.Clear();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
