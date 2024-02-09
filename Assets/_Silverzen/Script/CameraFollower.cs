using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private float lerpSpeed = 0.125f;
    [SerializeField] private Transform target;
    [SerializeField] private float duration, magnitude;
    Vector3 lerpPos;

    private void Start()
    {
        DOTween.Init();
        transform.position = target.position;
        transform.rotation = target.rotation;
    }


    public IEnumerator ShakeAnimation(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
    private void LateUpdate()
    {
        CameraMove();
    }
    void CameraMove()
    {
        if (target == null) return;

        lerpPos = Vector3.Lerp(transform.position, target.position, lerpSpeed* Time.deltaTime);
        transform.localPosition = lerpPos;
    }
    public void StartShake()
    {
        StartCoroutine(ShakeAnimation(duration, magnitude));
    }
    public void changeRotation(Vector3 rotation)
    {
        transform.DORotate(rotation, .8f);
    }
}
