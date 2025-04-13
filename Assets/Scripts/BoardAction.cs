using System.Collections;
using AndoomiUtils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BoardAction : MonoBehaviour
{
    public static BoardAction instance;
    bool _isUsed;

    [SerializeField] Canvas boardActionCanvas;
    [SerializeField] RectTransform boardRect;
    void Awake()
    {
        if(instance == null) instance = this;
        else if (instance != this) Destroy(instance);
    }

    // void Update()
    // {
    //     if(Input.GetMouseButtonDown(1) && _isUsed) {
    //         _isUsed = false;
    //         DestroyChoices();
    //     }
    // }

    [SerializeField] Button prefabButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void CreateBoard(BoardButton[] _boardButtons) {
        frame_f = 0;
        StopCoroutine("CheckCloseBoard");
        _isUsed = !_isUsed;
        if(_isUsed){
            boardRect.anchoredPosition = Input.mousePosition/boardActionCanvas.scaleFactor + (boardRect.sizeDelta.Vec2ToVec3YInverted()/2.0f) * 1.1f;
            foreach(var rb in _boardButtons) {
                var b = Instantiate(prefabButton, Vector3.zero, prefabButton.transform.rotation, boardRect).gameObject;
                b.SetActive(true);
                b.GetComponent<BoardUIButton>().InitButton(rb, this);
            }
            StartCoroutine("CheckCloseBoard");
        }else {
            DestroyChoices();
        }
    }

    public void DestroyChoices() {
            for(int i = boardRect.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(boardRect.transform.GetChild(i).gameObject);
            }
    }

    public void TurnOffBoard() {
        _isUsed=false;
        DestroyChoices();
    }

    int frame_f = 0;
    IEnumerator CheckCloseBoard() {
        
        while(_isUsed) {
            if(Input.GetMouseButtonDown(1) && frame_f > 0) {
                _isUsed = false;
                DestroyChoices();
            }
            frame_f++;
            yield return null;
        }
        yield return null;
    }
}
