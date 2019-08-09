using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopupMoving : MonoBehaviour
{
    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Time.deltaTime*20, Time.deltaTime * 7,0);
        if (this.transform.localPosition.x > 00 && this.transform.localPosition.y > 250)
            Destroy(this.gameObject);
    }
}
