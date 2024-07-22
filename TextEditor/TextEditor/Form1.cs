using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class Form1 : Form
    {
        private Editor editor;
        public Form1()
        {
            InitializeComponent();
            editor = new Editor(openFileDialog, saveFileDialog);

            editor.listUpdate += RecentDocList;
            editor.RecentUpdate();

            editor.docSet.docSetListUpdate += RecentDocSets;
            RecentDocSets(editor.docSet.GetSetList());

            this.panel1.Controls.Add(editor);
            this.FormClosing += Form1Closing;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.New();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.Open();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.Save();
        }

        private void saveAsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            editor.SaveAs();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.Close();
        }

        private void RecentDocList(List<string> list)
        {
            recentToolStripMenuItem.DropDownItems.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
                toolStripMenuItem.Text = Path.GetFileName(list[i]);
                toolStripMenuItem.Tag = list[i];
                toolStripMenuItem.Click += editor.OpenRecentFile;
                recentToolStripMenuItem.DropDownItems.Add(toolStripMenuItem);
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.Quit();
            this.Close();
        }

        private void Form1Closing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Выйти из программы??", "TextEditor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            if (e.CloseReason == CloseReason.UserClosing)
            {
                editor.Quit();
            }
        }

        private void RecentDocSets(List<string> list)
        {
            recentSetsToolStripMenuItem.DropDownItems.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
                toolStripMenuItem.Text = Path.GetFileName(list[i]);
                toolStripMenuItem.Tag = list[i];
                toolStripMenuItem.Click += editor.OpenRecentDocSet;
                recentSetsToolStripMenuItem.DropDownItems.Add(toolStripMenuItem);
            }
        }

        private void saveSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.SaveDocSet();
        }
    }
}
