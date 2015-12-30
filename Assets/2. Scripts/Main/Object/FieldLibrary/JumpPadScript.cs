using UnityEngine;
using System.Collections;

public class JumpPadScript : MonoBehaviour {
    public float jumpPower = 7.0f;
    public bool onlyPlayer = true;

    GameObject normal;
    GameObject pressed;

    GameObject target;

    MeshRenderer bodyShader;
    MeshRenderer normalShader;

    public AudioClip pressJumpPadSound;
    public AudioClip upJumpPadSound;

	void Start () {
        normal = transform.FindChild("normal").gameObject;
        pressed = transform.FindChild("pressed").gameObject;

        normal.SetActive(true);
        pressed.SetActive(false);

        normalShader = normal.GetComponent<MeshRenderer>();
        ReturnShader();
	}

    void OnTriggerEnter(Collider collider)
    {
        if (onlyPlayer)
        {

            if (collider.CompareTag("Player") || collider.CompareTag("GROUND"))
            {
                pressPad(collider);
            }
        }
        else
        {
            pressPad(collider);
        }
    }
    void OnTriggerExit(Collider collider)
    {

        if (onlyPlayer)
        {
            if (collider.CompareTag("Player") || collider.CompareTag("GROUND"))
            {
                upPad(collider);
            }
        }
        else
        {
            upPad(collider);
        }
    }

    void pressPad(Collider collider)
    {
        normal.SetActive(false);
        pressed.SetActive(true);
        target = collider.gameObject;
        Invoke("JumpTarget", 0.3f);
        NewSoundMgr.instance.PlaySingle(pressJumpPadSound);
    }
    void upPad(Collider collider)
    {
        normal.SetActive(true);
        pressed.SetActive(false);
        CancelInvoke("JumpTarget");

        target = null;
    }
   
    void JumpTarget()
    {
        if (target != null)
        {
            target.GetComponent<Rigidbody>().AddForce(new Vector3(0, jumpPower, 0),ForceMode.VelocityChange);
            NewSoundMgr.instance.PlaySingle(upJumpPadSound);
            normalShader.material.shader = Shader.Find("Mobile/Particles/Additive");
            Invoke("ReturnShader", jumpPower*0.1f);
            //target.GetComponent<LampInfo>().setJumpState(true);
            if (target.name == "Lamp")
            {
                target.GetComponent<LampInfo>().setJumpState(true);
                Debug.Log("qweqwewww");
            }
        }
    }

    void ReturnShader()
    {
        normalShader.material.shader = Shader.Find("Specular");
    }

}
