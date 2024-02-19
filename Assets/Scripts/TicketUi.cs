using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketUi : MonoBehaviour
{

    private Ticket _connectedTicket;
    [SerializeField]
    private Transform[] _lines;
    private bool _fulfilled;

    [SerializeField]
    private float _startHeight;
    private float _baseHeight;
    [SerializeField]
    TweenSettings _enterTween;
    [SerializeField]
    TweenSettings _exitTween;
    [SerializeField]
    TweenSettings _shakeTween;


    [SerializeField]
    SpriteRenderer _fillAmount;
    [SerializeField]
    SpriteRenderer _whiteFlash;

    private bool _transitioning = false;
    private void Start()
    {
        _baseHeight = transform.localPosition.y;
        transform.localPosition = new Vector3(transform.localPosition.x, _startHeight, transform.localPosition.z);
        _fulfilled = true;
        _whiteFlash.enabled = false;
        InvokeRepeating("ShakeWhenLow", _shakeTween.duration, _shakeTween.duration);
    }
    public void Init(Ticket ticket)
    {
        _fulfilled = false;
        _whiteFlash.enabled = false;

        for (int i = 0; i < _lines.Length; i++)
        {
            _lines[i].gameObject.SetActive(false);
        }
        _connectedTicket = ticket;
        for(int i = 0; i < _connectedTicket.Cuts.Count; i++)
        {
            _lines[_connectedTicket.Cuts[i]].gameObject.SetActive(true);
        }
        transform.DOLocalMoveY(_baseHeight, _enterTween.duration).SetEase(_enterTween.ease);
    }

    public Ticket GetTicket()
    {
        return _connectedTicket;
    }

    public void FulFillTicket()
    {
        _whiteFlash.enabled = true;
        _connectedTicket = null;

        transform.DOComplete();
        _fulfilled = true;
        _transitioning = true;
        transform.DOLocalMoveY(_startHeight, _exitTween.duration).SetEase(_exitTween.ease).onComplete += () => { _transitioning = false; };
        transform.DOShakeScale(_exitTween.duration, _shakeTween.strength * 4, 10, 89, false);
    }
    public bool IsFulfilled()
    {
        if(_transitioning) return false;
        return _fulfilled;
    }

    public void Update()
    {
        if (!_fulfilled)
        {
            if(_connectedTicket != null)
            {
                _fillAmount.material.SetInt("_Arc1", Mathf.RoundToInt(360 * (1 -_connectedTicket.GetTimePercent())));
            }
        }
    }
    private void ShakeWhenLow()
    {
        if(_connectedTicket != null && !_fulfilled)
        {
            if(_connectedTicket.GetTimePercent() < 0.3f)
            {
                transform.DOShakePosition(_shakeTween.duration, _shakeTween.strength, 10, 89, false);
                transform.DOShakeScale(_shakeTween.duration, _shakeTween.strength, 10, 89, false);

            }
        }
    }
    public bool IsOutOfTime()
    {
        if(_connectedTicket == null) return false;
        return (_connectedTicket.GetTimePercent() <= 0f);
    }

}
