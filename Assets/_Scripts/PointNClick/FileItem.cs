using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    [Serializable]
    public class FileItem
    {
        public enum FileType
        {
            Church_Diary,
            Restaurant_Diary,
            Patient_Chart,
            RacoonCity_Report,
            Umbrella_Secret_File,
            Janitor_Diary,
            Prison_Diary,
            Young_Man_Diary,
            Vincents_Diary,
            Lotts_Diary,
            Church_Arcade,
            Servers_Memo,
            Theater_Note,
        }

        public FileType fileType;
        public string fileName;

        public FileType GetFileType()
        {
            return fileType;
        }

        public string[] GetTextBody(){
            switch (fileType){
                default:
                case FileType.Church_Diary:         return FileAssets.Instance.churchDiary;
                case FileType.Restaurant_Diary:     return FileAssets.Instance.restaurantDiary;
                case FileType.Patient_Chart:        return FileAssets.Instance.patientChart;
                case FileType.Umbrella_Secret_File: return FileAssets.Instance.umbrellaFile;
                case FileType.Lotts_Diary:          return FileAssets.Instance.lottsDiary;
                case FileType.RacoonCity_Report:    return FileAssets.Instance.racoonCityReport;
                case FileType.Church_Arcade:        return FileAssets.Instance.churchArcadeEntry;
                case FileType.Servers_Memo:         return FileAssets.Instance.serversMemo;
                case FileType.Theater_Note:         return FileAssets.Instance.theaterNote;

            }
        }


    }
}
