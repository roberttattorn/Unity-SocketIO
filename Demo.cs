using UnityEngine;
using KyleDulce.SocketIo;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class ActionEvent
{
    public string ActionType;
    public float TimeElapsed;
}

public class Demo : MonoBehaviour
{
    Socket s;
    public Text text;
    private bool started = false;
    // Start is called before the first frame update
    IEnumerator Start()
    {

        #if UNITY_WEBGL && !UNITY_EDITOR
            s = SocketIo.establishSocketConnection(Application.absoluteURL);
        #else 
            s = SocketIo.establishSocketConnection("https://socket-io-er98.onrender.com");
        #endif
        s.connect();
        yield return new WaitForSeconds(3.0f);
        // define reception callbacks here
        Debug.Log("connecting...");
        s.on("connection", call);
        StartCoroutine("Run");
    }

    void call(string d) {
        
        s.emit("connectionstatus",  "confirmed");
        //Debug.Log("connected");
    }
    void read(string d)
    {
       // var rope = JsonUtility.FromJson<string>(d);
        Debug.Log(d);
        text.text = d;
    }

    void Update()
    {

        // dummy method to send a JSON structure each time the space bar is pressed
        // the server must have a method socket.on("chat") to receive and process this event.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActionEvent respObject = new ActionEvent();
            respObject.ActionType="jump";
            respObject.TimeElapsed=Time.realtimeSinceStartup;
            // string respJson = JsonUtility.ToJson("hello.");
            string respJson = "hello.";
            s.emit("chat", respJson);
            Debug.Log("emitted");
        }

        //s.on("chat", read);

    }

    IEnumerator Run()
    {
        if (started == false)
        {
           //Debug.Log("running coroutine run") ; started = true;
        }
        yield return null;
        s.on("chat", read);
    }

    void OnDestroy()
    {
        s.close();
    }

}
