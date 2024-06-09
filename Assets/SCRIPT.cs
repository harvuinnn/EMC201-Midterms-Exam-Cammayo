using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SCRIPT : MonoBehaviour
{
    public GameObject chick, hen, rooster, egg;

    private int eggCount = 0;
    private int chickCount = 0;
    private int henCount = 0;
    private int roosterCount = 0;

    public TextMeshProUGUI eggText;
    public TextMeshProUGUI chickText;
    public TextMeshProUGUI henText;
    public TextMeshProUGUI roosterText;

    private bool firstChickMatured = false;

    //START
    void Start()
    {
        SpawnEgg();
        CounterUI();
    }

    //UPDATE
    void Update()
    {

    }

    private void SpawnEgg()
    {
        GameObject eggIns = Instantiate(egg, transform.position, Quaternion.identity);
        eggIns.tag = "Egg";

        eggCount++;
        CounterUI();
        StartCoroutine(HatchEgg(eggIns));
    }

    private IEnumerator HatchEgg(GameObject egg)
    {
        yield return new WaitForSeconds(10);
        Destroy(egg);
        eggCount--;
        CounterUI();

        GameObject chickIns = Instantiate(chick, egg.transform.position, Quaternion.identity);
        chickCount++;
        CounterUI();

        if (!firstChickMatured)
        {
            firstChickMatured = true;
            StartCoroutine(MatureChick(chickIns, true)); // Force the first chick to mature into a hen
        }
        else
        {
            StartCoroutine(MatureChick(chickIns, false));
        }
    }

    private IEnumerator MatureChick(GameObject chick, bool forceHen)
    {
        yield return new WaitForSeconds(10);
        Destroy(chick);
        chickCount--;
        CounterUI();

        GameObject adultChicken;
        if (forceHen || Random.value < 0.5f)
        {
            adultChicken = Instantiate(hen, chick.transform.position, Quaternion.identity);
            henCount++;
            adultChicken.tag = "Hen";
            StartCoroutine(HenLifeCycle(adultChicken));
        }
        else
        {
            adultChicken = Instantiate(rooster, chick.transform.position, Quaternion.identity);
            roosterCount++;
            StartCoroutine(DestroyRooster(adultChicken)); // Start coroutine to destroy rooster after 20 seconds
        }
        CounterUI();
    }

    private IEnumerator HenLifeCycle(GameObject hen)
    {
        yield return new WaitForSeconds(30);

        int numEggs = Random.Range(2, 11);

        for (int i = 0; i < numEggs; i++)
        {
            SpawnEgg();
        }

        yield return new WaitForSeconds(10);

        henCount--;
        Destroy(hen);
        CounterUI();
    }

    private IEnumerator DestroyRooster(GameObject rooster)
    {
        yield return new WaitForSeconds(40);
        roosterCount--;
        Destroy(rooster);
        CounterUI();
    }

    private void CounterUI()
    {
        eggText.text = " " + eggCount;
        chickText.text = " " + chickCount;
        henText.text = " " + henCount;
        roosterText.text = " " + roosterCount;
    }
}
