using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JsonTests : MonoBehaviour
{
    private const string httpServer = "https://localhost:44392";

    public InputField jsonInputField1;
    public Text debugConsoleText;

    public void ReceiveJsonString()
    {
        UnityWebRequest httpClient = new UnityWebRequest(httpServer + "/api/Values/GetString", "GET");
        httpClient.downloadHandler = new DownloadHandlerBuffer();
        //httpClient.SetRequestHeader("Content-Type", "application/json");
        httpClient.SetRequestHeader("Accept", "application/json");
        httpClient.SendWebRequest();
        while(!httpClient.isDone)
        {
            Task.Delay(1);
        }
        if (httpClient.isHttpError || httpClient.isNetworkError)
        {
            throw new Exception("JsonTests > ReceiveJsonString: " + httpClient.error);
        }

        string jsonResponse = httpClient.downloadHandler.text;


        //string response = JsonUtility.FromJson<string>(jsonResponse);
        // ArgumentException: JSON must represent an object type.
        // To avoid this error we must define a class with only 1 string.
        // This is not very elegant.

        // Better & quicker solution:
        string response = jsonResponse.Replace("\"", "");
        
        debugConsoleText.text += "\nJsonTests > ReceiveJsonString: " + jsonResponse;
        debugConsoleText.text += "\nJsonTests > ReceiveJsonString: " + response;
    }

    public void SendJsonString()
    {
        UnityWebRequest httpClient = new UnityWebRequest(httpServer + "/api/Values/PostString", "POST");
        //httpClient.downloadHandler = new DownloadHandlerBuffer();
        string stringToSend = jsonInputField1.text;
        
        //
        //string jsonString = JsonUtility.ToJson(stringToSend); // Serialize object
        //byte[] data = Encoding.UTF8.GetBytes(jsonString);
        // JsonUtility.ToJson serializes a single string as an empty json object. Bad.
        
        // To avoid that, when sending a string (only a string) do:

        byte[] data = Encoding.UTF8.GetBytes("\"" + stringToSend + "\"");
        httpClient.uploadHandler = new UploadHandlerRaw(data);
        
        httpClient.SetRequestHeader("Content-Type", "application/json");
        //httpClient.SetRequestHeader("Accept", "application/json");
        httpClient.SendWebRequest();
        while (!httpClient.isDone)
        {
            Task.Delay(1);
        }
        if (httpClient.isHttpError || httpClient.isNetworkError)
        {
            throw new Exception("JsonTests > SendJsonString: " + httpClient.error);
        }

        debugConsoleText.text += "\nJsonTests > SendJsonString: " + httpClient.responseCode;
    }

}
