using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UMP.DefinedArguments;
using UMP.Interfaces;
using UMP.Events;
using UMP.Helpers;
using UMP.Loggers;
using UMP;

public class UniversalMediaPlayer : MonoBehaviour, IMediaListener, IPlayerTextureCreatedListener, IPlayerTimeChangedListener, IPlayerPositionChangedListener, IPlayerSnapshotTakenListener
{
    #region Editor Visible Properties
    [SerializeField]
    private GameObject[] _renderingObjects;

    [SerializeField]
    private string _path;

    [SerializeField]
    private bool _autoPlay;

    [SerializeField]
    private bool _loop;

    [SerializeField]
    private bool _mute;

    [SerializeField]
    private int _volume;

    [SerializeField]
    private float _playRate;

    [SerializeField]
    private float _position;

    [SerializeField]
    private int _fileCaching = 300;

    [SerializeField]
    private int _liveCaching = 300;

    [SerializeField]
    private int _diskCaching = 300;

    [SerializeField]
    private int _networkCaching = 300;

    [SerializeField]
    private LogDetail _consoleLogDetail = LogDetail.Disable;

#pragma warning disable 0414
    [SerializeField]
    private bool _showAdvancedProperties = false;

    [SerializeField]
    private string _lastEventMsg = string.Empty;
#pragma warning restore 0414

    [SerializeField]
    private bool _isParsing;

    [SerializeField]
    private UnityEvent _openingEvent;

    [Serializable]
    private class BufferingType : UnityEvent<float> { }

    [SerializeField]
    private BufferingType _bufferingEvent;

    [Serializable]
    private class TextureCreatedType : UnityEvent<Texture2D> { }

    [SerializeField]
    private TextureCreatedType _textureCreatedEvent;

    [SerializeField]
    private UnityEvent _playingEvent;

    [SerializeField]
    private UnityEvent _pausedEvent;

    [SerializeField]
    private UnityEvent _stoppedEvent;

    [SerializeField]
    private UnityEvent _endReachedEvent;

    [SerializeField]
    private UnityEvent _encounteredErrorEvent;

    [Serializable]
    private class TimeChangedType : UnityEvent<long> { }

    [SerializeField]
    private TimeChangedType _timeChangedEvent;

    [Serializable]
    private class PositionChangedType : UnityEvent<float> { }

    [SerializeField]
    private PositionChangedType _positionChangedEvent;

    [Serializable]
    private class SnapshotTakenType : UnityEvent<string> { }

    [SerializeField]
    private SnapshotTakenType _snapshotTakenEvent;
    #endregion

    #region Properties
    public GameObject[] RenderingObjects
    {
        get
        {
            if (_mediaPlayer != null)
                return _mediaPlayer.VideoOutputObjects;
            return null;
        }
        set
        {
            if (_mediaPlayer != null)
                _mediaPlayer.VideoOutputObjects = value;
        }
    }

    public string Path
    {
        set { _path = value; }
        get { return _path; }
    }

    public bool AutoPlay
    {
        set { _autoPlay = value; }
        get { return _autoPlay; }
    }

    public bool Loop
    {
        set { _loop = value; }
        get { return _loop; }
    }

    public bool Mute
    {
        set { _mute = value; }
        get { return _mute; }
    }

    public float Volume
    {
        set { _volume = (int)value; }
        get { return _volume; }
    }

    public float PlayRate
    {
        set { _playRate = value; }
        get { return _playRate; }
    }

    public bool AbleToPlay
    {
        get
        {
            if (_mediaPlayer != null)
                return _mediaPlayer.AbleToPlay;
            return false;
        }
    }

    public int FrameCounter
    {
        get
        {
            if (_mediaPlayer != null)
                return _mediaPlayer.FrameCounter;
            return 0;
        }
    }

    public float Fps
    {
        get
        {
            if (_mediaPlayer != null)
                return _mediaPlayer.Fps;
            return 0;
        }
    }

    public bool VideoTextureExist
    {
        get
        {
            if (_mediaPlayer != null)
                return _mediaPlayer.VideoTextureExist;
            return false;
        }
    }

    public Vector2 VideoSize
    {
        get
        {
            if (_mediaPlayer != null)
                return _mediaPlayer.GetVideoSize();
            return new Vector2(0, 0);
        }
    }

