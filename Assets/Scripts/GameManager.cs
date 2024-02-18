using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public bool GameStarted = false;

    float _ticketTimer;
    [SerializeField]
    private Vector2 _ticketMaxMinSpawnTimer;
    [SerializeField]
    private Vector2 _ticketMaxMinTimer;
    [SerializeField]
    private Vector2 _ticketMinMaxCuts;


    [SerializeField]
    private float _maxSpeedTransition = 30f;
    private float _speedTransition = 0;

    private List<Ticket> _tickets;

    public delegate void TicketDelegate(Ticket ticket);
    public TicketDelegate TicketSpawned;
    
    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
            Instance = this;

    }
    private void Start()
    {
        StartGame();

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameStarted) return;
        UpdateTimers();
    }

    public void StartGame()
    {
        _tickets = new List<Ticket>();
        GameStarted = true;
        SpawnTicket();
    }

    private void UpdateTimers()
    {
        _speedTransition += Time.deltaTime;
        _speedTransition = Mathf.Min(_speedTransition, _maxSpeedTransition);
        _ticketTimer -= Time.deltaTime;
        if(_ticketTimer < 0)
        {
            SpawnTicket();
        }
    }

    private void SpawnTicket()
    {
        _ticketTimer = Mathf.Lerp(_ticketMaxMinSpawnTimer.x, _ticketMaxMinSpawnTimer.y, GetSpeedTransition());
        Debug.Log("Spawn Ticket");

        float ticketLength = Mathf.Lerp(_ticketMaxMinTimer.x, _ticketMaxMinTimer.y, GetSpeedTransition());
        int cuts = Mathf.RoundToInt(Mathf.Lerp(_ticketMinMaxCuts.x, _ticketMinMaxCuts.y, GetSpeedTransition()));
        Ticket newTicket = new Ticket(ticketLength, cuts);

        _tickets.Add(newTicket);

        if(TicketSpawned != null)
            TicketSpawned(newTicket);

    }
    private float GetSpeedTransition()
    {
        return _speedTransition/_maxSpeedTransition;
    }
}
