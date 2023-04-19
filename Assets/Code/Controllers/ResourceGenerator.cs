using System;
using System.Collections.Generic;
using ResourceSystem;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class ResourceGenerator : IDisposable
{
    private BaseBuildAndResources[,] _installedBuildings;
    private List<Vector2Int> _possiblePlaceResource = new List<Vector2Int>();
    private List<Vector2Int> _spawnedResources = new List<Vector2Int>();
    private GameConfig _gameConfig;
    private Mineral _mineral;
    private GeneratorLevelController _generatorLevelController;
    private int _numOfVariant = 0;
    private bool _flag;

    private List<GameObject> _listOfMineral=new List<GameObject>();

    private List<MineralConfig> _resourcesTierOne;
    private List<MineralConfig> _resourcesTierTwo;
    private List<MineralConfig> _resourcesTierThree;
    private LoadContainersWithResources _listOfResources=new LoadContainersWithResources();
   


    
    public ResourceGenerator(BaseBuildAndResources[,] installedBuildings,
        GameConfig gameConfig, GeneratorLevelController generatorLevelController,LoadContainersWithResources listOfResources)
    {
        _listOfResources = listOfResources;
        _installedBuildings = installedBuildings;
        _gameConfig = gameConfig;
        _generatorLevelController = generatorLevelController;
        _resourcesTierOne = gameConfig.MineralConfigs.Minerals.FindAll(x => x.Tier == TierNumber.One);
        _resourcesTierTwo = gameConfig.MineralConfigs.Minerals.FindAll(x => x.Tier == TierNumber.Two);
        _resourcesTierThree = gameConfig.MineralConfigs.Minerals.FindAll(x => x.Tier == TierNumber.Three);
        
        _generatorLevelController.SpawnResources += SpawnResources;
    }
    
    public ResourceGenerator(BaseBuildAndResources[,] installedBuildings,
        GameConfig gameConfig, GeneratorLevelController generatorLevelController, int i)
    {
        _installedBuildings = installedBuildings;
        _gameConfig = gameConfig;
        _generatorLevelController = generatorLevelController;
        _generatorLevelController.SpawnResources += SpawnResources;
        _numOfVariant = i;
    }
    private void SpawnResources(VoxelTile tile)
    {
        
        GetPossiblePlace(tile);
        _listOfResources.SetResources(_listOfMineral);
    }

    private void GetPossiblePlace(VoxelTile tile)
    {
        int numTile = tile.NumZone;
        int count = 0;
        int x = (int) tile.transform.position.x - 1;
        int y = (int) tile.transform.position.z - 1;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                _possiblePlaceResource.Add(new Vector2Int(x + i, y + j));
            }
        }

        _possiblePlaceResource.Remove(new Vector2Int((int) tile.transform.position.x,
            (int) tile.transform.position.z));

        foreach (var byteAccess in tile.TablePassAccess)
        {
            if (byteAccess == 1)
            {
                switch (count)
                {
                    case 0:
                        _possiblePlaceResource.Remove(new Vector2Int((int) tile.transform.position.x,
                            (int) tile.transform.position.z - 1));
                        break;
                    case 1:
                        _possiblePlaceResource.Remove(new Vector2Int((int) tile.transform.position.x - 1,
                            (int) tile.transform.position.z));
                        break;
                    case 2:
                        _possiblePlaceResource.Remove(new Vector2Int((int) tile.transform.position.x,
                            (int) tile.transform.position.z + 1));
                        break;
                    case 3:
                        _possiblePlaceResource.Remove(new Vector2Int((int) tile.transform.position.x + 1,
                            (int) tile.transform.position.z));
                        break;
                }
            }
            count++;
        }

        foreach (var building in _installedBuildings)
        {
            if (building != null)
            {
                _possiblePlaceResource.Remove(new Vector2Int((int) building.transform.position.x,
                    (int) building.transform.position.z));
            }
        }

        if (_numOfVariant == 0)
        {
            PlaceResources(numTile);
        }
        else
        {
            PlaceResourcesSecondVariant(numTile);
        }
        
    }

    private void PlaceResources(int numTile)
    {
        int numberOfMineralsToSpawn;
        int random;
        // если использовать данный сетап, то редко получается 3, но общее количество ресурсов больше
        //int[] q = {1,2,2,3};
        //int numberOfMineralsToSpawn = q[Random.Range(0, q.Length)];
        if (!_flag)
        {
            random = 50;
            _flag = true;
        }
        else
        {
            random = Random.Range(0, 101);
        }
        
        if (random <= 25)
        {
            numberOfMineralsToSpawn = 1;
        }
        else if (random > 25 && random < 75)
        {
            numberOfMineralsToSpawn = 2;
        }
        else
        {
            numberOfMineralsToSpawn = 3;
        }
        
        float weightT1 = _gameConfig.TearOneWeightVariantNik;
        float weightT2 = _gameConfig.TearTwoWeightVariantNik * numTile;
        float weightT3 = _gameConfig.TearThirdWeightVariantNik * Mathf.Pow(numTile, 1.5f);
        float sumAllWeight = weightT1 + weightT2 + weightT3;
        int randomChance = 0;
        

        #region ControlValues_delete
        // 2 ресурса итерация 1
        int test = 100 - (int)Math.Round(weightT1 * 2f / (weightT1 * 2f + weightT2 * 0.5f + weightT3 * 0.5f) * 100);
        int test2 = 100 - (int)Math.Round(weightT2/2f/(weightT1*2f+weightT2*0.5f+weightT3*0.5f) * 100);
        int test3 = 100 - (int)Math.Round((weightT3 / (weightT1 * 2f + weightT2 * 0.5f + weightT3 * 0.5f) / 2f) * 100);
        // 2 ресурса итерация 2
        int test4 = 100 - (int)Math.Round(weightT1 / sumAllWeight * 100);
        int test5 = 100 - (int)Math.Round(weightT2 / sumAllWeight * 100);
        int test6 = 100 - (int)Math.Round(weightT3 / sumAllWeight * 100);
        
        // 3 ресурса итерация 2
        int test7 = 100 - (int)Math.Round(weightT1 / sumAllWeight * 100);
        int test8 = 100 - (int)Math.Round(weightT2 / sumAllWeight * 100);
        int test9 = 100 - (int)Math.Round(weightT3/sumAllWeight * 100);
        // 3 ресура итерация 3
        int test10 = 100 - (int)Math.Round(weightT1 / sumAllWeight * 100);
        int test11 = 100 - (int)Math.Round(weightT2 / sumAllWeight * 100);
        int test12 = 100 - (int)Math.Round(weightT3/sumAllWeight * 100);

        #endregion

        if (_possiblePlaceResource.Count != 0)
        {
            _listOfMineral.Clear();
            switch (numberOfMineralsToSpawn)
            {
                case 1:
                    randomChance = Random.Range(0, 101);
                    if ((int)Math.Round(weightT1 * 100) >= randomChance)
                    {
                        CreateResources(_resourcesTierOne[Random.Range(0,_resourcesTierOne.Count)]);
                    }
                    else if ((int)Math.Round(weightT2 * 100) >= 100 - randomChance && 
                             (int)Math.Round(weightT3 * 100) < 100 - randomChance)
                    {
                        CreateResources(_resourcesTierTwo[Random.Range(0,_resourcesTierTwo.Count)]);
                    }
                    else if ((int)Math.Round(weightT3 * 100) >= 100 - randomChance)
                    {
                        CreateResources(_resourcesTierThree[Random.Range(0,_resourcesTierThree.Count)]);
                    }
                    break;
                case 2:
                    for (int i = 1; i <= 2; i++)
                    {
                        randomChance = Random.Range(0,101);
                        if (i==1)
                        {
                            if ((int)Math.Round(weightT1 * 2f / (weightT1 * 2f + weightT2 * 0.5f + weightT3 * 0.5f) * 100) >= randomChance)
                            {
                                CreateResources(_resourcesTierOne[Random.Range(0,_resourcesTierOne.Count)]);
                            }
                            else if ((int)Math.Round(weightT2/2f/(weightT1*2f+weightT2*0.5f+weightT3*0.5f) * 100) >= 100 - randomChance &&
                                      (int)Math.Round((weightT3 / (weightT1 * 2f + weightT2 * 0.5f + weightT3 * 0.5f) / 2f) * 100) < 100 - randomChance)
                            {
                                CreateResources(_resourcesTierTwo[Random.Range(0,_resourcesTierTwo.Count)]);
                            }
                            else if ((int)Math.Round((weightT3 / (weightT1 * 2f + weightT2 * 0.5f + weightT3 * 0.5f) / 2f) * 100) >= 100 - randomChance)
                            {
                                CreateResources(_resourcesTierThree[Random.Range(0,_resourcesTierThree.Count)]);
                            }
                        }
                        else if (i == 2)
                        {
                            if ((int)Math.Round(weightT1 / sumAllWeight * 100) >= randomChance)
                            {
                                CreateResources(_resourcesTierOne[Random.Range(0,_resourcesTierOne.Count)]);
                            }
                            else if ((int)Math.Round(weightT2 / sumAllWeight * 100) >= 100 - randomChance && 
                                     (int)Math.Round(weightT3 / sumAllWeight * 100) < 100 - randomChance)
                            {
                                CreateResources(_resourcesTierTwo[Random.Range(0,_resourcesTierTwo.Count)]);
                            }
                            else if ((int)Math.Round(weightT3 / sumAllWeight * 100) >= 100 - randomChance)
                            {
                                CreateResources(_resourcesTierThree[Random.Range(0,_resourcesTierThree.Count)]);
                            }
                        }
                    }
                    break;
                case 3:
                    for (int i = 1; i <= 3; i++)
                    {
                        randomChance = Random.Range(0, 101);
                        if (i==1)
                        {
                            if (50f >= randomChance)
                            {
                                CreateResources(_resourcesTierOne[Random.Range(0,_resourcesTierOne.Count)]);
                            }
                        }
                        else if (i == 2)
                        {
                            if ((int)Math.Round(weightT1 / sumAllWeight / 2f * 100) >= randomChance)
                            {
                                CreateResources(_resourcesTierOne[Random.Range(0,_resourcesTierOne.Count)]);
                            }
                            else if ((int)Math.Round(weightT2 / sumAllWeight / 2f * 100) >= 100 - randomChance && 
                                     (int)Math.Round(weightT3/sumAllWeight * 100) < 100 - randomChance)
                            {
                                CreateResources(_resourcesTierTwo[Random.Range(0,_resourcesTierTwo.Count)]);
                            }
                            else if ((int)Math.Round(weightT3/sumAllWeight * 100) >= 100 - randomChance)
                            {
                                CreateResources(_resourcesTierThree[Random.Range(0,_resourcesTierThree.Count)]);
                            }
                        }
                        else if (i == 3)
                        {
                            if ((int)Math.Round(weightT1 / sumAllWeight / 2f * 100) >= randomChance)
                            {
                                CreateResources(_resourcesTierOne[Random.Range(0,_resourcesTierOne.Count)]);
                            }
                            else if ((int)Math.Round(weightT2 / sumAllWeight / 2f * 100) >= 100 - randomChance &&
                                     (int)Math.Round(weightT3/sumAllWeight * 100) < 100 - randomChance)
                            {
                                CreateResources(_resourcesTierTwo[Random.Range(0,_resourcesTierTwo.Count)]);
                            }
                            else if ((int)Math.Round(weightT3/sumAllWeight * 100) >= randomChance)
                            {
                                CreateResources(_resourcesTierThree[Random.Range(0,_resourcesTierThree.Count)]);
                            }
                        }
                    }
                    
                    
                    break;
            }
            
            _possiblePlaceResource.Clear();
        }
    }

    private void PlaceResourcesSecondVariant(int numTile)
    {
        int n = _possiblePlaceResource.Count;
        for (int i = 0; i < n; i++)
        {
            int random = Random.Range(0, 101);
            if (random <= _gameConfig.TearOneWeightSecondVariant*100)
            {
                CreateResources(_resourcesTierOne[Random.Range(0, _resourcesTierOne.Count)]);
            }
            else if (random > _gameConfig.TearTwoWeightSecondVariant*100 && random <= _gameConfig.TearOneWeightSecondVariant*100 + _gameConfig.TearTwoWeightSecondVariant*100)
            {
                CreateResources(_resourcesTierTwo[Random.Range(0, _resourcesTierTwo.Count)]);
            }
            else if (random > _gameConfig.TearOneWeightSecondVariant*100 + _gameConfig.TearTwoWeightSecondVariant*100 
                     && random <= _gameConfig.TearOneWeightSecondVariant*100 + _gameConfig.TearThirdWeightSecondVariant*100 + _gameConfig.TearTwoWeightSecondVariant*100)
            {
                CreateResources(_resourcesTierThree[0]);
            }
            else
            {
                Debug.Log("Спавн ничего");
            }
        }
        _possiblePlaceResource.Clear();
    }

    private void CreateResources(MineralConfig resourceConfig)
    {
        var pos = _possiblePlaceResource[Random.Range(0, _possiblePlaceResource.Count)];
        if (pos != null)
        {
            CreateMineralGameObject(resourceConfig, pos);
            _installedBuildings[pos.x, pos.y] = _mineral;
            _possiblePlaceResource.Remove(pos);
        }
    }

    private void CreateMineralGameObject(MineralConfig mineralConfig, Vector2Int posistion)
    {
        GameObject currentMineral = Object.Instantiate(mineralConfig.Prefab);
        currentMineral.transform.position = new Vector3(posistion.x, 0.1f, posistion.y);
        currentMineral.transform.rotation = Quaternion.identity;
        Mineral _tempMineral = currentMineral.AddComponent<Mineral>();
        _tempMineral.SetModelOfMine(mineralConfig);
        BoxCollider _boxCollider = currentMineral.AddComponent<BoxCollider>();
        _mineral = _tempMineral;
        _listOfMineral.Add(currentMineral);
        
        
    }
    
    public void Dispose()
    {
        _generatorLevelController.SpawnResources -= SpawnResources;
    }
}