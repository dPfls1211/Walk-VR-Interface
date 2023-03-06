using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_cc : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    float _countT = 0;
    public int _countTime()
    {
        _countT += Time.deltaTime;

        if (_countT > 1.0f){
            _countT = 0;
            return 1;
        }
        else
            return 0;
    }
}
