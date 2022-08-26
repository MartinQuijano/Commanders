using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class MouseController : MonoBehaviour
{
    private OverlayTile lastSelectedTile;

    public CharacterInfo selectedCharacter;
    private CharacterInfo clickedCharacter;

    private OverlayTile hoveredOverlayTile;

    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    void Update()
    {
        var focusedTileHit = GetFocusedOnTile();
        if (focusedTileHit != null)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            hoveredOverlayTile = focusedTileHit[0].collider.gameObject.GetComponent<OverlayTile>();
            transform.position = hoveredOverlayTile.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = hoveredOverlayTile.GetComponent<SpriteRenderer>().sortingOrder;

            if (Input.GetMouseButtonDown(0))
            {
                if (focusedTileHit.Length == 2)
                {
                    clickedCharacter = focusedTileHit[1].collider.gameObject.GetComponent<CharacterInfo>();
                    if (selectedCharacter != null && clickedCharacter.isFromCurrentPlayingTeam)
                    {
                        GridHUDDisplayer.Instance.ClearMovementTilesInRangeDisplayed();
                        GridHUDDisplayer.Instance.ClearAttackTilesInRangeDisplayed();
                        GridHUDDisplayer.Instance.ClearPath();
                    }

                    if (clickedCharacter.isFromCurrentPlayingTeam)
                    {
                        if (selectedCharacter != null)
                            selectedCharacter.selected = false;
                        selectedCharacter = clickedCharacter;
                        selectedCharacter.OnSelected();
                    }
                    else
                    {
                        if (GridHUDDisplayer.Instance.inRangeOfAttackTiles.Contains(hoveredOverlayTile))
                            selectedCharacter.AttackEnemy(clickedCharacter);
                    }
                }
                else if (focusedTileHit.Length < 2 && selectedCharacter != null)
                {
                    if (GridHUDDisplayer.Instance.path.Contains(hoveredOverlayTile))
                        if (GridHUDDisplayer.Instance.path.Count > 0)
                        {
                            selectedCharacter.MoveAlongPath(GridHUDDisplayer.Instance.path);
                        }
                }
            }
            GridHUDDisplayer.Instance.DisplayArrowPath(selectedCharacter, hoveredOverlayTile);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        }
    }

    public RaycastHit2D[] GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);
        if (hits.Length > 0)
        {
            return hits.Reverse().ToArray();
        }
        return null;
    }
}