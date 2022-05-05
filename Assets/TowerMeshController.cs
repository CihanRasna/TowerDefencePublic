using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class TowerMeshController : MonoBehaviour
{
    public Transform shootingPoint;

    [SerializeField] private float verticalMoveTime = 1f;
    [SerializeField] private float verticalMoveDistance = 1f;
    [SerializeField] private List<Transform> verticalMoverObjects;

    [SerializeField] private float selfRotateSpeed = 0.5f;
    [SerializeField] private List<Transform> selfRotatingObjects;

    private void OnEnable()
    {
        //GetComponentInParent<BaseTower>().shootingPoint = this.shootingPoint;
        verticalMoverObjects?.ForEach(m =>
            m.DOLocalMoveY(m.transform.localPosition.y + Random.Range(verticalMoveDistance * 0.5f,verticalMoveDistance * 2f), verticalMoveTime)
                .SetLoops(-1, LoopType.Yoyo));
        selfRotatingObjects?.ForEach(r =>
            r.DOLocalRotate(Vector3.up * 360f, selfRotateSpeed, RotateMode.FastBeyond360).SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental));
    }
}