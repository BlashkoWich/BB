using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BB.Buffs;
using BB.Effects;
using BB.Moving;
using BB.ScriptableObjects;
using BB.Weapon;
using CharacterStateMachine;
using CharacterVisual.CharacterSupport;
using UnityEngine;
using DamageReceiving;
using Mirror;


namespace BB.Core
{
    public class Character : NetworkBehaviour, IConcealerObserver, ITargetable, IBuffAgregator, IHealthObserver
    {
        public event Action<Character, Team> Dead;
        public event Action<Character, bool> VisibleChanged;
        public event Action<CharacterState> StateEntered;
        public event Action<CharacterState> StateExited;


        public event Action CharacterStopped;
        private string _name;


        #region SerializeFields
        [SerializeField]
        private BB.ScriptableObjects.CharacterSpecification _characterSO;

        [SerializeField]
        private Rigidbody _rigidBody;

        [SerializeField]
        private GameObject _model;

        public GameObject ModelForFinishPanel;

        [SerializeField]
        private Mover _mover;

        [SerializeField]
        private BB.Weapon.Weapon _weapon;

        [SerializeField]
        private BB.Weapon.Weapon _ultimateWeapon;



        [SerializeField]
        private DamageReceiving.DamageReceiver _damageReceiver;

        [SerializeField]
        private CrystallReceiver _crystallReceiver;

        [SerializeField]
        private DamageReceiving.HealthUI _healthUI;

        [SerializeField]
        private CharacterVisual.VisualHider _characterHider;


        [SerializeField]
        private CharacterVisual.CharacterSupport.CharacterConcealer _characterConcealer;

        [SerializeField]
        private HitPointsRefiller _hitPointsRefiller;
        #endregion

        //-----------------
        #region PrivateFields
        private DamageReceiving.HealthStorage _healthStorage;
        private DamageReceiving.HealthController _healthStorageController;
        [SerializeField]
        private Team _team;
        #endregion

        #region Buffs

        private List<Buff> _currentBuffs = new List<Buff>();
        private List<IBuffReactor> _buffReactors = new List<IBuffReactor>();

        #endregion

        public bool IsVisible { get; private set; } = true;


        public CharacterSpecification CharacterSO => _characterSO;

        private bool _characterStopped = false;


        #region interface
        public Vector3 CurrentPosition { get => transform.position; }
        public Team Team { get => _team; }

        #endregion
        #region ForMirror
        [SerializeField]
        private SetPlayerInfo _setPlayerInfo;
        [SerializeField]
        private PlayerSpecifications _playerSO;

        [SerializeField]
        private GameObject _camera;
        [SerializeField]
        private SyncRevive _syncRevive;

        private bool isInvert = false;
        [SerializeField]
        private SyncHealth _syncHealth;
        [SerializeField]
        private SelectSpawnPoint _selectSpawnPoint;
        [SyncVar]
        [SerializeField]
        private Team _serializeTeam;

        private bool _isActivateStateDamageRecieve;
        private bool _isActivateStateRevival;
        private bool _isActivateStateHeal;
        private bool _isActivateStateAttack;
        private bool _isActivateStateBuff;
        private bool _isActivateStateUltimate;


        public bool isLocalCharacter;

        #endregion

        private void OnEnable()
        {
            _hitPointsRefiller.HealStarted +=OnHealStarted;
            _hitPointsRefiller.HealFinished += OnHealFinished;

            _crystallReceiver.CrystallTouched += OnCrystallTouched;

            _weapon.WeaponAct += OnWeaponAct;
        }



        private void OnDisable()
        {
            _hitPointsRefiller.HealFinished -= OnHealStarted;
            _hitPointsRefiller.HealFinished -= OnHealFinished;

            _crystallReceiver.CrystallTouched -= OnCrystallTouched;

            _weapon.WeaponAct -= OnWeaponAct;
        }



        private void Start()
        {
            
            if(isLocalPlayer == true)
            {
                isLocalCharacter = true;
            }



            if (_serializeTeam == Team.Obstacle && isServer)
            {
                if (isServer)
                {
                    ServerSerializeTeam();
                }
                SetTeam(_serializeTeam);
            }
            else if(_serializeTeam != Team.Obstacle)
            {
                _team = _serializeTeam;
            }
            else
            {
                PlayerList playerList = FindObjectOfType<PlayerList>();
                int thisPlayersInTeam = 2000;
                foreach (var thisDictionary in playerList.EveryTeamPlayerCount)
                {
                    if (thisDictionary.Value < thisPlayersInTeam)
                    {
                        thisPlayersInTeam = thisDictionary.Value;
                        _team = thisDictionary.Key;

                    }
                }
            }

            Initialize(_team, _playerSO);
        }

        [Server]
        private void ServerSerializeTeam()
        {
            PlayerList playerList = FindObjectOfType<PlayerList>();
            int thisPlayersInTeam = 1000;
            BB.Core.Character[] characters = FindObjectsOfType<BB.Core.Character>();
            foreach (var thisDictionary in playerList.EveryTeamPlayerCount)
            {
                if (thisDictionary.Value < thisPlayersInTeam)
                {
                    thisPlayersInTeam = thisDictionary.Value;
                    _serializeTeam = thisDictionary.Key;

                    SetTeam(_serializeTeam);
                }
            }
        }

