using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ExecutionEditModeForFanus : MonoBehaviour
{
    [SerializeField] private Fanus[] fanus;
    [SerializeField] private int startValue;
    private int startValueC;
    [SerializeField] private float multiplierValue;
    private float multiplierValueC;
    [SerializeField] private float startDistance;
    private float startDistanceC;
    [SerializeField] private float inceraseDistance;
    [Header("----------------------")]
    [SerializeField] private float increaseForAmount;
    [SerializeField] private float multiplierValueIncrease;
    [SerializeField] private int startValueIncrese;
    void Awake()
    {
        startDistanceC = startDistance;
        startValueC = startValue;
        multiplierValueC = multiplierValue;
        int j = 0;
        int x = 0;
        for (int i = 0; i < fanus.Length; i++)
        {
            x++;
            if(x == increaseForAmount)
            {
                x = 0;
                startValueC += startValueIncrese;
                multiplierValueC += multiplierValueIncrease;
            }
            j++;
            fanus[i].fanusInit(startValueC);
            fanus[i].transform.position = new Vector3(fanus[i].transform.position.x, fanus[i].transform.position.y, startDistanceC);
            if (j == 3)
            {
                startDistanceC += inceraseDistance;
                startValueC = (int) Mathf.Round(startValueC * multiplierValueC);
                j = 0;
            }
        }
        Debug.Log("Execution Run!");
    }
}
