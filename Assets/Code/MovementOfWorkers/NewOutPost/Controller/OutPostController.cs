using System;
using System.Collections.Generic;
using BuildingSystem;
using Code.BuildingSystem;
using Code.BuldingsSystem.ScriptableObjects;
using ResourceSystem;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Controllers.NewOutPost
{
    public enum UnitStates
    {
        MovingToWorkInResource, MovingToWorkInBuilding,
        WorkInBuilding, WorkInResorce, idle, MovingToSpawn
    }
    /// <summary>
    /// Контроллер наших юнитов
    /// </summary>
    public class OutPostController : IDisposable, IOnUpdate, IOnController
    {
#region Fields
           
        private Button _backToSpawnButton; 
        private Button _goToWorkInResourceButton;
        private Button _goToWorkInBuildingButton;
           
        private OutPostBtn _btn;
        private Mineral _mineral;
        private GameObject _prefab;
        private ResourceMine _mine;
        //private WareHouseBuildModel _model;
        
        private Vector3 _whereToSpawn;
        private Vector3 _whereToGo;

        private List<GameObject> _unitList = new List<GameObject>();
        private List<Mineral> _minerals = new List<Mineral>();
        
        private BuildingList _buildingList;
        
        private float _TimeMining;
        private bool _isWorkInResource = true;
        private bool _isWorkInBuilding = true;
        private bool _isGoToWork = true;
        
        #endregion
        
        #region UnityMethods

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="btn"> кнопка</param>
        /// <param name="gameConfig"> конфиг</param>
        /// <param name="prefab"> префаб юинта</param>
        /// <param name="lvlController"></param>
        public OutPostController(OutPostBtn btn, GameConfig gameConfig, 
            GameObject prefab, GeneratorLevelController controller, 
            BuildingList model)
        {
            _whereToSpawn = new Vector3(gameConfig.MapSizeX / 2.0f, 0, 
                gameConfig.MapSizeY / 2.0f);//костыль

            _btn = btn;
            _prefab = prefab;
            _btn.spawnUnit += BuyAUnit;
        }


        public void OnUpdate(float deltaTime)
        {
            for (var index = 0; index < _unitList.Count; index++)
            {
                var unit = _unitList[index];
                var view = unit.GetComponent<OutPostView>();
                var worker = unit.GetComponentInChildren<WorkerView_old>();
                if (_goToWorkInResourceButton && _isGoToWork)
                {
                    StateMachineUnit(UnitStates.MovingToWorkInResource, worker, view);
                }
                else if (_goToWorkInBuildingButton && _isGoToWork)
                {
                    StateMachineUnit(UnitStates.MovingToWorkInBuilding, worker, view);
                }
                else
                {
                    StateMachineUnit(UnitStates.MovingToSpawn, worker, view);

                    var curDistance = view.transform.position - _whereToSpawn;
                    if (curDistance.sqrMagnitude <= 0.1)
                    {
                        //worker.GetResurseOutOfHolder(_model);
                        _minerals.Remove(_mineral);
                        _whereToGo = Vector3.zero;
                        _isGoToWork = true;
                    }
                }

                if (_isWorkInResource)
                {
                    var curDistance = view.transform.position - _whereToGo;
                    if (curDistance.sqrMagnitude <= 0.1)
                    {
                        StateMachineUnit(UnitStates.WorkInResorce, worker, view);
                    }
                }
                else if(_isWorkInBuilding)
                {
                    var curDistance = view.transform.position - _whereToGo;
                    if (curDistance.sqrMagnitude <= 0.1)
                    {
                        StateMachineUnit(UnitStates.WorkInBuilding, worker, view);
                    }
                }
            }
        }

        #endregion

        #region Methods

        #region Micro Methods

       /* private void initializationWareHouse(WareHouseBuildModel model)
        {
            _model = model;
        }*/

        /// <summary>
        /// добавление шахты в лист
        /// </summary>
        private void AddListMine(Mineral mineral)
        {
            _minerals.Add(mineral);
        }
        
        /// <summary>
        /// Стейтовая машина для юнитов
        /// </summary>
        /// <param name="states">стаейты</param>
        private void StateMachineUnit(UnitStates states, 
            WorkerView_old workerView, OutPostView view)
        {
            switch (states)
            {
                case UnitStates.idle:
                    // Idle Unit
                    break;
                case UnitStates.MovingToWorkInResource:
                    MineTypeCheck();
                    Move(view, _whereToGo);
                    break;
                case UnitStates.MovingToWorkInBuilding:
                    BuildingTypeCheck();
                    Move(view, _whereToGo);
                    break;
                case UnitStates.MovingToSpawn:
                    Move(view, _whereToSpawn);
                    break;
                case UnitStates.WorkInResorce:
                    WorkingInResource(view, workerView);
                    break;
                case UnitStates.WorkInBuilding:
                    WorkingInBuilding(view, workerView);
                    break;
            }
        }

        #endregion
        /// <summary>
        /// Спавн Юнита
        /// </summary>
        /// <param name="point"></param>
        private void BuyAUnit(OutPostPoint point)
        {
            float maxCountOfNPC = 5;
            if (maxCountOfNPC > _unitList.Count)
            {
                var spawn = Object.Instantiate(
                    _prefab, _whereToSpawn, new Quaternion());
                _unitList.Add(spawn);
            }
        }

        /// <summary>
        /// Поиск к пути, к шахте.
        /// </summary>
        /// <param name="typeOfMine">Виды шахт</param>
        private void MineTypeCheck()
        {
            for (var index = 0; index < _minerals.Count; index++)
            {
                var minerals = _minerals[index];
             /*   switch (minerals.ThisResourceMine.TypeOfMine)
                {
                    case 601:
                        SettingsOfWorkerInMineral(minerals);
                        break;
                    case 602:
                        SettingsOfWorkerInMineral(minerals);
                        break;
                    case 603:
                        SettingsOfWorkerInMineral(minerals);
                        break;
                    case 604:
                        SettingsOfWorkerInMineral(minerals);
                        break;
                    case 605:
                        SettingsOfWorkerInMineral(minerals);
                        break;
                    case 606:
                        SettingsOfWorkerInMineral(minerals);
                        break;
                    case 607:
                        SettingsOfWorkerInMineral(minerals);
                        break;
                }*/
            }
        }
        
        private void BuildingTypeCheck()
        {
            for (var index = 0; index < _buildingList.BuildingsConfig.Count; index++)
            {
                var buildings = _buildingList.BuildingsConfig[index];
                switch (buildings.BuildingType)
                {
                    case BuildingTypes.TextileFabrick:
                        SettingsOfWorkerInBuilding(buildings);
                        break;
                    // case 602:
                    //     SettingsOfWorkerInMineral(minerals);
                    //     break;
                    // case 603:
                    //     SettingsOfWorkerInMineral(minerals);
                    //     break;
                    // case 604:
                    //     SettingsOfWorkerInMineral(minerals);
                    //     break;
                    // case 605:
                    //     SettingsOfWorkerInMineral(minerals);
                    //     break;
                    // case 606:
                    //     SettingsOfWorkerInMineral(minerals);
                    //     break;
                    // case 607:
                    //     SettingsOfWorkerInMineral(minerals);
                    //     break;
                }
            }
        }

        private void SettingsOfWorkerInMineral(Mineral minerals)
        {
            _isWorkInResource = true;
            _whereToGo = minerals.transform.position;
            _TimeMining = minerals.ThisResourceMine.ExtractionTime;
            _mine = minerals.ThisResourceMine;
            _mineral = minerals;
        }
        
        private void SettingsOfWorkerInBuilding(BuildingConfig buildings)
        {
            _isWorkInBuilding = true;
            _whereToGo = buildings.BuildingPrefab.transform.position;
            // _mine = minerals.ThisResourceMine;
            // _mineral = minerals;
        }

        /// <summary>
        /// метод передвижения
        /// </summary>
        /// <param name="view"></param>
        /// <param name="worker"></param>
        /// <param name="whereToGo"></param>
        private void Move(OutPostView view, Vector3 whereToGo)
        {
            view.MeshAgent.SetDestination(whereToGo);
        }

        /// <summary>
        /// Метод для того чтобы выдвать задачу юнитам
        /// </summary>
        /// <param name="view"></param>
        /// <param name="workerView"></param>
        private void WorkingInResource(OutPostView view, WorkerView_old workerView)
        {
         /*  view.MeshAgent.isStopped = true;
           if (_mine.ResourceHolderMine.CurrentValue == 0)
           {
               view.MeshAgent.isStopped = false;
               _isWorkInResource = false;
               _isGoToWork = false;
           }
           else
           {
               workerView.MineResurse(_mine, _TimeMining);
               Debug.Log(_mine.ResourceHolderMine.CurrentValue);
           }*/
        }
        
        private void WorkingInBuilding(OutPostView view, WorkerView_old workerView)
        {
            // view.MeshAgent.isStopped = true;
        }
        #endregion
        public void Dispose()
        {
            _btn.spawnUnit -= BuyAUnit;
        }

    }
}