    public int VideoWidth
    {
        get
        {
            if (_mediaPlayer != null)
                return _mediaPlayer.GetVideoWidth();
            return 0;
        }
    }

    public int VideoHeight
    {
        get
        {
            if (_mediaPlayer != null)
                return _mediaPlayer.GetVideoHeight();
            return 0;
        }
    }

    public string LastError
    {
        get
        {
            if (_mediaPlayer != null && !MediaPlayerHelper.IsMobilePlatform())
                return (_mediaPlayer as StandaloneMediaPlayer).GetLastError();
            return string.Empty;
        }
    }

    public long Length
    {
        get
        {
            if (_mediaPlayer != null)
                return _mediaPlayer.GetLength();
            return 0;
        }
    }

    public float Position
    {
        set { _position = value; }
        get { return _position; }
    }

    public long Time
    {
        set
        {
            if (_mediaPlayer != null)
                _mediaPlayer.Time = value;
        }
        get
        {
            if (_mediaPlayer != null)
                return _mediaPlayer.Time;
            return -1;
        }
    }

    public bool IsPlaying
    {
        get
        {
            if (_mediaPlayer != null)
                return _mediaPlayer.IsPlaying;
            return false;
        }
    }

    public bool IsReady
    {
        get {
            if (_mediaPlayer != null)
                return _mediaPlayer.IsReady;
            return false;
        }
    }

    public bool IsParsing
    {
        get
        {
            return _isParsing;
        }
    }
    #endregion

    private MediaPlayer _mediaPlayer;
    private VideoHostingsParser _videoHostingParser;
    private LogManager _logManager;

#pragma warning disable 0414
    private bool _isFirstEditorStateChange = true;
#pragma warning restore 0414
    private bool _savedPlayerPlayState;
    private string _savedPlayerPath = "";
    private float _savedPosition;

    private void Awake()
    {
#if UNITY_EDITOR
        EditorApplication.playmodeStateChanged += HandleOnPlayModeChanged;
#endif

        if (_renderingObjects != null)
        {
            // Create instance of MediaPlayer object to specific platform with defined arguments
            if (MediaPlayerHelper.IsMobilePlatform())
            {
                _mediaPlayer = new MobileMediaPlayer(this, _renderingObjects, new DefinedArgs(null)
                {
                    FileCaching = _fileCaching,
                    LiveCaching = _liveCaching,
                    DiskCaching = _diskCaching,
                    NetworkCaching = _networkCaching,
                    HardwareDecoding = DefinedArgs.ArgsState.Default
                });
            }
            else
            {
                _mediaPlayer = new StandaloneMediaPlayer(this, _renderingObjects, new DefinedArgs(null)
                {
                    FileCaching = _fileCaching,
                    LiveCaching = _liveCaching,
                    DiskCaching = _diskCaching,
                    NetworkCaching = _networkCaching,
                    HardwareDecoding = DefinedArgs.ArgsState.Default,
                    //AudioOutputDevice = DefinedArgs.GetAudioOutputDevice("Rift Audio")
                });

#if UNITY_EDITOR
                _logManager = (_mediaPlayer as StandaloneMediaPlayer).LogManager;
                if (_logManager != null)
                {
                    // Set delegate for LogManager to show native library logging in Unity console
                    _logManager.LogMessageListener += UnityConsoleLogging;
                    // Set debugging level
                    _logManager.LogDetail = _consoleLogDetail;
                }
#endif
            }

            // Audio output of the video will be not muted
            _mediaPlayer.Mute = _mute;
            // Set audio output volume
            _mediaPlayer.Volume = _volume;
            // Create scpecial parser to add possibiity of get video link from different video hosting servies (like youtube)
            _videoHostingParser = new VideoHostingsParser(this);
            // Attach scecial listeners to MediaPlayer instance
            AddListeners();
        }
    }

    #region Editor Additional Possibility
    private void HandleOnPlayModeChanged()
    {
#if UNITY_EDITOR
        if (_isFirstEditorStateChange)
        {
            _isFirstEditorStateChange = false;
            return;
        }

        if (_mediaPlayer == null)
            return;

        if (EditorApplication.isPaused)
        {
            _savedPlayerPlayState = _mediaPlayer.IsPlaying;
            _mediaPlayer.Pause();
        }
        else
        {
            if (!isActiveAndEnabled)
            {
                Stop();
                return;
            }

            if (_savedPlayerPlayState)
                _mediaPlayer.Play();
            else
                _mediaPlayer.Pause();
        }
#endif
    }
    #endregion

