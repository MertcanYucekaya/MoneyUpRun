using UnityEngine;
using DG.Tweening;

public class FanusDiamond : MonoBehaviour
{
    [SerializeField] private Fanus fanus;
    [SerializeField] private  Collider col;
    private int diamondAmount;

    private void Start()
    {
        DOTween.Init();
        diamondAmount = fanus.diamondAmount;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindAnyObjectByType<GameManager>().instantDiamondImage(transform,diamondAmount);
            Destroy(gameObject);
        }
    }
    public void active(Vector3 target)
    {
        transform.SetParent(null);
        col.enabled = true;
        transform.DOMove(target, .4f);
    }
}
