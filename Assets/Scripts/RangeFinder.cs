using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RangeFinder
{
    public List<OverlayTile> GetTilesInMovementRange(OverlayTile startingTile, int range)
    {
        var inRangeTiles = new List<OverlayTile>();

        inRangeTiles.Add(startingTile);

        var tileForPreviousStep = new List<OverlayTile>();
        tileForPreviousStep.Add(startingTile);
        while (tileForPreviousStep.Count > 0)
        {
            var surroundingTiles = new List<OverlayTile>();

            foreach (var item in tileForPreviousStep)
            {
                foreach (var tile in MapManager.Instance.GetNeighbourTiles(item, new List<OverlayTile>()))
                {
                    if (!tile.tileData.isTerrainBlocked)
                    {
                        if (tile.characterOnTile != null && !tile.characterOnTile.isFromCurrentPlayingTeam) { }
                        else
                        {
                            tile.costToMoveToThisTile = tile.tileData.terrainCost + item.costToMoveToThisTile;

                            if (range > tile.costToMoveToThisTile)
                            {
                                surroundingTiles.Add(tile);
                            }
                            else if (range == tile.costToMoveToThisTile && tile.characterOnTile == null)
                            {
                                surroundingTiles.Add(tile);
                            }
                        }
                    }
                }
            }

            inRangeTiles.AddRange(surroundingTiles);
            tileForPreviousStep = surroundingTiles.Distinct().ToList();
        }
        foreach (OverlayTile tile in MapManager.Instance.map.Values)
        {
            tile.costToMoveToThisTile = 0;
        }

        inRangeTiles = inRangeTiles.Distinct().ToList();
        return inRangeTiles;
    }

    public List<OverlayTile> GetTilesInAttackRange(OverlayTile startingTile, int range)
    {
        var inRangeTiles = new List<OverlayTile>();

        inRangeTiles.Add(startingTile);

        var tileForPreviousStep = new List<OverlayTile>();
        tileForPreviousStep.Add(startingTile);
        while (range > 0)
        {
            var surroundingTiles = new List<OverlayTile>();

            foreach (var item in tileForPreviousStep)
            {
                foreach (var tile in MapManager.Instance.GetNeighbourTiles(item, new List<OverlayTile>()))
                {
                    surroundingTiles.Add(tile);
                }
            }
            range--;
            inRangeTiles.AddRange(surroundingTiles);
            tileForPreviousStep = surroundingTiles.Distinct().ToList();
        }

        inRangeTiles.Remove(startingTile);
        inRangeTiles = inRangeTiles.Distinct().ToList();
        return inRangeTiles;
    }
}