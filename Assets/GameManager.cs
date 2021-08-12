using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //各必要変数
    public GameObject ResultMenu, Startobj;
    public Text number1, number2, cal, answertext, counttext, ResultCorrect, ResultIncorrect, starttext, notice, correctanswer, Score;
    string[] vs = { "+", "-", "×" };
    public int right, answer, question, correct, incorrect, score, addscore;
    public AudioSource AudioSource;
    public AudioClip[] audios;
    public Animator noticeanimator, correctincorrectanimator;
    public Image correctincorrect;
    public Sprite[] sprites;
    delegate void SomeDelegate();
    SomeDelegate a;

    private void Awake()
    {
        if (SelectGameManager.grade == Grade.easy)
            a = QuestionUpdate;
        else if (SelectGameManager.grade == Grade.normal)
            a = DoubleQuestionUpdate;
        else if(SelectGameManager.grade == Grade.Hand)
            a = HardQuestionUpdate;
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
        
    }

    void QuestionUpdate()
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
    }
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
        Debug.Log(count);
    }

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
    public void CheckAnswer(int setnumber)
    {
        answer *= 10;
        answer += setnumber;
        answertext.text = answer.ToString();
        if (answer >= right)
        {

            StartCoroutine("AnswerShow");
        }
    }

    void QuestionCountDown()
    {
        question++;

        if (question < 50)
        {
            answertext.text = "?";
            //QuestionUpdate();
            a();
            counttext.text = question.ToString() + "/50問";
        }
        else
        {
            Debug.Log("終了！" + "正解:" + correct + "不正解" + incorrect);
            ResultFunction();
        }
    }

    public void Back()
    {
        answer /= 10;
        answertext.text = answer.ToString();
    }

    void ResultFunction()
    {
        Score.text = score.ToString();
        ResultCorrect.text = correct.ToString();
        ResultIncorrect.text = incorrect.ToString();
        ResultMenu.SetActive(true);
    }

    IEnumerator Starting()
    {
        //Debug.Log("よーい");
        starttext.text = "よーい";
        yield return new WaitForSeconds(3);

        //Debug.Log("スタート");
        starttext.text = "スタート";
        yield return new WaitForSeconds(1);

        //非アクティブ
        Startobj.SetActive(false);

        a();
    }

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
            addscore += addscore;

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

    public void SceneLoad(int number)
    {
        SceneManager.LoadScene(number);
    }
}
