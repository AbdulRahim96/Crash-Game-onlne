using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrashSystem : MonoBehaviour
{
    public float minRange, maxRange, currentMultiplier, crashValue, currentTime, currentReward;
    public float numiratorValue = 10000;
    public bool hasStarted = false;
    //public Transform rocket, scaling, horizontalScaling;
    public GameObject rocketModel;
    public ParticleSystem effects, explosion;
    public Text currentMultiplierText, currentRedwardText;
    private float amount, min, max;
    public float probability;
    public Button LeaveButton;


    [Space(10)]
    [Header("Cash Out")]
    public Text cashOutText, cashoutScore;
    public Text crashText, currentCount;
    public static float cashBackPercentage, minimumRange, remoteCrash;


    [Space(10)]
    [Header("History")]
    public int currentSaveIndex;
    public GameObject content, textObject;
    public Color red, green;
    public Sprite gold;
    private void Start()
    {
        float highscore = PlayerPrefs.GetFloat("cashout", 0);
        cashoutScore.text = highscore.ToString("0.00");
    }

    void updatehistory()
    {
        if (PlayerPrefs.HasKey("currentSave"))
        {
            currentSaveIndex = PlayerPrefs.GetInt("currentSave", 0);
            print("current save: " + currentSaveIndex);
            loadRecords();
        }
        else
            currentSaveIndex = 0;
    }
    public void betAmount(string value)
    {
        amount = float.Parse(value);
        //amountText.text = "bet: $" + value.ToString("0");

        probability = 100 / 201;
        probability += Random.Range(0, 0.5f);
        if (probability > 0.7f)
        {
            crashValue = Random.Range(minRange, maxRange * probability);
            if (crashValue > 20)
                crashValue *= Random.Range(0.3f, 1);
        }
        else
        {
            crashValue = Random.Range(minRange, Random.Range(minRange, Random.Range(minRange, maxRange)) * probability);

        }
        crashValue = crashValue < minRange ? Random.Range(minRange, Random.Range(minRange, 3f)) : crashValue;
        currentMultiplier = 1;
        currentTime = 0;
        hasStarted = true;
        effects.Play();
    }

   /* public void SetScale()
    {
        float currentScale = 1;
        float currentSeconds = 0;
        for (int i = 0; i < maxRange + 5; i++)
        {
            GameObject y_axis = Instantiate(textMesh, scaling.position + new Vector3(0, currentScale, 0), scaling.rotation);
            y_axis.GetComponent<TextMesh>().text = "x" + currentScale.ToString("0.0");
            currentScale += 0.5f;
            y_axis.transform.SetParent(scaling);
        }

        for (int i = 0; i < maxRange * 50; i++)
        {
            GameObject X_axis = Instantiate(textMesh, horizontalScaling.position + new Vector3(currentSeconds/5, 0, 0), Quaternion.identity);
            X_axis.GetComponent<TextMesh>().text = currentSeconds.ToString("0") + "s";
            currentSeconds += 10;
            X_axis.transform.SetParent(horizontalScaling);
        }
    }*/

    void LateUpdate()
    {
        if(hasStarted)
        {
            if (currentMultiplier > 1.01f)
                LeaveButton.interactable = true;

            currentMultiplier += Time.deltaTime * Random.Range(min, max);
            if(max > 1)
                max += Time.deltaTime * 0.1f;
            else
                max += Time.deltaTime * 0.01f;

            min = max / 2;

            currentTime += Time.deltaTime * 0.2f;
            currentMultiplierText.text = "x" + currentMultiplier.ToString("0.00");
            rocketModel.transform.position = new Vector3(0, currentMultiplier * currentTime, 0);
            //scaling.position = new Vector3(currentTime + 2.5f, 0, 0);
            //horizontalScaling.position = new Vector3(0, currentMultiplier - 1.1f, 0);

            currentReward = amount * currentMultiplier;
            currentRedwardText.text = "cash out - $" + currentReward.ToString("0.00");
            if(currentMultiplier >= crashValue)
            {
                
                hasStarted = false;
                Instantiate(explosion, rocketModel.transform.position, rocketModel.transform.rotation);
                rocketModel.GetComponent<SpriteRenderer>().enabled = false;
                currentMultiplierText.text = "Crashed at\n" + "x" + currentMultiplier.ToString("0.00");
                currentMultiplierText.color = Color.red;

                //AddtoHistory();
                return;
            }
        }
        else
            LeaveButton.interactable = hasStarted;

    }

    public void report()
    {
        hasStarted = false;
        cashOutText.text = "$" + (amount * currentMultiplier).ToString("0.00");
        currentCount.text = "x" + currentMultiplier.ToString("0.00");
        crashText.text = "x" + crashValue.ToString("0.00");

        AddtoHistory();
        if (currentMultiplier > PlayerPrefs.GetFloat("cashout"))
            PlayerPrefs.SetFloat("cashout", currentMultiplier);

        if (currentMultiplier >= remoteCrash)
            PlayerPrefs.SetInt("topmultiplier", 0);
    }

    public void AddtoHistory()
    {
        PlayerPrefs.SetFloat("record" + currentSaveIndex, crashValue);
        currentSaveIndex++;
        PlayerPrefs.SetInt("currentSave", currentSaveIndex);
    }

    public void Cashback()
    {

        float total = amount * (cashBackPercentage / 100);
        
        currentMultiplierText.text = total.ToString("0") + " Cashback";
        currentMultiplierText.color = Color.green;
    }


    public void CrashValue(float probability)
    {
        
        crashValue = Random.Range(minRange, maxRange * probability * probability);
        crashValue = crashValue < 1.2 ? Random.Range(1.2f, 3) : crashValue;
        /*  for (int i = 0; i < 2; i++)
          {
              print("Value: " + i + ": " + crashValue);
              crashValue = Random.Range(minRange, crashValue * probability);
          }*/
    }

    public void AddRecord(float value)
    {
        GameObject obj = Instantiate(textObject);
        obj.transform.SetParent(content.transform);
        obj.GetComponentInChildren<Text>().text = value.ToString("0.00");
        obj.transform.localScale = new Vector3(1, 1, 1);
        if (value >= 1 && value < 1.5f)
        {
            // red
            obj.GetComponent<Image>().color = red;
            obj.GetComponentInChildren<Text>().color = Color.red;
        }
        else if (value >= 1.5 && value < 10)
        {
            // green
            obj.GetComponent<Image>().color = green;
            obj.GetComponentInChildren<Text>().color = Color.green;
        }
        else
        {
            // gold
            obj.GetComponent<Image>().sprite = gold;
            obj.GetComponentInChildren<Text>().color = Color.black;
        }

    }

    public void loadRecords()
    {
        if(currentSaveIndex > 10)
        {
            for (int i = currentSaveIndex - 1; i >= currentSaveIndex - 10; i--)
            {
                float value = PlayerPrefs.GetFloat("record" + i);
                print("Load: " + i + " - " + value);
                AddRecord(value);
            }
        }
        else
        {
            for (int i = currentSaveIndex - 1; i >= 0; i--)
            {
                float value = PlayerPrefs.GetFloat("record" + i);
                print("Load: " + i + " - " +value);
                AddRecord(value);
            }
        }
        
    }
}
