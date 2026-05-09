using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] List<GameObject> EnemyPrefabs = new List<GameObject>();
    private bool isProduce = true;

    private void Start()
    {
        StartCoroutine(UpdateItem());
    }
    private void Update()
    {
        if(transform.childCount < 50 && isProduce)
        {
            StarProduce();
        }
        else if(transform.childCount >= 50 && !isProduce)
        {
            StopProduce();
        }
    }
    private IEnumerator UpdateItem()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 10f));
            foreach (var item in EnemyPrefabs)
            {
                Instantiate(item, new Vector3(Random.Range(-40f, 40f), 0, Random.Range(-40f, 40f)), Quaternion.identity, this.transform);
            }
        }

    }

    private void StarProduce()
    {
        StartCoroutine(UpdateItem());
        isProduce = false;
    }

    private void StopProduce()
    {
        StopAllCoroutines();
        isProduce = true;
    }
}
