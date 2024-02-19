using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] int _startingBPM = 86;
    private int _bpm = 86;
    float time = 0;

    [SerializeField]
    AudioSource _lowIntensitySource;
    [SerializeField]
    AudioSource _mediumIntensitySource;
    [SerializeField]
    float _medPitchRate = 0.01f;
    [SerializeField]
    float _medPitchMax = 1.5f;

    [SerializeField]
    AudioSource _mainMenuSource;
    [SerializeField]
    AudioSource _readyUpMusic;
    bool _measure = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _lowIntensitySource.pitch = (1f / _startingBPM) * _bpm;
        _mediumIntensitySource.pitch+=  Time.deltaTime * _medPitchRate ;
        _mediumIntensitySource.pitch = Mathf.Min(_medPitchMax, _mediumIntensitySource.pitch);
        _bpm = _startingBPM + Mathf.RoundToInt( GameManager.Instance.GetSpeedTransitionClamped() * 10);
        time += Time.deltaTime;

        float measureTime = (60f/_bpm ) * 4;
        if(time%measureTime < 0.1f)
        {
            if (!_measure)
            {
                _measure = true;
                OnMeasure();
            }
        }
        else
        {
            _measure = false;
        }
        
    }

    public void ReadyUpMusic()
    {
        _readyUpMusic.Play();
        _mainMenuSource.Stop();
    }
    public void StartMusic()
    {
        _readyUpMusic.Stop();
        _mainMenuSource.Stop();
        time = (60f / _bpm);
        _bpm = _startingBPM;
        _lowIntensitySource.Play();
    }

    public void StopGame()
    {
        _readyUpMusic.Stop();
        _mainMenuSource.Play();
        _lowIntensitySource.Stop();
        _mediumIntensitySource.Stop();

    }
    private void OnMeasure()
    {

        if(GameManager.Instance.GetSpeedTransitionClamped() == 1)
        {
            if (_lowIntensitySource.isPlaying)
            {
                _mediumIntensitySource.pitch = 1;
                _mediumIntensitySource.Play();
                _lowIntensitySource.Stop();
            }
        }
    }
}
