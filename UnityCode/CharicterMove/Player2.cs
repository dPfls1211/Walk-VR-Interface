using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;
using System;
using ArduinoBluetoothAPI;
using UnityEngine.UI;
using System.IO;
using System.IO.Ports;
public class Player2 : MonoBehaviourPun
{//photonView 프로퍼티만  추가하여 단순 확장한 클래스다
    private PhotonView PV;
    public GameObject people;
    //예린쓰 추가
    float i = 0;
    int j = 0;
    public float Speed = 0;
    public float rotation = 100f;
    public Animation animation;
    List<string> animArray;
    //0 : wark
    //1 : Fast Run
    //2 : Running
    public GameObject cameraObject;
    float speed = 1;
    BluetoothHelper bluetoothHelper, bluetoothHelper2;
    string deviceName, deviceName2;
    string received_message;
    public Animation zom;
    public StreamWriter writer, writer2;
    public FileStream fs, fs2;
    float moveZ;
    float moveX;
    public Text _text, text2;
    GameObject obj2, obj3;
    int[] walknum;
    private SerialPort sp;
    string message = "";
    int[] recentnum_five, recentnum_five2;
    int check_num = 0, is_zombie = 0, is_zombie2=0;

    //1번이 저는 다리 2번이 안저는 다리
    void Start()
    {
        recentnum_five = new int[5];
        recentnum_five2 = new int[5];

        for(int i = 0; i < 5; i++)
        {
            recentnum_five[i] = 0;
            recentnum_five2[i] = 0;
        }
        deviceName = "BL06";
        deviceName2 = "d";
        obj2 = GameObject.FindWithTag("text");
       // obj3 = GameObject.FindWithTag("text2");
        _text = obj2.GetComponent<Text>();
       // text2 = obj3.GetComponent<Text>();
        fs = new FileStream("test1.txt", FileMode.Create, FileAccess.Write);
        writer = new StreamWriter(fs, System.Text.Encoding.Unicode);

        try
        {
            bluetoothHelper = BluetoothHelper.GetInstance(deviceName);

            bluetoothHelper.OnConnected += OnConnected;
            bluetoothHelper.OnConnectionFailed += OnConnectionFailed;
            bluetoothHelper.OnDataReceived += OnMessageReceived; //read the data
            bluetoothHelper.setTerminatorBasedStream("\n");

            if (bluetoothHelper.isDevicePaired())
            {
                Debug.Log("try to connect");
                bluetoothHelper.Connect(); // tries to connect
            }
            else
            {
                Debug.Log("not DevicePaired");
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        try
        {
            bluetoothHelper2 = BluetoothHelper.GetInstance(deviceName2);

            bluetoothHelper2.OnConnected += OnConnected;
            bluetoothHelper2.OnConnectionFailed += OnConnectionFailed;
            bluetoothHelper2.OnDataReceived += OnMessageReceived; //read the data
            bluetoothHelper2.setTerminatorBasedStream("\n");

            if (bluetoothHelper2.isDevicePaired())
            {
                Debug.Log("try to connect");
                bluetoothHelper2.Connect(); // tries to connect
            }
            else
            {
                Debug.Log("not DevicePaired");
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }


        //animation = gameObject.GetComponent<Animation>();
        animation = gameObject.GetComponentInChildren<Animation>();
        animArray = new List<string>();
        AnimationArray();
        animation.wrapMode = WrapMode.Loop;
        action(0);
        //cameraObject = transform.GetChild(2).gameObject;   //모바일아닐시  주석처리
        //Input.gyro.enabled = true;          //모바일아닐시  주석처리

        //Debug.Log(_text.text);
        /* sp = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
         sp.Open();
         sp.ReadTimeout = 50;

         sp.Write("test");
         Debug.Log("send message");*/
        /*sp = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
        sp.Open();
        sp.ReadTimeout = 50;

        sp.Write("test");
        Debug.Log("send message");*/
    }
    void OnConnected()
    {

        Debug.Log("ymmmummy");
        try
        {
            bluetoothHelper.StartListening();
            Debug.Log("Connected");
        }
        catch (Exception ex)
        {
            Debug.Log("disConnected");
            Debug.Log(ex.Message);
        }
    }
    void OnConnectionFailed()
    {
        //Debug.Log("Connection Failed");
    }
    void Update()
    {
        //카메라 위아래 회전
        //Debug.Log(cameraObject.name);     //모바일아닐시  아래 주석처리
        //cameraObject.transform.Rotate(-Input.gyro.rotationRateUnbiased.x, 0,0);
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
            //Debug.Log(message[0]);
            if (message[0] == 'M')
                people.transform.Translate(Vector3.right * Time.deltaTime * 7);
            //Debug.Log("receive: " + message);
            writer.WriteLine(message);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            writer.Close();
        }
        if (Input.GetKey(KeyCode.Z))
        {
            action(3);
        }
        //애니메이션 기본자세 설정
        basicAni();

        //만약 자신이 로컬 플레이어라면
        if (!photonView.IsMine)
        {
            return;
        }

        //0628새로 수정 부분
        moveZ = 0f;
        moveX = 0f;


        if (Input.GetKey(KeyCode.W))
        {
            //action(1);
            //j = 0;
            //moveZ += 1f;

            //0628새로 수정 부분 예시
            Moving(1.0f, 1);
        }
        
        if (Input.GetKey(KeyCode.Y))
        {
            Moving(1.0f, 0);
        }
        if (Input.GetKey(KeyCode.U))
        {
            Moving(3.0f, 2);
        }
        if (Input.GetKey(KeyCode.I))
        {
            Moving(6.0f, 1);
        }

        if (Input.GetKey(KeyCode.S))
        {
            action(1);
            j = 0;
            moveZ -= 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            action(0);
            j = 0;
            //moveX -= 1f;
            transform.Rotate(0, -5, 0);
           // transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y, 0);     //모바일아닐시  주석처리
        }

        if (Input.GetKey(KeyCode.D))
        {
            action(0);
            j = 0;
            // moveX += 1f;
            transform.Rotate(0, 5, 0);

             //transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y, 0);     //모바일아닐시  주석처리
        }

        //_text.text = "!!!!!asdffg";
        // message = bluetoothHelper.Read();
        //message = "";

        // _text.text = "g: " + message;
        if (bluetoothHelper != null && bluetoothHelper2 !=null)
        {
            //   message = bluetoothHelper.Read();
            //   Debug.Log("메세지:" + message);
            //message = "";

            //    _text.text = "g: " + message;
            if (bluetoothHelper.Available && bluetoothHelper2.Available)
            {
                Debug.Log("1234");
                string message;
                message = bluetoothHelper.Read();

                string message2;
                message2 = bluetoothHelper2.Read();
                if (message != "")
                {
                    check_num += 1;
                    int message_num = int.Parse(message);
                    _text.text = "g: " + message;

                    int message_num2 = int.Parse(message2);
                    _text.text = "g: " + message2;

                    writer.WriteLine(message);
                    Debug.Log(message);

                    //저는 다리

                    float speed_num = 0;
                    if (check_num >= 5)
                    {
                        for (int ll = 0; ll < 4; ll++)
                        {
                            recentnum_five[ll] = recentnum_five[ll + 1];
                        }
                        recentnum_five[4] = message_num;
                    }
                    else
                    {
                        recentnum_five[check_num - 1] = message_num;
                    }
                    for (int ll = 0; ll < 5; ll++)
                    {
                        if (recentnum_five[ll] < 1500 && recentnum_five[ll] > -1500)
                        {
                            is_zombie = 1;  //좀비 1은 좀비
                        }
                        else
                        {
                            is_zombie = 0; //좀비0은 움직임
                            ll = 5;
                        }
                    }
                    for (int ll = 0; ll < 5; ll++)
                    {
                        if (recentnum_five[ll] < 500 && recentnum_five[ll] > -500)
                        {
                            is_zombie = 2;//좀비2는 멈춤
                        }
                        else
                        {
                            ll = 5;
                        }
                    }

                    //안저는 다리
                    if (check_num >= 5)
                    {
                        for (int ll = 0; ll < 4; ll++)
                        {
                            recentnum_five2[ll] = recentnum_five2[ll + 1];
                        }
                        recentnum_five2[4] = message_num2;
                    }
                    else
                    {
                        recentnum_five2[check_num - 1] = message_num2;
                    }
                    for (int ll = 0; ll < 5; ll++)
                    {
                        if (recentnum_five2[ll] < 500 && recentnum_five2[ll] > -500)
                        {
                            is_zombie2 = 2;
                        }
                        else
                        {
                            ll = 5;
                        }
                    }



                    if (message_num > 3000 || message_num < -3000)
                        message_num = 3000;
                    if (message_num < -1000 && message_num > -3000)
                        message_num = message_num * -1;

                    if (message_num2 > 3000 || message_num2 < -3000)
                        message_num2 = 3000;
                    if (message_num2 < -1000 && message_num2 > -3000)
                        message_num2 = message_num2 * -1;
                    Debug.Log("message_num : " + message_num);
                    speed_num = (float)message_num / 3000f;
                    speed_num = speed_num * 5;
                    Debug.Log("speed_num : " + speed_num);
                    if (message_num != 0)
                    {
                        Debug.Log("Move");
                        if(is_zombie==2 && is_zombie2 == 2)
                        {

                            Moving(0, 0);
                            Debug.Log("멈춰!");
                            speed = 0;
                        }
                        else if (is_zombie == 1)
                        {
                            Moving(speed_num, 3);
                            Debug.Log("좀비 조좀비");
                            speed = speed_num;
                        }
                        else
                        {
                            if (message_num < 300 && message_num2< 300) //혹시모를 한번더 체크
                            {
                                Moving(speed_num, 0);
                                Debug.Log("멈춰!");
                                speed = speed_num;
                            }
                            else if (message_num < 3000)
                            {
                                Moving(speed_num, 2);
                                speed = speed_num;
                            }
                            else
                            {
                                Moving(speed_num, 1);
                                speed = speed_num;
                            }
                        }
                        
                    }
                    //Debug.Log("receive: " + message);

                }
            }
            if (Input.GetKey(KeyCode.Q))
            {
                writer.Close();
            }
        }
        

        {//모바일아닐시  주석처리
          //  transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y, 0);
            
        }
        

        

        //이동하게 하는 코드
        //Vector3(moveX, 0f, moveZ) * 0.1f);
        var directionL = transform.forward * Time.deltaTime * speed;
        transform.Translate(directionL, Space.World);
    }

    public void AnimationArray()
    {

        foreach (AnimationState state in animation)
        {
            animArray.Add(state.name);
        }
    }

    public void action(int index)
    {
        animation.Play(animArray[index]);
        i = 0;
    }

    //0628새로 수정 부분
    public void basicAni()
    {
        i += Time.deltaTime;
        if (i > 1)
        {
            action(0);
            i = 0.9f;
            if (j > 2)
            {
                animation.Stop();
            }
            j++;
        }
    }

    public void Moving(float speed = 0.1f, int ani = 1)
    {
        action(ani);
        j = 0;
        moveZ += speed;
    }
    void OnMessageReceived()
    {
        received_message = bluetoothHelper.Read();
        //Debug.Log(received_message);

        if (received_message.Contains("Move!!"))
        {
        }
        if (received_message.Contains("NOMove!!"))
        {
        }
    }
}