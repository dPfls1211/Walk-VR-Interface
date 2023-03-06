using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (Instance == null) instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }
    
    public GameObject[] playerPositions;
    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        SpownPlayer();

        if (PhotonNetwork.IsMasterClient)
        {
            //방장만이 실행시킬 수 있는 것
            //예를 들어 배경이 스폰 되어야함.
        }
    }

    //void OnLevelWasLoaded(int level)
    //{
    //    PhotonNetwork.IsMessageQueueRunning = true;
    //}

    private void SpownPlayer()
    {
        var lacalPlayerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        var spownPosition = playerPositions[lacalPlayerIndex % playerPositions.Length]; ;
        var num = lacalPlayerIndex % playerPositions.Length;

        GameObject MyPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spownPosition.transform.position, spownPosition.transform.rotation);
        GameObject camera = GameObject.FindWithTag("MainCamera");
        camera.transform.position = MyPlayer.transform.position;
        //camera.transform.position = new Vector3(MyPlayer.transform.position.x, MyPlayer.transform.position.y + 5.178f, MyPlayer.transform.position.z);
        camera.transform.parent = MyPlayer.transform;
        camera.transform.localPosition = new Vector3(0, 1, 0);

        GameObject smallCamera = GameObject.FindWithTag("smallCamera");
        smallCamera.transform.position = MyPlayer.transform.position;
        smallCamera.transform.parent = MyPlayer.transform;
        smallCamera.transform.localPosition = new Vector3(-1.2f, 1.8f, 1.5f);

        Debug.Log(camera.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
    }
    
}
