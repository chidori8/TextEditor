using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace TextEditor
{
    internal class DocSet
    {
        private string docSetPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\TextEditor";
        private List<string> docSets { get; set; }
        private List<string> docsPaths = new List<string>();
        public delegate void DocSetList(List<string> docPaths);
        public event DocSetList docSetListUpdate;

        public DocSet()
        {
            this.docSets = new List<string>();
            LoadSets();
        }

        public void SaveSet(string docSetName, List<string> docsPaths)
        {
            this.docsPaths = docsPaths;
            using (StreamWriter writer = File.CreateText(docSetPath + $"\\{docSetName}.txt"))
            {
                for (int i = 0; i < docsPaths.Count; i++)
                {
                    if (docsPaths[i] != null)
                        writer.WriteLine(docsPaths[i]);
                }
            }
            using (StreamWriter writer = File.CreateText(docSetPath + "\\DocSets.txt"))
            {
                if (!docSets.Contains(docSetPath + $"\\{docSetName}.txt"))
                {
                    writer.WriteLine(docSetPath + $"\\{docSetName}.txt");
                    docSets.Insert(0, docSetPath + $"\\{docSetName}.txt");
                }
                else
                {
                    docSets.Remove(docSetPath + $"\\{docSetName}.txt");
                    docSets.Insert(0, docSetPath + $"\\{docSetName}.txt");
                }
            }
            SaveSets();
        }

        private void SaveSets()
        {
            File.WriteAllLines(docSetPath + "\\DocSets.txt", docSets);
            docSetListUpdate.Invoke(docSets);
        }

        private void LoadSets()
        {
            if (!File.Exists(docSetPath + "\\DocSets.txt"))
                File.Create(docSetPath + "\\DocSets.txt").Close();
            using (StreamReader reader = File.OpenText(docSetPath + "\\DocSets.txt"))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    docSets.Add(line);
                    line = reader.ReadLine();
                }
            }
        }

        public List<string> OpenSet(string filePath)
        {
            docsPaths.Clear();
            if (!File.Exists(filePath))
                return null;
            using (StreamReader reader = File.OpenText(filePath))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    docsPaths.Add(line);
                    line = reader.ReadLine();
                }
            }
            return docsPaths;
        }

        public List<string> GetSetList()
        {
            return docSets;
        }
    }
}
