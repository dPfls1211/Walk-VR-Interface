using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileGyro : MonoBehaviour
{
    public Text texts;
    public Text textss;
    public GameObject cameraObject;
    public GameObject _head;
    public GameObject _move;
    // Start is called before the first frame update
    void Start()
    {
        Input.gyro.enabled = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        _head.transform.Rotate(-Input.gyro.rotationRateUnbiased.x, 0 , Input.gyro.rotationRateUnbiased.z);
        cameraObject.transform.Rotate(-Input.gyro.rotationRateUnbiased.x, 0, Input.gyro.rotationRateUnbiased.z);
        //if(Input.acceleration.y>0.05f || Input.acceleration.y<-0.05f)
            //transform.GetComponent<ChMove>().leftRotation();
        //moblie 빌드시에만. 하는 조건은.. 후에
        transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y, 0);
        texts.text = "y : " + (Input.gyro.rotationRateUnbiased.y).ToString();
        textss.text = "accelery" + Input.acceleration.y.ToString();
    }
}
