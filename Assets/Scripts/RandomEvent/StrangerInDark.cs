using System.Collections.Generic;
public class StrangerInDark : RandomEvent
{
	public StrangerInDark()
	{
		eventName = "어둠속의 이방인";
		eventNum = "002";
		eventDesc = new List<string>() { "갑자기 주위가 어두워지기 시작했다. 잠시 후, 등 뒤에서 어떤 목소리가 들려온다."," 요리를 해보자." };


		eventNodes = new Dictionary<string, EventNode>() {
			{"node_00", new EventNode(){id = "",
				desc = new List<string>(){ "갑자기 주위가 어두워지기 시작했다. 잠시 후, 등 뒤에서 어떤 목소리가 들려온다.", "" },
				eventSelections = new()
				{new EventSelection(){text = "물건을 준다.",
					resultText = new List<string>(){"" },
					nextNodeId = "node_01",
					requireType = ERequireType.None,
					requireValue = "",
					effect = EEventEffectType.None,
					},
				new EventSelection(){text = "물건을 주지 않는다.",
					resultText = new List<string>(){""},
					nextNodeId = "node_02",
					requireType = ERequireType.None,
					requireValue = "",
					effect = EEventEffectType.None,
					}
				}
				
			}},
			{"node_01", new EventNode(){id = "", desc = new List<string>(){""},
				eventSelections = null

			}},
			{"node_02", new EventNode(){id = "", desc = new List<string>(){""},
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