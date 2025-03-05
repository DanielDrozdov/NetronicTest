using System;
using System.IO;
using UniRx;
using UnityEngine;

namespace Data.Save
{
    public class GameDataSaver : IGameDataAccess
    {
        #region [Properties]

        public GameSaveData GameSaveData => _data;
        
        private string FullPath
        {
            get
            {
                if (_fullPath != null) return _fullPath;

                string path = Path.Combine(DirPath, FileName);

                _fullPath = path;

                return _fullPath;
            }
        }
        
        private string DirPath => _dirPath ??= Path.Combine(Application.persistentDataPath, DirName);

        private string _fullPath;
        private string _dirPath;

        private const string DirName = "Data";
        private const string FileName = "PlayerData.dat";

        #endregion

        #region [Fields -- Generic]
        
        private bool SavedThisFrame => Time.frameCount == _lastSavedFrame;
        private int _lastSavedFrame = -1;

        private GameSaveData _data;
        
        #endregion
        
        public GameDataSaver()
        {
            /*LoadData();

            SubscribeSaveDataOnApplicationFocus();
            SubscribeSaveDataOnApplicationPause();*/
        }

        private void SubscribeSaveDataOnApplicationFocus()
        {
            /*Observable
                .EveryApplicationFocus()
                .Subscribe(isFocused =>
                {
                    if (!isFocused) SaveData();
                });*/
        }

        private void SubscribeSaveDataOnApplicationPause()
        {
            /*Observable
                .EveryApplicationPause()
                .Subscribe(isPaused =>
                {
                    if (isPaused) SaveData();
                });*/
        }

        /*#region [Save / Load]

        private void LoadData()
        {
            if (!File.Exists(FullPath))
            {
                _data = new GameSaveData();
                return;
            }

            try
            {
                byte[] savedDataBytes = File.ReadAllBytes(FullPath);
                _data = MessagePackSerializer.Deserialize<GameSaveData>(savedDataBytes);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("PlayerProfile:: Failed to load player data (" + ex.Message + "). Using new instance");
            }
            finally
            {
                _data ??= new GameSaveData();
            }
        }

        private void SaveData()
        {
            if (SavedThisFrame) return;

            Directory.CreateDirectory(DirPath);
            
            try
            {
                byte[] savedDataBytes = MessagePackSerializer.Serialize(_data);
                File.WriteAllBytes(FullPath, savedDataBytes);
            }
            catch (Exception ex)
            {
                Debug.LogWarning("PlayerProfile:: Failed to save player data: " + ex.Message);
            }

            _lastSavedFrame = Time.frameCount;
        }

        #endregion*/
    }
}