﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    Vector2 dPosition; // Delta position. It changes de position of the sprites direction

    public BodyPart following = null;

    private bool isTail = false;

    private SpriteRenderer spriteRenderer = null;

    const int PARTSREMEMBERED = 10;
    public Vector3[] previousPositions = new Vector3[PARTSREMEMBERED];

    public int setIndex = 0;
    public int getIndex = -(PARTSREMEMBERED);

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    virtual public void Update()
    {
        Vector3 followPosition;
        if (following != null)
        {
            if (following.getIndex > -1)
                followPosition = following.previousPositions[following.getIndex];
            else
                followPosition = following.transform.position;
        }
        else
            followPosition = gameObject.transform.position;

        previousPositions[setIndex].x = gameObject.transform.position.x;
        previousPositions[setIndex].y = gameObject.transform.position.y;
        previousPositions[setIndex].z = gameObject.transform.position.z;

        setIndex++;
        if (setIndex >= PARTSREMEMBERED) setIndex = 0;
        getIndex++;
        if (getIndex >= PARTSREMEMBERED) getIndex = 0;


        if (following != null) // ex.: Não é a cabeça (i.e not the head)
        {
            Vector3 newPosition;
            if (following.getIndex > -1)
            {
                newPosition = followPosition;
            }
            else
            {
                newPosition = following.transform.position;
            }

            newPosition.z = newPosition.z + 0.01f;

            SetMovement(newPosition - gameObject.transform.position);
            UpdateDirection();
            UpdatePosition();
        }
    }

    public void SetMovement(Vector2 movement)
    {
        dPosition = movement;
    }

    public void UpdatePosition()
    {
        gameObject.transform.position += (Vector3)dPosition;
    }

    public void UpdateDirection()
    {
        if (dPosition.y > 0) // up
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        else if (dPosition.y < 0) // down
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);
        else if (dPosition.x < 0) // left
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
        else if (dPosition.x > 0) // right
            gameObject.transform.localEulerAngles = new Vector3(0, 0, -90);
    }

    public void TurnIntoTail()
    {
        isTail = true;
        spriteRenderer.sprite = GameController.instance.tailSprite;
    }

    public void TurnIntoBodyPart()
    {
        isTail = false;
        spriteRenderer.sprite = GameController.instance.bodySprite;
    }
}
