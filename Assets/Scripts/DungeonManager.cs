using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DungeonManager : MonoBehaviour, ILockable
{

	public EDirection currentDirection;
	public Sprite directionSprite;

	public List<Transform> directionLocations;

	public AudioSource backGroundMusic;
	public AudioSource soundEffect;
	public List<AudioClip> runningInGrassSound;
	public Image fadeImage;
	int lineCount;
	int mouseOnRoomNum;
	bool moveLocked;
	ItemData clickedItem;
	int clickedItemOrder;
	GameObject clickedItemInfo;
	public GameObject itemDescriptionWindow;
	private int pageLimit;
	Dungeon dungeon;
	public Camera camera;
	public Node startNode;
	int width;
	int height;
	int maxGold;
	int enemyLimit;
	int dungeonEndFloor;
	Dictionary<int, string> safeFloorList;
	List<ItemData> itemDatabase;
	int floor;
	int floorSize;
	string dungeonName;
	List<Node> map;
	List<GameObject> nodeMap;
	List<int> nodeNumList;
	List<string> messageList;
	Dictionary<ItemData, int> itemList;
	Dictionary<Enemy, int> enemyList;
	public GameObject roomNodePrefab;
	public GameObject wallNodePrefab;
	public GameObject itemNodePrefab;
	public GameObject encounterNodePrefab;
	public GameObject monsterNodePrefab;
	public GameObject stairNodePrefab;
	public Transform mapObject;
	public GameObject buttonPrefab;
	public GameObject player;
	public GameObject dungeonEnemyPrefab;
	public GameObject stairAlert;
	public GameObject itemAlert;
	public GameObject wayPointWindow;
	public GameObject dungeonClearWindow;
	public GameObject gameOverWindow;

	public List<Sprite> itemSpriteList;

	
	public LineRenderer cardDragLine;
	public GameObject popUpMessageWindow;
	public GameObject alertMessage;


	public GameObject nextButton;
	public GameObject backButton;

	public List<GameObject> itemInfoPrefab;

	public TMP_Text dungeonNameText;
	public TMP_Text floorText;

	public TMP_Text healthText;
	public TMP_Text goldText;
	public TMP_Text textbox;

	public Toggle toolToggle;
	public Toggle othersToggle;


	public ItemData selectedItem;

	int currentPlayerLocation;
	int previousPlayerLocation;

	private bool updateLock;


	private List<int> secretRoomNodes = new List<int>();
	private int obstacleNodeIndex = -1;


	public Transform toolLocation;

	List<GameObject> itemObjectList;
	List<GameObject> enemyObjectList;
	public Sprite decorateBlock;
	private float moveDistance = 2f; // 한 번에 이동할 거리
	private float moveDuration = 0.2f; // 이동하는 데 걸리는 시간
	private Queue<Vector2> moveQueue = new Queue<Vector2>(); // 이동할 방향 저장
	private bool isMoving = false;
	private float energyGainLimit = 40;
	private Dictionary<DungeonEnemy, int> dungeonEnemies;

	public static DungeonManager Inst{get; private set;}

	int currentPage;


	void Update()
	{
		if (updateLock)
			return;

		UpdatePlayerData();
		if (Input.GetKeyDown(KeyCode.W) && !moveLocked)
		{
			if (player.GetComponent<Player>().GetDirection() != EDirection.North) { player.GetComponent<Player>().SetDirection(EDirection.North); }
			else if (CheckOutOfIndex(currentPlayerLocation - width))
			{
				if (nodeMap[currentPlayerLocation - width].GetComponent<RoomNode>().GetRoomType() != ERoomType.EWall)
				{
					previousPlayerLocation = currentPlayerLocation;
					currentPlayerLocation -= width;
					MovePlayer(currentPlayerLocation);
					EnqueueMove(Vector2.up);
				}
			}
			CameraController.Inst.SetFollowing();
		}
		else if (Input.GetKeyDown(KeyCode.A) && !moveLocked)
		{
			if (currentPlayerLocation % width == 0)
				return;

			if (player.GetComponent<Player>().GetDirection() != EDirection.West) { player.GetComponent<Player>().SetDirection(EDirection.West); }
			else if(CheckOutOfIndex(currentPlayerLocation - 1))
			{
				if (nodeMap[currentPlayerLocation - 1].GetComponent<RoomNode>().GetRoomType() != ERoomType.EWall)
				{
					previousPlayerLocation = currentPlayerLocation;
					currentPlayerLocation -= 1;
					MovePlayer(currentPlayerLocation);
					EnqueueMove(Vector2.left);
				}
			}
			CameraController.Inst.SetFollowing();
		}
		else if (Input.GetKeyDown(KeyCode.S) && !moveLocked)
		{
			if (player.GetComponent<Player>().GetDirection() != EDirection.South) { player.GetComponent<Player>().SetDirection(EDirection.South); }
			else if(CheckOutOfIndex(currentPlayerLocation + width))
			{
				if (nodeMap[currentPlayerLocation + width].GetComponent<RoomNode>().GetRoomType() != ERoomType.EWall)
				{
					previousPlayerLocation = currentPlayerLocation;
					currentPlayerLocation += width;
					MovePlayer(currentPlayerLocation);
					EnqueueMove(Vector2.down);
				}
			}
			CameraController.Inst.SetFollowing();
		}
		else if (Input.GetKeyDown(KeyCode.D) && !moveLocked)
		{
			if (currentPlayerLocation % width == width - 1)
				return;

			if (player.GetComponent<Player>().GetDirection() != EDirection.East) { player.GetComponent<Player>().SetDirection(EDirection.East); }
			else if(CheckOutOfIndex(currentPlayerLocation + 1))
			{
				if (nodeMap[currentPlayerLocation + 1].GetComponent<RoomNode>().GetRoomType() != ERoomType.EWall)
				{
					previousPlayerLocation = currentPlayerLocation;
					currentPlayerLocation += 1;
					MovePlayer(currentPlayerLocation);

					EnqueueMove(Vector2.right);
				}
			}
			CameraController.Inst.SetFollowing();
		}
		//else if (Input.GetKeyDown(KeyCode.Space))
		//{
		//	CameraController.Inst.ZoomIn(2f);
		//}
		//else if (Input.GetKeyUp(KeyCode.Space))
		//{
		//	CameraController.Inst.ZoomOut(2f);
		//}
		//else if (Input.GetKeyUp(KeyCode.Z))
		//{
		//	PlayerData.saveData.health -= 1;
		//}

	}



	void Awake()
	{
		moveLocked = true;

		if (Inst != null)
		{
			Destroy(gameObject);
			return;
		}

		Inst = this;
		itemObjectList = new();
		messageList = new();

		enemyObjectList = new();
		dungeonEnemies = new();

		//myItemList = new();
		//currentItemList = new();

		itemDatabase = DataController.Inst.LoadItemDatabase();
		currentPage = 0;
		lineCount = 30;

		DungeonSetUp();

		if(DungeonData.map == null)
		{
			CreateFloor();
			//LoadItemList();

			//UpdateItemPage();
		}
		else
		{ReCreateFloor();}

		StartCoroutine(FadeIn());
		
		//DontDestroyOnLoad(this);
	}



	private void CreateSecretRoom()
	{
		// 1) 비밀방 시작 위치: 맵의 빈 공간 중 하나
		int start;
		do
		{
			start = Random.Range(0, map.Count);
		}
		while (map[start] != null); // 기존 맵과 겹치지 않는 곳

		// 2) 비밀방 크기 (4~6개의 노드)
		int secretRoomCount = Random.Range(4, 7);
		List<int> tempSecretNodes = new List<int>();

		// 첫 노드 생성
		map[start] = new Node();
		map[start].SetRoomType(ERoomType.None);
		tempSecretNodes.Add(start);

		// 연결된 비밀방 확장
		for (int i = 1; i < secretRoomCount; ++i)
		{
			int dir = Random.Range(0, 4); // 상하좌우
			int next = GetNextIndex(tempSecretNodes[tempSecretNodes.Count - 1], dir);
			if (IsValidIndex(next) && map[next] == null)
			{
				map[next] = new Node();
				map[next].SetRoomType(ERoomType.None);
				tempSecretNodes.Add(next);
			}
		}

		// 3) 입구 노드와 장애물 노드 생성
		int entrance = tempSecretNodes[0]; // 첫 방을 입구로 설정
		int obstaclePos = GetNextIndex(entrance, Random.Range(0, 4));

		if (IsValidIndex(obstaclePos) && map[obstaclePos] == null)
		{
			map[obstaclePos] = new Node();
			map[obstaclePos].SetRoomType(ERoomType.EObstacle); // 장애물 노드
			obstacleNodeIndex = obstaclePos;

			
		}
		// 비밀방 노드 저장
		secretRoomNodes = tempSecretNodes;
	}
	private bool IsValidIndex(int index)
	{
		// 먼저 전체 범위 체크
		if (!CheckOutOfIndex(index))
			return false;

		// 1차원 배열이므로 x, y 계산
		int x = index % width;
		int y = index / width;

		// 가로/세로 범위 확인 (사실 CheckOutOfIndex가 이미 y 범위도 보장함)
		return (x >= 0 && x < width && y >= 0 && y < (floorSize / width));
	}

	private IEnumerator FadeIn()
	{
		float time = 0;
		Color color = fadeImage.color;
		
		while (time < 1f)
		{
			time += Time.deltaTime;
			color.a = Mathf.Lerp(1, 0, time / 1f);
			fadeImage.color = color;
			yield return null;
		}
		fadeImage.gameObject.SetActive(false);
		moveLocked = false;
	}

	private IEnumerator FadeOut()
	{
		fadeImage.gameObject.SetActive(true);
		moveLocked = true;
		float time = 0;
		Color color = fadeImage.color;

		while (time < 1f)
		{
			time += Time.deltaTime;
			color.a = Mathf.Lerp(0, 1, time / 1f);
			fadeImage.color = color;
			yield return null;
		}
	}

	
	public IEnumerator FindPath(int target)
	{
		if(moveLocked)
		{yield break;}

		moveLocked = true;
		Queue<int> queue = new Queue<int>();
		Dictionary<int, int> cameFrom = new Dictionary<int, int>();

		queue.Enqueue(currentPlayerLocation);
		cameFrom[currentPlayerLocation] = 0;

		while (queue.Count > 0)
		{
			int current = queue.Dequeue();
			if (current == target) break;

			foreach (int neighbor in GetNeighborNode(current))
			{
				if (!cameFrom.ContainsKey(neighbor) && nodeNumList.Contains(neighbor))
				{
					queue.Enqueue(neighbor);
					cameFrom[neighbor] = current;
				}
			}
		}
		int temp = target;
		List<string> directions = new List<string>();
		while (temp != 0)
		{
			int prev = cameFrom[temp];
			int dx = temp - prev;

			if (dx == 1) directions.Add("Right");
			else if (dx == -1) directions.Add("Left");
			else if (dx == -width) directions.Add("Up");
			else if (dx == width) directions.Add("Down");
			temp = prev;

		}
		directions.Reverse();

		for(int i = 0; i < directions.Count; ++i)
		{
			previousPlayerLocation = currentPlayerLocation;
			
			switch(directions[i])
			{
				case "Up":
				currentPlayerLocation -= width;
				break;

				case "Down":
				currentPlayerLocation += width;
				break;

				case "Left":
				currentPlayerLocation -= 1;
				break;

				case "Right":
				currentPlayerLocation += 1;
				break;
			}
			MovePlayer(currentPlayerLocation);
			

			switch(directions[i])
			{
				case "Up":
				EnqueueMove(Vector2.up);
				break;

				case "Down":
				EnqueueMove(Vector2.down);
				break;

				case "Left":
				EnqueueMove(Vector2.left);
				break;

				case "Right":
				EnqueueMove(Vector2.right);
				break;
			}
			yield return new WaitForSeconds(0.2f);
		}
		moveLocked = false;
	}

	List<int> GetNeighborNode(int nodeNum)
	{
		List<int> newList = new();

		if(nodeNumList.Contains(nodeNum - 1))
		newList.Add(nodeNum - 1);

		if(nodeNumList.Contains(nodeNum + 1))
		newList.Add(nodeNum + 1);

		if(nodeNumList.Contains(nodeNum - width))
		newList.Add(nodeNum - width);

		if(nodeNumList.Contains(nodeNum + width))
		newList.Add(nodeNum + width);

		return newList;
	}

	public void RevealMap()
	{
		foreach(GameObject node in nodeMap)
		{
			if(node != null)
			{node.SetActive(true);}
		}
	}

	public void SetDungeon(Dungeon dungeon)
	{this.dungeon = dungeon;}

	public void SetSelectedItem(int itemNum)
	{selectedItem = itemDatabase[itemNum];}

	//public void LoadItemList()
	//{
	//	foreach(KeyValuePair<string, int> value in PlayerData.saveData.others)
	//	{myItemList.Add(itemDatabase[Int32.Parse(value.Key)], value.Value);}
	//}

	public void ChangePage(bool value)
	{
		if(value)
		{currentPage++;}
		else{currentPage--;}

		if(currentPage < 0)
		{currentPage = 0;}

		if(currentPage >= pageLimit)
		{currentPage = pageLimit;}

		//UpdateItemPage();
	}
	public void SetEnemyCourse()

	{
		List<DungeonEnemy> keys = dungeonEnemies.Keys.ToList();



		for (int index = 0; index < keys.Count; index++)

		{

			DungeonEnemy enemy = keys[index];



			List<EEnemyDirection> dummy = new();



			if (nodeNumList.Contains(enemy.GetCurrentNodeNum() + 1) && !dungeonEnemies.ContainsValue(enemy.GetCurrentNodeNum() + 1))

				dummy.Add(EEnemyDirection.East);


			if (nodeNumList.Contains(enemy.GetCurrentNodeNum() - 1) && !dungeonEnemies.ContainsValue(enemy.GetCurrentNodeNum() - 1))

				dummy.Add(EEnemyDirection.West);


			if (nodeNumList.Contains(enemy.GetCurrentNodeNum() + width) && !dungeonEnemies.ContainsValue(enemy.GetCurrentNodeNum() + width))

				dummy.Add(EEnemyDirection.South);


			if (nodeNumList.Contains(enemy.GetCurrentNodeNum() - width) && !dungeonEnemies.ContainsValue(enemy.GetCurrentNodeNum() - width))

				dummy.Add(EEnemyDirection.North);



			if (dummy.Count == 0)

			{



			}

			else if (enemy.GetMoveLock())

			{


			}

			else if (dummy.Count == 1)

			{

				enemy.SetCurrentNodeNum(ReturnForwardNode(dummy[0], enemy.GetCurrentNodeNum()));

				dungeonEnemies[enemy] = enemy.GetCurrentNodeNum();

				enemy.SetEnemyDirection(dummy[0]);

			}

			else

			{

				dummy.Remove(ReturnReverseDirection(enemy.GetEnemyDirection()));

				int randomNum = Random.Range(0, dummy.Count);

				enemy.SetCurrentNodeNum(ReturnForwardNode(dummy[randomNum], enemy.GetCurrentNodeNum()));

				dungeonEnemies[enemy] = enemy.GetCurrentNodeNum();

				enemy.SetEnemyDirection(dummy[randomNum]);

			}



			enemy.SetMoveLock(enemy.GetCurrentNodeNum() + 1 == currentPlayerLocation

			|| enemy.GetCurrentNodeNum() - 1 == currentPlayerLocation

			|| enemy.GetCurrentNodeNum() + width == currentPlayerLocation

			|| enemy.GetCurrentNodeNum() - width == currentPlayerLocation);

		}

	}

	public void BackToCampsite()
	{
		SceneManager.LoadScene("Campsite");
		Destroy(gameObject);
	}

	private bool CheckBattleStart()
	{
		if(dungeonEnemies.Values.Contains(currentPlayerLocation))
		{return true;}
		else
		{
			List<DungeonEnemy> enemies = dungeonEnemies.Keys.ToList();

			for(int i = 0; i < enemies.Count; ++i)
			{
				if(enemies[i].GetCurrentNodeNum() == previousPlayerLocation
				&& EnemyFootStep(enemies[i]) == currentPlayerLocation)
				{return true;}
			}
			return false;
		}
		
		
	}

	public int EnemyFootStep(DungeonEnemy dungeonEnemy)
	{
		switch(dungeonEnemy.GetEnemyDirection())
		{
			case EEnemyDirection.North:
			return dungeonEnemy.GetCurrentNodeNum() + width;

			case EEnemyDirection.West:
			return dungeonEnemy.GetCurrentNodeNum() + 1;

			case EEnemyDirection.East:
			return dungeonEnemy.GetCurrentNodeNum() - 1;

			case EEnemyDirection.South:
			return dungeonEnemy.GetCurrentNodeNum() - width;
		}

		return 0;
	}

	private IEnumerator ReadyBattle()
	{
		moveLocked = true;
		yield return new WaitForSeconds(0.2f);

		int[] offsets = { -1 - width, -width, 1 - width, -1, 0, 1, -1 + width, width, 1 + width };

		List<Enemy> enemies = dungeonEnemies
			.Where(enemy => offsets.Contains(enemy.Value - currentPlayerLocation)) // 반경 1칸 내 적 필터링
			.Where(enemy => CheckConnect(enemy.Key.GetCurrentNodeNum())) // 벽이 없는지 확인
			.Select(enemy => enemy.Key.GetEnemy()) // 최종적으로 적 객체 리스트 변환
			.ToList();

		List<DungeonEnemy> selectedEnemies = dungeonEnemies
			.Where(enemy => offsets.Contains(enemy.Value - currentPlayerLocation)) // 반경 1칸 내 적 필터링
			.Where(enemy => CheckConnect(enemy.Key.GetCurrentNodeNum())) // 벽이 없는지 확인
			.Select(enemy => enemy.Key) // 최종적으로 적 객체 리스트 변환
			.ToList();


		foreach(DungeonEnemy selectedEnemy in selectedEnemies)
		{
			enemyObjectList.Remove(selectedEnemy.gameObject);
			dungeonEnemies.Remove(selectedEnemy);
		}

		DungeonData.map = map;
		DungeonData.currentPlayerLocation = currentPlayerLocation;
		DungeonData.previousPlayerLocation = previousPlayerLocation;
		DungeonData.currentFloor = floor;
		DungeonData.nodeNumList = nodeNumList;
		DungeonData.dungeonEnemies = dungeonEnemies;
		Dictionary<int, bool> activeNodes = new();
		Dictionary<int, bool> visitedNodes = new();


		foreach(int roomNum in nodeNumList)
		{activeNodes.Add(roomNum, nodeMap[roomNum].activeSelf);}

		foreach(int roomNum in nodeNumList)
		{visitedNodes.Add(roomNum, nodeMap[roomNum].GetComponent<RoomNode>().GetVisited());}


		

		DungeonData.activeNodes = activeNodes;
		DungeonData.visitedNodes = visitedNodes;

		BattleData.enemies = enemies;

		CameraController.Inst.ZoomIn(1f);
		StartCoroutine(FadeOut());
		yield return new WaitForSeconds(1f);

		DOTween.KillAll();

		SceneManager.LoadScene("Battle");
	}

	IEnumerator SetActiveBattleScene()
	{
		yield return new WaitForSeconds(0.2f);

		SceneManager.SetActiveScene(
			SceneManager.GetSceneByName("Battle")
		);
	}

	
	
	private bool CheckConnect(int enemyLocation)
	{
		int[] offsets = { 0, 1, width};

		if(offsets.Contains(Math.Abs(enemyLocation - currentPlayerLocation)))
		{return true;}

		int path1 = 0;
		int path2 = 0;

		if(enemyLocation - currentPlayerLocation == 1 + width) // 오른쪽 아래
		{
			path1 = currentPlayerLocation + width;
			path2 = currentPlayerLocation + 1;
		}
		else if(enemyLocation - currentPlayerLocation == 1 - width) // 오른쪽 위
		{
			path1 = currentPlayerLocation - width;
			path2 = currentPlayerLocation + 1;
		}
		else if(enemyLocation - currentPlayerLocation == -1 - width) // 왼쪽 위
		{
			path1 = currentPlayerLocation - width;
			path2 = currentPlayerLocation - 1;
		}
		else if(enemyLocation - currentPlayerLocation == -1 + width) // 왼쪽 아래
		{
			path1 = currentPlayerLocation + width;
			path2 = currentPlayerLocation - 1;
		}


		return nodeNumList.Contains(path1) || nodeNumList.Contains(path2);
	}

	private void MoveEnemy()
	{
		if(dungeonEnemies.Count == 0)
		{return;}
		
		int count = 0;

		foreach(KeyValuePair<DungeonEnemy, int> dungeonEnemy in dungeonEnemies)
		{

			enemyObjectList[count].transform.DOMove(CalculateNodePosition(dungeonEnemy.Value) + mapObject.transform.position, moveDuration)
			.SetEase(Ease.OutQuad)
			.OnStart(() => {
				if(nodeMap[dungeonEnemy.Key.GetCurrentNodeNum()].gameObject.activeSelf && !dungeonEnemy.Key.gameObject.activeSelf) //방문한 곳이고 자신이 지금 visible false라면?
				{StartCoroutine(dungeonEnemy.Key.FadeOut());}

			})
			.OnComplete(() => {
				if(!nodeMap[dungeonEnemy.Key.GetCurrentNodeNum()].gameObject.activeSelf && dungeonEnemy.Key.gameObject.activeSelf) //방문한 곳이 아니고 자신이 지금 visible True라면?
				{StartCoroutine(dungeonEnemy.Key.FadeIn());}
				});
			count++;
		}
	}



	private int ReturnForwardNode(EEnemyDirection enemyDirection, int nodeNum)
	{
		switch(enemyDirection)
		{
			case EEnemyDirection.North:
			return nodeNum - width;

			case EEnemyDirection.South:
			return nodeNum + width;

			case EEnemyDirection.East:
			return nodeNum + 1;

			case EEnemyDirection.West:
			return nodeNum - 1;
		}
		return -1;
	}

	private EEnemyDirection ReturnReverseDirection(EEnemyDirection enemyDirection)
	{
		switch(enemyDirection)
		{
			case EEnemyDirection.North:
			return EEnemyDirection.South;

			case EEnemyDirection.South:
			return EEnemyDirection.North;

			case EEnemyDirection.East:
			return EEnemyDirection.West;

			case EEnemyDirection.West:
			return EEnemyDirection.East;
		}
		return EEnemyDirection.None;
	}


	public void ShowItemDescription(int itemNum)
	{
		itemDescriptionWindow.SetActive(true);
		itemDescriptionWindow.GetComponent<DescriptionWindow>().SetUp(itemDatabase[itemNum].GetName(),
		itemDatabase[itemNum].GetItemDescription());
	}

	public void HideItemDescription()
	{itemDescriptionWindow.SetActive(false);}


	private void DungeonSetUp()
	{
		dungeon = DungeonData.dungeon;
		dungeonName = dungeon.GetDungeonName();
		width = dungeon.GetDungeonWidth();
		height = dungeon.GetDungeonHeight();
		floorSize = dungeon.GetDungeonFloorSize();
		maxGold = dungeon.GetMaxGold();
		dungeonEndFloor = dungeon.GetDungeonEndFloor();
		safeFloorList = dungeon.GetSafeFloorList();
		itemList = dungeon.GetItemList();
		enemyLimit = dungeon.GetEnemyLimit();
	}

	void AlertPopUpMessage(string message)
	{
		GameObject onMessage = Instantiate(alertMessage, popUpMessageWindow.transform);
		onMessage.GetComponent<AlertMessage>().SetText(message);
		StartCoroutine(onMessage.GetComponent<AlertMessage>().FadeAway());
	}

	void EnqueueMove(Vector2 direction)
	{
		moveQueue.Enqueue(direction);

		if (!isMoving)
		{StartNextMove();}
	}

	void StartNextMove()
	{
		if (moveQueue.Count > 0)
		{
			isMoving = true;
			Vector2 direction = moveQueue.Dequeue();
			Vector3 targetPosition = player.transform.position + (Vector3)direction * moveDistance;

			
			SetEnemyCourse();

			soundEffect.PlayOneShot(runningInGrassSound[Random.Range(0, runningInGrassSound.Count)]);



			player.transform.DOMove(targetPosition, moveDuration)
				.SetEase(Ease.OutQuad) // 부드러운 감속 효과
				.OnComplete(() => {
					isMoving = false;
					StartNextMove(); // 다음 이동 실행
				});
			
			// if(CheckBattleStart())
			// {moveDistance = moveDistance / 2;}

			MoveEnemy();
			if(CheckBattleStart())
			{StartCoroutine(ReadyBattle());}
		}
		
	}

	public void CloseItemInfo()
	{
		if(clickedItem == null)
		{return;}

		if(clickedItem != null)
		{
			clickedItem = null;
			Destroy(clickedItemInfo.gameObject);
		}
	}

	//public bool Foo(EventSelection selectionNode)
	//{
	//	if(selectionNode == null)
	//	return false;

	//	switch(selectionNode.GetRequireType())
	//	{
	//		case ERequireType.None:
	//		return true;

	//		case ERequireType.EGold:
	//		return selectionNode.GetRequireGold() <= PlayerManager.Inst.GetGold();

	//		case ERequireType.EHealth:
	//		return selectionNode.GetRequireHealth() <= PlayerManager.Inst.GetHealth();

	//		// case ERequireType.EItem:
	//		// return myItemList.Contains(selectionNode.GetRequireItem());

	//		case ERequireType.ECard:
	//		return true;
	//	}
	//	return false;

	//	//아이템 종류
		
	//}


	

	private ItemData ReturnDungeonItem()
	{
		if(itemList != null)
		{   
			int count = 0;
			Dictionary<ItemData, int> rewardRoullet = new();

			foreach(KeyValuePair<ItemData, int> reward in itemList)
			{
				count += reward.Value;
				rewardRoullet.Add(reward.Key, count);
			}

			int randomNum = Random.Range(0, count + 1);

			foreach(KeyValuePair<ItemData, int> reward in rewardRoullet)
			{
				if(randomNum <= reward.Value)
				{
					randomNum = Random.Range(1, 3);
					return reward.Key;
				}
			}
		}

		return null;
	}

	public void SetEnemyInNode(Node node)
	{
		node.SetEnemy(new UnknownMonster());
	}

	public void ReCreateFloor()
	{
		
		map = DungeonData.map;
		currentPlayerLocation = DungeonData.currentPlayerLocation;
		previousPlayerLocation = DungeonData.previousPlayerLocation;
		floor = DungeonData.currentFloor;
		floorText.text = floor.ToString() + "F";


		dungeonEnemies = DungeonData.dungeonEnemies;
		
		nodeNumList = DungeonData.nodeNumList;

		InstantiateNode();

		foreach(KeyValuePair<int, bool> keyValuePair in DungeonData.activeNodes)
		{nodeMap[keyValuePair.Key].SetActive(keyValuePair.Value);}

		foreach(KeyValuePair<int, bool> keyValuePair in DungeonData.visitedNodes)
		{nodeMap[keyValuePair.Key].GetComponent<RoomNode>().SetVisited(keyValuePair.Value);}

		MovePlayer(currentPlayerLocation);
		nodeMap[currentPlayerLocation].GetComponent<RoomNode>().SetVisited();
		player.transform.position = CalculateNodePosition(currentPlayerLocation) + mapObject.transform.position;
		camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, camera.transform.position.z);
	   
		ReCreateEnemy();
		//LoadItemList();
		//UpdateItemPage();
	}


	public void CreateFloor()
	{
		// 맵 생성 재시도 제한
		const int MAX_ATTEMPTS = 10;
		int attempts = 0;

		do
		{
			map = new List<Node>(new Node[floorSize]);
			CreateFirstRoom();
			CreateNodeNumList();
			attempts++;
		}
		while (nodeNumList.Count < 40 && attempts < MAX_ATTEMPTS);

		if (nodeNumList.Count < 40)
		{
			Debug.LogWarning("Dungeon generation failed to meet minimum node count.");
		}

		// 층 표시
		floorText.text = floor + "F";

		// 복도 추가 및 노드 설정
		AddLoopCorridor();
		CreateNodeNumList();

		CreateSecretRoom();
		SetNodeRoom();

		// 프리팹 생성
		InstantiateNode();

		// 플레이어 위치 설정
		MovePlayer(currentPlayerLocation);
		player.transform.position = CalculateNodePosition(currentPlayerLocation) + mapObject.position;
		camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, camera.transform.position.z);

		// 적 및 장식 생성
		CreateEnemy();
		DecorateFloor();
	}

	private void InstantiateNode()
	{
		nodeMap = new List<GameObject>(new GameObject[floorSize]);

		for (int i = 0; i < map.Count; ++i)
		{
			GameObject prefab;
			ERoomType roomType = map[i] != null ? map[i].GetRoomType() : ERoomType.EWall;

			prefab = GetPrefabByRoomType(roomType);

			GameObject nodeObj = Instantiate(prefab, Vector3.zero, Utils.QI);
			nodeObj.transform.SetParent(mapObject);
			nodeObj.transform.position = mapObject.position + CalculateNodePosition(i);

			RoomNode roomNode = nodeObj.GetComponent<RoomNode>();
			roomNode.SetNodeNum(i);
			roomNode.SetRoomType(roomType);
			roomNode.filled = (roomType != ERoomType.EWall);

			nodeObj.SetActive(false);

			nodeMap[i] = nodeObj;
		}
	}

	private GameObject GetPrefabByRoomType(ERoomType roomType)
	{
		switch (roomType)
		{
			case ERoomType.EStair: return stairNodePrefab;
			case ERoomType.EEncount: return encounterNodePrefab;
			case ERoomType.EGold: return itemNodePrefab;
			case ERoomType.EItem: return itemNodePrefab;
			case ERoomType.EObstacle: return monsterNodePrefab;
			case ERoomType.None: return roomNodePrefab;
			case ERoomType.EWall:
			default: return wallNodePrefab;
		}
	}

	public void LockControl()
	{ moveLocked = true; }

	public void UnlockControl()
	{ moveLocked = false; }

	private void SetNodeRoom()
	{
		if (nodeNumList.Count == 0) return;

		int encounterIdx = Random.Range(0, nodeNumList.Count);
		map[nodeNumList[encounterIdx]].SetRoomType(ERoomType.EEncount);
		map[nodeNumList[encounterIdx]].SetDialogueNum(dungeon.GetDialogueList()[0]);

		HashSet<int> used = new HashSet<int>();
		for (int i = 0; i < 5 && used.Count < nodeNumList.Count; ++i)
		{
			int num;
			do { num = Random.Range(0, nodeNumList.Count); }
			while (used.Contains(num));

			used.Add(num);

			int node = nodeNumList[num];
			if (Random.Range(0, 10) == 0)
			{
				map[node].SetRoomType(ERoomType.EItem);
				map[node].SetItem(ReturnDungeonItem());
			}
			else
			{
				map[node].SetRoomType(ERoomType.EGold);
				map[node].SetGold(Random.Range(1, maxGold + 1));
			}
		}

		int stairIdx;
		do
		{
			stairIdx = Random.Range(0, nodeNumList.Count);
		}
		while (map[nodeNumList[stairIdx]].GetRoomType() != ERoomType.None
			   || secretRoomNodes.Contains(nodeNumList[stairIdx]));

		map[nodeNumList[stairIdx]].SetRoomType(ERoomType.EStair);

		int startIdx;
		do { startIdx = Random.Range(0, nodeNumList.Count); }
		while (map[nodeNumList[startIdx]].GetRoomType() != ERoomType.None);

		currentPlayerLocation = nodeNumList[startIdx];
		previousPlayerLocation = currentPlayerLocation;
	}

	public void AddLoopCorridor()
	{
		if (nodeNumList.Count < 2) return;

		int random = Random.Range(0, 3);
		int first = nodeNumList[random];
		int last = nodeNumList[nodeNumList.Count - 1];

		// 항상 작은 값 → 큰 값 순서로 정렬
		if (first > last) (first, last) = (last, first);

		int startRow = first / width;
		int endRow = last / width;

		int current = first;

		// 세로로 이동
		for (int row = startRow; row < endRow; ++row)
		{
			current += width;
			if (map[current] == null) map[current] = new Node();
			if (!nodeNumList.Contains(current)) nodeNumList.Add(current);
		}

		// 가로로 이동
		for (int col = current; col < last; ++col)
		{
			current += 1;
			if (map[current] == null) map[current] = new Node();
			if (!nodeNumList.Contains(current)) nodeNumList.Add(current);
		}
	}

	private void CreateFirstRoom()
	{
		int roomNum = floorSize / 2;
		map[roomNum] = new Node();
		map[roomNum].SetRoomType(ERoomType.None);

		CreateRoomNode(roomNum + 1);
		CreateRoomNode(roomNum - 1);
		CreateRoomNode(roomNum + width);
		CreateRoomNode(roomNum - width);
	}

	private void CreateNodeNumList()
	{
		nodeNumList = new List<int>();
		for (int i = 0; i < map.Count; ++i)
		{
			if (map[i] != null)
				nodeNumList.Add(i);
		}
	}

	public void DecorateFloor()
	{
		for (int i = 0; i < nodeMap.Count; ++i)
		{
			if (Random.Range(0, 2) == 1) continue;

			RoomNode node = nodeMap[i]?.GetComponent<RoomNode>();
			if (node != null && !node.filled && node.GetRoomType() == ERoomType.EWall)
			{
				node.filled = true;
				var sr = node.GetComponent<SpriteRenderer>();
				sr.sprite = decorateBlock;
				sr.color = Color.red; // 나중에 테마 기반 색상 적용 가능
			}
		}
	}
	public void ReCreateEnemy()
	{

		foreach(KeyValuePair<DungeonEnemy, int> dungeonEnemy in dungeonEnemies)
		{
			GameObject enemyObject = Instantiate(dungeonEnemyPrefab);
			enemyObject.GetComponent<DungeonEnemy>().SetEnemy(dungeonEnemy.Key.GetEnemy());
			enemyObject.GetComponent<DungeonEnemy>().SetCurrentNodeNum(dungeonEnemy.Value);
			enemyObject.GetComponent<DungeonEnemy>().SetEnemyDirection(dungeonEnemy.Key.GetEnemyDirection());
			enemyObject.GetComponent<DungeonEnemy>().SetVisible(dungeonEnemy.Key.GetVisible());

			enemyObject.GetComponent<DungeonEnemy>().SetMoveLock(
			dungeonEnemy.Value + 1 == currentPlayerLocation
			||dungeonEnemy.Value - 1 == currentPlayerLocation
			||dungeonEnemy.Value + width == currentPlayerLocation
			||dungeonEnemy.Value - width == currentPlayerLocation
			);

			enemyObjectList.Add(enemyObject);
		}


		dungeonEnemies = new Dictionary<DungeonEnemy, int>();

		foreach(GameObject dungeonEnemy in enemyObjectList)
		{dungeonEnemies.Add(dungeonEnemy.GetComponent<DungeonEnemy>(), dungeonEnemy.GetComponent<DungeonEnemy>().GetCurrentNodeNum());}

		MoveEnemy();
	}

	public void CreateEnemy()
	{
		List<int> usedNumbers = new List<int>();
		dungeonEnemies.Clear();

		for(int i = 0; i < enemyLimit; ++i)
		{
			int num;
			do {num = Random.Range(0, nodeNumList.Count);}
			while (usedNumbers.Contains(num) || nodeNumList[num] == currentPlayerLocation);
			
			usedNumbers.Add(nodeNumList[num]);
		}

		for(int i = 0; i < enemyLimit; ++i)
		{
			GameObject enemyObject = Instantiate(dungeonEnemyPrefab);

			enemyObject.GetComponent<DungeonEnemy>().SetEnemy(new UnknownMonster());
			enemyObject.GetComponent<DungeonEnemy>().SetCurrentNodeNum(usedNumbers[i]);

			List<EEnemyDirection> dummy = new();

			if(nodeNumList.Contains(usedNumbers[i] + 1)) 
				dummy.Add(EEnemyDirection.East);
			
			if(nodeNumList.Contains(usedNumbers[i] - 1)) 
				dummy.Add(EEnemyDirection.West);
			
			if(nodeNumList.Contains(usedNumbers[i] + width)) 
				dummy.Add(EEnemyDirection.South);
			
			if(nodeNumList.Contains(usedNumbers[i] - width)) 
				dummy.Add(EEnemyDirection.North);

			dungeonEnemies.Add(enemyObject.GetComponent<DungeonEnemy>(), usedNumbers[i]);

			int randomNum = Random.Range(0, dummy.Count);
			enemyObject.GetComponent<DungeonEnemy>().SetEnemyDirection(dummy[randomNum]);

			enemyObjectList.Add(enemyObject);

			enemyObject.GetComponent<DungeonEnemy>().SetVisible(nodeMap[enemyObject.GetComponent<DungeonEnemy>().GetCurrentNodeNum()]
			.GetComponent<RoomNode>().GetVisited());
			enemyObject.GetComponent<DungeonEnemy>().SetMoveLock(
			dungeonEnemies[enemyObject.GetComponent<DungeonEnemy>()] + 1 == currentPlayerLocation
			||dungeonEnemies[enemyObject.GetComponent<DungeonEnemy>()] - 1 == currentPlayerLocation
			||dungeonEnemies[enemyObject.GetComponent<DungeonEnemy>()] + width == currentPlayerLocation
			||dungeonEnemies[enemyObject.GetComponent<DungeonEnemy>()] - width == currentPlayerLocation
			);

		}
		MoveEnemy();
	}

	public void DestroyFloor()
	{
		foreach(GameObject node in nodeMap)
		{Destroy(node);}

		foreach(GameObject enemyObject in enemyObjectList)
		{
			DOTween.Kill(enemyObject);
			Destroy(enemyObject);
		}
		enemyObjectList.Clear();

	}





	private void CreateRoomNode(int roomNum)
	{
		if(!CheckOutOfIndex(roomNum))
		{return;}

		if(map[roomNum] != null)
		{return;}
		if(roomNum % width == width - 1)
		return;

		if(TrueOrFalse())
		{return;}

		if(CountNeighbourhood(roomNum) >= 2)
		{return;}

		map[roomNum] = new Node();

	
		CreateRoomNode(roomNum + 1);
		CreateRoomNode(roomNum - 1);
		CreateRoomNode(roomNum + width);
		CreateRoomNode(roomNum - width);
	}

	private Vector3 CalculateNodePosition(int roomNum)
	{
		int x = ((roomNum  % width) - 9) * 2;
		int y = -(((roomNum / width) - 8) * 2);

		return new Vector3(x, y, 0);
	}

	private int CountNeighbourhood(int roomNum)
	{
		int count = 0;

		if(CheckOutOfIndex(roomNum - 1))
		{
			if(map[roomNum - 1] != null)
			{count++;}
		}

		if(CheckOutOfIndex(roomNum + 1))
		{
			if(map[roomNum + 1] != null)
			{count++;}
		}

		if(CheckOutOfIndex(roomNum + width))
		{
			if(map[roomNum + width] != null)
			{count++;}
		}

		if(CheckOutOfIndex(roomNum - width))
		{
			if(map[roomNum - width] != null)
			{count++;}
		}

		return count;

	}

	private bool CheckOutOfIndex(int roomNum)
	{
		if(roomNum < 0)
		{return false;}

		if(roomNum > floorSize - 1)
		{return false;}

		return true;
	}

	private int GetNextIndex(int currentIndex, int direction)
	{
		// direction: 0=위, 1=아래, 2=왼쪽, 3=오른쪽
		int nextIndex = currentIndex;

		switch (direction)
		{
			case 0: // 위
				nextIndex = currentIndex - width;
				break;
			case 1: // 아래
				nextIndex = currentIndex + width;
				break;
			case 2: // 왼쪽
					// 같은 행에 있을 때만 이동
				if (currentIndex % width != 0)
					nextIndex = currentIndex - 1;
				else
					return -1; // 왼쪽 끝이면 이동 불가
				break;
			case 3: // 오른쪽
					// 같은 행에 있을 때만 이동
				if (currentIndex % width != width - 1)
					nextIndex = currentIndex + 1;
				else
					return -1; // 오른쪽 끝이면 이동 불가
				break;
		}

		// 범위 밖 체크
		if (!CheckOutOfIndex(nextIndex))
			return -1;

		return nextIndex;
	}



	private bool CheckCorridor(int value)
	{

		if(!CheckOutOfIndex(value))
		{return true;}

		if(map[value] == null)
		{return true;}

		if(CountNeighbourhood(value) != 2)
		{return false;}

		if(CheckOutOfIndex(value - 1) && CheckOutOfIndex(value + 1))
		{
			if(map[value + 1] != null && map[value - 1] != null)
			{return true;}
		}

		if(CheckOutOfIndex(value - width) && CheckOutOfIndex(value + width))
		{
			if(map[value + width] != null && map[value - width] != null)
			{return true;}
		}
		
		return false;
	}

	public void GoToNextFloor()
	{
		floor++;
		moveLocked = false;
		StartCoroutine(ReadyNextFloor());

	}

	private IEnumerator ReadyNextFloor()
	{
		StartCoroutine(FadeOut());
		yield return new WaitForSeconds(1f);
		if(safeFloorList.ContainsKey(floor))
		{
			SceneManager.LoadScene(safeFloorList[floor]);
		}
		else if(dungeonEndFloor == floor)
		{
			Debug.Log("던전을 클리어 했습니다");
			dungeonClearWindow.GetComponent<Window>().OnOff();
			moveLocked = true;
			DungeonData.Reset();
		}
		else
		{
			DestroyFloor();
			CreateFloor();
			CameraController.Inst.SetFollowing();
		}


		StartCoroutine(FadeIn());
	}

	public void RemoveClickedItem()
	{
		//if (myItemList.ContainsKey(clickedItem))
		//{
		//	myItemList[clickedItem]--;

		//	if (myItemList[clickedItem] <= 0)
		//	{myItemList.Remove(clickedItem);}
		//}
		//UpdateItemPage();
	}

	public void UseItem()
	{

		switch(clickedItem.GetNum())
		{
			case "1": // 거대한포션
				PlayerData.saveData.health += 5;
				if (PlayerData.saveData.health > 30)
					PlayerData.saveData.health = 30;
				break;

			case "2": // 황금주사위
			
			break;

			case "3": // 부숴진나침반
			
			break;

			case "4": // 불길한향로
			
			break;

			case "5": // 불길한향로
			
			break;

			case "6": // 빨간 포션
				PlayerData.saveData.health += 2;

				if (PlayerData.saveData.health > 30)
					PlayerData.saveData.health = 30;
				break;
		}

		AlertPopUpMessage(clickedItem.GetName() + "을(를) 사용하였습니다.");
	}

	public void UpdatePlayerData()
	{
		goldText.text = PlayerData.saveData.gold.ToString();
		healthText.text = PlayerData.saveData.health.ToString() +" / 30";

		if(PlayerData.saveData.health <= 0)
		{
			updateLock = true;
			PlayerData.saveData.health = 0;

			StopAllCoroutines();
			StartCoroutine(GameOver());
		}
	}

	public IEnumerator GameOver()
	{
		StartCoroutine(FadeOut());
		yield return new WaitForSeconds(1f);
		float elapsed = 0f;
		while (elapsed < 1)
		{
			elapsed += Time.deltaTime;
			gameOverWindow.GetComponent<CanvasGroup>().alpha = Mathf.Clamp01(elapsed / 1);
			yield return null;
		}

		gameOverWindow.GetComponent<CanvasGroup>().interactable = true;
		gameOverWindow.GetComponent<CanvasGroup>().blocksRaycasts = true;
		yield return null;
	}

	public void SelectUsingItem(ItemData item)
	{
		clickedItem = item;
		itemAlert.GetComponent<Window>().OnOff();
		itemAlert.GetComponent<ItemAlert>().SetText(clickedItem.GetName());
	}

	public void MoveLock(bool value)
	{ moveLocked = value; }
	

	public void MovePlayer(int roomNum)
	{

		nodeMap[roomNum].SetActive(true);
		nodeMap[roomNum].GetComponent<RoomNode>().SetVisited();

		if(CheckOutOfIndex(roomNum + 1) && roomNum % width != width - 1)
		{

			if(nodeMap[roomNum + 1].GetComponent<RoomNode>().filled)
			{
				if (!nodeMap[roomNum + 1].activeSelf)
				{
					nodeMap[roomNum + 1].SetActive(true);
					StartCoroutine(nodeMap[roomNum + 1].GetComponent<RoomNode>().FadeOut());
				}
			}
			
		}

		if(CheckOutOfIndex(roomNum + width))
		{
			if(nodeMap[roomNum + width].GetComponent<RoomNode>().filled)
			{
				if (!nodeMap[roomNum + width].activeSelf)
				{
					nodeMap[roomNum + width].SetActive(true);
					StartCoroutine(nodeMap[roomNum + width].GetComponent<RoomNode>().FadeOut());
				}

			}
			
		}

		if(CheckOutOfIndex(roomNum - 1) && roomNum % width != 0)
		{
			if(nodeMap[roomNum - 1].GetComponent<RoomNode>().filled)
			{
				if (!nodeMap[roomNum - 1].activeSelf)
				{
					nodeMap[roomNum - 1].SetActive(true);
					StartCoroutine(nodeMap[roomNum - 1].GetComponent<RoomNode>().FadeOut());
				}
			}
			


		}

		if(CheckOutOfIndex(roomNum - width))
		{
			if(nodeMap[roomNum - width].GetComponent<RoomNode>().filled)
			{
				if (!nodeMap[roomNum - width].activeSelf)
				{
					nodeMap[roomNum - width].SetActive(true);
					StartCoroutine(nodeMap[roomNum - width].GetComponent<RoomNode>().FadeOut());
				}
			}
			
		}

		if(map[roomNum].GetRoomType() == ERoomType.EStair)
		{ShowStairAlert();}
		else if(map[roomNum].GetRoomType() == ERoomType.EEncount)
		{
			ShowEncounter(map[roomNum].GetDialogueNum());
			map[roomNum].SetRoomType(ERoomType.None);
			nodeMap[roomNum].GetComponent<RoomNode>().ClearRoom();
		}
		else if(map[roomNum].GetRoomType() == ERoomType.EGold)
		{
			GainGold(map[roomNum]);
			map[roomNum].SetRoomType(ERoomType.None);
			nodeMap[roomNum].GetComponent<RoomNode>().ClearRoom();
		}
		else if(map[roomNum].GetRoomType() == ERoomType.EItem)
		{
			switch(map[roomNum].GetItem().GetItemCategory())
			{
				case EItemCategory.ETool:
					if (PlayerData.saveData.inventory_items.Count <= 8)
					{
						PlayerData.saveData.inventory_items.Add(map[roomNum].GetItem().GetNum());
						AlertPopUpMessage(map[roomNum].GetItem().GetName() + " " + " 획득");
						map[roomNum].SetRoomType(ERoomType.None);
						nodeMap[roomNum].GetComponent<RoomNode>().ClearRoom();
					}
					else
					{
						AlertPopUpMessage("아이템을 얻을 수 없습니다.");
					}
					break;
			}
			
		}
	}

	private void ShowEncounter(int value)
	{
		DialogueManager.Inst.SetLockTarget(this);
		DialogueManager.Inst.StartDialogue(value);
	}



	private void GainGold(Node node)
	{
		PlayerData.saveData.gold += node.GetGold();
		AlertPopUpMessage(node.GetGold().ToString() + " " +"골드 획득");
	}

	private void GainItem(Node node)
	{
		AlertPopUpMessage(node.GetItem().GetName() + " " +" 획득");
		//if (myItemList.ContainsKey(node.GetItem()))
		//{myItemList[node.GetItem()]++;}
		//else
		//{myItemList.Add(node.GetItem(), 1);}
	}

	public void ShowStairAlert()
	{
		moveLocked = true;
		stairAlert.GetComponent<Window>().OnOff();
	}

	public void HideStairAlert()
	{
		moveLocked = false;
		stairAlert.GetComponent<Window>().OnOff();
	}

	public void DrawDragLine(Vector2 startPoint, bool isUsuable)
	{
		 Vector3[] point = new Vector3[lineCount];
		float posA = 3f;
		float posB = 3f;
		cardDragLine.positionCount = lineCount;

		if(isUsuable)
		{cardDragLine.endColor = Color.blue;}
		else
		{cardDragLine.endColor = Color.red;}
		
		Vector3 targetPoint = camera.ScreenToWorldPoint(Input.mousePosition);

		startPoint = camera.ScreenToWorldPoint(startPoint);

		for(int i = 0; i < lineCount; ++i)
		{
			float t;
			if (i == 0)
			{t = 0;}
			else
			{t = (float)i / (lineCount - 1);}
			
			point[i] = Bezier(startPoint, PointSetting(startPoint),
			PointSetting(targetPoint),targetPoint, t);
			point[i].z = 0;
		}
		cardDragLine.SetPositions(point);
		

		Vector3 PointSetting(Vector3 origin){
			float x, y;
			x = posA * Mathf.Cos(120 * Mathf.Deg2Rad) + origin.x;
			y = posB * Mathf.Sin(120 * Mathf.Deg2Rad) + origin.y;
	
			return new Vector3(x, y);
		}
		Vector3 Bezier(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, float t)
		{
			Vector3 M0 = Vector3.Lerp(P0, P1, t);
			Vector3 M1 = Vector3.Lerp(P1, P2, t);
			Vector3 M2 = Vector3.Lerp(P2, P3, t);

			Vector3 B0 = Vector3.Lerp(M0, M1, t);
			Vector3 B1 = Vector3.Lerp(M1, M2, t);

			return Vector3.Lerp(B0, B1, t);
		}
	}

	public void ActivateTeleport()
	{
		int randomNum = Random.Range(0, nodeNumList.Count);

		previousPlayerLocation = currentPlayerLocation;
		currentPlayerLocation = nodeNumList[randomNum];
		MovePlayer(nodeNumList[randomNum]);
		player.transform.position = CalculateNodePosition(currentPlayerLocation) + mapObject.transform.position;
		CameraController.Inst.SetFollowing();

	}

	public void DeleteDragLine()
	{
		cardDragLine.positionCount = 0;
		cardDragLine.endColor = Color.blue;
	}


	public bool CheckCardUsable(CardData cardData, int nodeNum)
	{
		if(mouseOnRoomNum == 0)
		{return false;}

		return true;
	}


	public int ReturnMouseOnNode()
	{return mouseOnRoomNum;}

	public void SetMouseOnNode(int roomNum)
	{mouseOnRoomNum = roomNum;}

	public void ResetMouseOnNode()
	{mouseOnRoomNum = 0;}

	private void ApplyEncountResult(int encounterNum, int value)
	{
		switch(encounterNum)
		{
			//모자장수
			case 0:
			switch(value)
			{
				case 0:
				break;

				case 1:
				break;

				case 2:
				break;
			}
			break;
		}
	}

	private bool TrueOrFalse()
	{
		int value = Random.Range(0,3);
		
		if(value == 0)
		{return true;}
		else
		{return false;}
	}
}

public enum ERoomType
{
	None,
	EStair,
	EEncount,
	EGold,
	EItem,
	EWall,
	EObstacle // 퍼즐로 제거될 장애물
}
public enum EDirection
{North,South,West,East}