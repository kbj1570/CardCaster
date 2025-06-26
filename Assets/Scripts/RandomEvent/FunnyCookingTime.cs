using System.Collections.Generic;
public class FunnyCookingTime : RandomEvent
{
	public FunnyCookingTime()
	{
		eventName = "즐거운 요리시간";
		eventNum = "001";
		eventDesc = "냄비를 발견했다. 요리를 해보자.";


		eventNodes = new Dictionary<string, EventNode>() {
			{"node_00", new EventNode(){id = "",
				desc = new List<string>(){"어떤 재료를 넣어볼까?" },
				eventSelections = new()
				{new EventSelection(){text = "대파",
					resultText = new List<string>(){"맛있는 냄새가 난다" },
					nextNodeId = "node_01",
					requireType = ERequireType.None,
					requireValue = "",
					effect = EEventEffectType.EAddValue,
					effectValue = "1"
					},
				new EventSelection(){text = "감자",
					resultText = new List<string>(){"뭔가 이상하다.." },
					nextNodeId = "node_01",
					requireType = ERequireType.None,
					requireValue = "",
					effect = EEventEffectType.EAddValue,
					effectValue = "-1"
					}
				}
				
			}},
			{"node_01", new EventNode(){id = "",
				desc = new List<string>(){"그 다음은.,." },
				eventSelections = new()
				{new EventSelection(){text = "소금을 넣는다.",
					resultText = new List<string>(){"맛있는 냄새가 난다." },
					nextNodeId = "node_02",
					requireType = ERequireType.None,
					requireValue = "",
					effect = EEventEffectType.EAddValue,
					effectValue = "1"
					},
				new EventSelection(){text = "설탕을 넣는다.",
					resultText = new List<string>(){"뭔가 이상하다.." },
					nextNodeId = "node_02",
					requireType = ERequireType.None,
					requireValue = "",
					effect = EEventEffectType.EAddValue,
					effectValue = "-1"
					}
				}

			}},
			{"node_02", new EventNode(){id = "",
				desc = new List<string>(){"요리는 어느덧 막바지로 향해간다.",
					"보글보글 끓기 시작한 냄비에는 잔거품이 잔뜩 일어나 조금씩 넘치기 시작한다. 어떻게 할까?" },
				eventSelections = new()
				{new EventSelection(){text = "불을 꺼버린다.",
					resultText = new List<string>(){""},
					nextNodeId = "node_03",
					requireType = ERequireType.None,
					requireValue = "",
					effect = EEventEffectType.EAddValue,
					effectValue = "-1"
					},
				new EventSelection(){
					text = "물을 더 넣는다",
					resultText = new List<string>(){"거품이 가라앉았다."},
					nextNodeId = "node_03",
					requireType = ERequireType.None,
					requireValue = "",
					effect = EEventEffectType.EAddValue,
					effectValue = "1"
					}
				}

			} },
			{"node_03", new EventNode(){id = "", desc = new List<string>(){""},
				eventSelections = null
			}
		}};
	}

	public override string GetResult()
	{
		if(resultValue < 0)
		{
			return "요리는 완전 망했다.";
		}
		else if(resultValue == 0)
		{
			return "그냥저냥 괜찮다.";
		}
		else
		{
			return "성공적인 요리였다.";
		}

	}


}