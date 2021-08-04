using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class soGameplay : MonoBehaviour
{
    public Animator MicroAnimator;
    enum RecordingState { NotRecording, KickRecording, Recording, Saving};

    Animator _timelineAnimator = null;
    Recorder.Recorder _recorder = null;
    private RecordingState _recordingState = RecordingState.NotRecording;

    float _fRecordingTime = 0.0f;
    bool _recodingMaintained = false;
    public Button recordButton;
    void Awake()
    {
        _timelineAnimator = GetComponent<Animator>();
        _recorder = GetComponent<Recorder.Recorder>();
    }

    // Update is called once per frame
    void Update()
    {
        bool microVent = _timelineAnimator.GetCurrentAnimatorStateInfo(0).IsName("03_MicroVent");
        bool chuchoter = _timelineAnimator.GetCurrentAnimatorStateInfo(0).IsName("11_Chuchoter");
        string filePrefix = "";
        if (microVent)
            filePrefix = "_REC-1_";
        if (chuchoter)
            filePrefix = "_REC-2_";
        if (MicroAnimator.GetCurrentAnimatorStateInfo(0).IsName("MicIdleLoop"))
        {
            _recordingState = RecordingState.NotRecording;
        }

        if(_recordingState == RecordingState.NotRecording)
        {
            if (microVent || chuchoter)
            {
                _recordingState = RecordingState.KickRecording;
            }
        }
        if (_recordingState == RecordingState.KickRecording)
        {
            if (MicroAnimator.GetCurrentAnimatorStateInfo(0).IsName("MicIdleAnimVu"))
            {
                _fRecordingTime = 0.0f;
                StartCoroutine(__kickRecording());
                _recordingState = RecordingState.Recording;
            }
        }
        if (_recordingState == RecordingState.Recording)
        {
            if (MicroAnimator.GetCurrentAnimatorStateInfo(0).IsName("MicOut"))
            {
                _recorder.StopRecording(filePrefix);
                _recordingState = RecordingState.NotRecording;
                if (microVent)
                    _timelineAnimator.SetTrigger("Recorded1");
                if (chuchoter)
                    _timelineAnimator.SetTrigger("Recorded2");
            }
        }
        _recodingMaintained = false;
    }

    public void maintainRecording()
    {
        MicroAnimator.SetTrigger("Record");
        _recodingMaintained = true;
    }

    IEnumerator __kickRecording()
    {
        _recorder.StartRecording();
        for(; ; )
        {
            _fRecordingTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
