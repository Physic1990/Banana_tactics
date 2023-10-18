using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UITest
{
    private UIManager uiManager;


    [Test]
    public void UnitSelectionIsOpen()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        uiManager.OpenUnitSelection();

        Assert.IsTrue(uiManager.GetIsUnitSelectionVisible(), "Unit Selection is open");
    }

    [Test]
    public void UnitSelectionIsClosed()
    {
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        uiManager.CloseUnitSelection();

        Assert.IsTrue(!uiManager.GetIsUnitSelectionVisible(), "Unit Selection is closed");
    }
}
