using DG.Tweening;
using System.Collections;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Raycast : MonoBehaviour
{
    private GameObject selectedObject;
    [SerializeField] private Transform startMergeMoney;
    [SerializeField] private AddMoney addMoney;
    [SerializeField] private MoneyGridCheck moneyGridCheck;

    private void Start()
    {
        DOTween.Init();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedObject == null)
            {
                RaycastHit hit = castRay();

                if (hit.collider != null)
                {
                    if (!hit.collider.CompareTag("Merge/Money"))
                    {
                        return;
                    }
                    selectedObject = hit.collider.gameObject;
                    selectedObject.transform.SetParent(null);
                    if (selectedObject.transform.localScale != startMergeMoney.localScale)
                    {
                        selectedObject.transform.DOKill();
                        selectedObject.transform.localScale = startMergeMoney.localScale;
                    }
                }
            }
        }
        if(Input.GetMouseButtonUp(0) && selectedObject != null)
        {
            StartCoroutine(checkDelay());
            selectedObject = null;
        }
        if (selectedObject != null)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
            selectedObject.transform.position = new Vector3(worldPosition.x, .3f, worldPosition.z);
        }
    }

    private RaycastHit castRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }
    IEnumerator checkDelay()
    {
        yield return new WaitUntil(() => selectedObject.GetComponent<MergeMoney>().posReset() == true);
        addMoney.checkGrid();
        moneyGridCheck.check();
    }
}
