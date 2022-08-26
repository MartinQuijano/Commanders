using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TeamManager teamOne;
    public TeamManager teamTwo;

    public GameObject characterFighterTeamOnePrefab;
    public GameObject characterArcherTeamOnePrefab;

    public GameObject characterFighterTeamTwoPrefab;
    public GameObject characterArcherTeamTwoPrefab;

    void Start()
    {
        //team 1
        CharacterInfo characterInfo = Instantiate(characterFighterTeamOnePrefab).GetComponent<CharacterInfo>();
        characterInfo.PositionCharacterOnTile(MapManager.Instance.map[new Vector2Int(-6, 0)]);
        characterInfo.isFromCurrentPlayingTeam = true;
        MapManager.Instance.map[new Vector2Int(-6, 0)].characterOnTile = characterInfo;
        teamOne.AddCharacter(characterInfo);

        characterInfo = Instantiate(characterFighterTeamOnePrefab).GetComponent<CharacterInfo>();
        characterInfo.PositionCharacterOnTile(MapManager.Instance.map[new Vector2Int(-5, -1)]);
        characterInfo.isFromCurrentPlayingTeam = true;
        MapManager.Instance.map[new Vector2Int(-5, -1)].characterOnTile = characterInfo;
        teamOne.AddCharacter(characterInfo);

        characterInfo = Instantiate(characterArcherTeamOnePrefab).GetComponent<CharacterInfo>();
        characterInfo.PositionCharacterOnTile(MapManager.Instance.map[new Vector2Int(-5, -3)]);
        characterInfo.isFromCurrentPlayingTeam = true;
        MapManager.Instance.map[new Vector2Int(-5, -3)].characterOnTile = characterInfo;
        teamOne.AddCharacter(characterInfo);

        //team2
        characterInfo = Instantiate(characterFighterTeamTwoPrefab).GetComponent<CharacterInfo>();
        characterInfo.PositionCharacterOnTile(MapManager.Instance.map[new Vector2Int(4, 1)]);
        characterInfo.isFromCurrentPlayingTeam = false;
        characterInfo.ChangeSpriteToUnactive();
        MapManager.Instance.map[new Vector2Int(4, 1)].characterOnTile = characterInfo;
        teamTwo.AddCharacter(characterInfo);

        characterInfo = Instantiate(characterFighterTeamTwoPrefab).GetComponent<CharacterInfo>();
        characterInfo.PositionCharacterOnTile(MapManager.Instance.map[new Vector2Int(3, -1)]);
        characterInfo.isFromCurrentPlayingTeam = false;
        characterInfo.ChangeSpriteToUnactive();
        MapManager.Instance.map[new Vector2Int(3, -1)].characterOnTile = characterInfo;
        teamTwo.AddCharacter(characterInfo);

        characterInfo = Instantiate(characterArcherTeamTwoPrefab).GetComponent<CharacterInfo>();
        characterInfo.PositionCharacterOnTile(MapManager.Instance.map[new Vector2Int(3, -3)]);
        characterInfo.isFromCurrentPlayingTeam = false;
        characterInfo.ChangeSpriteToUnactive();
        MapManager.Instance.map[new Vector2Int(3, -3)].characterOnTile = characterInfo;
        teamTwo.AddCharacter(characterInfo);
    }
}