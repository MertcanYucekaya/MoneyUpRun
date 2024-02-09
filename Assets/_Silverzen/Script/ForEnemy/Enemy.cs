using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float fireRate;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int hitPoint;
    private int currentHitPoint;
    Transform bulletTransform;
    [Header("ThrowMoneyParticle")]
    [SerializeField] public GameObject throwMoneyParticle;
    [SerializeField] public float particleDeActiveTime;
    [SerializeField] public Transform usedThrowMoneyParticle;
    [SerializeField] public Transform unUsedThrowMoneyParticle;
    [HideInInspector] public List<GameObject> particles = new();
    [Header("BloodParticle")]
    [SerializeField] private GameObject bloodParticle;
    [SerializeField] private Transform particleSpawn;
    [Header("References")]
    [SerializeField] private EnemyCollider enemyCollider;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPos;
    [SerializeField] private Transform usedBulletParent;
    [SerializeField] private Transform unUsedBulletParent;
    [SerializeField] private ZMove zMove;
    [SerializeField] private TextMeshProUGUI hitPointtext;
    [SerializeField] private Animator animator;
    Coroutine fireThread;
    

    private void Start()
    {
        zMove.setMove(false);
        currentHitPoint = hitPoint;
        hitPointtext.text = hitPoint + "$";
        animator.enabled = false;
    }
    IEnumerator instantBullet()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);
            bulletTransform = null;
            if (unUsedBulletParent.childCount > 0)
            {
                bulletTransform = unUsedBulletParent.GetChild(0).transform;
                bulletTransform.gameObject.SetActive(true);
            }
            else
            {
                bulletTransform = Instantiate(bullet,Vector3.zero,bullet.transform.rotation).transform;
            }
            if (bulletTransform.name != bulletSpeed.ToString())
            {
                bulletTransform.name = bulletSpeed.ToString();
            }
            bulletTransform.SetParent(usedBulletParent);
            bulletTransform.position = bulletSpawnPos.position;
        }
    }
    public void activate()
    {
        if(animator.enabled == false)
        {
            zMove.setMove(true);
            animator.enabled = true;
            fireThread =  StartCoroutine(instantBullet());
        }
    }
    public void stopEnemy()
    {
        if(fireThread != null)
        {
            StopCoroutine(fireThread);
            zMove.setMove(false);
            animator.enabled = false;
        }
        
    }
    private object _lock = new object();
    public void getDamage(int damage)
    {
        lock (_lock)
        {
            currentHitPoint -= damage;
            if (currentHitPoint > 0)
            {
                hitPointtext.text = currentHitPoint + "$";
                blood(false);
            }
            else
            {
                canvas.SetActive(false);
                enemyCollider.gameObject.SetActive(false);
                blood(true);
                animator.SetBool("Death", true);
                zMove.setMove(false);
                Destroy(gameObject,2);
            }
        } 
    }
    public void setHitpoint(float multiplier)
    {
        hitPoint = (int)Mathf.Round(hitPoint * multiplier);
        hitPointtext.text = hitPoint + "$";
    }
    void blood(bool death)
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject g = Instantiate(bloodParticle, particleSpawn.position, Quaternion.identity);
            g.transform.SetParent(null);
            if (death)
            {
                g.transform.localScale = Vector3.one * 0.3f;
            }
            else
            {
                g.transform.localScale = Vector3.one * 0.2f;
            }
            g.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0, 0.1f), Random.Range(-0.3f, -0.1f)) * 300);
            g.transform.DOScale(Vector3.zero, 4).OnComplete(() => 
            {
                Destroy(g);
            });
        }
    }
}