    private void Start()
    {
        if (!_autoPlay)
            return;

        Play();
    }

    private void Update()
    {
        if (_mediaPlayer != null && _mediaPlayer.IsPlaying)
        {
            _mediaPlayer.Mute = _mute;
            _mediaPlayer.Volume = _volume;
            _mediaPlayer.PlaybackRate = _playRate;

#if UNITY_EDITOR
            if (_logManager != null)
            {
                _logManager.LogDetail = _consoleLogDetail;
            }
#endif

            if (_savedPosition != _position)
            {
                _mediaPlayer.Position = _position;
                _savedPosition = _position;
            }
        }
    }

    public void OnDisable()
    {
        if (_mediaPlayer != null && _mediaPlayer.IsPlaying)
        {
            Stop();
        }
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        if (EditorApplication.playmodeStateChanged != null)
        {
            EditorApplication.playmodeStateChanged -= HandleOnPlayModeChanged;
            EditorApplication.playmodeStateChanged = null;
        }
#endif

        if (_mediaPlayer != null)
        {
            // Release MediaPlayer
            Release();
        }
    }

    private void AddListeners()
    {
        if (_mediaPlayer == null || _mediaPlayer.EventManager == null)
            return;

        // Add to MediaPlayer new main group of listeners
        _mediaPlayer.AddMediaListener(this);
        // Add to MediaPlayer new "OnPlayerTextureCreated" listener
        _mediaPlayer.EventManager.PlayerTextureCreatedListener += OnPlayerTextureCreated;
        // Add to MediaPlayer new "OnPlayerTimeChanged" listener
        _mediaPlayer.EventManager.PlayerTimeChangedListener += OnPlayerTimeChanged;
        // Add to MediaPlayer new "OnPlayerPositionChanged" listener
        _mediaPlayer.EventManager.PlayerPositionChangedListener += OnPlayerPositionChanged;
        // Add to MediaPlayer new "OnPlayerSnapshotTaken" listener
        _mediaPlayer.EventManager.PlayerSnapshotTakenListener += OnPlayerSnapshotTaken;
    }

    private void RemoveListeners()
    {
        if (_mediaPlayer == null)
            return;

        // Remove from MediaPlayer the main group of listeners
        _mediaPlayer.RemoveMediaListener(this);
        // Remove from MediaPlayer "OnPlayerTextureCreated" listener
        _mediaPlayer.EventManager.PlayerTextureCreatedListener -= OnPlayerTextureCreated;
        // Remove from MediaPlayer "OnPlayerTimeChanged" listener
        _mediaPlayer.EventManager.PlayerTimeChangedListener -= OnPlayerTimeChanged;
        // Remove from MediaPlayer "OnPlayerPositionChanged" listener
        _mediaPlayer.EventManager.PlayerPositionChangedListener -= OnPlayerPositionChanged;
        // Remove from MediaPlayer new "OnPlayerSnapshotTaken" listener
        _mediaPlayer.EventManager.PlayerSnapshotTakenListener -= OnPlayerSnapshotTaken;
    }

    public void Play()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPaused)
            return;
#endif
        
        if (_mediaPlayer == null || string.IsNullOrEmpty(_path))
            return;

        if (!_savedPlayerPath.Equals(_path))
        {
            if (_videoHostingParser.IsVideoHostingUrl(_path))
            {
                var videoInfos = _videoHostingParser.GetCachedVideoInfos(_path);
                if (videoInfos == null)
                {
#if UNITY_EDITOR
                    _lastEventMsg = "Parsing";
#endif
                    _isParsing = true;
                    _videoHostingParser.ParseVideoInfos(_path, (res) => {
                        _isParsing = false;
                        Play(); });
                    return;
                }
                else
                {
                    _mediaPlayer.SetDataSource(new Uri(_videoHostingParser.GetBestCompatibleVideo(videoInfos).DownloadUrl));
                    _savedPlayerPath = _path;
                }
            }
            else
            {
                _mediaPlayer.SetDataSource(new Uri(_path));
                _savedPlayerPath = _path;
            }
        }

