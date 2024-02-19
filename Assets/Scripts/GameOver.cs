using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    CanvasGroup _canvas;

    [SerializeField] Image p1;
    [SerializeField] Image p2;
    [SerializeField] Image single;
    [SerializeField] Image multipe;

    [SerializeField] float _time = 5f;
    [SerializeField]
    TweenSettings _tweenSettings;

    AudioSource _winTheme;
    private void Start()
    {
        _winTheme = GetComponent<AudioSource>();
        _canvas = GetComponent<CanvasGroup>();
        GameManager.Instance.FirstPlayerWin += FirstPlayer;
        GameManager.Instance.SecondPlayerWin += SecondPlayer;
        GameManager.Instance.SinglePlayerWin += SinglePlayer;
    }
    

    private void OnDisable()
    {
        GameManager.Instance.FirstPlayerWin -= FirstPlayer;
        GameManager.Instance.SecondPlayerWin -= SecondPlayer;
        GameManager.Instance.SinglePlayerWin -= SinglePlayer;
    }

    private void SinglePlayer()
    {
        _canvas.alpha = 1;

        multipe.enabled = false;
        single.enabled = true;
        p1.enabled = false;
        p2.enabled = false;
        StartCoroutine(Disapear());

    }

    private void FirstPlayer()
    {
        _canvas.alpha = 1;

        multipe.enabled = true;
        single.enabled = false;
        p1.enabled = true;
        p2.enabled = false;
        StartCoroutine(Disapear());

    }

    private void SecondPlayer()
    {
        _canvas.alpha = 1;
        multipe.enabled = true;
        single.enabled = false;
        p1.enabled = false;
        p2.enabled = true;

        StartCoroutine(Disapear());
    }

    IEnumerator Disapear()
    {
        _winTheme.Play();
        transform.DOShakePosition(_tweenSettings.duration, _tweenSettings.strength, 10, 90, false, true);
        yield return new WaitForSeconds(_time);
        _canvas.alpha = 0;
    }
}
