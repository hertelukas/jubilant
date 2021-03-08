using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPackage : Package
{
    public TestPackage() : base("TestUser")
    {
        
    }

    public override string GetContent()
    {
        return "This is a test.";
    }
}
