using System;
using System.Collections;
using TMPro;
using UnityEngine;

    public class Type  : MonoBehaviour
    {
        public TextMeshProUGUI textMeshPro;
        public AudioSource TypingSource; 
        public float typingSpeed = 0.4f;
        public AudioClip typing;
        public bool finish = false;


        private void Start()
        {
            TypingSource.clip = typing;
            TypingSource.loop = true;
        }

        public IEnumerator TypeSentence(string sentence)
        {
            finish = false;
            textMeshPro.text = "";
            TypingSource.Play();
            Debug.Log("typing: " +TypingSource.isPlaying);

            for (int i=0 ; i<sentence.Length ; i++)
            {
                char character = sentence[i];
                //Debug.Log(character);
                textMeshPro.text += character;
                yield return new WaitForSeconds(typingSpeed);
            }
            TypingSource.Stop();
            finish = true;
        }
    }
