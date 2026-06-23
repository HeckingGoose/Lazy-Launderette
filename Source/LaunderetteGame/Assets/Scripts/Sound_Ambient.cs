using UnityEngine;

public class Sound_Ambient : MonoBehaviour
{
    // Const
    private const float CROSSFADE_TIME = 2f;

    // Enum
    private enum CrossfadeState
    {
        NotFading,
        FirstFrame,
        Working,
        Done
    }
    private enum Fade
    {
        In,
        Out,
        None
    }

    // Editor Variables
    [SerializeField]
    private bool _playOnStart = true;
    [Header("Playbacks")]
    [SerializeField]
    private AudioSource _playbackOne;
    [SerializeField]
    private AudioSource _playbackTwo;
    [Header("Ambient Sound")]
    [SerializeField]
    private AudioClip _ambient;

    // Private Variables
    private AudioSource[] _playbacks;
    private Fade _fading = Fade.None;
    private float _fadeTime = CROSSFADE_TIME;
    private float _fadeTimer;
    private float _peakVolume;
    private int _currentDevice;
    private CrossfadeState _currentCrossfadeState = CrossfadeState.NotFading;
    private float _fadeMin = 0;
    private const float VOL = 1.0f;

    // Unity Methods
    private void Start()
    {
        // Setup array
        _playbacks = new AudioSource[2]
        {
            _playbackOne,
            _playbackTwo
        };

        // Set Volume
        _playbacks[0].volume = 0;
        _playbacks[1].volume = 0;

        // Set initial clip
        _playbacks[0].clip = _ambient;
        _playbacks[1].clip = _ambient;

        // Play one and stop two
        _playbacks[0].Play();
        _playbacks[1].Stop();

        // Set first clip as currently playing
        _currentDevice = 0;

        // Do an infade, if we start with the scene
        if (_playOnStart)
        {
            FadeIn(0.5f);
        }
    }

    private void Update()
    {
        // Are we fading?
        if (_fading == Fade.In || _fading == Fade.Out)
        {
            // Are we fading out?
            if (_fading == Fade.Out)
            {
                // Lerp volume by peak and fade time
                _playbacks[_currentDevice].volume = Mathf.Lerp(_peakVolume, _fadeMin, _fadeTimer);
            }

            // Are we fading in?
            else if (_fading == Fade.In)
            {
                // Lerp by volume and fade time
                _playbacks[_currentDevice].volume = Mathf.Lerp(_peakVolume, VOL, _fadeTimer);
            }

            // Are we outta time?
            if (_fadeTimer >= _fadeTime)
            {
                // What were we doing before?
                if (_fading == Fade.Out)
                {
                    // Then set to 0
                    _playbacks[_currentDevice].volume = _fadeMin;
                }
                else if (_fading == Fade.In)
                {
                    // Then set to target
                    _playbacks[_currentDevice].volume = VOL;
                }
                _fading = Fade.None;
            }

            // Increment fade time
            _fadeTimer = Mathf.Clamp(_fadeTimer + Time.deltaTime, 0, _fadeTime);
        }

        // Otherwise, run crossfade logic
        else
        {
            // What crossfade state are we in?
            switch (_currentCrossfadeState)
            {
                // We are waiting to crossfade
                case CrossfadeState.NotFading:
                    // Are we within crossfade time?
                    if (_ambient.length - _playbacks[_currentDevice].time < CROSSFADE_TIME)
                    {
                        // Switch into crossfading
                        _currentCrossfadeState = CrossfadeState.FirstFrame;
                    }
                    break;

                // This is the first frame
                case CrossfadeState.FirstFrame:
                    // Set clip for sub device
                    _playbacks[1 - _currentDevice].clip = _ambient;
                    _playbacks[1 - _currentDevice].time = 0;
                    _playbacks[1 - _currentDevice].volume = 0;

                    // Start playing
                    _playbacks[1 - _currentDevice].Play();

                    // Increment state
                    _currentCrossfadeState = CrossfadeState.Working;

                    break;

                // This is it continuing, so crossfade
                case CrossfadeState.Working:
                    // Crossfade volumes
                    CrossfadeVolume(
                        ref _playbacks[_currentDevice],
                        ref _playbacks[1 - _currentDevice],
                        (_ambient.length - _playbacks[_currentDevice].time) / CROSSFADE_TIME
                        );

                    // Is the main playback done?
                    if (!_playbacks[_currentDevice].isPlaying)
                    {
                        // Then move into next state
                        _currentCrossfadeState = CrossfadeState.Done;
                    }
                    break;

                // We're done
                case CrossfadeState.Done:
                    // Flip devices
                    _currentDevice = 1 - _currentDevice;

                    // Force volumes just in case
                    _playbacks[_currentDevice].volume = VOL;
                    _playbacks[1 - _currentDevice].volume = 0;

                    // Reset state
                    _currentCrossfadeState = CrossfadeState.NotFading;
                    break;
            }
        }
    }

    // Public Methods
    /// <summary>
    /// Begins fading the audio out over a given time.
    /// </summary>
    /// <param name="fadeTime">The time to spend fading out.</param>
    public void FadeOut(float fadeTime, float fadeMin = 0f)
    {
        // Set fading out to be true
        _fading = Fade.Out;

        // Pass in values
        _fadeTime = fadeTime;
        _fadeMin = fadeMin;

        // Take a snapshot of current volume of main playback device
        _peakVolume = _playbacks[_currentDevice].volume;

        // Mute other device just in case
        _playbacks[1 - _currentDevice].volume = 0;

        // Reset fade timer
        _fadeTimer = 0;
    }
    /// <summary>
    /// Begins fading the audio in over a given time.
    /// </summary>
    /// <param name="fadeTime">The time to spend fading in.</param>
    public void FadeIn(float fadeTime)
    {
        // Set fading out to be true
        _fading = Fade.In;

        // Pass in value
        _fadeTime = fadeTime;

        // Take a snapshot of current volume of main playback device
        _peakVolume = _playbacks[_currentDevice].volume;

        // Mute other device just in case
        _playbacks[1 - _currentDevice].volume = 0;

        // Reset fade timer
        _fadeTimer = 0;
    }

    // Private Methods
    private void CrossfadeVolume(ref AudioSource main, ref AudioSource sub, float progress)
    {
        // Set volume of main to percentage of target based on progress
        main.volume = Mathf.Lerp(0, VOL, progress);
        sub.volume = Mathf.Lerp(VOL, 0, progress);
    }
}
