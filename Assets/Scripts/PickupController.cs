using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    //Start is called before the first frame update.
    void Start()
    {
        
    }

    //Update is called once per frame.
    void Update()
    {

        //Rotates the cube constantly.
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        
    }
}
