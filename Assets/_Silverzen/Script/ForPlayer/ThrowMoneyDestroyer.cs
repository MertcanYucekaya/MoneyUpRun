using UnityEngine;
using DG.Tweening;
public class ThrowMoneyDestroyer : MonoBehaviour
{
    [SerializeField] private float zRotate;
    [SerializeField] private float yDistance;
    [SerializeField] private float duration;
    [SerializeField] private Transform unUsedThrowMoney;
    private void Start()
    {
        DOTween.Init();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ThrowMoney"))
        {
            throwMoneyDestroyAnimation(other.transform);
        }
    }
    
    public void throwMoneyDestroy(Transform other)
    {
        other.gameObject.SetActive(false);
        other.SetParent(unUsedThrowMoney);
    }
    private object _lock = new object();
    void throwMoneyDestroyAnimation(Transform other)
    {
        lock (_lock)
        {
            other.DOKill();
            other.DORotate(new Vector3(other.eulerAngles.x, other.eulerAngles.y, zRotate), duration);
            other.DOMoveY(other.transform.position.y - yDistance, duration).OnComplete(() =>
            {
                throwMoneyDestroy(other);
            });
        }
    }
}
