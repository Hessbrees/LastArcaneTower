using UnityEngine;
using UnityEngine.UI;



    [DefaultExecutionOrder(1)]
    public class AudioController : MonoBehaviour
    {
        public bool isMusicOff = false;

        [SerializeField] private Button[] musicButtons;
        [SerializeField] private Sprite musicON;
        [SerializeField] private Sprite musicOFF;

        private bool isClicked;

        private void Awake()
        {
            musicON = Resources.Load<Sprite>("Art/button_music");
            musicOFF = Resources.Load<Sprite>("Art/button_music_off");
            musicButtons = GameManager.Instance.GetMusicButtons().ToArray();
        }

        void Start()
        {
            if (isClicked)
            {
                foreach (var music in musicButtons)
                    music.image.sprite = musicOFF;
            }
            else
            {
                foreach (var music in musicButtons)
                    music.image.sprite = musicON;
            }
        }

        public void ToggleMusic()
        {
            if (!isClicked)
            {
                foreach (var music in musicButtons)
                    music.image.sprite = musicOFF;
                isClicked = true;

                //to do
                //change music audio to off
            }
            else
            {
                foreach (var music in musicButtons)
                    music.image.sprite = musicON;
                isClicked = false;

                //to do
                //change music audio to on
            }
        }
    }

