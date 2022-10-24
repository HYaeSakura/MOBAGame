using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camp : Turret
{
    public override void DeathResponse()
    {
        gameObject.SetActive(false);
    }

    public override void ResurgeResponse()
    {
        gameObject.SetActive(true);
    }
}
