using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public List<Ticket>[] _tickets;

    public UnityEvent OnStartGame;
    public UnityEvent OnReadyGame;


    public delegate void TicketDelegate(Ticket ticket);
    public TicketDelegate TicketSpawned;

    public delegate void GameOverDel();
    public GameOverDel SinglePlayerWin;
    public GameOverDel FirstPlayerWin;

    public GameOverDel SecondPlayerWin;


    [SerializeField]
    Transform _camera;
    [SerializeField]
    float _startY;
    [SerializeField] TweenSettings _camTween;
    bool _readyUp = false;
    private bool _readySpawned = false;

    [SerializeField]
    GameObject[] _p2Objs;
    [SerializeField]
    PlayerManager[] _players;
    [SerializeField] Pizza[] pizzas;
    public UnityEvent OnEndGame;
    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
            Instance = this;

    }
    private void Start()
    {
        Vector3 pos = _camera.transform.position;
        pos.y = _startY;
        _camera.transform.position = pos; 
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_readyUp)
        {
            bool bothReadied = true;
            for (int i = 0; i < _tickets.Length; i++)
            {
                Debug.Log(i + ", " + _tickets[i].Count + ", " + _readySpawned);

                if (_tickets[i].Count != 0)
                {
                    _readySpawned = true;
                    bothReadied = false;
                }
            }


            if (bothReadied && _readySpawned)
                ReadyToStart();

            return;
        }


        if (!GameStarted) return;



            UpdateTimers();
            SpawnIfEmpty();
        


    }

    public void StartGame1P()
    {
        _tickets = new List<Ticket>[1];
        foreach (GameObject obj in _p2Objs)
        {
            obj.SetActive(false);
        }
        _players[0].SwitchState(_players[0]._playerGameState);
        StartGame();

    }
    public void StartGame2P()
    {
        _tickets = new List<Ticket>[2];
        foreach(GameObject obj in _p2Objs)
        {
            obj.SetActive(true);
        }
        _players[0].SwitchState(_players[0]._playerGameState);
        _players[1].SwitchState(_players[1]._playerGameState);

        StartGame();
    }

    public void StartGame()
    {
        Debug.Log(_tickets.Length);
        for (int i = 0; i < _tickets.Length; i++)
        {
            _tickets[i] = new List<Ticket>();
        }
        _readyUp = true;

        GameStarted = true;
        _speedTransition = 0;
        _readySpawned = false;
        _camera.DOMoveY(0, _camTween.duration).SetEase(_camTween.ease).onComplete += ()=> {

            Ticket readyTicket = new Ticket(10, 2, true);
            TicketSpawned(readyTicket);
            for (int i = 0; i < _tickets.Length; i++)
            {
                _tickets[i].Add(readyTicket);
                pizzas[i].StartPizza();
            }
            if (OnReadyGame != null)
                OnReadyGame.Invoke();
        }; 

    }
    private void ReadyToStart()
    {

        _readyUp = false;

        _ticketTimer = 0.2f;

        if (OnStartGame != null)
            OnStartGame.Invoke();
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

        for(int i = 0; i < _tickets.Length; i++)
        {
            for(int j = 0; j < _tickets[i].Count; j++)
            {
                _tickets[i][j].UpdateTime();
            }
        }
    }
    private void SpawnIfEmpty()
    {
        for(int i = 0; i < _tickets.Length; i++)
        {
            if (_tickets[i].Count == 0)
            {
                SpawnTicket();
            }
        }
    }
    private void SpawnTicket()
    {
        _ticketTimer = Mathf.Lerp(_ticketMaxMinSpawnTimer.x, _ticketMaxMinSpawnTimer.y, GetSpeedTransition());

        float ticketLength = Mathf.Lerp(_ticketMaxMinTimer.x, _ticketMaxMinTimer.y, GetSpeedTransition());
        int cuts = Mathf.RoundToInt(Mathf.Lerp(_ticketMinMaxCuts.x, _ticketMinMaxCuts.y, GetSpeedTransition()));
        Ticket newTicket = new Ticket(ticketLength, cuts);

        for (int i = 0; i < _tickets.Length; i++)
        {
            _tickets[i].Add(newTicket);
        }
        if (TicketSpawned != null)
            TicketSpawned(newTicket);

    }
    public float GetSpeedTransition()
    {
        return _speedTransition/_maxSpeedTransition;
    }
    public float GetSpeedTransitionClamped()
    {
        return Mathf.Clamp01( _speedTransition / _maxSpeedTransition);
    }

    public void RemoveTicketFromPlayer(int playerIndex, Ticket ticket)
    {
        Debug.Log(playerIndex + "(index)");
        if(_tickets.Length > playerIndex)
        {
            _tickets[playerIndex].Remove(ticket);
            Debug.Log("ticket removed");
        }
    }

    public void EndGame(int playerIndex)
    {
        OnEndGame.Invoke();
        Debug.Log("Player " + playerIndex + " wins.");
        _players[0].SwitchState(_players[0]._playerTitleState);
        _players[1].SwitchState(_players[1]._playerTitleState);
        GameStarted = false;
        if(_tickets.Length == 0)
        {
            if(SinglePlayerWin != null)
                SinglePlayerWin();
        }
        else
        {
            if(playerIndex == 1)
            {
                if(SecondPlayerWin != null)
                    SecondPlayerWin();
            }
            else
            {
                if(FirstPlayerWin != null)
                    FirstPlayerWin();
            }
        }
        _camera.DOMoveY(_startY, _camTween.duration).SetEase(_camTween.ease);

    }
}
