using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePlayerHealth : MonoBehaviour {

    [SerializeField]
	public Image healthbar;
    [SerializeField]
	private bool losingHealth;
    [SerializeField]
	private float waitTime = 30.0f;
    [SerializeField]
    private Text myText;
	
	// Update is called once per frame
	// Use this to simulate losing health over time.
	void Update()
	{
		if (losingHealth == true)
		{
			//Reduce fill amount over 30 seconds
			healthbar.fillAmount -= 1.0f / waitTime * Time.deltaTime;
		}
	}

    // Update the fill value of a given image and textbox
    public void updateHealthValue(int newValue)
    {
        if(newValue <= 100 && newValue >= 0)
        {
            if(healthbar)
            {
                healthbar.fillAmount = newValue/100f;
            }
            if(myText)
            {
                myText.text = newValue.ToString() + "%";
            }
        }
       
    }
	// Lose health on hit.
}