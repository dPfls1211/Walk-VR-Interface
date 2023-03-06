using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System.Text;

public class Arduino3 : MonoBehaviour
{
    private SerialPort sp;
    public string message;
    public GameObject people;

    void Start()
    {
        //아래 COM7과 76800 수정
        sp = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
        sp.Open();
        sp.ReadTimeout = 50;

        sp.Write("test");
        Debug.Log("send message");
    }

    // Update is called once per frame
    void Update()
    {
        message = "";
        try
        {
            byte tempB = (byte)sp.ReadByte();
            while (tempB != 255)
            {
                message += ((char)tempB);
                tempB = (byte)sp.ReadByte();
            }
        }
        catch (System.Exception e)
        {

        }

        if (message != "")
        {
            Debug.Log(message[0]);
            if (message[0] == 'M')
                people.transform.Translate(Vector3.right * Time.deltaTime * 7);
            Debug.Log("receive: " + message);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            people.transform.Translate(Vector3.right * Time.deltaTime * 7);
        }
    }

}