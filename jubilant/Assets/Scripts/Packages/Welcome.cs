using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Welcome : Package
{
    private string message;
    public Welcome() : base(PackageType.Welcome) { }

    public override string GetContent()
    {
        return GameManager.username;
    }
}
