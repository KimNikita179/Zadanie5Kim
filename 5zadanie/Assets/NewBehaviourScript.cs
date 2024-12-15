using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NewBehaviourScript : MonoBehaviour
{
    public Text nm;
    public string jsonURL;
    public Text lvl;
    public Slider dat;
    public Text dat1;
    public Jsonclass jsnData;

    void Start()
    {
        dat.interactable = false; // Делаем слайдер неактивным
        StartCoroutine(getData());
    }

    IEnumerator getData()
    {
        Debug.Log("Загрузка...");
        var uwr = new UnityWebRequest(jsonURL);
        uwr.method = UnityWebRequest.kHttpVerbGET;

        var resultFile = Path.Combine(Application.persistentDataPath, "result.json");
        var dh = new DownloadHandlerFile(resultFile);
        dh.removeFileOnAbort = true;
        uwr.downloadHandler = dh;

        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            nm.text = "Ошибка!";
            Debug.LogError("Ошибка загрузки: " + uwr.error);
        }
        else
        {
            Debug.Log("Файл сохранен по пути: " + resultFile);
            jsnData = JsonUtility.FromJson<Jsonclass>(File.ReadAllText(resultFile));

            if (jsnData != null)
            {
                nm.text = jsnData.Name;
                lvl.text = jsnData.Level.ToString();
                dat.value = jsnData.TestParam;
                dat1.text = jsnData.TestParam.ToString();
            }
            else
            {
                Debug.LogError("Ошибка парсинга JSON");
            }
        }
    }

    [System.Serializable]
    public class Jsonclass
    {
        public string Name;
        public int Level;
        public int TestParam;
    }
}
