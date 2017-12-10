using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour {
   
        public Button start_button;
        private new string tag;

        // Use this for initialization
        void Start()
        {
            Button btn = start_button.GetComponent<Button>();
            tag = btn.tag;
            btn.onClick.AddListener(StartOnClick);

        }

        void StartOnClick()
        {
            if (tag == "Original")
            {
                Debug.Log(tag);
                SceneManager.LoadScene("Original-Game", LoadSceneMode.Single);
            }
           
        }
        // Update is called once per frame
        void Update()
        {

        }
    
}

