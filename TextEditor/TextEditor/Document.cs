using System;
using System.IO;
using System.Windows.Forms;

namespace TextEditor
{
    internal class Document : TabPage
    {
        private RichTextBox textBox;
        public string filePath { get; set; }
        public string fileName { get; set; }

        public Document(string filePath)
        {
            this.filePath = filePath;
            this.fileName = Path.GetFileName(filePath);
            this.Text = fileName;
            textBox = new RichTextBox();
            textBox.Dock = DockStyle.Fill;
            this.Controls.Add(textBox);
            textBox.TextChanged += TextCh;
        }

        public Document()
        {
            textBox = new RichTextBox();
            textBox.Dock = DockStyle.Fill;
            this.Controls.Add(textBox);
            this.Text = "NewPage";
            this.fileName = this.Text;
            textBox.TextChanged += TextCh;
        }

        public bool Open()
        {
            if (File.Exists(filePath))
            {
                textBox.Text = File.ReadAllText(filePath);
                this.fileName = Path.GetFileName(filePath);
                this.Text = fileName;
                return true;
            }
            else
            {
                MessageBox.Show($"Файл {fileName} не существует");
                return false;
            }
        }

        public void SaveAs(string path)
        {
            if (textBox != null & path != null) {
                if (fileName != null)
                    this.Text = fileName;
                File.WriteAllText(path, textBox.Text);
                this.filePath = path;
                this.fileName = Path.GetFileName(filePath);
                textBox.Modified = false;
            }
        }
        public void Save()
        {
            if (textBox != null & filePath != null)
            {
                if (fileName != null)
                    this.Text = fileName;
                File.WriteAllText(filePath, textBox.Text);
                textBox.Modified = false;
            }
            else
            {
                throw new Exception();
            }
        }
        
        public bool IsModified()
        {
            return textBox.Modified;
        }

        public bool PathIsEmpty()
        {
            return (filePath == null);
        }

        public void TextCh(object s, EventArgs e)
        {
            this.Text = fileName + "*";
        }
    }
}
