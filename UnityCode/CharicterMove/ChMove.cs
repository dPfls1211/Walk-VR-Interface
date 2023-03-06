using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChMove : MonoBehaviour
{
    float i = 0;
    int j = 0;
    public float Speed=1;
    public float rotation = 100f;
    public Animation animation;
    List<string> animArray;
    //0 : wark
    //1 : Fast Run
    //2 : Running
    // Start is called before the first frame update
    void Start()
    {
        animation=gameObject.GetComponent<Animation>();
        animArray = new List<string>();
        AnimationArray();
        animation.wrapMode = WrapMode.Loop;
        action(0);
    }
    // Update is called once per frame
    void Update()
    {

        //leftRotation();
        //leftRotation();
        //i++;
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
    public void AnimationArray()
    {
        foreach (AnimationState state in animation)
        {
            animArray.Add(state.name);
        }
    }
    //앞으로
    public void forword(int num=0) //z축이동 forward  -x축 left
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        action(num);
        j = 0;
    }

    //뒤로
    public void backword(int num=0)
    {
        transform.Translate(Vector3.back * Speed * Time.deltaTime);
        action(num);
        j = 0;
    }

    //왼쪽회전
    public void leftRotation()
    {
       
        action(0);
    }
    public void moveZero()
    {
        float i = 0.0f;
        i += Time.deltaTime;
        if (i > 3)
            animation.Stop();
    }

    //오른쪽회전
    public void rightRotation()
    {
        action(0);
    }
    public void action(int index)
    {
        animation.Play(animArray[index]);
        i = 0;
    }
}
