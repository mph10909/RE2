using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    [System.Serializable]
    public class FileInventory
    {
        List<FileItem> itemList;
        public List<FileData> fileList;

        public FileInventory()
        {
            itemList = new List<FileItem>();
            fileList = new List<FileData>();
        }

        public void AddItem(FileItem item)
        {
            itemList.Add(item);
        }

        public void AddFileItem(FileData item)
        {
            fileList.Add(item);
        }

        public void RemoveItem(FileItem item)
        {
            itemList.Remove(item);
        }

        public List<FileItem> GetItemList()
        {
            return itemList;
        }

        public List<FileData> GetFileDataList()
        {
            return fileList;
        }
    }
}
