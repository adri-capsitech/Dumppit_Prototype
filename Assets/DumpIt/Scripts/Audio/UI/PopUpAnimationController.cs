using DG.Tweening;
using UnityEngine;
using TMPro;

public class PopupAnimationController : MonoBehaviour
{
    public RectTransform rectPanel;
    public string popupName;
    public string popupText;
    public float waitTime = 2;
    public TMP_Text popupTextUI;
    void OnEnable()
    {
        popupTextUI.text = popupText;
        DOTween.Sequence()
            .Append(rectPanel.DOAnchorPosY(30, 0.5f, false))
            .AppendInterval(waitTime)
            .Append(rectPanel.DOAnchorPosY(-350, 1, false))
            .OnComplete(() =>
             {
                 AppStateManager.Instance.HideOverlay(popupName);
             });
    }
}
