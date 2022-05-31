using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FillStarScript : MonoBehaviour
{
    [SerializeField] private List<Image> starImages;

    public void CheckIfIsActive(int idx)
    {
        for (var i = 0; i < starImages.Count; i++)
        {
            if (i < idx)
            {
                starImages[i].fillAmount = 1f;
                starImages[i].gameObject.SetActive(true); 
            }
            else
            {
                starImages[i].fillAmount = 0f;
                starImages[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenNewStar(int idx)
    {
        for (var i = 1; i < idx; i++)
        {
            if (i != idx - 1) continue;
            var sequence = DOTween.Sequence();
            starImages[i].fillAmount = 0f;
            starImages[i].gameObject.SetActive(true);
            sequence.Append(starImages[i].DOFillAmount(1f, 0.2f));
            sequence.Append(starImages[i].transform.DOPunchScale(Vector3.one * 1.5f, 0.2f));
        }
    }
}