#if UNITY_EDITOR
        _lastEventMsg = "Playing";
#endif

        if (!_mediaPlayer.IsPlaying)
            _mediaPlayer.Play();
    }

    public void Pause()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPaused)
            return;
#endif

        if (_mediaPlayer == null)
            return;

        if (_mediaPlayer.IsPlaying)
            _mediaPlayer.Pause();
    }

    public void Stop()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPaused)
            return;
#endif

        _position = 0;
        _savedPosition = 0;

        if (_mediaPlayer == null)
            return;

        _mediaPlayer.Stop();
    }

    public void Stop(bool clearVideoTexture)
    {
#if UNITY_EDITOR
        if (EditorApplication.isPaused)
            return;
#endif

        _position = 0;
        _savedPosition = 0;

        if (_mediaPlayer == null)
            return;

        _mediaPlayer.Stop(clearVideoTexture);
    }

    public void Release()
    {
#if UNITY_EDITOR
        if (EditorApplication.playmodeStateChanged != null)
        {
            EditorApplication.playmodeStateChanged -= HandleOnPlayModeChanged;
            EditorApplication.playmodeStateChanged = null;
        }
#endif
        Stop();

        if (_mediaPlayer != null)
        {
            // Release MediaPlayer
            _mediaPlayer.Release();
            _mediaPlayer = null;

            if (_videoHostingParser != null)
                _videoHostingParser.Release();

            RemoveListeners();

            _openingEvent.RemoveAllListeners();
            _bufferingEvent.RemoveAllListeners();
            _textureCreatedEvent.RemoveAllListeners();
            _playingEvent.RemoveAllListeners();
            _pausedEvent.RemoveAllListeners();
            _stoppedEvent.RemoveAllListeners();
            _endReachedEvent.RemoveAllListeners();
            _encounteredErrorEvent.RemoveAllListeners();
            _timeChangedEvent.RemoveAllListeners();
            _positionChangedEvent.RemoveAllListeners();
            _snapshotTakenEvent.RemoveAllListeners();
        }
    }

    public string GetFormattedLength(bool detail)
    {
        if (_mediaPlayer != null)
            return _mediaPlayer.GetFormattedLength(detail);
        return string.Empty;
    }

    public void Snapshot(string path)
    {
#if UNITY_EDITOR
        if (EditorApplication.isPaused)
            return;
#endif

        if (_mediaPlayer == null)
            return;

        if (_mediaPlayer.AbleToPlay)
        {
            if (_mediaPlayer is StandaloneMediaPlayer)
                (_mediaPlayer as StandaloneMediaPlayer).TakeSnapShot(path);
#if UNITY_EDITOR
            Debug.Log("Snapshot path: " + path);
#endif
        }
    }

    private void UnityConsoleLogging(LogMessage args)
    {
        if (args.Level != _consoleLogDetail)
            return;

        Debug.Log(args.Level.ToString() + ": " + args.Message);
    }

    public void OnPlayerOpening()
    {
#if UNITY_EDITOR
        _lastEventMsg = "Opening";
#endif
        if (_openingEvent != null)
            _openingEvent.Invoke();
    }

    public void AddOpeningEvent(UnityAction action)
    {
        _openingEvent.AddListener(action);
    }

    public void RemoveOpeningEvent(UnityAction action)
    {
        _openingEvent.RemoveListener(action);
    }

    public void OnPlayerBuffering(float percentage)
    {
#if UNITY_EDITOR
        _lastEventMsg = "Buffering: " + percentage;
#endif
        if (_bufferingEvent != null)
            _bufferingEvent.Invoke(percentage);
    }

    public void AddBufferingEvent(UnityAction<float> action)
    {
        _bufferingEvent.AddListener(action);
    }

    public void RemoveBufferingEvent(UnityAction<float> action)
    {
        _bufferingEvent.RemoveListener(action);
    }

    public void OnPlayerTextureCreated(Texture2D videoTexture)
    {
#if UNITY_EDITOR
        _lastEventMsg = "TextureCreated";
#endif
        if (_textureCreatedEvent != null)
            _textureCreatedEvent.Invoke(videoTexture);
    }

    public void AddTextureCreatedEvent(UnityAction<Texture2D> action)
    {
        _textureCreatedEvent.AddListener(action);
    }

    public void RemoveTextureCreatedEvent(UnityAction<Texture2D> action)
    {
        _textureCreatedEvent.RemoveListener(action);
    }

    public void OnPlayerPlaying()
    {
#if UNITY_EDITOR
        _lastEventMsg = "Playing";
#endif
        if (_playingEvent != null)
            _playingEvent.Invoke();
    }

    public void AddPlayingEvent(UnityAction action)
    {
        _playingEvent.AddListener(action);
    }

    public void RemovePlayingEvent(UnityAction action)
    {
        _playingEvent.RemoveListener(action);
    }

    public void OnPlayerPaused()
    {
#if UNITY_EDITOR
        _lastEventMsg = "Paused";
#endif
        if (_pausedEvent != null)
            _pausedEvent.Invoke();
    }

    public void AddPausedEvent(UnityAction action)
    {
        _pausedEvent.AddListener(action);
    }

    public void RemovePausedEvent(UnityAction action)
    {
        _pausedEvent.RemoveListener(action);
    }

    public void OnPlayerStopped()
    {
#if UNITY_EDITOR
        if (!_lastEventMsg.Contains("Error"))
            _lastEventMsg = "Stopped";
#endif
        if (_stoppedEvent != null)
            _stoppedEvent.Invoke();
    }

    public void AddStoppedEvent(UnityAction action)
    {
        _stoppedEvent.AddListener(action);
    }

    public void RemoveStoppedEvent(UnityAction action)
    {
        _stoppedEvent.RemoveListener(action);
    }

    public void OnPlayerEndReached()
    {
#if UNITY_EDITOR
        _lastEventMsg = "End";
#endif
        if (_endReachedEvent != null)
            _endReachedEvent.Invoke();

        _mediaPlayer.Stop(!_loop);
        _position = 0;

        if (_loop && !string.IsNullOrEmpty(_path))
            Play();
    }

    public void AddEndReachedEvent(UnityAction action)
    {
        _endReachedEvent.AddListener(action);
    }

    public void RemoveEndReachedEvent(UnityAction action)
    {
        _endReachedEvent.RemoveListener(action);
    }

    public void OnPlayerEncounteredError()
    {
#if UNITY_EDITOR
        _lastEventMsg = "Error (" + (_mediaPlayer as StandaloneMediaPlayer).GetLastError() + ")";
#endif
        Stop();

        if (_encounteredErrorEvent != null)
            _encounteredErrorEvent.Invoke();
    }

    public void AddEncounteredErrorEvent(UnityAction action)
    {
        _encounteredErrorEvent.AddListener(action);
    }

    public void RemoveEncounteredErrorEvent(UnityAction action)
    {
        _encounteredErrorEvent.RemoveListener(action);
    }

    public void OnPlayerTimeChanged(long time)
    {
#if UNITY_EDITOR
        _lastEventMsg = "TimeChanged";
#endif
        if (_timeChangedEvent != null)
            _timeChangedEvent.Invoke(time);
    }

    public void AddTimeChangedEvent(UnityAction<long> action)
    {
        _timeChangedEvent.AddListener(action);
    }

    public void RemoveTimeChangedEvent(UnityAction<long> action)
    {
        _timeChangedEvent.RemoveListener(action);
    }

    public void OnPlayerPositionChanged(float position)
    {
#if UNITY_EDITOR
        _lastEventMsg = "PositionChanged";
#endif
        _position = _mediaPlayer.Position;
        _savedPosition = _position;

        if (_positionChangedEvent != null)
            _positionChangedEvent.Invoke(position);
    }

    public void AddPositionChangedEvent(UnityAction<float> action)
    {
        _positionChangedEvent.AddListener(action);
    }

    public void RemovePositionChangedEvent(UnityAction<float> action)
    {
        _positionChangedEvent.RemoveListener(action);
    }

    public void OnPlayerSnapshotTaken(string path)
    {
        if (_snapshotTakenEvent != null)
            _snapshotTakenEvent.Invoke(path);
    }

    public void AddSnapshotTakenEvent(UnityAction<string> action)
    {
        _snapshotTakenEvent.AddListener(action);
    }

    public void RemoveSnapshotTakenEvent(UnityAction<string> action)
    {
        _snapshotTakenEvent.RemoveListener(action);
    }

    public void OnPlayerBuffering()
    {
       
    }
}
