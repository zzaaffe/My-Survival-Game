using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    [SerializeField] Light sunLight;

    public  float dayTime;
    public  float dayToNightTime;
    public float nightTime;
    public float nightToDayTime;

    public float lightValue;
    private int dayNum = 0;
    [SerializeField] Image timeStateImg;
    [SerializeField] Text dayNumText;
    [SerializeField] Sprite[] dayStateSprite;

    private bool isDay = true;

    public bool IsDay 
    { 
        get => isDay; 
        set
        {
            isDay = value;
            if(isDay)
            {
                dayNum += 1;
                dayNumText.text = "Day " + dayNum.ToString();
                timeStateImg.sprite = dayStateSprite[0];
            }
            else
            {
                timeStateImg.sprite = dayStateSprite[1];
            }
        }
    }

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        IsDay = true;
        StartCoroutine(UpdateTime());
    }

    private IEnumerator UpdateTime()
    {
        while(true)
        {
            yield return null;
            if(IsDay)
            {
                lightValue -= 1 / dayToNightTime * Time.deltaTime; 
                SetLightValue(lightValue);
                if(lightValue < 0)
                {
                    IsDay = false;
                    yield return new WaitForSeconds(nightTime);
                }
            }
            else
            {
                lightValue += 1 / nightToDayTime * Time.deltaTime;
                SetLightValue(lightValue);
                if (lightValue >= 1)
                {
                    IsDay = true;
                    yield return new WaitForSeconds(dayTime);
                }
            }
        }
    }

    private void SetLightValue(float lightValue)
    {
        RenderSettings.ambientIntensity = lightValue;
        sunLight.intensity = lightValue;
    }
}
