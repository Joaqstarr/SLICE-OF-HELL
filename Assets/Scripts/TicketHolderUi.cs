using System.Collections;
using System.Collections.Generic;
using Unity.XR.Oculus.Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class TicketHolderUi : MonoBehaviour
{

    [SerializeField]
    TicketUi _ticketPrefab;
    [SerializeField]
    float _ticketSpace = 2f;
    [SerializeField]
    TicketUi[] _ticketUis;
    [SerializeField]
    Pizza _playerPizza;
    [SerializeField]
    PlayerInput _associatedPlayer;
    private int _playerIndex;
    [SerializeField]
    HealthSystem healthSystem;
    [SerializeField]
    AudioSource _ticketSpawned;
    [SerializeField]
    AudioSource _ticketFailed;
    [SerializeField]
    AudioSource _ticketSuccess;
    private void Start()
    {
        _playerIndex = _associatedPlayer.playerIndex;
        GameManager.Instance.TicketSpawned += SpawnTicket;
        _playerPizza.SubmitPizza += ValidatePizza;

    }

    private void OnDisable()
    {
        GameManager.Instance.TicketSpawned -= SpawnTicket;
        _playerPizza.SubmitPizza -= ValidatePizza;
    }

    private void SpawnTicket(Ticket ticket)
    {
        TicketUi ticketUi = GetFirstReadyTicket();
        if (ticketUi != null)
        {
            _ticketSpawned.Play();
            ticketUi.Init(ticket);
        }

        
    }

    public void ValidatePizza(CutInfo[] Pizza)
    {
        bool isValid = false;

        for(int i = 0; i < _ticketUis.Length; i++)
        {
            if (_ticketUis[i].GetTicket() != null)
            if (_ticketUis[i].GetTicket().ValidatePizza(Pizza))
            {
                isValid = true;
                    _ticketUis[i].GetTicket().FulfillTicket();
                    GameManager.Instance.RemoveTicketFromPlayer(_playerIndex, _ticketUis[i].GetTicket());

                    _ticketUis[i].FulFillTicket();
                break;
            }
        }

        if (isValid)
        {
            Debug.Log("Valid");
            healthSystem.TicketServed();
            _ticketSuccess.Play();
            //yippee
        }
        else
        {
            Debug.Log("Invalid");

            //oh no :((
        }

    }

    private TicketUi GetFirstReadyTicket()
    {
        for(int i = 0; i < _ticketUis.Length ; i++)
        {
            if (_ticketUis[i].IsFulfilled())
            {
                return _ticketUis[i];
            }
        }
        return null;
    }

    private void Update()
    {
        if (!GameManager.Instance.GameStarted)
        {
            for (int i = 0; i < _ticketUis.Length; i++)
            {
                if (!_ticketUis[i].IsFulfilled())
                {
                    _ticketUis[i].FulFillTicket();
                }
            }
            return;
        }
        for (int i = 0; i < _ticketUis.Length; i++)
        {
            if (!_ticketUis[i].IsFulfilled() && _ticketUis[i].IsOutOfTime())
            {
                _ticketUis[i].FulFillTicket();
                healthSystem.TicketFailed();
                _ticketFailed.Play();
            }
        }
    }

    
}

