using System.Collections.Generic;
using _Game.Scripts.Tower;
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

    private Sequence _rotateSequence;
    private Sequence _verticalSequence;

    private void OnEnable()
    {
        _rotateSequence = DOTween.Sequence();
        _verticalSequence = DOTween.Sequence();
        GetComponentInParent<BaseTower>().shootingPoint = this.shootingPoint;
        verticalMoverObjects?.ForEach(m =>
            _verticalSequence.Append(m.DOLocalMoveY(
                m.transform.localPosition.y + Random.Range(verticalMoveDistance * 0.5f, verticalMoveDistance * 2f),
                verticalMoveTime)));
        selfRotatingObjects?.ForEach(r =>
            _rotateSequence.Join(r.DOLocalRotate(Vector3.up * 360f, selfRotateSpeed, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)));
        _verticalSequence.SetLoops(-1, LoopType.Yoyo);
        _rotateSequence.SetLoops(-1, LoopType.Incremental);
    }

    private void OnDisable()
    {
        _rotateSequence.Kill();
        _verticalSequence.Kill();
    }
}