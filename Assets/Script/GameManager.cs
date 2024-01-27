using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Mode
{
    Stop,
    Play
}
public class GameManager : MonoBehaviour
{
    //各必要変数
    public GameObject ResultMenu, Startobj;
    public Text number1, number2, cal, answertext, counttext, ResultCorrect, ResultIncorrect, notice, correctanswer, Score, limittext;
    string[] vs = { "+", "-", "×" };
    public int right, answer, question, correct, incorrect, score, addscore, magnification;
    public float limit;
    public bool stop;
    public AudioSource AudioSource;
    public AudioClip[] audios;
    public Animator noticeanimator, correctincorrectanimator, Startanimator;
    public Image correctincorrect, Rank;
    public Sprite[] sprites, Ranks;
    public Button[] Buttons;
    public Mode mode;
    delegate void SomeDelegate();
    SomeDelegate QuestionUpdate;

    /// <summary>
    /// 難易度チェック
    /// </summary>
    private void Awake()
    {
        mode = Mode.Stop;

        //難易度によって問題生成関数を設定する
        if (SelectGameManager.grade == Grade.easy)
        {
            QuestionUpdate = OneQuestionUpdate;
            Startanimator.SetTrigger("Easy");
        }
        else if (SelectGameManager.grade == Grade.normal)
        {
            QuestionUpdate = DoubleQuestionUpdate;
            Startanimator.SetTrigger("Normal");
        }
        else if (SelectGameManager.grade == Grade.Hand)
            QuestionUpdate = HardQuestionUpdate;
    }
    // Start is called before the first frame update
    void Start()
    {

        counttext.text = question.ToString() + "/50問";

        StartCoroutine("Starting");
    }

    // Update is called once per frame
    void Update()
    {
        if (mode == Mode.Play)
        {
            if (limit > 0f)
                limit -= Time.deltaTime;
            else
                addscore = 10;

            limittext.text = ((int)limit).ToString();
        }
    }

    /// <summary>
    /// 問題生成（答えが一桁問題）
    /// </summary>
    void OneQuestionUpdate()
    {
        int setnumber, setnumber2;

        setnumber= Random.Range(0, 10);

        int vsint = Random.Range(0, 2);
        cal.text = vs[vsint];
        switch (vs[vsint])
        {
            case "+":
                setnumber2 = Random.Range(0, 10 - setnumber);
                right = setnumber + setnumber2;
                Debug.Log(setnumber + setnumber2);

                break;
            default:
                setnumber2 = Random.Range(0, setnumber);
                right = setnumber - setnumber2;
                Debug.Log(setnumber - setnumber2);

                break;
        }


        number1.text = setnumber.ToString();
        number2.text = setnumber2.ToString();
        //Debug.Log(setnumber + setnumber2);
        ButtonDisplay(true);
    }

    /// <summary>
    /// 問題生成（答えが二桁問題）
    /// </summary>
    void DoubleQuestionUpdate()
    {
        int setnumber, setnumber2;

        setnumber = Random.Range(0, 100);

        int vsint = Random.Range(0, 2);
        cal.text = vs[vsint];
        switch (vs[vsint])
        {
            case "+":
                setnumber2 = Random.Range(0, 100 - setnumber);
                right = setnumber + setnumber2;
                Debug.Log(setnumber + setnumber2);

                break;
            default:
                setnumber2 = Random.Range(0, setnumber);
                right = setnumber - setnumber2;
                Debug.Log(setnumber - setnumber2);

                break;
        }

        number1.text = setnumber.ToString();
        number2.text = setnumber2.ToString();
        //Debug.Log(setnumber + setnumber2);
        int count = (right == 0) ? 1 : ((int)Mathf.Log10(right) + 1);
        //Debug.Log(count);
        ButtonDisplay(true);

    }

    /// <summary>
    /// 問題生成（答えが三桁問題）
    /// </summary>
    void HardQuestionUpdate()
    {
        int setnumber, setnumber2;

        setnumber = Random.Range(0, 100);

        int vsint = Random.Range(0, 3);
        cal.text = vs[vsint];
        switch (vs[vsint])
        {
            case "+":
                setnumber2 = Random.Range(0, 100 - setnumber);
                right = setnumber + setnumber2;
                Debug.Log(setnumber + setnumber2);
                break;
            case "-":
                setnumber2 = Random.Range(0, setnumber);
                right = setnumber - setnumber2;
                Debug.Log(setnumber - setnumber2);
                break;
            default:
                int max = 100 / setnumber;
                setnumber2 = Random.Range(0, max);
                right = setnumber * setnumber2;
                Debug.Log(setnumber * setnumber2);
                break;
        }

        number1.text = setnumber.ToString();
        number2.text = setnumber2.ToString();
        //Debug.Log(setnumber + setnumber2);
        int count = (right == 0) ? 1 : ((int)Mathf.Log10(right) + 1);
        Debug.Log(count);
    }

