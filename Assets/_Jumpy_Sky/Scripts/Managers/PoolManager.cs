using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ClawbearGames
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { private set; get; }

        [SerializeField] private ObstacleController obstacleControllerPrefab = null;
        [SerializeField] private ItemController[] itemControllerPrefabs = null;
        [SerializeField] private PlatformPrefabConfiguration[] platformPrefabConfigurations = null;

        private List<ObstacleController> listObstacleController = new List<ObstacleController>();
        private List<ItemController> listItemController = new List<ItemController>();
        private List<PlatformController> listPlatformController = new List<PlatformController>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(Instance.gameObject);
                Instance = this;
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }


        /// <summary>
        /// Get a list of active platforms.
        /// </summary>
        /// <returns></returns>
        public List<PlatformController> GetActivePlatforms()
        {
            List<PlatformController> listResult = new List<PlatformController>();
            foreach(PlatformController platformController in listPlatformController) 
            {
                if(platformController.gameObject.activeSelf)
                { listResult.Add(platformController); }
            }
            return listResult;
        }


        /// <summary>
        /// Find PlatformController acording to given transform.
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public PlatformController FindPlatformController(Transform trans)
        {
            List<PlatformController> listActivePlatform = GetActivePlatforms();
            foreach(PlatformController platform in listActivePlatform)
            {
                if (platform.transform.Equals(trans)) return platform;
            }

            return null;
        }



        /// <summary>
        /// Get an inactive ObstacleController object.
        /// </summary>
        /// <returns></returns>
        public ObstacleController GetObstacleController()
        {
            //Find in the list
            ObstacleController obstacleController = listObstacleController.Where(a => !a.gameObject.activeSelf).FirstOrDefault();

            if (obstacleController == null)
            {
                //Did not find one -> create new one
                obstacleController = Instantiate(obstacleControllerPrefab, Vector3.zero, Quaternion.identity);
                obstacleController.gameObject.SetActive(false);
                listObstacleController.Add(obstacleController);
            }

            return obstacleController;
        }



        /// <summary>
        /// Get an inactive ItemController with given type.
        /// </summary>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public ItemController GetItemController(ItemType itemType)
        {
            //Find in the list
            ItemController itemController = listItemController.Where(a => !a.gameObject.activeSelf && a.ItemType == itemType).FirstOrDefault();

            if (itemController == null)
            {
                //Did not find one -> create new one
                ItemController prefab = itemControllerPrefabs.Where(a => a.ItemType == itemType).FirstOrDefault();
                itemController = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                itemController.gameObject.SetActive(false);
                listItemController.Add(itemController);
            }

            return itemController;
        }


        /// <summary>
        /// Get an inactive PlatformController object.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public PlatformController GetPlatformController(PlatformType type, PlatformSize size)
        {
            //Find in the list
            PlatformController platformController = listPlatformController.Where(a => !a.gameObject.activeSelf && a.PlatformType.Equals(type) && a.PlatformSize.Equals(size)).FirstOrDefault();

            if (platformController == null)
            {
                //Did not find one -> create new one
                PlatformPrefabConfiguration prefabConfig = platformPrefabConfigurations.Where(a => a.PlatformType.Equals(type)).FirstOrDefault();
                PlatformController prefab = prefabConfig.PlatformControllerPrefabs.Where(a => a.PlatformSize.Equals(size)).FirstOrDefault();
                platformController = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                platformController.gameObject.SetActive(false);
                listPlatformController.Add(platformController);
            }

            return platformController;
        }

    }
}
