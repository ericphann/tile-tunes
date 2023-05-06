using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public Conductor conductor;
    public Board board;

    public float timeSinceLastGoodHit = 0.0f;

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
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastGoodHit += Time.deltaTime;

        if(Input.GetKeyDown(keyToPress)) {
            sr.sprite = pressedImage;
            float timing = conductor.songPositionInBeats % 1;
            if (timing < 0.25 || timing > 0.75) {
                Debug.Log("slaying the game");
                timeSinceLastGoodHit = 0;
                if(board.multiplier < 4) {
                    board.multiplier++;
                }

            } else {
                Debug.Log("bad");
                board.multiplier = 1;
            }

        }

        if (timeSinceLastGoodHit > 1.5f) {
            board.multiplier = 1;
        }

        if(Input.GetKeyUp(keyToPress)) {
            sr.sprite = defaultImage;
        }
    }
}
