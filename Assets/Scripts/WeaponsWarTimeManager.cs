using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsWarTimeManager : MonoBehaviour
{
    public static float _timer = 0;
    [SerializeField] Text _text;
    public bool IsSurvive = true;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        _timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSurvive)
        {
            _timer += Time.deltaTime;
        }
        _text.text = "¶‘¶ŽžŠÔ : " + _timer.ToString("F2");
    }
}