using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class URLPropertiesReader : MonoBehaviour
{
    public void ReadPropertiesFromURL()
    {
        string url = "https://fpaniaguaformacion.github.io/unitytest/config.properties";
        StartCoroutine(ReadPropertiesFromURL(url));
    }

    private IEnumerator ReadPropertiesFromURL(string url)
    {
        url = url + "?" + Random.value;//Para evitar la caché
        UnityWebRequest webResource = UnityWebRequest.Get(url);
        yield return webResource.SendWebRequest();//Hasta que no termina, no sigue
        if (webResource.error != null)
        {
            ProcesarError(webResource.error);
            yield break;
        }
        ProcesarProperties(webResource.downloadHandler.text);
    }

    private void ProcesarProperties(string propertiesText)
    {
        var properties = new Dictionary<string, string>();
        string[] lineas = propertiesText.Split('\n');
        foreach (string linea in lineas)
        {
            string[] pares = linea.Split('=');
            properties.Add(pares[0], pares[1]);
        }
        //Cambiamos la escala de Engendro
        GameObject.Find("Engendro").GetComponent<Transform>().localScale =
            new Vector3(
                float.Parse(properties["scale"]),
                float.Parse(properties["scale"]),
                float.Parse(properties["scale"]));
        //Cambiamos el color del cubo
        Material m = GameObject.Find("Cubo").GetComponent<MeshRenderer>().material;
        m.color = new Color(
            float.Parse(properties["r"]),
            float.Parse(properties["g"]),
            float.Parse(properties["b"]),
            float.Parse(properties["alpha"]));
        GameObject.Find("Cubo").GetComponent<MeshRenderer>().material = m;
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
                GameObject.Find("TxtInfo").GetComponent<Text>().text = "Recuerso no encontrado";
                break;
            case "500"://Otro mítico. El servidor ha encontrado un error.
                break;
        }
    }
}
