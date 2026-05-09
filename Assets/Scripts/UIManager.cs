using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] Image hpImage;
    [SerializeField] Image hungryImage;
    [SerializeField] Text  timeText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateHungry()
    {
        hungryImage.fillAmount = Player_Controller.Instance.HungryValue / 100;
    }

    public void UpdateHp()
    {
        hpImage.fillAmount = Player_Controller.Instance.Hp / 100;
    }
}
