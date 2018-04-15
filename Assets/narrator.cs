using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class narrator : MonoBehaviour {

    public GameObject[] guns;
    public GameObject[] ui;
    public LineRenderer[] lines;
    public GameObject[] gunText;
    public GameObject[] demoItems;
    public Text tutText;
    public string text;
    public GameObject board;
    public int tutorialStep = 0;
    public float tutTime = 0;
    public float beforeAdvanceTime = 1.0f;
    public float beforeTextTime = 2.0f;
    public bool proceed = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        tutTime += Time.deltaTime;

        if ((Input.GetAxis("LPalmTrigger") > 0.5f && Input.GetAxis("RPalmTrigger") > 0.5f) && tutTime > beforeAdvanceTime)
        {
            tutorialStep++;
            tutTime = 0;
        }

        
        // tutorial stages
        if (tutorialStep == 0)
        {
            text = "Welcome!";
        }
        else if (tutorialStep == 1)
        {
            text = "The goal of WEB_SURFER_95 is to 'surf the web' from the start to the end of the path before time runs out!";
        }
        else if (tutorialStep == 2)
        {
            text = "You want to arrive at this finish line way down the path before Mom uses the phone, and interrupts your DSL modem!";
            demoItems[4].SetActive(true);
            lines[0].SetPosition(1, demoItems[4].transform.position);
        }
        else if (tutorialStep == 3)
        {
            text = "Along this path there are powerups, some bad some good, and enemies to shoot.";
            demoItems[4].SetActive(false);
            lines[0].SetPosition(1, new Vector3(0,5,50));
        }
        else if (tutorialStep == 4)
        {
            text = "You can move along this path in a limited area, to the left and right.";
        }
        else if (tutorialStep == 5)
        {
            text = "Try moving left and right with either joytick!";
        }
        else if (tutorialStep == 6)
        {
            text = "You can pick up powerups like this to speed you up";
            demoItems[0].SetActive(true);
        }
        else if (tutorialStep == 7)
        {
            text = "and powerups like this to give you more time to finish";
            demoItems[0].SetActive(false);
            demoItems[1].SetActive(true);
        }
        else if (tutorialStep == 8)
        {
            text = "and powerups like this will slow you down, so avoid these!";
            demoItems[1].SetActive(false);
            demoItems[2].SetActive(true);
        }
        else if (tutorialStep == 9)
        {
            text = "this is an enemy, shoot these, if they hit you you'll lose time!";
            demoItems[2].SetActive(false);
            demoItems[3].SetActive(true);
        }
        else if (tutorialStep == 10)
        {
            text = "These are your floppy drive guns, press the index triggers to shoot.";
            guns[0].SetActive(true);
            guns[1].SetActive(true);

            lines[0].SetPosition(1, guns[0].transform.position);
            lines[1].SetPosition(1, guns[1].transform.position);
        }
        else if (tutorialStep == 11)
        {
            text = "Your left floppy gun will show you your speed (the arrows), and your progress toward the end.";
            lines[0].SetPosition(1, guns[0].transform.position);
            lines[1].SetPosition(1, new Vector3(0, 5, 50f));
            ui[0].SetActive(true);
        }
        else if (tutorialStep == 12)
        {
            text = "Your right floppy gun will show you how much time you have left until mom calls, and the score you win with.";
            lines[0].SetPosition(1, new Vector3(0, 5, 50f));
            lines[1].SetPosition(1, guns[1].transform.position);
            ui[1].SetActive(true);
        }
        else if (tutorialStep == 13)
        {
            lines[0].SetPosition(1, new Vector3(0, 5, 50f));
            lines[1].SetPosition(1, new Vector3(0, 5, 50f));
            text = "That's all! Press both palm triggers again to load the level! Good luck!";
        }
        else if (tutorialStep == 14)
        {
            SceneManager.LoadScene("environment");
        }


        // Add proceed hint
        if (tutTime > beforeTextTime && tutorialStep == 0)
        {
            tutText.text = text + "\n\n Press both palm triggers(where your middle fingers are) to continue!";
        }
        else if (tutTime > beforeTextTime && tutorialStep > 0)
        {
            tutText.text = text + "\n\n Press both palm triggers to continue!";
        }
        else
        {
            tutText.text = text;
        }

    }
}
