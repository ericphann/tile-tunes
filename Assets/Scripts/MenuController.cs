using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

    public float timingDifficulty = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        SetNormal();
    }

    void Awake(){
        DontDestroyOnLoad(transform.gameObject);
    }

    public void SetNormal(){
        timingDifficulty = 0.25f;
        GameObject.Find("Normal").GetComponent<Text>().color = Color.white;
        GameObject.Find("Hard").GetComponent<Text>().color = Color.gray;
    }

    public void SetHard(){
        timingDifficulty = 0.125f;
        GameObject.Find("Normal").GetComponent<Text>().color = Color.gray;
        GameObject.Find("Hard").GetComponent<Text>().color = Color.white;
    }

    public void Play() {
        SceneManager.LoadScene("Main");
    }

    public void Quit() {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
