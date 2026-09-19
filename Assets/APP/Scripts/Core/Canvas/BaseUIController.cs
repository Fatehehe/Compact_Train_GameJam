using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

public abstract class BaseUIController : MonoBehaviour
{
    [Header("UI Container")]
    [SerializeField] protected GameObject root;
    [SerializeField] protected CanvasGroup canvasGroup;

    [Header("Animation Settings")]
    [SerializeField] protected float fadeDuration = 0.25f;

    [Header("Feedbacks")]
    [SerializeField] protected MMF_Player showUIFeedback;
    [SerializeField] protected MMF_Player hideUIFeedback;

    private bool isActive;
    public bool IsActive => isActive;

    protected virtual void Awake()
    {
        if (root != null)
            root.SetActive(false);

        if (canvasGroup != null)
            canvasGroup.alpha = 0;
    }

    protected virtual void Start()
    {
    }

    protected virtual void OnDestroy()
    {
        if (showUIFeedback != null) showUIFeedback.Events.OnComplete.RemoveListener(OnShowComplete);
        if (hideUIFeedback != null) hideUIFeedback.Events.OnComplete.RemoveListener(OnHideComplete);
    }

    public virtual void SetActive(bool isActive)
    {
        if (root == null) return;

        if (this.isActive == isActive) return;

        if (canvasGroup == null)
        {
            this.isActive = isActive;
            root.SetActive(isActive);
            return;
        }

        canvasGroup.DOKill();
        if (showUIFeedback != null) showUIFeedback.StopFeedbacks();
        if (hideUIFeedback != null) hideUIFeedback.StopFeedbacks();

        this.isActive = isActive;

        if (isActive)
        {
            root.SetActive(true);

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            if (showUIFeedback != null)
            {
                showUIFeedback.Events.OnComplete.RemoveListener(OnShowComplete);
                showUIFeedback.Events.OnComplete.AddListener(OnShowComplete);
                showUIFeedback.PlayFeedbacks();
            }
            else
            {
                canvasGroup.DOFade(1, fadeDuration)
                    .SetEase(Ease.OutQuad)
                    .SetUpdate(true)
                    .OnComplete(OnShowComplete);
            }
        }
        else
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            if (hideUIFeedback != null)
            {
                hideUIFeedback.Events.OnComplete.RemoveListener(OnHideComplete);
                hideUIFeedback.Events.OnComplete.AddListener(OnHideComplete);
                hideUIFeedback.PlayFeedbacks();
            }
            else
            {
                canvasGroup.DOFade(0, fadeDuration)
                    .SetEase(Ease.InQuad)
                    .SetUpdate(true)
                    .OnComplete(OnHideComplete);
            }
        }
    }

    private void OnShowComplete()
    {
        if (canvasGroup != null)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    private void OnHideComplete()
    {
        if (root != null)
        {
            root.SetActive(false);
        }
    }
}

