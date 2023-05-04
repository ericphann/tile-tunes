using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{

    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindAllMatches(){
        StartCoroutine(FindAllMatchesCo());
    }

    private IEnumerator FindAllMatchesCo() {
        yield return new WaitForSeconds(.2f);
        for(int i = 0; i < board.width; i++) {
            for(int j = 0; j < board.height; j++) {
                GameObject currentDot = board.allDots[i, j];
                if(currentDot != null) {
                    if(i > 0 && i < board.width - 1) { // horizontal check
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];
                        if(leftDot != null && rightDot != null) {
                            if(leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag) {
                                if(!currentMatches.Contains(leftDot)) {
                                    currentMatches.Add(leftDot);
                                }
                                leftDot.GetComponent<Dot>().isMatched = true;
                                
                                if(!currentMatches.Contains(rightDot)) {
                                    currentMatches.Add(rightDot);
                                }
                                rightDot.GetComponent<Dot>().isMatched = true;
                                
                                if(!currentMatches.Contains(currentDot)) {
                                    currentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<Dot>().isMatched = true;
                            }
                        }
                    }
                    if(j > 0 && j < board.height - 1) { //vertical check
                        GameObject upDot = board.allDots[i, j + 1];
                        GameObject downDot = board.allDots[i, j - 1];
                        if(upDot != null && downDot != null) {
                            if(upDot.tag == currentDot.tag && downDot.tag == currentDot.tag) {
                                if(!currentMatches.Contains(upDot)) {
                                    currentMatches.Add(upDot);
                                }
                                upDot.GetComponent<Dot>().isMatched = true;

                                if(!currentMatches.Contains(downDot)) {
                                    currentMatches.Add(downDot);
                                }
                                downDot.GetComponent<Dot>().isMatched = true;

                                if(!currentMatches.Contains(currentDot)) {
                                    currentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<Dot>().isMatched = true;
                            }
                        }   
                    }
                }
            }
        }
    }   
}