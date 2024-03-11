using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent onReset;
    public static GameManager instance;
    public GameObject readyPannel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI messageText;

    public bool isRoundActive = false;
    private int score = 0;
    public ShooterRotator shooterRotator;
    public CamFollow cam;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        UpdateUI();
    }

    void Start(){
        StartCoroutine("RoundRoutine");

    }
    public void AddScore(int newScore){
        score += newScore;
        UpdateBestScore();
        UpdateUI();
    }

    void UpdateBestScore(){
        if(GetBestScore() < score){
            PlayerPrefs.SetInt("BestScore", score);
        }
    }

    int GetBestScore(){
        int bestScore = PlayerPrefs.GetInt("BestScore");
        return bestScore;
    }

    // Update is called once per frame
    void UpdateUI()
    {
        scoreText.text = "Score : " + score;
        bestScoreText.text = "Best Score : " + GetBestScore();
    }

    public void OnBallDestroy(){
        UpdateUI();
        isRoundActive = false;

    }

    public void Reset(){
        score = 0;
        UpdateUI();

        //라운드 다시 처음부터 시작
        StartCoroutine("RoundRoutine");
    }

    IEnumerator RoundRoutine(){
        //READY
        onReset.Invoke();
        
        readyPannel.SetActive(true);
        cam.SetTarget(shooterRotator.transform, CamFollow.State.Idle);
        shooterRotator.enabled = false;

        isRoundActive = false;

        messageText.text = "Ready...";

        yield return new WaitForSeconds(3f);
        //PLAY
        isRoundActive = true;
        readyPannel.SetActive(false);
        shooterRotator.enabled = true;

        cam.SetTarget(shooterRotator.transform, CamFollow.State.Ready);

        while(isRoundActive == true){
            yield return null;
        }
        //END

        readyPannel.SetActive(true);
        shooterRotator.enabled = false;

        messageText.text = "Wait For Next Round...";

        yield return new WaitForSeconds(3f);
        Reset();
    }
}
