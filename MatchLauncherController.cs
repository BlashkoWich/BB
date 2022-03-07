using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BB.ScriptableObjects;
using BB.Core;


namespace BB.Gameplay.Contollers
{
    public class MatchLauncherController : MonoBehaviour
    {
        public event Action<Dictionary<Team, List<Character>>> CharactersLaunched;

        //[SerializeField]
        //private MatchController _matchController;

        [SerializeField]
        private List<Transform> _redTeamBasePlaces = new List<Transform>();

        [SerializeField]
        private List<Transform> _blueTeamBasePlaces = new List<Transform>();
        private Dictionary<Team, List<Transform>> _teamsBasePlaces = new Dictionary<Team, List<Transform>>();

        private Dictionary<Team, List<Character>> _teamsCharacters = new Dictionary<Team, List<Character>>();

        public void LaunchMatchDebug(Dictionary<Team, List<Character>> teamsCharacters)
        {
            _teamsCharacters = new Dictionary<Team, List<Character>>(teamsCharacters);

            _teamsBasePlaces.Add(Team.Red, _redTeamBasePlaces);
            _teamsBasePlaces.Add(Team.Blue, _blueTeamBasePlaces);

            foreach (var teamCharacters in _teamsCharacters)
            {
                for (int i = 0; i < teamCharacters.Value.Count; i++)
                {
                    var character = teamCharacters.Value[i];
                    character.transform
                        .SetPositionAndRotation(
                            _teamsBasePlaces[teamCharacters.Key][i].transform.position,
                            _teamsBasePlaces[teamCharacters.Key][i].transform.rotation
                            );
                }
            }

            LaunchCharacters();
        }

        public void LaunchCharacters()
        {
            CharactersLaunched?.Invoke(_teamsCharacters);
        }

        public void OnIntroFinished()
        {
            //_matchController.LaunchMatch(_teamsCharacters);
        }
    }
}