using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obj_movement : MonoBehaviour
{
    [SerializeField] GameObject objeto;
    [SerializeField] GameObject punto_inicial;
    [SerializeField] GameObject punto_final;
    public GameObject player;
    private Collider2D player_collider;

    private Rigidbody2D rb2d;
    private Rigidbody2D player_rb2d;
    public float move_speed;
    [SerializeField] bool touching_me;
    [SerializeField] bool trigger_me;
    [SerializeField] bool touching_me_by_side;
    public bool moving;
    public string que_soy;

    [SerializeField] private LayerMask player_mask;
    private AudioSource rupee_sound;

    public bool game_logic_bool;
    public game_logic game_logic;
    public int valor;
    public bool available; 

    void Start()
    {
        available = true;
        game_logic = FindObjectOfType<game_logic>();
        game_logic_bool = false;
        rupee_sound = GetComponent<AudioSource>();
        player_collider = player.GetComponent<Collider2D>(); 
        touching_me = false;
        moving = false;
        move_speed = 5.0f;
        rb2d = GetComponent<Rigidbody2D>();
        transform.position = punto_inicial.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        game_logic_bool = game_logic.running;
        if (moving & game_logic_bool)
        {
            available = true;
            Debug.DrawRay(this.transform.position, Vector2.left * 2.0f, Color.red);
            is_touching_by_side();
            if (transform.position.x > punto_final.transform.position.x)
            {
                transform.Translate(new Vector3(-1, 0, 0) * move_speed * Time.deltaTime);
            }
            else
            {
                regresar_inicio();
            }
        }
        if (trigger_me)
        {
            // Aqui Recolectamos X cosa
            Debug.Log("Ahora si perro!");
            aumentar_contador();
            available = false;
            regresar_inicio();
            sonido_recolectable();
            
        }
        if (touching_me)
        {
            switch (que_soy)
            {
                case "enemigo":
                    Debug.Log("Te moriste prro");
                    reiniciando_por_muerte();
                    break;
            }
        }

        if (touching_me_by_side)
        {
            switch (que_soy)
            {
                case "bloque":
                    player.GetComponent<player_movement>().on_pushing = true;
                    break;
            };
        }
        else
        {
            switch (que_soy)
            {
                case "bloque":
                    player.GetComponent<player_movement>().on_pushing = false;
                    break;
            };
        }
        
    }

    public void aumentar_contador()
    {
        if (available)
        {
            switch (que_soy)
            {
                case "rupee":
                    game_logic.score_partida += valor;
                    PlayerPrefs.SetInt("score_partida", game_logic.score_partida);
                    game_logic.score.text = game_logic.score_partida.ToString();
                    break;
            };
        }
        
    }


    public void reiniciando_por_muerte()
    {
        game_logic.running = false;
        game_logic.is_link_dead = true;
        game_logic.turn_on_dead_screen();
        //player.GetComponent<player_movement>().die = true;
        regresar_inicio();
    }

    private void sonido_recolectable()
    {
        switch (que_soy)
        {
            case "rupee":
                rupee_sound.Play();
                break;
        }
    }

    public void regresar_inicio()
    {
        transform.position = punto_inicial.transform.position;
        moving = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Aqui es cuando recien estas tocando el objeto
        touching_me = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Aqui es cuando ya no estas tocando al objeto
        touching_me = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        trigger_me = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        trigger_me = false;
    }

    void is_touching_by_side()
    {
        if (Physics2D.Raycast(this.transform.position, Vector2.left, 2.0f, player_mask))
        {
            touching_me_by_side = true;
        }
        else
        {
            touching_me_by_side = false;
        }
    }
}
