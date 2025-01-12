using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class SaveManager : MonoBehaviour
{
    const string SavePathKey = "savepath";
    const string CanonPathKey = "canonpath";

    [SerializeField] Button saveButton;
    [SerializeField] Button loadButton;
    [SerializeField] Button canonButton;
    [SerializeField] TextMeshProUGUI label;

    [HideInInspector]
    public static SaveData saveData;

    string _savePath = "";
    string _canonPath = "";

    static SaveManager _instance;

    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        loadButton.onClick.AddListener(DoLoad);
        saveButton.onClick.AddListener(DoSave);
        canonButton.onClick.AddListener(DoSelectCanonFolder);

        _savePath = PlayerPrefs.GetString(SavePathKey, "");
        _canonPath = PlayerPrefs.GetString(CanonPathKey, "");
        Load(_savePath);
    }

    public static void Load() => _instance.DoLoad();

    public static void Save() => _instance.DoSave();

    public static string SearchBy(string search)
    {
        if (_instance._canonPath.Length == 0) return "No Canon Path given!";

        DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(_instance._canonPath);
        FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles("*" + search + "*.*", SearchOption.AllDirectories);

        if (filesInDir.Length == 0) return "No Canon Entry found!";

        var bestFit = FuzzySharp.Process.ExtractOne(search, filesInDir.Select(f => f.Name));
        var bestFile = filesInDir[bestFit.Index];

        Debug.Log("Best match is: " + bestFile.FullName);

        return File.ReadAllText(bestFile.FullName);
    }

    void DoLoad()
    {
        FileBrowser.ShowLoadDialog((paths) => {
            var path = paths[0];

            Load(path);
        }, () => { }, FileBrowser.PickMode.Files, initialPath: _savePath);
    }

    void DoSave()
    {
        if (_savePath.Length > 0)
        {
            Save(_savePath);
            return;
        }

        FileBrowser.ShowSaveDialog((paths) => {
            var path = paths[0].Contains(".txt")
                ? paths[0]
                : Path.Combine(paths[0], "save_data.txt");
            Save(path);
        }, () => { }, FileBrowser.PickMode.FilesAndFolders, initialPath: _savePath);
    }

    void DoSelectCanonFolder()
    {
        FileBrowser.ShowLoadDialog((paths) => {
            var path = paths[0];
            _canonPath = path;
            PlayerPrefs.SetString(CanonPathKey, _canonPath);
        }, () => { }, FileBrowser.PickMode.Folders, initialPath: _canonPath);
    }

    void Load(string path)
    {
        if (path.Length > 0 && File.Exists(path))
        {
            try
            {
                string fileContents = File.ReadAllText(path);
                saveData = JsonConvert.DeserializeObject<SaveData>(fileContents);
                _savePath = path;
                PlayerPrefs.SetString(SavePathKey, _savePath);
            } catch
            {
                saveData = new SaveData();
                _savePath = "";
            }

        } else
        {
            saveData = new SaveData();
        }

        DisplayManager.Init();

    }

    void Save(string path)
    {
        if (File.Exists(path)) Debug.Log("File Exists");

        File.WriteAllText(path, JsonConvert.SerializeObject(saveData));
    }
}