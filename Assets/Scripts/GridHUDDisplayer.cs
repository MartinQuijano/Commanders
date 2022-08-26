using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ArrowTranslator;

public class GridHUDDisplayer : MonoBehaviour
{
    private static GridHUDDisplayer _instance;
    public static GridHUDDisplayer Instance { get { return _instance; } }

    private List<OverlayTile> inRangeOfMovementTiles = new List<OverlayTile>();
    public List<OverlayTile> inRangeOfAttackTiles = new List<OverlayTile>();
    public List<OverlayTile> path = new List<OverlayTile>();

    private RangeFinder rangeFinder;
    private PathFinder pathFinder;
    private ArrowTranslator arrowTranslator;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        rangeFinder = new RangeFinder();
        pathFinder = new PathFinder();
        arrowTranslator = new ArrowTranslator();
    }

    public void DisplayMovementRange(OverlayTile startingTile, int rangeOfMovement)
    {
        foreach (var item in inRangeOfMovementTiles)
        {
            item.HideTile();
        }

        inRangeOfMovementTiles = rangeFinder.GetTilesInMovementRange(startingTile, rangeOfMovement);

        foreach (var item in inRangeOfMovementTiles)
        {
            if (item.characterOnTile == null)
            {
                item.ShowTile();
            }
        }
    }

    public void DisplayAttackRange(OverlayTile startingTile, int rangeOfAttack)
    {
        foreach (var item in inRangeOfAttackTiles)
        {
            item.HideTile();
        }

        inRangeOfAttackTiles = rangeFinder.GetTilesInAttackRange(startingTile, rangeOfAttack);

        foreach (var item in inRangeOfAttackTiles)
        {
            item.ShowRedTile();
        }
    }

    public void DisplayArrowPath(CharacterInfo selectedCharacter, OverlayTile hoveredOverlayTile)
    {
        if (selectedCharacter != null && hoveredOverlayTile.characterOnTile == null && inRangeOfMovementTiles.Contains(hoveredOverlayTile))
        {
            if (inRangeOfMovementTiles.Contains(hoveredOverlayTile) && !selectedCharacter.isMoving)
            {
                path = pathFinder.FindPath(selectedCharacter.standingOnTile, hoveredOverlayTile, inRangeOfMovementTiles);

                foreach (var item in inRangeOfMovementTiles)
                {
                    item.SetArrowSprite(ArrowDirection.None);
                }

                for (int i = 0; i < path.Count; i++)
                {
                    var previousTile = i > 0 ? path[i - 1] : selectedCharacter.standingOnTile;
                    var futureTile = i < path.Count - 1 ? path[i + 1] : null;

                    var arrowDir = arrowTranslator.TranslateDirection(previousTile, path[i], futureTile);
                    path[i].SetArrowSprite(arrowDir);
                }
            }
        }
        else
        {
            foreach (var item in inRangeOfMovementTiles)
            {
                item.SetArrowSprite(ArrowDirection.None);
            }
        }
    }

    public void ClearMovementTilesInRangeDisplayed()
    {
        foreach (var item in inRangeOfMovementTiles)
        {
            item.HideTile();
        }
        inRangeOfMovementTiles.Clear();
    }

    public void ClearAttackTilesInRangeDisplayed()
    {
        foreach (var item in inRangeOfAttackTiles)
        {
            item.HideTile();
        }
        inRangeOfAttackTiles.Clear();
    }

    public void ClearPath()
    {
        path.Clear();
    }
}
