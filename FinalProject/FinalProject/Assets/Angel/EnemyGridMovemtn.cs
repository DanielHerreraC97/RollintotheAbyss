using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyGridMovemtn : MonoBehaviour
{

    [SerializeField] private Tilemap floor;
    [SerializeField] private Tilemap walls;

    [SerializeField] private string enemyDiceTag;

    private int movementDirection


    //dice parameters 
    public int recall;
    private Dice _dice;

    void Start()
    {
        _dice = GameObject.Find("dice").GetComponent<Dice>();
    }

    private void Update()
    {
        recall = _dice.finalSide;
    }

    public void ChooseDirectionBeforeToMove()
    {
        
    }

    private void Move(Vector2 direction)
    {
        if (CanMove(direction) && recall > 0)
        {
            transform.position += (Vector3)direction;
            _dice.finalSide -= 1;
            if (_dice.finalSide == 0)
            {
                _dice.StartCoroutine("RollTheDice");
            }
        }
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = floor.WorldToCell(transform.position + (Vector3)direction);
        if (!floor.HasTile(gridPosition) || walls.HasTile(gridPosition))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
