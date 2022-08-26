using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterInfo : MonoBehaviour
{
    public OverlayTile standingOnTile;
    public Sprite[] sprites;
    public SpriteRenderer spriteRenderer;
    public bool isFromCurrentPlayingTeam;

    public float speed;

    public int max_health;
    public int currentHealth;
    public int damage;

    public bool selected;
    public bool isMoving;

    public bool moved;
    public bool attacked;

    public int movementRange;
    public int attackRange;

    public List<OverlayTile> currentMovingPath;
    public TeamManager myTeam;

    public TextMeshPro healthpoints;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthpoints = GetComponentInChildren<TextMeshPro>();
        selected = false;
        isMoving = false;
    }

    void Start()
    {
        moved = false;
        attacked = false;
        max_health = 10;
        damage = 2;
        currentHealth = max_health;
        healthpoints.text = currentHealth.ToString();
    }

    void Update()
    {
        if (selected)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!moved)
                {
                    moved = true;
                    GridHUDDisplayer.Instance.ClearMovementTilesInRangeDisplayed();
                    GridHUDDisplayer.Instance.DisplayAttackRange(standingOnTile, attackRange);
                }
                else if (!attacked)
                {
                    attacked = true;
                    ChangeSpriteToUnactive();
                    GridHUDDisplayer.Instance.ClearAttackTilesInRangeDisplayed();
                }
            }
        if (currentMovingPath.Count > 0)
            MoveAlongPath(currentMovingPath);
        if (currentHealth <= 0)
        {
            myTeam.RemoveCharacter(this);
            Destroy(this.gameObject);
        }
    }

    public void PositionCharacterOnTile(OverlayTile tile)
    {
        transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, 0);
        standingOnTile = tile;
    }

    public void SwapIsFromCurrentPlayingTeam()
    {
        isFromCurrentPlayingTeam = !isFromCurrentPlayingTeam;
    }

    public void ChangeSpriteToActive()
    {
        spriteRenderer.sprite = sprites[0];
    }

    public void ChangeSpriteToUnactive()
    {
        spriteRenderer.sprite = sprites[1];
    }

    public void MoveAlongPath(List<OverlayTile> path)
    {
        isMoving = true;
        moved = true;
        var step = speed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, path[0].transform.position, step);

        if (Vector2.Distance(transform.position, path[0].transform.position) < 0.0001f)
        {
            if (path.Count == 1)
            {
                standingOnTile.characterOnTile = null;
                PositionCharacterOnTile(path[0]);
                path[0].SetCharacterStandingOnTile(this);
            }
            path.RemoveAt(0);
        }

        if (path.Count == 0)
        {
            GridHUDDisplayer.Instance.ClearMovementTilesInRangeDisplayed();
            isMoving = false;

            GridHUDDisplayer.Instance.DisplayAttackRange(standingOnTile, attackRange);
        }

        currentMovingPath = path;
    }

    public void OnSelected()
    {
        selected = true;
        if (!moved)
        {
            GridHUDDisplayer.Instance.DisplayMovementRange(standingOnTile, movementRange);
        }
        else if (!attacked)
        {
            GridHUDDisplayer.Instance.DisplayAttackRange(standingOnTile, attackRange);
        }
    }

    public void AttackEnemy(CharacterInfo enemy)
    {
        enemy.TakeDamage(damage);
        attacked = true;
        ChangeSpriteToUnactive();
        GridHUDDisplayer.Instance.ClearAttackTilesInRangeDisplayed();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthpoints.text = currentHealth.ToString();
    }

    public void SetMyTeam(TeamManager team)
    {
        myTeam = team;
    }
}