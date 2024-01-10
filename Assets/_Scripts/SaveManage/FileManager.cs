using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace ResidentEvilClone
{
    public class FileManager : MonoBehaviour
    {
        public static bool WriteToFile(string a_FileName, string a_FileContents)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, a_FileName);

            try
            {
                File.WriteAllText(fullPath + ".txt", a_FileContents);
                File.WriteAllText(fullPath, a_FileContents);
                return true;
            }
            catch(Exception e)
            {
                Debug.LogError($"Failed to read from {fullPath} with exception {e}");

            }
            return false;
        }

        public static bool LoadFromFile(string a_FileName, out string result)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, a_FileName);

            try
            {
                result = File.ReadAllText(fullPath);
                return true;
            }
            catch (Exception)
            {
                result = "";
                return false;
            }
            
        }

    }


}
