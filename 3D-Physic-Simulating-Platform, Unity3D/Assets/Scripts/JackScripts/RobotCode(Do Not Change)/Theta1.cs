using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Theta1 : MonoBehaviour {

	float theta;
	public Transform RobotBase;
	//private Text theta1Text;
	private float x;


    public Slider sliderTheta0;//base rotation
    public Slider sliderTheta1;//lower rotation
    public Slider sliderTheta2;//upper rotation

    public Text angle0Text;
    public Text angle1Text;
    public Text angle2Text;
    //Show each motors angle
	
	//sets and displays theta1 each frame
	void Update () {

        angle0Text.text = sliderTheta0.value.ToString("0.#");
        angle1Text.text = sliderTheta1.value.ToString("0.#");
        angle2Text.text = sliderTheta2.value.ToString("0.#");

        theta = RobotBase.eulerAngles.y;

		//sets and displays theta
		if(theta == 0f)
		{
			x = 0f;
		}
		else
		{
			x = 360f;
		}

		DHParameters.setTheta1(x-theta);
		//theta1Text.text = ""+(x-theta);
	}
}