        private void SetTeam(Team team)
        {
            _team = team;
        }
        public void RevertUI()
        {
            if (isLocalPlayer == true && _team == Team.Blue)
            {

                _camera.transform.localPosition = new Vector3(0, 21, 10);
                _camera.transform.rotation = Quaternion.Euler(58, 180, 0); // переделать

                RevertUI[] revertUIs = FindObjectsOfType<RevertUI>();
                foreach (var revert in revertUIs)
                {
                    revert.RevertGameObject();
                }

                isInvert = true;
            }
        }


        private void Initialize(Team team, BB.ScriptableObjects.PlayerSpecifications playerSO)
        {
            _name = _characterSO.Name;

            _mover.Initialize(_rigidBody, _characterSO.StandardSpeed);
            _weapon.Initialize(_team, this);
            _ultimateWeapon.Initialize(_team, this);

            _healthStorage = new DamageReceiving.HealthStorage(_characterSO.MaximumHealth);
            _healthStorageController = new DamageReceiving.HealthController(_damageReceiver, _healthStorage, team);

            

            _team = team;
            _setPlayerInfo.Initialize(team);

            _hitPointsRefiller.Initialize(_healthStorage, _characterSO.HealthRefillValue);

            _characterConcealer.AddObserver(this);

            _healthStorage.AddObserver(_healthUI);
            _healthStorageController.GetHit += OnGetHit;
            _healthStorage.AddObserver(_hitPointsRefiller);
            _healthStorage.AddObserver(this);

            _buffReactors.Add(_mover);
            _buffReactors.Add(_weapon);
            _buffReactors.Add(_syncHealth);

            _syncHealth.SetHealthStorage(_healthStorage);
            _syncRevive.SetTeam(team);
            _syncRevive.SetHealthStorage(_healthStorage);

            _weapon.AddObserver(_characterConcealer);
        }


        public void AddWeaponTargetables(IEnumerable<ITargetable> targetables)
        {
            _weapon.AddAimerTargetables(targetables);
            _ultimateWeapon.AddAimerTargetables(targetables);
        }
        public void RemoveWeaponTargetables(IEnumerable<ITargetable> targetables)
        {
            _weapon.RemoveAimerTargetables(targetables);
            _ultimateWeapon.RemoveAimerTargetables(targetables);
        }

        public void Die()
        {
            _damageReceiver.gameObject.SetActive(false);
            Dead?.Invoke(this, _team);

            StateEntered?.Invoke(CharacterState.Die);
        }

        private void OnHealStarted()
        {
            if(_isActivateStateHeal == false && _healthStorage.CurrentHealth < _healthStorage.MaxHealth)
            {    
                _isActivateStateHeal = true;
                CmdInvokeMethod(nameof(OnHealStarted));
                StateEntered?.Invoke(CharacterState.Recovery);
            }
        }

        private void OnHealFinished()
        {
            if(_isActivateStateHeal == true && _healthStorage.CurrentHealth == _healthStorage.MaxHealth)
            { 
                _isActivateStateHeal = false;      
                CmdInvokeMethod(nameof(OnHealFinished));
                StateExited?.Invoke(CharacterState.Recovery);
            }
        }


        public void OnGetHit()
        {
            if(_isActivateStateDamageRecieve == true)
            {
                return;
            }

            _isActivateStateDamageRecieve = true;
            CmdInvokeMethod(nameof(OnGetHit));
            CancelInvoke(nameof(ExitOnGetHit));

            StateEntered?.Invoke(CharacterState.DamageReceiving);

            Invoke(nameof(ExitOnGetHit), 1);
        }
        public void ExitOnGetHit()
        {
            if(_isActivateStateDamageRecieve == true)
            {
                _isActivateStateDamageRecieve = false;
                StateExited?.Invoke(CharacterState.DamageReceiving);
                CmdInvokeMethod(nameof(ExitOnGetHit));
            }
            
        }
        
        public void RevivalAfterSpecialTime()
        {
            if(_isActivateStateRevival == true)
            {
                return;
            }
            _isActivateStateRevival = true;
            
            CmdInvokeMethod(nameof(RevivalAfterSpecialTime));

            StateEntered?.Invoke(CharacterState.Revival);
        }

        public void Revival()
        {
            if(_isActivateStateRevival == true)
            {
                _isActivateStateRevival = false;

                CmdInvokeMethod(nameof(Revival));
                _healthStorage.ResetHealth();

                Invoke(nameof(EnableDamageReceiver), 3);
            
                //_dissolveEffect.RevivalEffectAnimation();

               StateExited?.Invoke(CharacterState.Revival);
            }
                
            
        }

        private void EnableDamageReceiver()
        {
            _damageReceiver.gameObject.SetActive(true);
        }

        public void Stop()
        {
            CharacterStopped?.Invoke();
        }


