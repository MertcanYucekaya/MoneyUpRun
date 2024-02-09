using DG.Tweening;
using UnityEngine;

public class ThrowMoney : MonoBehaviour
{
    [SerializeField] private Material[] materials;
    [SerializeField] private MeshRenderer meshRenderer;
    
    void Start()
    {
        setMaterial();
    }
    void setMaterial()
    {
        meshRenderer.material = materials[int.Parse(transform.name) - 1];
    }
}
