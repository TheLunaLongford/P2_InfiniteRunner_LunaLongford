using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
public class player_movement : MonoBehaviour
{
    [SerializeField] float move_vector;
    [SerializeField] float move_speed;

    Rigidbody2D rigib_body;
    [SerializeField] float jump_speed;
    [SerializeField] float gravity;

    [SerializeField] private LayerMask ground;
    [SerializeField] private bool is_on_ground;

    private Animator animator;
    private SpriteRenderer sprite_renderer;

    private AudioSource player_sound;

    void OnMove(InputValue value)
    {
        move_vector = value.Get<float>();
    }

    void OnJump()
    {
        if (is_on_ground)
        {
            player_sound.Play();
            rigib_body.AddForce(Vector2.up * jump_speed, ForceMode2D.Impulse);
        }
    }

    void Update()
    {
        Debug.DrawRay(this.transform.position, Vector2.down * 2.0f, Color.red);
        Debug.DrawRay(this.transform.position + (new Vector3(1f,0,0)), Vector2.down * 2.0f, Color.red);
        Debug.DrawRay(this.transform.position + (new Vector3(-1f, 0, 0)), Vector2.down * 2.0f, Color.red);
        is_character_on_floor();
        transform.Translate(new Vector3(move_vector, 0, 0) * move_speed * Time.deltaTime);
        if (move_vector == 0.0f)
        {
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isRunning", true);
            if (move_vector > 0)
            {
                sprite_renderer.flipX = true;
            }
            else
            {
                sprite_renderer.flipX = false;
            }

        }
    }

    void is_character_on_floor()
    {
        if (
            Physics2D.Raycast(this.transform.position, Vector2.down, 2.0f, ground)
            || (Physics2D.Raycast(this.transform.position + (new Vector3(1f, 0, 0)), Vector2.down, 2.0f, ground))
            || (Physics2D.Raycast(this.transform.position + (new Vector3(-1f, 0, 0)), Vector2.down, 2.0f, ground))
           )
        {
            is_on_ground = true;
            animator.SetBool("onAir", false);
        }
        else
        {
            is_on_ground = false;
            animator.SetBool("onAir", true);
        }
    }

    private void Awake()
    {
        rigib_body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        rigib_body.gravityScale = 4;
        gravity = 4;
        move_speed = 10;
        jump_speed = 20;
        player_sound = GetComponent<AudioSource>();
    }
}
