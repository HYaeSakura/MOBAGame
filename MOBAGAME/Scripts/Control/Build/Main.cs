using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : Turret
{
    public override void DeathResponse()
    {
        GetComponent<Animation>().CrossFade("death");
    }
}