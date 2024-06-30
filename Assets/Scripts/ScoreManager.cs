using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static int _score;
    public static int _screUp = 100;
    [SerializeField] Text _scoreText;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        _score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = "“¾“_ : " + _score.ToString();
    }
}