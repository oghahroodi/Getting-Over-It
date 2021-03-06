using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityStandardAssets.Cameras;
using UnityEngine.UI;

public class GamePresenter : MonoBehaviour
{
    GameModel gameModel = new GameModel();
    public GameObject [] playerPrefab;
    public GameObject [] hammerPrefab;
    public GameObject  dwarfPrefab;
    public GameObject  dwarfPrefabLevel;
    public GameObject MainMenuUIObject;
    public GameObject G;
    public GameObject Level;
    public GameObject Winner;
    //public float MaxDistance;
    public AudioSource zoor;
    public AudioSource win;
    public ServiceLocator ServiceLocator;
    public Dropdown selectPlayer;
    public Dropdown selectHammer;
   
    //private bool canContinue;
    public Text ScoreText;
    public Text FinalScore;
    public Text ScoreTextLevel;
    
    //private float timer;
    //private int score;
    //private int level_player;
    private GameObject player;
    private GameObject hammer;
    private Rigidbody body_rig;
    private Rigidbody hammer_rig;
    private Transform hammer_anchor;
    //private float RelativeDistance;
    private GameObject dwarf;
    //private bool isPlay = false;
    private static Vector3 startPos = new Vector3(4, 8, -94);


    // Start is called before the first frame update
    void Start()
    {
        //Application.targetFrameRate = 10;

    }

    void Update()
    {
        if (gameModel.get_isPlay()) {
            float time = gameModel.get_timer();
            gameModel.set_timer(time + Time.deltaTime);

            //timer += Time.deltaTime;
            //if (player.transform.position.x > 100 && player.transform.position.x < 110 && player.transform.position.y > 150 && player.transform.position.y < 160 && gameModel.get_level_player() == 2)
            if (gameModel.checkWin(player.transform.position.x, player.transform.position.y, gameModel.get_level_player()))
            {
               this.Win();
            }
            //if (player.transform.position.x > 260 && player.transform.position.x < 270 && gameModel.get_level_player() == 1)
            if(gameModel.checklvl(player.transform.position.x, gameModel.get_level_player()))
            {
                this.CreateSaveGameObject();
                this.nextLevel();
                
            }
            if (gameModel.get_timer() > 1f)
            {

                //score += 1;
                int playerScore = gameModel.get_score() + 1;
                gameModel.set_score(playerScore);

                //We only need to update the text if the score changed.
                ScoreText.text = gameModel.get_score().ToString();
                ScoreTextLevel.text = gameModel.get_score().ToString();
                FinalScore.text = gameModel.get_score().ToString();
                //Reset the timer to 0.
                //timer = 0;
                gameModel.set_timer(0);

                PlayerPrefs.SetInt("Score", gameModel.get_score());

            }

           

        }
        if (Input.GetKeyUp("escape"))
        {
            //this.CreateSaveGameObject();
            this.Pause();
            MainMenuUIObject.SetActive(value: !MainMenuUIObject.active);
            G.SetActive(value: !G.active);
        }
    }

    [System.Serializable]
    public class Save
    {
        public Vector3 pos = new Vector3((float)4, (float)8, (float)-94);
        public Vector3 pos_p = new Vector3((float)-2.835723, (float)-3.5, (float)-3.472275);
        public Vector3 pos_h = new Vector3((float)1.250509, (float)-0.3657408, (float)-4.594345);
        public int p_score;
        public int level_player;
        public bool canContinue = false;
    }

    public void CreateSaveGameObject()
    {
        Save save = new Save();
        save.level_player = gameModel.get_level_player();
        save.p_score = gameModel.get_score();
        save.canContinue = gameModel.get_canContinue();
        save.pos_p.x = player.transform.position.x;
        save.pos_p.y = player.transform.position.y; 
        save.pos_p.z = player.transform.position.z;
        save.pos_h.x = hammer.transform.position.x;
        save.pos_h.y = hammer.transform.position.y;
        save.pos_h.z = hammer.transform.position.z;
        save.pos.x = dwarf.transform.position.x;
        save.pos.y = dwarf.transform.position.y;
        save.pos.z = dwarf.transform.position.z;
        Destroy(player);
        Destroy(hammer);
        string json = JsonUtility.ToJson(save);
        Debug.Log(json);
        PlayerPrefs.SetString("save", json);
    }

    public void LoadSave()
    {
        
        string json = PlayerPrefs.GetString("save");
        Save save = JsonUtility.FromJson<Save>(json);
        bool cango = save.canContinue;
        if (cango == true){
        gameModel.set_score(save.p_score);
        gameModel.set_level_player(save.level_player);
        Debug.Log(json);
        
        player = Instantiate(playerPrefab[selectPlayer.value], save.pos_p, Quaternion.identity);
        hammer = Instantiate(hammerPrefab[selectHammer.value], save.pos_h, Quaternion.identity);
       
        if(gameModel.get_level_player() == 1){
             player.transform.parent = dwarfPrefab.transform;
             hammer.transform.parent = dwarfPrefab.transform;
       

            body_rig = player.GetComponent<Rigidbody>();
            hammer_rig = hammer.GetComponent<Rigidbody>();
            hammer_anchor = hammer.transform.GetChild(2);

        //dwarf.transform.localPosition = startPos;
            AbstractTargetFollower.set_m_Target(player.transform);
            gameModel.set_isPlay(true);
            G.SetActive(true);
        }
        else if(gameModel.get_level_player() == 2){
            player.transform.parent = dwarfPrefabLevel.transform;
            hammer.transform.parent = dwarfPrefabLevel.transform;
       

            body_rig = player.GetComponent<Rigidbody>();
            hammer_rig = hammer.GetComponent<Rigidbody>();
            hammer_anchor = hammer.transform.GetChild(2);

        //dwarf.transform.localPosition = startPos;
            AbstractTargetFollower.set_m_Target(player.transform);
            gameModel.set_isPlay(true);
            Level.SetActive(true);
        }
         MainMenuUIObject.SetActive(false);
        }
        
        
    }

