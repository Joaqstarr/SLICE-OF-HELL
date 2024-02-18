using System.Collections;
using System.Collections.Generic;
using Unity.XR.Oculus.Input;
using UnityEngine;

public class TicketHolderUi : MonoBehaviour
{

    [SerializeField]
    TicketUi _ticketPrefab;
    [SerializeField]
    float _ticketSpace = 2f;
    List<TicketUi> _ticketUis = new List<TicketUi>();
    [SerializeField]
    Pizza _playerPizza;

    private void Start()
    {
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
        TicketUi ticketUi = Instantiate(_ticketPrefab, transform);
        ticketUi.Init(ticket);
        ticketUi.transform.localPosition = new Vector2( _ticketUis.Count * _ticketSpace,ticketUi.transform.localPosition.y);
        _ticketUis.Add(ticketUi);
        
    }

    public void ValidatePizza(CutInfo[] Pizza)
    {
        bool isValid = false;

        for(int i = 0; i < _ticketUis.Count; i++)
        {
            if (_ticketUis[i].GetTicket().ValidatePizza(Pizza))
            {
                isValid = true;
                break;
            }
        }

        if (isValid)
        {
            Debug.Log("Valid");
            //yippee
        }
        else
        {
            Debug.Log("Invalid");

            //oh no :((
        }

    }
}
