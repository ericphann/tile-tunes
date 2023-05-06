using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public Conductor conductor;
    public Board board;

    public float timeSinceLastGoodHit = 0.0f;
    public float acceptableTiming = 0.25f;

    //sprites and images
    private SpriteRenderer sr;
    public Sprite defaultImage;
    public Sprite pressedImage;

    //key to trigger image
    public KeyCode keyToPress;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        conductor = FindObjectOfType<Conductor>();
        board = FindObjectOfType<Board>();
        acceptableTiming = FindObjectOfType<MenuController>().timingDifficulty;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastGoodHit += Time.deltaTime;

        if(Input.GetKeyDown(keyToPress)) {
            sr.sprite = pressedImage;
            float timing = conductor.songPositionInBeats % 1;
            if (timing < acceptableTiming || timing > 1 - acceptableTiming) {
                Debug.Log("slaying the game");
                timeSinceLastGoodHit = 0;
                if(board.multiplier < 32) {
                    board.multiplier++;
                }

            } else {
                Debug.Log("bad");
                board.multiplier = 1;
            }

        }

        if (timeSinceLastGoodHit > 1 + 2 * acceptableTiming) {
            board.multiplier = 1;
        }

        if(Input.GetKeyUp(keyToPress)) {
            sr.sprite = defaultImage;
        }

        if(Input.GetKeyDown("escape") || Input.GetKeyDown("backspace")) {
            SceneManager.LoadScene("SceneMenu");
        }
    }
}
