using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    Camera MainCam;
    // Start is called before the first frame update
    void Start()
    {
        MainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(MainCam.transform.position.x - 0.5f, MainCam.transform.position.y, transform.position.z);
    }

    public void StartAnim()
    {
        GetComponent<Animator>().SetTrigger("DoTransition");
    }
}
