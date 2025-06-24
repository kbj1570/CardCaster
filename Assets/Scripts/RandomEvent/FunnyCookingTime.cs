using System.Collections.Generic;
public class FunnyCookingTime : RandomEvent
{
	int value;
	public FunnyCookingTime()
	{
		eventName = "즐거운 요리시간";
		eventNum = "001";
		eventDesc = "";
		value = 0;

		eventNodes = new Dictionary<string, EventNode>() {
			{"node_01", new EventNode(){id = "", desc = "어떤 재료를 넣어볼까?",
				eventSelections = new()
				{new EventSelection(){text = "",
					resultText = "",
					nextNodeId = "node_02",
					requireType = ERequireType.None,
					requireValue = ""
					}
				}
			}},
			{"node_02", new EventNode(){id = "", desc = "그 다음은.,.",
				eventSelections = new()
				{new EventSelection(){text = "소금을 넣는다.",
					resultText = "맛있는 냄새가 난다.",
					nextNodeId = "node_03",
					requireType = ERequireType.None,
					requireValue = ""
					},
				new EventSelection(){text = "설탕을 넣는다.",
					resultText = "뭔가 이상하다..",
					nextNodeId = "node_03",
					requireType = ERequireType.None,
					requireValue = ""
					}
				}

			}},
			{"node_03", new EventNode(){id = "", desc = "요리는 어느덧 막바지로 향해간다.보글보글 끓기 시작한 냄비에는 잔거품이 잔뜩 일어나 조금씩 넘치기 시작한다. 어떻게 할까?",
				eventSelections = new()
				{new EventSelection(){text = "불을 꺼버린다.",
					resultText = "",
					nextNodeId = "node_04",
					requireType = ERequireType.None,
					requireValue = ""
					},
				new EventSelection(){
					text = "물을 더 넣는다",
					resultText = "거품이 가라앉았다.",
					nextNodeId = "node_04",
					requireType = ERequireType.None,
					requireValue = "",
					effect = "",
					effectValue = ""
					}
				}

			} },
			{"node_04", new EventNode(){id = "", desc = "",
				eventSelections = null
			}
		}};
	}

	public void GetResult()
	{
		if(value < 0)
		{

		}
		else if(value == 0)
		{

		}
		else
		{

		}

	}


}