using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace TextEditor
{
    internal class Editor : TabControl
    {
        private SaveFileDialog saveFileDialog;
        private OpenFileDialog openFileDialog;
        private RecentDocList recentDocList;
        public DocSet docSet;
        public delegate void ListUpdate(List<string> list);
        public event ListUpdate listUpdate;

        public Editor(OpenFileDialog openFileDialog, SaveFileDialog saveFileDialog)
        {
            this.openFileDialog = openFileDialog;
            this.saveFileDialog = saveFileDialog;
            this.saveFileDialog.Filter = "Text File(*.txt)|*.txt";
            recentDocList = new RecentDocList();
            docSet = new DocSet();
            recentDocList.listChange += RecentUpdate;
            Add(new Document());
            this.Dock = DockStyle.Fill;
        }
        
        public void Save()
        {
            Document document = (Document)this.SelectedTab;
            if (!document.IsModified())
                return;
            if (document.PathIsEmpty())
            {
                if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                    return;
                document.SaveAs(saveFileDialog.FileName);
                recentDocList.Add(document);
                return;
            }
            document.Save();
        }

        public void SaveAs()
        {
            Document document = (Document)this.SelectedTab;
            if (!document.IsModified())
                return;
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            document.SaveAs(saveFileDialog.FileName);
            recentDocList.Add(document);
        }

        public void Open()
        {
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            string fileName = openFileDialog.FileName;
            Document document = new Document(fileName);
            Document doc = AlreadyOpen(document);
            if (doc != null)
            {
                this.SelectTab(doc);
                return;
            }
            if (document.Open())
                Add(document);
        }

        public void New()
        {
            Document document = new Document();
            Add(document);
        }

        public void Quit()
        {
            foreach (Document document in TabPages)
            {
                Close();
            }
        }

        public void Close()
        {
            Document document = (Document) this.SelectedTab;
            if (document.IsModified())
            {
                if (MessageBox.Show("Файл был изменен, внести изменения?", document.fileName, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Save();
                    //if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                    //{
                    //    Remove(document);
                    //    return;
                    //}
                    //document.SaveAs(saveFileDialog.FileName);
                    recentDocList.Add(document);
                }
            }
            Remove(document);
        }

        private void Add(Document doc)
        {
            this.TabPages.Add(doc);
            this.SelectTab(doc);
            if (!String.IsNullOrEmpty(doc.filePath))
                recentDocList.Add(doc);
        }

        private void Remove(Document doc)
        {
            this.TabPages.Remove(doc);
        }

        public void RecentUpdate()
        {
            listUpdate?.Invoke(recentDocList.GetRecentDocList());
        }

        public void OpenRecentFile(object sender, EventArgs e)
        {
            Document document = new Document(((ToolStripMenuItem)sender).Tag.ToString());
            Document doc = AlreadyOpen(document);
            if (doc != null)
            {
                this.SelectTab(doc);
                return;
            }
            if (document.Open())
                Add(document);
        }

        public Document AlreadyOpen(Document document)
        {
            foreach (Document doc in TabPages)
            {
                if (document.filePath == doc.filePath)
                {
                    return doc;
                }
            }
            return null;
        }

        public void OpenRecentDocSet(object sender, EventArgs e)
        {
            Quit();
            List<string> list = docSet.OpenSet(((ToolStripMenuItem)sender).Tag.ToString());
            foreach (string path in list)
            {
                OpenFilepath(path);
                //Add(new Document(path));
                
            }
        }

        public void OpenFilepath(string fileName)
        {
            Document document = new Document(fileName);
            Document doc = AlreadyOpen(document);
            if (doc != null)
            {
                this.SelectTab(doc);
                return;
            }
            if (document.Open())
                Add(document);
        }

        public void SaveDocSet()
        {
            List<string> docPaths = new List<string>();
            foreach (Document document in TabPages)
            {
                if (document.filePath == null)
                {
                    if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                        return;
                    document.SaveAs(saveFileDialog.FileName);
                }
                else
                {
                    document.Save();
                }
                docPaths.Add(document.filePath);
            }
            docSet.SaveSet(Interaction.InputBox("Название DocSet", "SaveDocSet"), docPaths);
        }
    }
}
