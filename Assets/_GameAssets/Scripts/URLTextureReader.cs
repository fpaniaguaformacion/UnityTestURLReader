using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class URLTextureReader : MonoBehaviour
{
    UnityWebRequest webResource;
    bool reading = false;
    public void ReadTextureFromURL()
    {
        string url = "https://fpaniaguaformacion.github.io/unitytest/Robot_Color_Modificado.jpg";
        StartCoroutine(ReadSpriteFileFromURL(url));
    }

    private void Update()
    {
        if (reading) {
            GameObject.Find("TxtInfo").GetComponent<Text>().text=webResource.downloadProgress.ToString();
        }
    }

    private IEnumerator ReadSpriteFileFromURL(string url)
    {
        reading = true;
        url = url + "?" + Random.value;//Para evitar la caché
        webResource = UnityWebRequestTexture.GetTexture(url);
        yield return webResource.SendWebRequest();//Hasta que no termina, no sigue
        if (webResource.error != null)
        {
            ProcesarError(webResource.error);
            yield break;
        }
        ProcesarTextura(((DownloadHandlerTexture)webResource.downloadHandler).texture);
        reading = false;
    }

    private void ProcesarTextura(Texture texture)
    {
        GameObject engendro = GameObject.Find("Engendro");
        Material material = engendro.GetComponentInChildren<SkinnedMeshRenderer>().materials[0];
        print(material.name);
        material.SetTexture("_MainTex", texture);
        engendro.GetComponentInChildren<SkinnedMeshRenderer>().materials[0] = material;
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
