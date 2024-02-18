using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketUi : MonoBehaviour
{

    private Ticket _connectedTicket;
    [SerializeField]
    private Transform[] _lines;
    public void Init(Ticket ticket)
    {
        _connectedTicket = ticket;
        for(int i = 0; i < _connectedTicket.Cuts.Count; i++)
        {
            _lines[_connectedTicket.Cuts[i]].gameObject.SetActive(true);
        }
        
    }

    public Ticket GetTicket()
    {
        return _connectedTicket;
    }
}
