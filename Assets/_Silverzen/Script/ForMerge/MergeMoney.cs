using DG.Tweening;
using System;
using UnityEngine;

public class MergeMoney : MonoBehaviour
{
    [SerializeField] private LayerMask middleGridLayer;
    [SerializeField] private GameObject mergeMoneyObject;
    [SerializeField] private Material[] mergeMoneyMaterials;
    [SerializeField] private MeshRenderer meshRenderer;
    private Collider startGrid;
    private void Start()
    {
        DOTween.Init();
        startGrid = middleGridOverlap()[0];
        transform.SetParent(startGrid.transform);
        transform.position = new Vector3(startGrid.transform.position.x, transform.position.y, startGrid.transform.position.z);
        setMoney();
        transform.DOPunchScale(Vector3.one * 0.2f, .3f);
    }
    public bool posReset()
    {
        Collider[] hitColliders = middleGridOverlap();
        if (hitColliders.Length > 0)
        {
            Collider nearGrid= hitColliders[0];
            // NEAR MIDDLE GRID
            float nearAmount = Vector3.Distance(transform.position, hitColliders[0].transform.position);
            
            for(int i = 0; i < hitColliders.Length; i++)
            {
                if (Vector3.Distance(transform.position, hitColliders[i].transform.position) < nearAmount)
                {
                    nearGrid = hitColliders[i];
                }
            }
            if (nearGrid.transform.childCount > 0)
            {
                // MERGE MONEY
                if (nearGrid.transform.CompareTag("Merge/Middle") && transform.name == nearGrid.transform.GetChild(0).transform.name)
                {
                    GameObject g = Instantiate(mergeMoneyObject, new Vector3(nearGrid.transform.position.x, .1f, nearGrid.transform.position.z), mergeMoneyObject.transform.rotation);
                    int name = int.Parse(transform.name);
                    name++;
                    g.transform.name = name.ToString();
                    Destroy(nearGrid.transform.GetChild(0).gameObject);
                    Destroy(gameObject);
                    return true;
                }
            }
            else
            {
                startGrid = nearGrid;
            }
        }
        transform.SetParent(startGrid.transform);
        transform.position = new Vector3(startGrid.transform.position.x, .1f, startGrid.transform.position.z);
        return true;
    }

    Collider[] middleGridOverlap()
    {
       return Physics.OverlapBox(gameObject.transform.position, new Vector3(.5f,1,.5f), transform.rotation, middleGridLayer);
    }
    void setMoney()
    {
        meshRenderer.material = mergeMoneyMaterials[int.Parse(transform.name) - 1];
    }
}

