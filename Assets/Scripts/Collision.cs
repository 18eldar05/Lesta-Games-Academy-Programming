using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript1 : MonoBehaviour
{
    Material orange;
    Material white;
    Material red;
    Material grey;
    GameObject cam;
    GameObject restartButton;
    public Text victory;
    public Text resultTime;
    public Text defeat;
    bool isVictory, isDefeat;
    public Rigidbody rb;
    public float acceleration = 10f;
    public float jump = 0.1f;
    public float direction = 10f;
    public float windAcceleration = 4f;
    float windRest = 0f;
    int randomNumber;
    float directionX, directionZ;
    float timeStart = 0f;
    float enemyTimeStart = 0f;
    float rest = 0f;
    public Text textTimer;
    public Text textSlider;
    public Slider slider;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (rest > 0f)
            {
                other.gameObject.GetComponent<Renderer>().material = white;
            }
            else
            {
                other.gameObject.GetComponent<Renderer>().material = orange;
                enemyTimeStart += Time.deltaTime;
                if (enemyTimeStart >= 1f)
                {
                    other.gameObject.GetComponent<Renderer>().material = red;
                    slider.value -= 34.55f;
                    textSlider.text = "Health: " + slider.value.ToString("F2");
                    rest = 5f;
                    enemyTimeStart = 0f;
                }
            }
        }

        if (other.gameObject.tag == "Wind")
        {
            other.gameObject.GetComponent<Renderer>().material = grey;

            if (windRest <= 0f)
            {
                System.Random rand = new System.Random();
                randomNumber = rand.Next(4);
                windRest = 2f;
            }

            if (randomNumber == 0)
            {
                print("North East");
                directionX = directionZ = direction;
            }
            else if (randomNumber == 1)
            {
                print("South West");
                directionX = directionZ = -direction;
            }
            else if (randomNumber == 2)
            {
                print("South East");
                directionX = direction;
                directionZ = -direction;
            }
            else
            {
                print("North West");
                directionX = -direction;
                directionZ = direction;
            }

            rb.AddForce(new Vector3(directionX, 0f, directionZ).normalized * windAcceleration);
        }

        if (other.gameObject.tag == "Finish")
        {
            victory.enabled = true;
            resultTime.text = textTimer.text;
            resultTime.enabled = true;
            restartButton.SetActive(true);
            isVictory = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<Renderer>().material = white;
        enemyTimeStart = 0;
    }

    void Start()
    {
        orange = Resources.Load("Orange") as Material;
        white = Resources.Load("White") as Material;
        red = Resources.Load("Red") as Material;
        grey = Resources.Load("Grey") as Material;
        //cube2.GetComponent<Renderer>().material.color = Color.green;

        cam = GameObject.Find("Main Camera");
        victory = GameObject.Find("Victory").GetComponent<Text>();
        resultTime = GameObject.Find("Result Time").GetComponent<Text>();
        defeat = GameObject.Find("Defeat").GetComponent<Text>();
        restartButton = GameObject.Find("Restart Button");
        rb = GameObject.Find("Sphere").GetComponent<Rigidbody>();
        textTimer = GameObject.Find("Time").GetComponent<Text>();
        textSlider = GameObject.Find("Health").GetComponent<Text>();
        slider = GameObject.Find("Slider").GetComponent<Slider>();
        isVictory = false;
        isDefeat = false;
        restartButton.SetActive(false);

        textTimer.text = "Time: " + timeStart.ToString("F2");
        textSlider.text = "Health: 100";
    }

    void FixedUpdate()
    {
        if (!isVictory && !isDefeat)
        {
            if (transform.position.y < 0f || slider.value <= 0f)
            {
                defeat.enabled = true;
                restartButton.SetActive(true);
                isDefeat = true;
            }

            timeStart += Time.deltaTime;
            textTimer.text = "Time: " + timeStart.ToString("F2");

            rest -= Time.deltaTime;
            windRest -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(new Vector3(0f, 0f, direction).normalized * acceleration);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector3(-direction, 0f, 0f).normalized * acceleration);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(new Vector3(0f, 0f, -direction).normalized * acceleration);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(new Vector3(direction, 0f, 0f).normalized * acceleration);
        }
        if (Input.GetKey(KeyCode.Space) && transform.position.y <= 2.5f && transform.position.y >= 1.494f)
        {
            rb.AddForce(new Vector3(0f, direction, 0f).normalized * jump, ForceMode.Impulse);
        }
        //transform.Translate(direction.normalized * speed);

        cam.transform.position = Vector3.Lerp(new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z), new Vector3(transform.position.x, transform.position.y + 11f, transform.position.z - 5f), 1f);
    }
}