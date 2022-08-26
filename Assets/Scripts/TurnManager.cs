using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public TeamManager teamOne;
    public TeamManager teamTwo;

    private int teamPlaying;

    void Awake()
    {
        teamPlaying = 1;
    }

    public void EndTurn()
    {
        GridHUDDisplayer.Instance.ClearMovementTilesInRangeDisplayed();
        GridHUDDisplayer.Instance.ClearAttackTilesInRangeDisplayed();
        GridHUDDisplayer.Instance.ClearPath();
        if (teamPlaying == 1)
        {
            teamTwo.RefreshCharacters();
            teamTwo.MakeAllCharacterActive();
            teamOne.MakeAllCharacterUnactive();
            teamPlaying = 2;
        }
        else
        {
            teamOne.RefreshCharacters();
            teamOne.MakeAllCharacterActive();
            teamTwo.MakeAllCharacterUnactive();
            teamPlaying = 1;
        }
        teamOne.ChangeIsFromCurrentPlayingTeamFromCharacters();
        teamTwo.ChangeIsFromCurrentPlayingTeamFromCharacters();
    }
}