using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework.Internal;
using System;

public class StressTest
{
    private GameObject unitSelectionMenu;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);

        yield return null;
    }

    [UnityTest]
    public IEnumerator StressTestUnitSelectionMenu()
    {
        // Finds the Unit Selection Menu.
        unitSelectionMenu = GameObject.FindGameObjectWithTag("UnitSelectionMenu");

        int numDuplicates = 0;
        float frameRateThreshold = 15.0f;
        float delaySeconds = 0.1f;
        int maxIterations = 1000;
        int increment = 100;

        for (int numIterations = increment; numIterations <= maxIterations; numIterations += increment)
        {
            // Wait to allow game to run
            yield return new WaitForSeconds(delaySeconds);

            for (int i = 0; i < numIterations; i++)
            {
                // Duplicates the Unit Selection Menu Game Object
                GameObject duplicatedMenu = GameObject.Instantiate(unitSelectionMenu);

                numDuplicates++;

                // Current frame rate
                float frameRate = 1.0f / Time.deltaTime;

                // Assertion fails if the current frame rate is below the frame rate threshold
                Assert.GreaterOrEqual(frameRate, frameRateThreshold, "Frame rate below threshold. Duplicated Objects: " + numDuplicates);

                // Yield a frame to allow any updates to occur
                yield return null;
            }
        }
    }
}
