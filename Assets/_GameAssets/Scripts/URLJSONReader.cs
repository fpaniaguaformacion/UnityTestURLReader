using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class URLJSONReader : MonoBehaviour
{
    public void ReadJSONFromURL()
    {
        string url = "https://fpaniaguaformacion.github.io/unitytest/config.json";
        StartCoroutine(ReadJSONFileFromURL(url));
    }

    private IEnumerator ReadJSONFileFromURL(string url)
    {
        url = url + "?" + Random.value;//Para evitar la caché
        UnityWebRequest webResource = UnityWebRequest.Get(url);
        yield return webResource.SendWebRequest();//Hasta que no termina, no sigue
        if (webResource.error != null)
        {
            ProcesarError(webResource.error);
            yield break;
        }
        ProcesarJSON(webResource.downloadHandler.text);    
    }

    private void ProcesarJSON(string json)
    {
        Weapon weapon = JsonUtility.FromJson<Weapon>(json);
        GameObject.Find("TxtInfo").GetComponent<Text>().text = "Speed:" + weapon.speed;
    }
    private void ProcesarError(string error)
    {
        string[] errorSplit = error.Split(' ');
        string errorCode = errorSplit[1];
        switch (errorCode)
        {
            case "400"://La sintaxis de la petición es incorrecta
                GameObject.Find("TxtInfo").GetComponent<Text>().text = "Petición mal formada";
                break;
            case "403"://La petición no dispone de permisos para acceder al recurso;
                break;
            case "404"://El mítico NOT FOUND. No se ha podido encontrar el contenido solicitado.
                GameObject.Find("TxtInfo").GetComponent<Text>().text = "Recurso no encontrado";
                break;
            case "500"://Otro mítico. El servidor ha encontrado un error.
                break;
        }

    }
}
