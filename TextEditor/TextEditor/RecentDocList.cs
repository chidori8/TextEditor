using System;
using System.IO;
using System.Collections.Generic;

namespace TextEditor
{
    internal class RecentDocList
    {
        private List<string> recentDocList = new List<string>(5);
        private string recentDocListFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\TextEditor";
        public delegate void ListChange();
        public event ListChange listChange;

        private string this[int i]
        {
            get
            {
                if (i >= 0 & i < recentDocList.Count)
                {
                    return recentDocList[i];
                }
                else
                {
                    throw new IndexOutOfRangeException("IndexOutOfRangeException");
                }
                    
            }
        }

        public int Count { get { return recentDocList.Count; } }

        public RecentDocList()
        {
            CreateDir();
            LoadData();
        }

        public void SaveData()
        {
            using (StreamWriter writer = File.CreateText(recentDocListFilePath + "\\RecentDocFiles.txt"))
            {
                for (int i = 0; i < recentDocList.Count; i++)
                {
                    writer.WriteLine(recentDocList[i]);
                }
            }
        }

        private void LoadData()
        {
            using (StreamReader reader = File.OpenText(recentDocListFilePath + "\\RecentDocFiles.txt"))
            {
                string line = reader.ReadLine();
                while(line != null & Count < recentDocList.Capacity)
                {
                    recentDocList.Add(line);
                    line = reader.ReadLine();
                }
            }
        }

        public void Add(Document document)
        {
            if (document != null)
            {
                if (!MatchCheck(document.filePath))
                {
                    if (Count < recentDocList.Capacity)
                    {
                        recentDocList.Insert(0, document.filePath);
                    }
                    else
                    {
                        recentDocList.RemoveAt(Count - 1);
                        recentDocList.Insert(0, document.filePath);
                    }
                }
                SaveData();
                listChange?.Invoke();
            }
        }

        public List<string> GetRecentDocList()
        {
            return recentDocList;
        }

        private void CreateDir()
        {
            if (Directory.Exists(recentDocListFilePath) == false)
            {
                Directory.CreateDirectory(recentDocListFilePath);
                if (File.Exists(recentDocListFilePath + "\\RecentDocFiles.txt") == false)
                {
                    FileStream fileStream = new FileStream(recentDocListFilePath + "\\RecentDocFiles.txt", FileMode.Create);
                    fileStream.Close();
                }
            }
            if (Directory.Exists(recentDocListFilePath) == true & File.Exists(recentDocListFilePath + "\\RecentDocFiles.txt") == false)
            {
                FileStream fileStream = new FileStream(recentDocListFilePath + "\\RecentDocFiles.txt", FileMode.Create);
                fileStream.Close();
            }
        }

        private bool MatchCheck(string s)
        {
            bool isExist = false;
            for (int i = 0; i < recentDocList.Count; i++)
            {
                if (recentDocList[i].Equals(s))
                {
                    recentDocList.RemoveAt(i);
                    recentDocList.Insert(0, s);
                    isExist = true;
                }
            }
            return isExist;
        }
    }
}
