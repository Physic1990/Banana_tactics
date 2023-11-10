using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework.Internal;
using System;


public class LevelSetupTests : MonoBehaviour
{
    [UnitySetUp]
    public IEnumerator Setup()
    {
        // Load the desired scene for testing
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);

        yield return null; // Wait for a frame to let any potential Start methods execute
    }
    [UnityTest]
    public IEnumerator CheckPlayerAndEnemyTagsInScene()
    {
        if (LevelSetup.Instance == null)
        {
            Assert.Fail("LevelSetup not found in the scene.");
            yield break;
        }
        // Assert
        Assert.AreEqual("Player", LevelSetup.Instance.PlayerTag, "Player tag is incorrect");
        Assert.AreEqual("Enemy", LevelSetup.Instance.EnemyTag, "Enemy tag is incorrect");
    }
}
