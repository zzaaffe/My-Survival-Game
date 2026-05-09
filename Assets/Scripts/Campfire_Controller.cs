using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire_Controller : MonoBehaviour
{
    public static Campfire_Controller Instance;
    [SerializeField] Light campFireLight;
    private float time = 30f; //最大燃烧时间
    private float currentTime = 30f; //剩余燃烧时间

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(currentTime <= 0)
        {
            currentTime = 0;
            // 关闭火焰粒子和灯光
            campFireLight.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            currentTime -= Time.deltaTime;
            campFireLight.intensity = Mathf.Clamp(currentTime / time, 0, 1) * 10f;
        }
    }

    public void AddWood()
    {
        currentTime += 15f;
        campFireLight.transform.parent.gameObject.SetActive(true);
    }

    public bool CanBake()
    {
        if(currentTime > 0)return true;
        return false;
    }
}
