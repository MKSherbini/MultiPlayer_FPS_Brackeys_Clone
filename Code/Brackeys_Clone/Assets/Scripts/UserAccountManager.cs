using UnityEngine;
using System.Collections;
using DatabaseControl;
using UnityEngine.SceneManagement;

public class UserAccountManager : MonoBehaviour
{

    public static UserAccountManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    public static string LoggedIn_Username { get; protected set; } //stores username once logged in
    private static string LoggedIn_Password = ""; //stores password once logged in
    public static Color LoggedIn_color = Color.black;

    public static bool IsLoggedIn { get; protected set; }
    // public static string LoggedIn_Data { get; protected set; }

    public string loggedInSceneName = "Lobby";
    public string loggedOutSceneName = "LoginMenu";

    public delegate void OnDataReceivedCallback(string data);

    public void LogOut()
    {
        LoggedIn_Username = "";
        LoggedIn_Password = "";

        IsLoggedIn = false;

        Debug.Log("User logged out!");

        SceneManager.LoadScene(loggedOutSceneName);
    }

    public void LogIn(string username, string password)
    {
        LoggedIn_Username = username;
        LoggedIn_Password = password;

        IsLoggedIn = true;

        Debug.Log("Logged in as " + username);

        SceneManager.LoadScene(loggedInSceneName);
    }

    public void SendData(string data)
    { //called when the 'Send Data' button on the data part is pressed
        if (IsLoggedIn)
        {
            //ready to send request
            StartCoroutine(SetDataN(data)); //calls function to send: send data request
        }
    }
    /*
        IEnumerator sendSendDataRequest(string username, string password, string data)
        {
            IEnumerator eee = DCF.SetUserData(username, password, data);
            while (eee.MoveNext())
            {
                yield return eee.Current;
            }
            WWW returneddd = eee.Current as WWW;
            if (returneddd.text == "ContainsUnsupportedSymbol")
            {
                //One of the parameters contained a - symbol
                Debug.Log("Data Upload Error. Could be a server error. To check try again, if problem still occurs, contact us.");
            }
            if (returneddd.text == "Error")
            {
                //Error occurred. For more information of the error, DC.Login could
                //be used with the same username and password
                Debug.Log("Data Upload Error: Contains Unsupported Symbol '-'");
            }
        }
        */
    public void GetData(OnDataReceivedCallback onDataReceived)
    { //called when the 'Get Data' button on the data part is pressed

        if (IsLoggedIn)
        {
            //ready to send request
            StartCoroutine(GetDataN(onDataReceived)); //calls function to send get data request
        }
    }

    /*IEnumerator sendGetDataRequest(string username, string password, OnDataReceivedCallback onDataReceived)
	{
		string data = "ERROR";

		IEnumerator eeee = DCF.GetUserData(username, password);
		while (eeee.MoveNext())
		{
			yield return eeee.Current;
		}
		WWW returnedddd = eeee.Current as WWW;
		if (returnedddd.text == "Error")
		{
			//Error occurred. For more information of the error, DC.Login could
			//be used with the same username and password
			Debug.Log("Data Upload Error. Could be a server error. To check try again, if problem still occurs, contact us.");
		}
		else
		{
			if (returnedddd.text == "ContainsUnsupportedSymbol")
			{
				//One of the parameters contained a - symbol
				Debug.Log("Get Data Error: Contains Unsupported Symbol '-'");
			}
			else
			{
				//Data received in returned.text variable
				string DataRecieved = returnedddd.text;
				data = DataRecieved;
			}
		}

		if (onDataReceived != null)
			onDataReceived.Invoke(data);
	}
    */

    //edit


    IEnumerator GetDataN(OnDataReceivedCallback onDataReceived)
    {
        IEnumerator e = DCF.GetUserData(LoggedIn_Username, LoggedIn_Password); // << Send request to get the player's data string. Provides the username and password
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Error")
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            Debug.Log("Data Upload(GET) Error. Could be a server error. To check try again, if problem still occurs, contact us.");


        }
        else
        {
            //The player's data was retrieved. Goes back to loggedIn UI and displays the retrieved data in the InputField
            //loadingParent.gameObject.SetActive(false);
            //  loggedInParent.gameObject.SetActive(true);
            //LoggedIn_Data = response;
            Debug.Log("Success Get");

            if (onDataReceived != null)
                onDataReceived.Invoke(response);

        }
    }
    IEnumerator SetDataN(string data)
    {
        IEnumerator e = DCF.SetUserData(LoggedIn_Username, LoggedIn_Password, data); // << Send request to set the player's data string. Provides the username, password and new data string
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Success")
        {
            //The data string was set correctly. Goes back to LoggedIn UI
            //    loadingParent.gameObject.SetActive(false);
            //loggedInParent.gameObject.SetActive(true);
            Debug.Log("Success Set");
        }
        else
        {
            //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
            Debug.Log("Data Upload(SET) Error. Could be a server error. To check try again, if problem still occurs, contact us.");
        }
    }

}
