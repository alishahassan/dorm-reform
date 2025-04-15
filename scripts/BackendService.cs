using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System; // For Action
using Newtonsoft.Json; // If using Newtonsoft.Json

public class BackendService : MonoBehaviour
{
    private string backendUrl = "https://a3f7cjt0nc.execute-api.us-east-2.amazonaws.com/dev"; // Replace with your API Gateway URL

    // ... (Other variables) ...

    public void RegisterUser(string username, string password, Action<string> onSuccess, Action<string> onError)
    {
        StartCoroutine(RegisterUserCoroutine(username, password, onSuccess, onError));
    }

    IEnumerator RegisterUserCoroutine(string username, string password, Action<string> onSuccess, Action<string> onError)
    {
        var requestBody = new RegisterRequest { username = username, password = password };
        string json = JsonConvert.SerializeObject(requestBody);

        using (UnityWebRequest www = new UnityWebRequest(backendUrl + "/users", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("User registered successfully!");
                onSuccess(www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error registering user: " + www.error);
                onError(www.error);
            }
        }
    }

    [Serializable] // For JsonUtility
    private class RegisterRequest // Define data structure for request
    {
        public string username;
        public string password;
    }

    public void LoginUser(string username, string password, Action<string, string> onSuccess, Action<string> onError)
    {
        StartCoroutine(LoginUserCoroutine(username, password, onSuccess, onError));
    }

    IEnumerator LoginUserCoroutine(string username, string password, Action<string, string> onSuccess, Action<string> onError)
    {
        var requestBody = new LoginRequest { username = username, password = password };
        string json = JsonConvert.SerializeObject(requestBody);

        using (UnityWebRequest www = new UnityWebRequest(backendUrl + "/login", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                LoginResponse response = JsonConvert.DeserializeObject<LoginResponse>(www.downloadHandler.text);
                onSuccess(response.message, response.token);
            }
            else
            {
                Debug.LogError("Error logging in: " + www.error);
                onError(www.error);
            }
        }
    }

    [Serializable]
    private class LoginRequest
    {
        public string username;
        public string password;
    }

    [Serializable]
    private class LoginResponse
    {
        public string message;
        public string token;
    }

    public void SaveRoom(string userId, string roomData, Action<string> onSuccess, Action<string> onError)
    {
        StartCoroutine(SaveRoomCoroutine(userId, roomData, onSuccess, onError));
    }

    IEnumerator SaveRoomCoroutine(string userId, string roomData, Action<string> onSuccess, Action<string> onError)
    {
        var requestBody = new SaveRoomRequest { userId = userId, roomData = roomData };
        string json = JsonConvert.SerializeObject(requestBody);
        Debug.Log("Saving Room with JSON: " + json);

        using (UnityWebRequest www = new UnityWebRequest(backendUrl + "/rooms", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();
            Debug.Log("Response: " + www.downloadHandler.text);

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Room saved successfully!");
                onSuccess("Room saved successfully!");
            }
            else
            {
                Debug.LogError("Error saving room: " + www.error);
                onError(www.error);
            }
        }
    }

    [Serializable]
    private class SaveRoomRequest
    {
        public string userId;
        public string roomData;
    }

    public void LoadRoom(string userId, string roomId, Action<string, string> onSuccess, Action<string> onError)
    {
        StartCoroutine(LoadRoomCoroutine(userId, roomId, onSuccess, onError));
    }

    IEnumerator LoadRoomCoroutine(string userId, string roomId, Action<string, string> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(backendUrl + "/rooms/" + userId + "/" + roomId))
        {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
            LoadRoomResponse response = JsonConvert.DeserializeObject<LoadRoomResponse>(www.downloadHandler.text);
                onSuccess(response.message, response.roomData);
            }
            else
            {
                onError(www.error);
            }
        }
    }

    [Serializable]
    private class LoadRoomResponse
    {
        public string message;
        public string roomData;
    }

    public void ListRooms(string userId, Action<string, string> onSuccess, Action<string> onError)
    {
        StartCoroutine(ListRoomsCoroutine(userId, onSuccess, onError));
    }

    IEnumerator ListRoomsCoroutine(string userId, Action<string, string> onSuccess, Action<string> onError)
    {
        // 1. Create UnityWebRequest (GET request with path parameter)
        using (UnityWebRequest www = UnityWebRequest.Get(backendUrl + "/rooms/" + userId))
        {
            // 2. Send Request
            yield return www.SendWebRequest();

            // 3. Handle Response
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Rooms listed successfully!");
                onSuccess(www.downloadHandler.text, www.downloadHandler.text); // Modify this to parse the response correctly
            }
            else
            {
                Debug.LogError("Error listing rooms: " + www.error);
                onError("Error listing rooms: " + www.error);
            }
        }
    }

    public void DeleteRoom(string userId, string roomId, Action<string> onSuccess, Action<string> onError)
    {
        StartCoroutine(DeleteRoomCoroutine(userId, roomId, onSuccess, onError));
    }

    IEnumerator DeleteRoomCoroutine(string userId, string roomId, Action<string> onSuccess, Action<string> onError)
    {
        // 1. Create UnityWebRequest (DELETE request with path parameters)
        using (UnityWebRequest www = UnityWebRequest.Delete(backendUrl + "/rooms/" + userId + "/" + roomId))
        {
            // 2. Send Request
            yield return www.SendWebRequest();

            // 3. Handle Response
            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Room deleted successfully!");
                onSuccess("Room deleted successfully!");
            }
            else
            {
                Debug.LogError("Error deleting room: " + www.error);
                onError("Error deleting room: " + www.error);
            }
        }
    }
}