    /// <summary>
    /// 答えと解答のチェック
    /// </summary>
    /// <param name="setnumber"></param>
    public void CheckAnswer(int setnumber)
    {
        answer *= 10;
        answer += setnumber;
        answertext.text = answer.ToString();
        if (answer >= right)
        {
            mode = Mode.Stop;
            ButtonDisplay(false);
            StartCoroutine("AnswerShow");
        }
    }

    /// <summary>
    /// 問題カウント処理
    /// </summary>
    void QuestionCountDown()
    {
        question++;

        if (question < 50)
        {
            answertext.text = "?";
            QuestionUpdate();
            counttext.text = question.ToString() + "/50問";
            mode = Mode.Play;
        }
        else
        {
            Debug.Log("終了！" + "正解:" + correct + "不正解" + incorrect);
            mode = Mode.Stop;
            ResultFunction();
        }
    }

    /// <summary>
    /// 解答の前の削除
    /// </summary>
    public void Back()
    {
        answer /= 10;
        answertext.text = answer.ToString();
    }

    /// <summary>
    /// リザルト処理
    /// </summary>
    void ResultFunction()
    {
        if (score > 10800)
        {
            Debug.Log("S");
            Rank.sprite = Ranks[0];
        }
        else if (score > 9600)
        {
            Debug.Log("A");
            Rank.sprite = Ranks[1];
        }
        else if (score > 6400)
        {
            Debug.Log("B");
            Rank.sprite = Ranks[2];
        }
        else if (score > 3200)
        {
            Debug.Log("C");
            Rank.sprite = Ranks[3];
        }
        else
        {
            Debug.Log("D");
            Rank.sprite = Ranks[4];
        }
        Score.text = score.ToString();
        ResultCorrect.text = correct.ToString();
        ResultIncorrect.text = incorrect.ToString();
        ResultMenu.SetActive(true);
    }

    /// <summary>
    /// スタート合図
    /// </summary>
    /// <returns></returns>
    IEnumerator Starting()
    {
        yield return new WaitForSeconds(1.5f);
        QuestionUpdate();
        Startobj.SetActive(false);
        mode = Mode.Play;
    }

    /// <summary>
    /// 正解・不正解処理
    /// </summary>
    /// <returns></returns>
    IEnumerator AnswerShow()
    {
        if (right == answer)
        {
            correctincorrect.sprite = sprites[0];
            AudioSource.PlayOneShot(audios[0]);
            correctincorrectanimator.SetTrigger("Play");
            correct++;
            Debug.Log("正解");
            score += addscore;
            addscore += 10;
            magnification *= 2;
            limit = 7;
            limittext.text = limit.ToString();
            //10問間隔正解アニメーション
            if (correct % 10 == 0)
            {
                notice.text = correct.ToString() + "問正解！";
                noticeanimator.SetTrigger("In");
            }
        }
        else
        {
            correctincorrect.sprite = sprites[1];
            AudioSource.PlayOneShot(audios[1]);
            correctincorrectanimator.SetTrigger("Play");
            incorrect++;
            Debug.Log("不正解");
            addscore = 10;
        }

        correctanswer.text = right.ToString();
        yield return new WaitForSeconds(1);
        correctanswer.text = "";

        answer = 0;
        QuestionCountDown();
    }

    /// <summary>
    /// ボタン有効・無効処理
    /// </summary>
    /// <param name="set"></param>
    public void ButtonDisplay(bool set)
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = set;   
        }
    }

    /// <summary>
    /// シーンの切り替え
    /// </summary>
    /// <param name="number"></param>
    public void SceneLoad(int number)
    {
        SceneManager.LoadScene(number);
    }

    //public void Entyo()
    //{
    //    limit = 5;
    //    limittext.text = ((int)limit).ToString();
    //    stop = true;
    //}

    //public void Ranking()
    //{
    //    Debug.Log("ランキング");
    //    if (SelectGameManager.grade == Grade.easy)
    //        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score, 0);
    //    else if(SelectGameManager.grade==Grade.normal)
    //        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score, 1);

    //}
}
