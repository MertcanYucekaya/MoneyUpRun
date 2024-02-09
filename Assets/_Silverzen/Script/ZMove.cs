
using UnityEngine;

public class ZMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private bool isMove;

    public void init(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }
    private void Awake()
    {
        isMove = true;
    }
    private void Start()
    {
        if (transform.tag.Equals("Bullet"))
        {
            moveSpeed = -int.Parse(transform.name);
        }
        if (transform.tag.Equals("ThrowMoney"))
        {
            moveSpeed = PlayerPrefs.GetFloat("ThrowMoneySpeed");
        }
    }
    private void Update()
    {
        if (isMove)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (Time.deltaTime * moveSpeed));
        }
    }
    public void setMove(bool b)
    {
        if (b == false)
        {
            isMove = false;
        }
        else
        {
            isMove = true;
        }
    }
}
