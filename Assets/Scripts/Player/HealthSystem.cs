using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealthSystem : MonoBehaviour
{

    [SerializeField]
    int _maxHealth;
    [SerializeField]
    int _health;

    [SerializeField]
    int _damage;
    [SerializeField]
    int _heal;

    [SerializeField]
    Transform _meter;

    [SerializeField]
    PlayerInput _associatedPlayer;

    [SerializeField]

    TweenSettings _moveSettings;
    public void PlayerSpawned()
    {
        _health = _maxHealth / 2;
        UpdateBar();
    }
    public void TicketServed()
    {
        _health += _heal;
        _health = Mathf.Min( _health, _maxHealth );
        UpdateBar();
    }

    public void TicketFailed()
    {
        _health -= _damage;

        UpdateBar();
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (_health <= 0)
        {
            GameManager.Instance.EndGame(_associatedPlayer.playerIndex + 1);
        }
    }
    private void UpdateBar()
    {
        _meter.DOScaleX(GetHealthPercent(), _moveSettings.duration).SetEase(_moveSettings.ease);
    }

    private float GetHealthPercent()
    {
        float hp = _health;
        return hp / _maxHealth;
    }
}
