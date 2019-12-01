using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class URLTxtReader : MonoBehaviour
{
    public void ReadTextFromURL()
    {
        string url = "https://fpaniaguaformacion.github.io/unitytest/config.txt";
        StartCoroutine(ReadTxtFileFromURL(url));
    }

    private IEnumerator ReadTxtFileFromURL(string url)
    {
        url = url + "?" + Random.value;//Para evitar la caché
        UnityWebRequest webResource = UnityWebRequest.Get(url);
        yield return webResource.SendWebRequest();//Hasta que no termina, no sigue
        if (webResource.error != null)
        {
            ProcesarError(webResource.error);
            yield break;
        }
        ProcesarTexto(webResource.downloadHandler.text);    
    }

    private void ProcesarTexto(string texto)
    {
        string[] lineas = texto.Split('\n');
        float x = float.Parse(lineas[0]);
        float y = float.Parse(lineas[1]);
        float z = float.Parse(lineas[2]);
        transform.position = new Vector3(x, y, z);
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