   public void nextLevel(){
        
        gameModel.set_level_player(2);
        
        zoor.mute = false;
        win.mute = true;
        

        player = Instantiate(playerPrefab[selectPlayer.value]);
        hammer = Instantiate(hammerPrefab[selectHammer.value]);

        body_rig = player.GetComponent<Rigidbody>();
        hammer_rig = hammer.GetComponent<Rigidbody>();
        hammer_anchor = hammer.transform.GetChild(2);

        dwarf = new GameObject();
        player.transform.parent = dwarf.transform;
        hammer.transform.parent = dwarf.transform;

        dwarf.transform.parent = Level.transform;
        dwarf.transform.localPosition = startPos;
        AbstractTargetFollower.set_m_Target(player.transform);
        gameModel.set_isPlay(true);
        G.SetActive(false);
        Level.SetActive(true);
    }


    public void Continue(){
        gameModel.set_isPlay(true);
        zoor.mute = false;
        win.mute = true;

    }

    public void Pause()
    {
        bool play = gameModel.get_isPlay();
        gameModel.set_isPlay(!play);
        win.mute = true;
    }

    public void Win()
    {
        Destroy(player);
        Destroy(hammer);
        bool play = gameModel.get_isPlay();
        gameModel.set_isPlay(!play);
        Winner.SetActive(true);
        G.SetActive(false);
        Level.SetActive(false);
        win.mute = false;
        win.Play();
        
    }
        public void FakeAdvertise(){
        if (gameModel.get_score() > 2)
        {
            int player_score = ServiceLocator.Instance.advertise.advertise(gameModel.get_score());
            gameModel.set_score(player_score);
            FinalScore.text = gameModel.get_score().ToString();
            Debug.Log(gameModel.get_score());
        }
    }

     public void FakePay(){
        if (gameModel.get_score() > 1)
        {
            int player_score = ServiceLocator.Instance.advertise.advertise(gameModel.get_score());
            gameModel.set_score(player_score);
            FinalScore.text = gameModel.get_score().ToString();
            //Debug.Log(score);
        }
    }

    public void NewGame()
    {
        Destroy(dwarf);
        //canContinue = true;
        gameModel.set_canContinue(true);
        zoor.mute = false;
        win.mute = true;
        //timer = 0;
        gameModel.set_timer(0);
        //score = 0;
        gameModel.set_score(0);
        //level_player = 1;
        gameModel.set_level_player(1);
        ScoreText.text = gameModel.get_score().ToString();
        PlayerPrefs.SetInt("Score", 0);

        player = Instantiate(playerPrefab[selectPlayer.value]);
        hammer = Instantiate(hammerPrefab[selectHammer.value]);

        body_rig = player.GetComponent<Rigidbody>();
        hammer_rig = hammer.GetComponent<Rigidbody>();
        hammer_anchor = hammer.transform.GetChild(2);

        dwarf = new GameObject();
        player.transform.parent = dwarf.transform;
        hammer.transform.parent = dwarf.transform;

        dwarf.transform.parent = G.transform;
        dwarf.transform.localPosition = startPos;
        AbstractTargetFollower.set_m_Target(player.transform);
        gameModel.set_isPlay(true);

    }

    public void Exit()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameModel.get_isPlay())
        {
            HammerControl();
        }
    }

    void HammerControl()
    {

        Vector2 MousePosition = GetConfinedPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        hammer_rig.velocity = (new Vector3(MousePosition.x, MousePosition.y, hammer.transform.position.z) - hammer_anchor.position) * 10.0f;


        if (hammer.GetComponent<CollisionDetection>().IsCollision)
        {
            zoor.mute = false;
            zoor.Play();
            BodyControl(hammer_rig.velocity);
        }

        Vector3 direction = (new Vector3(MousePosition.x, MousePosition.y, hammer_anchor.position.z) - new Vector3(player.transform.position.x, player.transform.position.y, hammer_anchor.position.z)).normalized;
        hammer.transform.RotateAround(hammer_anchor.position, Vector3.Cross(hammer_anchor.up, direction), Vector3.Angle(hammer_anchor.up, direction));
    }

    void BodyControl(Vector3 velocity)
    {
        body_rig.velocity = -velocity * 0.5f;
    }

   

    Vector2 GetConfinedPosition(Vector2 mouseposition)
    {
        Vector2 confinedMousePosition;
        Vector2 JackPosition = new Vector2(player.transform.position.x, player.transform.position.y);
        //RelativeDistance = Vector2.Distance(mouseposition, JackPosition);
        gameModel.set_RelativeDistance(Vector2.Distance(mouseposition, JackPosition));
        if (gameModel.get_RelativeDistance() > gameModel.get_MaxDistance())
        {
            confinedMousePosition = (mouseposition - JackPosition).normalized * gameModel.get_MaxDistance() + JackPosition;

        }
        else
        {
            confinedMousePosition = mouseposition;
        }
        return confinedMousePosition;
    }

}


