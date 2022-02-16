using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class soGameplay : MonoBehaviour
{
    public Animator MicroAnimator;
    enum RecordingState { NotRecording, ReadyToRecord, KickRecording, Recording, Saving};

    Animator _timelineAnimator = null;
    Recorder.Recorder _recorder = null;
    private RecordingState _recordingState = RecordingState.NotRecording;

    float _fRecordingTime = 0.0f;
    public Button recordButton;
    bool _bIsRestarting = false;
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
        bool credits = _timelineAnimator.GetCurrentAnimatorStateInfo(0).IsName("14_Credit");
        bool fin = _timelineAnimator.GetCurrentAnimatorStateInfo(0).IsName("12_Fin");

        if (MicroAnimator.GetCurrentAnimatorStateInfo(0).IsName("MicFadeIn"))
        {
            _recordingState = RecordingState.NotRecording;
        }

        if(_recordingState == RecordingState.ReadyToRecord)
        {
            if (fin)
            {
                _recordingState = RecordingState.KickRecording;
                StartCoroutine(__kickRecording());
            }
        }

        if(credits && !_bIsRestarting)
        {
            _bIsRestarting = true;
            StartCoroutine(__restart());
        }
    }

    IEnumerator __restart()
    {

        do
        {
            yield return new WaitForSeconds(10.0f);
        }while (Input.touchCount != 0);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        yield return null;
    }

    public void btnStartRecording()
    {
        if (_recordingState == RecordingState.NotRecording)
        {
            {
                _recordingState = RecordingState.ReadyToRecord;
            }
        }
    }

    public void btnStopRecording()
    {
        if (_recordingState == RecordingState.Recording)
        {
            string filePrefix = "";
            bool microVent = _timelineAnimator.GetCurrentAnimatorStateInfo(0).IsName("03_MicroVent");
            bool chuchoter = _timelineAnimator.GetCurrentAnimatorStateInfo(0).IsName("11_Chuchoter");
            filePrefix = "_" + Localization.soLanguage + "_";

            _recorder.StopRecording(filePrefix);
            _recordingState = RecordingState.NotRecording;
            if (microVent)
                _timelineAnimator.SetTrigger("Recorded1");
            //if (chuchoter)
            //    _timelineAnimator.SetTrigger("Recorded2");
            _recordingState = RecordingState.NotRecording;
        }
    }

    /*IEnumerator __autocompleteAnimator_KRAKRA(string Trig)
    {
        yield return new WaitForSeconds(0.5f);
        _timelineAnimator.SetTrigger(Trig);
        yield return null;
    }*/


    IEnumerator __kickRecording()
    {
        _recordingState = RecordingState.Recording;
        _recorder.StartRecording();
        //for(; ; )
        //{
        //    _fRecordingTime += Time.deltaTime;
        //    yield return null;
        //}
        yield return new WaitForSeconds(10.0f);
        btnStopRecording();
        yield return null;
    }
}
