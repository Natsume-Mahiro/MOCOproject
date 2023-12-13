namespace SaveManagement
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using System.IO;

    /// <summary>
    ///セーブしたいデータ
    /// </summary>
    [Serializable]
    public class SaveData
    {

        #region 各変数データ
        /// <summary>
        /// int保存データ
        /// </summary>
        [Serializable]
        public struct IntData
        {
            public string MethodName;
            public int[] i;

            public IntData(string Key, int I)
            {
                MethodName = Key;
                i = new int[1] { I };
            }

            public IntData(string Key, int[] I)
            {
                MethodName = Key;
                i = I;
            }
        }

        /// <summary>
        /// float保存データ
        /// </summary>
        [Serializable]
        public struct FloatData
        {
            public string MethodName;
            public float[] f;

            public FloatData(string Key, float F)
            {
                MethodName = Key;
                f = new float[1] { F };
            }

            public FloatData(string Key, float[] F)
            {
                MethodName = Key;
                f = F;
            }
        }

        /// <summary>
        /// string保存データ
        /// </summary>
        [Serializable]
        public struct StringData
        {
            public string MethodName;
            public string[] s;

            public StringData(string Key, string S)
            {
                MethodName = Key;
                s = new string[1] { S };
            }

            public StringData(string Key, string[] S)
            {
                MethodName = Key;
                s = S;
            }
        }

        /// <summary>
        /// bool保存データ
        /// </summary>
        [Serializable]
        public struct BoolData
        {
            public string MethodName;
            public bool[] b;

            public BoolData(string Key, bool B)
            {
                MethodName = Key;
                b = new bool[1] { B };
            }

            public BoolData(string Key, bool[] B)
            {
                MethodName = Key;
                b = B;
            }
        }
        
        public List<IntData> intData = new List<IntData>();
        public List<FloatData> floatData = new List<FloatData>();
        public List<StringData> stringData = new List<StringData>();
        public List<BoolData> boolData = new List<BoolData>();

        #endregion

        #region　セーブしたいデータ拡張用
        //変数定義
        public int a = 5;
        #endregion
        
    }

    /// <summary>
    /// ファイルパス等
    /// </summary>
    public class Data : MonoBehaviour
    {
        public static SaveData saveData = new SaveData();

        #region デモ変数
        public static string SaveFolderPath;
        static readonly string SaveFolderName = "SaveFolder";
        #endregion

        #region パス関連処理
        /// <summary>
        /// Application.dataPathの位置<br/>
        /// 単体で処理する場合は先にDataAccess()をしなければnull
        /// </summary>
        public static string ProjectPath;
        /// <summary>
        /// 直前に作成されたフォルダーパス
        /// </summary>
        public static string CreatedFolderPath;

        /// <summary>
        /// プロジェクトのパスにアクセス
        /// </summary>
        public static void DataAccess()
        {
            ProjectPath = Application.dataPath;
        }
        #endregion

        #region フォルダー作成
        /// <summary>
        /// フォルダー作成<br/>
        /// (Application.dataPath直下に作成される)<br/>
        /// CreatedFolderPathが一時保管パス
        /// </summary>
        /// <param name="FolderName">作成するフォルダー名</param>
        public static void CreateFolder(string FolderName)
        {
            ProjectPath = Application.dataPath;
            CreatedFolderPath = ProjectPath + "/" + FolderName;
            if (!File.Exists(CreatedFolderPath))
            {
                Directory.CreateDirectory(CreatedFolderPath);
            }
        }

        /// <summary>
        /// フォルダー作成<br/>
        /// CreatedFolderPathが一時保管パス
        /// </summary>
        /// <param name="FolderPath">作成するフォルダーのカレントディレクトリパス</param>
        /// <param name="FolderName">作成するフォルダー名</param>
        public static void CreateFolder(string FolderPath, string FolderName)
        {
            CreatedFolderPath = FolderPath + "/" + FolderName;
            if (!File.Exists(CreatedFolderPath))
            {
                Directory.CreateDirectory(CreatedFolderPath);
            }
        }

        /// <summary>
        /// フォルダー作成(フォルダー名:SaveFolder)<br/>
        /// </summary>
        /// <param name="FolderPath">作成するフォルダーのカレントディレクトリパス</param>
        public static void CreateSaveFolder(string FolderPath)
        {
            SaveFolderPath = FolderPath + "/" + SaveFolderName;
            if (!File.Exists(SaveFolderPath))
            {
                Directory.CreateDirectory(SaveFolderPath);
            }
        }

        /// <summary>
        /// フォルダー作成(フォルダー名:SaveFolder)<br/>
        /// </summary>
        public static void CreateSaveFolder()
        {
            ProjectPath = Application.dataPath;
            SaveFolderPath = ProjectPath + "/" + SaveFolderName;
            if (!File.Exists(SaveFolderPath))
            {
                Directory.CreateDirectory(SaveFolderPath);
            }
        }
        #endregion

        #region データ状態（未実装）
        public bool isLoad = false;
        public bool isSave = false;

        public bool IsLoadData
        {
            get { return isLoad; }
            set { isLoad = value; }
        }

        public bool IsSaveData
        {
            get { return isSave; }
            set { isSave = value; }
        }
        public static bool CheckSaveDataUpdate()
        {
            SaveData d = Load.DataLoad();
            Debug.Log(d.intData[0].i[0]);
            Debug.Log(saveData.intData[0].i[0]);

            if (saveData == d)
            {
                Debug.Log("更新されてます");
                return true;
            }
            return false;
        }
        #endregion

    }

    /// <summary>
    /// セーブ関連
    /// </summary>
    public class Save : Data
    {
        #region セーブ処理
        /// <summary>
        /// jsonに保存
        /// </summary>
        public static void DataSave()
        {
            string Path = SaveFolderPath + "/SaveFile.json";
            var data = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(Path, data);
        }

        /// <summary>
        /// jsonに保存(ファイル名指定)
        /// </summary>
        /// <param name="FileName">ファイル名</param>
        public static void DataSave(string FileName)
        {

            string Path = SaveFolderPath + "/" + FileName + ".json";
            var data = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(Path, data);
        }
        #endregion

        #region データ収納
        public static void IntDataClear()
        {
            saveData.intData.Clear();
        }
        /// <summary>
        /// intデータ格納
        /// </summary>
        /// <param name="Key">保存キー</param>
        /// <param name="i"></param>
        public static void IntData(string Key, int i)
        {
            saveData.intData.Add(new SaveData.IntData(Key, i));
        }

        /// <summary>
        /// intデータ格納
        /// </summary>
        /// <param name="Key">保存キー</param>
        /// <param name="i"></param>
        public static void IntData(string Key, int[] i)
        {
            saveData.intData.Add(new SaveData.IntData(Key, i));
        }
        /// <summary>
        /// floatデータ格納
        /// </summary>
        /// <param name="Key">保存キー</param>
        /// <param name="f"></param>
        public static void FloatData(string Key, float f)
        {
            saveData.floatData.Add(new SaveData.FloatData(Key, f));
        }
        /// <summary>
        /// floatデータ格納
        /// </summary>
        /// <param name="Key">保存キー</param>
        /// <param name="f"></param>
        public static void FloatData(string Key, float[] f)
        {
            saveData.floatData.Add(new SaveData.FloatData(Key, f));
        }
        /// <summary>
        /// stringデータ格納
        /// </summary>
        /// <param name="Key">保存キー</param>
        /// <param name="f"></param>
        public static void StringData(string Key, string s)
        {
            saveData.stringData.Add(new SaveData.StringData(Key, s));
        }
        /// <summary>
        /// stringデータ格納
        /// </summary>
        /// <param name="Key">保存キー</param>
        /// <param name="f"></param>
        public static void StringData(string Key, string[] s)
        {
            saveData.stringData.Add(new SaveData.StringData(Key, s));
        }
        /// <summary>
        /// boolデータ格納
        /// </summary>
        /// <param name="Key">保存キー</param>
        /// <param name="f"></param>
        public static void BoolData(string Key, bool b)
        {
            saveData.boolData.Add(new SaveData.BoolData(Key, b));
        }
        /// <summary>
        /// boolデータ格納
        /// </summary>
        /// <param name="Key">保存キー</param>
        /// <param name="f"></param>
        public static void BoolData(string Key, bool[] b)
        {
            saveData.boolData.Add(new SaveData.BoolData(Key, b));
        }
        #endregion

    }

    /// <summary>
    /// ロード関連
    /// </summary>
    public class Load : Data
    {
        #region ロード処理
        /// <summary>
        /// ファイルからデータをロード
        /// </summary>
        /// <returns>セーブデータ</returns>
        public static SaveData DataLoad()
        {
            string Path = SaveFolderPath + "/SaveFile.json";
            if (!File.Exists(Path))
            {
                Save.DataSave();
            }
            else
            {
                string dt = File.ReadAllText(Path);
                saveData = JsonUtility.FromJson<SaveData>(dt);
            }

            return saveData;
        }

        /// <summary>
        /// ファイルからデータをロード(ファイル名指定)
        /// </summary>
        /// <param name="FileName">ファイル名</param>
        /// <returns></returns>
        public static SaveData DataLoad(string FileName)
        {
            string Path = SaveFolderPath + "/" + FileName + ".json";
            if (!File.Exists(Path))
            {
                Save.DataSave();
            }
            else
            {
                string dt = File.ReadAllText(Path);
                saveData = JsonUtility.FromJson<SaveData>(dt);
            }

            return saveData;
        }
        #endregion

        #region "変数データ取得"
        /// <summary>
        /// intデータ取得
        /// </summary>
        /// <param name="i">格納用変数</param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static int IntData(int i,string Key)
        {
            foreach(var data in saveData.intData)
            {
                if(data.MethodName == Key)
                {
                    if(data.i.Length > 1)Debug.LogWarning("Warning: Attempting to assign an int[] to an int.   int[]をintに代入しようとしてます");
                    i = data.i[0];
                }
            }
            return i;
        }
        /// <summary>
        /// intデータ取得
        /// </summary>
        /// <param name="i">格納用変数</param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static int[] IntData(int[] i,string Key)
        {
            foreach(var data in saveData.intData)
            {
                if(data.MethodName == Key)
                {
                    i = data.i;
                }
            }
            return i;
        }
        /// <summary>
        /// floatデータ取得
        /// </summary>
        /// <param name="f">格納用変数</param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static float FloatData(float f,string Key)
        {
            foreach(var data in saveData.floatData)
            {
                if(data.MethodName == Key)
                {
                    if(data.f.Length > 1)Debug.LogWarning("Warning: Attempting to assign an float[] to an float.   float[]をfloatに代入しようとしてます");
                    f = data.f[0];
                }
            }
            return f;
        }
        /// <summary>
        /// floatデータ取得
        /// </summary>
        /// <param name="f">格納用変数</param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static float[] FloatData(float[] f,string Key)
        {
            foreach(var data in saveData.floatData)
            {
                if(data.MethodName == Key)
                {
                    f = data.f;
                }
            }
            return f;
        }
        /// <summary>
        /// stringデータ取得
        /// </summary>
        /// <param name="s">格納用変数</param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string StringData(string s,string Key)
        {
            foreach(var data in saveData.stringData)
            {
                if(data.MethodName == Key)
                {
                    if(data.s.Length > 1)Debug.LogWarning("Warning: Attempting to assign an string[] to an string.   string[]をstringに代入しようとしてます");
                    s = data.s[0];
                }
            }
            return s;
        }
        /// <summary>
        /// stringデータ取得
        /// </summary>
        /// <param name="s">格納用変数</param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string[] StringData(string[] s,string Key)
        {
            foreach(var data in saveData.stringData)
            {
                if(data.MethodName == Key)
                {
                    s = data.s;
                }
            }
            return s;
        }
        /// <summary>
        /// boolデータ取得
        /// </summary>
        /// <param name="b">格納用変数</param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static bool BoolData(bool b,string Key)
        {
            foreach(var data in saveData.boolData)
            {
                if(data.MethodName == Key)
                {
                    if(data.b.Length > 1)Debug.LogWarning("Warning: Attempting to assign an bool[] to an bool.   bool[]をboolに代入しようとしてます");
                    b = data.b[0];
                }
            }
            return b;
        }
        /// <summary>
        /// boolデータ取得
        /// </summary>
        /// <param name="b">格納用変数</param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static bool[] BoolData(bool[] b,string Key)
        {
            foreach(var data in saveData.boolData)
            {
                if(data.MethodName == Key)
                {
                    b = data.b;
                }
            }
            return b;
        }

        #endregion
        
    }

}
