using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Grade {
    easy,
    normal,
    Hand
};
public class SelectGameManager : MonoBehaviour
{
    public static Grade grade = Grade.easy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectBtn(int number)
    {
        grade = (Grade)number;
        Debug.Log(grade);
        SceneManager.LoadScene(1);
    }
}
