using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [Header("Board Variables")]
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public int targetX;
    public int targetY;
    public bool isMatched = false;

    private FindMatches findMatches;
    private Board board;
    private Conductor conductor;
    public GameObject otherDot;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;

    [Header("Swipe Stuff")]
    public float swipeAngle = 0;
    public float swipeResist = 0.5f;

    [Header("Powerup Stuff")]
    public bool isColorBomb;
    public bool isColumnBomb;
    public bool isRowBomb;
    public bool isAdjacentBomb;
    public GameObject adjacentMarker;
    public GameObject rowArrow;
    public GameObject columnArrow;
    public GameObject colorBomb;

    // Start is called before the first frame update
    void Start()
    {
        isColumnBomb = false;
        isRowBomb = false;
        isColorBomb = false;
        isAdjacentBomb = false;

        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        conductor = FindObjectOfType<Conductor>();
    }

    // testing and debug, turn any piece to row/column bomb
    private void OnMouseOver() {
        if(Input.GetMouseButtonDown(1)) {
            isAdjacentBomb = true;
            GameObject marker = Instantiate(adjacentMarker, transform.position, Quaternion.identity);
            marker.transform.parent = this.transform;
        }
    }
    // Update is called once per frame
    void Update()
    {
        float currentScale = 1.0f;
        if (conductor.songPosition <= conductor.musicSource.clip.length) {
            currentScale = 1.0f + 0.1f * Mathf.Max(Mathf.Sin(conductor.songPositionInBeats * 2 * Mathf.PI), 0); // once per beat * 2pi radians per cycle
        } else {
            board.currentState = GameState.wait;
        }
        this.GetComponent<Dot>().transform.localScale = new Vector3(currentScale, currentScale, 1.0f);

        targetX = column;
        targetY = row;

        // horizontal update
        if(Mathf.Abs(targetX - transform.position.x) > .1f) {
            // Move towards the target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != this.gameObject) {
                board.allDots[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        } else {
            // Directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }

        // vertical update
        if(Mathf.Abs(targetY - transform.position.y) > .1f) {
            // Move towards the target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != this.gameObject) {
                board.allDots[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        } else {
            //Directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }
    }

    private void OnMouseDown() {
        if(board.currentState == GameState.move){
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public IEnumerator CheckMoveCo() { 
        if (otherDot != null) {
            if(isColorBomb) {
                findMatches.MatchPiecesOfColor(otherDot.tag);
                isMatched = true;
            } else if (otherDot.GetComponent<Dot>().isColorBomb) {
                findMatches.MatchPiecesOfColor(this.gameObject.tag);
                otherDot.GetComponent<Dot>().isMatched = true;
            }
        }

        yield return new WaitForSeconds(.5f);
        if (otherDot != null) {
            if (!isMatched && !otherDot.GetComponent<Dot>().isMatched) {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;
                row = previousRow;
                column = previousColumn;
                yield return new WaitForSeconds(.5f);
                board.currentDot = null;
                board.currentState = GameState.move;
            } else {
                board.DestroyMatches();
            }
        } else {
            board.currentState = GameState.move;
        }
    }

    private void OnMouseUp() {
        if(board.currentState == GameState.move) {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
        }
    }

    void CalculateAngle() {
        if(Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist) {
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x);
            swipeAngle *= 180/Mathf.PI;
            MovePieces();
            board.currentState = GameState.wait;
            board.currentDot = this;
        } else {
            board.currentState = GameState.move;
        }
    }

    void MovePieces() {
        if(swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1) {
            // Right swipe
            otherDot = board.allDots[column + 1, row];
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;
        } else if(swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1) {
            // Up swipe
            otherDot = board.allDots[column, row + 1];    
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;
        } else if((swipeAngle > 135 || swipeAngle <= -135) && column > 0) {
            // Left swipe
            otherDot = board.allDots[column - 1, row];  
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        } else if(swipeAngle > -135 && swipeAngle < -45 && row > 0) {
            // Down swipe
            otherDot = board.allDots[column, row - 1];     
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }
        StartCoroutine(CheckMoveCo());
    }

    void FindMatches() {
        // horizontal check
        if(column > 0 && column < board.width - 1) {
            GameObject leftDot1 = board.allDots[column - 1, row];
            GameObject rightDot1 = board.allDots[column + 1, row];
            if (leftDot1 == null || rightDot1 == null) {
                // null moment
            } else if (leftDot1 == this.gameObject || rightDot1 == this.gameObject) {
                // horizontal bruh moment
            } else if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag) {
                leftDot1.GetComponent<Dot>().isMatched = true;
                rightDot1.GetComponent<Dot>().isMatched = true;
                isMatched = true;
            }
        }

        // vertical check
        if(row > 0 && row < board.height - 1) {
            GameObject upDot1 = board.allDots[column, row + 1];
            GameObject downDot1 = board.allDots[column, row - 1];
            if (upDot1 == null || downDot1 == null) {
                // null moment
            } else if (upDot1 == this.gameObject || downDot1 == this.gameObject) {
                // vertical bruh moment
            } else if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag) {
                upDot1.GetComponent<Dot>().isMatched = true;
                downDot1.GetComponent<Dot>().isMatched = true;
                isMatched = true;
            }
        }
    }

    public void MakeRowBomb() {
        isRowBomb = true;
        GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = this.transform;
    }

    public void MakeColumnBomb() {
        isColumnBomb = true;
        GameObject arrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = this.transform;
    }

    public void MakeColorBomb() {
        isColorBomb = true;
        GameObject color = Instantiate(colorBomb, transform.position, Quaternion.identity);
        color.transform.parent = this.transform;
    }

    public void MakeAdjacentBomb() {
        isAdjacentBomb = true;
        GameObject marker = Instantiate(adjacentMarker, transform.position, Quaternion.identity);
        marker.transform.parent = this.transform;
    }
}
