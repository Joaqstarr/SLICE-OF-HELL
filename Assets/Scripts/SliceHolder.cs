using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;
using DG.Tweening;

public class SliceHolder : MonoBehaviour
{
    public Slice[] HeldSlices;
    public int[] HeldIndices;
    public Dictionary<int, Slice> SliceMap;
    public float _radius = 5f;
    [SerializeField]
    private int _maxSlicesToMove;
    [Header("Tween")]
    [SerializeField] private float _moveDuration = 0.2f;
    [SerializeField] private Ease _ease;
    [SerializeField] private float _shakeSize = 0.2f;
    [SerializeField] private float _shakeRotSize = 0.2f;



    public void UpdateInfo(bool deleteIfEmpty = false , bool move = false)
    {
        HeldSlices = GetComponentsInChildren<Slice>();

        if (deleteIfEmpty)
        {
            if (HeldSlices.Length == 0)
            {
              //  Destroy(gameObject);
                return;
            }

        }


        HeldIndices = new int[HeldSlices.Length];
        SliceMap = new Dictionary<int, Slice>();
        float averageAngle = 0;
        for (int i = 0; i < HeldSlices.Length; i++)
        {
            HeldIndices[i] = HeldSlices[i].GetSliceNum();
            SliceMap[HeldIndices[i]] = HeldSlices[i];

            //if (HeldSlices.Length == 3)
               // Debug.Log("angle: " + averageAngle + " += " + ConvertAngleToNegative(HeldSlices[i].transform.rotation.eulerAngles.z));
            averageAngle += ConvertAngleToNegative(HeldSlices[i].transform.rotation.eulerAngles.z);

        }

        if (HeldSlices.Length > 0 && move)
        {
            averageAngle /= HeldSlices.Length;
            //Debug.Log(averageAngle);
            averageAngle *= -1;
            UpdatePosition(averageAngle);

        }
        

        
    }

    private void UpdatePosition(float angle)
    {
        Debug.Log(angle);
        if(Mathf.Abs( angle) == 0)
        {
            if (transform.GetChild(0).eulerAngles.z != 22.5f)
            {
                angle = 180;
            }
            
        }
        if (HeldSlices.Length > _maxSlicesToMove || HeldSlices.Length == 0)
            return;

        float theta = angle * Mathf.PI / 180f;
        Vector2 dir = new Vector2();
        dir.y = Mathf.Cos(theta);
        dir.x = Mathf.Sin(theta);
        Vector2 targPos = Vector2.zero + dir * _radius;

        if ((Vector2)transform.localPosition != targPos)
        {
            transform.DOLocalMove(targPos, _moveDuration).SetEase(_ease);
            transform.DOShakeScale(_moveDuration, _shakeSize, 9, 89, true, ShakeRandomnessMode.Harmonic);
            transform.DOShakeRotation(_moveDuration, _shakeRotSize, 89, 9, true, ShakeRandomnessMode.Harmonic);
        }

    }

    private void OnEnable()
    {
        UpdateInfo();
    }

    public int GetSliceSize()
    {
        return SliceMap.Count;
    }

    private float ConvertAngleToNegative(float angle)
    {
        return (angle > 180) ? angle - 360 : angle;
    }

    public void Kill()
    {
        foreach(Slice slice in HeldSlices)
        {
            slice.Kill();
        }
    }
}
