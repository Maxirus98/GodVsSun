using System.Collections;
using UnityEngine;

public class HelperHandler : MonoBehaviour
{
    public float seconds = 5f;

    private void OnEnable()
    {
        StartCoroutine(DisableAfterTime());
    }

    IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(seconds);
        if (gameObject.activeInHierarchy)
            gameObject.SetActive(false);
    }
}