        #region Moving_Methods
        public void Move(Vector3 direction)
        {
            if (isInvert == false)
            {
                _mover.MoveMovableToDirection(direction.normalized);
                _model.transform.LookAt(direction + transform.position);
            }
            else
            {
                _mover.MoveMovableToDirection(direction.normalized * -1);
                _model.transform.LookAt(direction * -1 + transform.position);
            }
        }

        public void StopMove()
        {
            _mover.StopMove();
        }
        #endregion  


        #region Attack
        //standard attack to target
        public void Attack()
        {
            _weapon.Attack();
        }

        public void AutoAtack()
        {
            _weapon.AutoAttack();
        }

        public void TakeAim()
        {
            _weapon.ActivateAimmmer();
        }
        public void UpdateAim(Vector3 direction)
        {
            if (isInvert == false)
                _weapon.UpdateAimmerTarget(direction);
            else
                _weapon.UpdateAimmerTarget(direction * -1);
        }
        public void HideAim()
        {
            _weapon.HideAimmer();
        }

        public void RemoveAim()
        {
            _weapon.DeactivateAimmer();
        }

        #endregion

        #region Ultimate
        public void UltimateAttack()
        {

            _ultimateWeapon.Attack();

            CmdInvokeMethod(nameof(RpcSwitcherUltimateState));
        }

        public void UltimateAutoAtack()
        {

            _ultimateWeapon.AutoAttack();

            CmdInvokeMethod(nameof(RpcSwitcherUltimateState));
        }
        private void RpcSwitcherUltimateState()
        {
            StateEntered?.Invoke(CharacterState.Ultimate);
            StateExited?.Invoke(CharacterState.Ultimate);
        }

        public void TakeUltimateAim()
        {
            _ultimateWeapon.ActivateAimmmer();
        }

        public void UpdateUltimateAim(Vector3 direction)
        {
            if (isInvert == false)
                _ultimateWeapon.UpdateAimmerTarget(direction);
            else
                _ultimateWeapon.UpdateAimmerTarget(direction * -1);
        }

        public void HideUltimateAim()
        {
            _ultimateWeapon.HideAimmer();
        }

        public void RemoveUltimateAim()
        {
            _ultimateWeapon.DeactivateAimmer();
        }

        #endregion

        #region BuffApplying


        public void AddBuffToReactors(Buff buff)
        {
            _currentBuffs.Add(buff);

            foreach (var buffReactor in _buffReactors)
            {
                buffReactor.ApplyBuff(buff);
            }

            StartCoroutine(nameof(WaitTillBuffEnd), buff);
        }
        private IEnumerator WaitTillBuffEnd(Buff buff)
        {
            yield return new WaitForSeconds(buff.Duration);
            RemoveBuffFromBuffReactors(buff);
        }

        public void RemoveBuffFromBuffReactors(Buff buff)
        {
            if (!_currentBuffs.Contains(buff))
                return;

            _currentBuffs.Remove(buff);

            foreach (var buffReactor in _buffReactors)
            {
                buffReactor.CancelBuff(buff);
            }
        }

        public void RemoveSpecialTypeBuffs(BuffType buffType)
        {
            var negativeBuffs = _currentBuffs.Where(buff => buff.BuffType == buffType);

            foreach (var buff in negativeBuffs)
            {
                foreach (var buffReactor in _buffReactors)
                {
                    buffReactor.CancelBuff(buff);
                }
            }
            _currentBuffs.RemoveAll(buff => negativeBuffs.Contains(buff));
        }

        public void RemoveAllBuff()
        {
            foreach (var buff in _currentBuffs)
            {
                foreach (var buffReactor in _buffReactors)
                {
                    buffReactor.CancelBuff(buff);
                }
            }

            _currentBuffs.Clear();
        }

        #endregion



        #region private

        void IConcealerObserver.Update(ConcealerUpdateInfo updateInfo)
        {
            IsVisible = updateInfo.IsVisible;
            VisibleChanged?.Invoke(this, IsVisible);
        }

        private void OnWeaponAct(WeaponActionInfo obj)
        {
            if (obj.CurrentWeaponAction == WeaponAction.Shoot)
            {
                CmdInvokeMethod(nameof(RpcOnWeaponAct));
            }
        }
        private void RpcOnWeaponAct()
        {
            StateEntered?.Invoke(CharacterState.Attack);
            StateExited?.Invoke(CharacterState.Attack);
        }

        private void OnCrystallTouched(Crystall crystall)
        {
            StateEntered?.Invoke(CharacterState.ItemRising);
            var buff = crystall.Buff;
            AddBuffToReactors(buff);
        }

        void IHealthObserver.Update(HealthStorageUpdateInfo updatedDataInfo)
        {
            if(updatedDataInfo.CurrentPercentageHelthPoints == 100 && _isActivateStateHeal == true)
            {
                OnHealFinished();
            }
        }
        #endregion

        [Command(requiresAuthority = false)]
        private void CmdInvokeMethod(string methodName)
        {
            ServerInvokeMethod(methodName);
        }
        [Server]
        private void ServerInvokeMethod(string methodName)
        {
            RpcInvokeMethod(methodName);
        }
        [ClientRpc]
        private void RpcInvokeMethod(string methodName)
        {
            Invoke(methodName, 0.01f);
        }
    }
}