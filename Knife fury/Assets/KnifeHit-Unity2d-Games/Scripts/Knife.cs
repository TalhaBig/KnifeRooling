using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Knife : MonoBehaviour
{
    public float speed = 20f;
    public AudioClip hitSound;
    public AudioClip fail;
    private Rigidbody2D knifeRigid;
    private bool moving = false;
    private GameObject spawn;
    public GameObject particle;
    public bool scriptEnabled = true;

    void Start()
    {
        knifeRigid = GetComponent<Rigidbody2D>();
        spawn = GameObject.Find("Spawn");
    }

    void FixedUpdate()
    {
        if (moving)
            knifeRigid.MovePosition(knifeRigid.position + Vector2.up * speed * Time.deltaTime);

        if (Input.GetMouseButton(0))
            moving = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (scriptEnabled)
        {
            if (collider.name == "Trunk")
            {
                moving = false;
                transform.parent = collider.transform;
                GetComponent<PolygonCollider2D>().enabled = false;
                knifeRigid.bodyType = RigidbodyType2D.Kinematic;
                transform.GetChild(0).GetComponent<PolygonCollider2D>().enabled = true;
                GetComponent<AudioSource>().PlayOneShot(hitSound);
                spawn.GetComponent<SpawnController>().CreateKnife();
                collider.GetComponent<Animator>().SetTrigger("Hit");
                collider.GetComponent<TrunkController>().Damage(1);

                Instantiate(particle, transform.position + transform.up * 0.25f, Quaternion.identity);

                GameController.SetScore(10);
            }
            else
            {
                // If hit another knife
                moving = false;
                knifeRigid.bodyType = RigidbodyType2D.Dynamic;
                GetComponent<AudioSource>().PlayOneShot(fail);
                GameObject.Find("TextMessage").GetComponent<Text>().text = "GAME OVER";
                GameController.SaveHighScore();
                GameController.ResetScore();
                StartCoroutine(HandleGameOver());
            }

            scriptEnabled = false;
            GetComponent<Knife>().enabled = false;
        }
    }

    private IEnumerator HandleGameOver()
    {
        // Wait for 2 seconds before restarting the game
        yield return new WaitForSeconds(2f);
        GameController.ResetGame(); // Use this method to handle game restart
    }
}