using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GridSaveMoney : MonoBehaviour
{
    [SerializeField] private GameObject mergeMoney;
    private void Awake()
    {
        if (PlayerPrefs.HasKey(transform.name))
        {
            Instantiate(mergeMoney, new Vector3(transform.transform.position.x, .1f, transform.transform.position.z), mergeMoney.transform.rotation).name = PlayerPrefs.GetInt(transform.name).ToString();
        }
        PlayerPrefs.DeleteKey(transform.name);
    }
    public void saveMoney()
    {
        if (transform.childCount > 0)
        {
            PlayerPrefs.SetInt(transform.name, int.Parse(transform.GetChild(0).name));
        }
    }
}
