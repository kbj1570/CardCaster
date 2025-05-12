public class HowAboutTrade : CutScenes
{

    public HowAboutTrade()
    {
        cutSceneNodes = new();

        CutSceneNode cutSceneNode = new();

        
        cutSceneNode.cutSceneCommand = ECutSceneCommand.FadeInScreen;
        cutSceneNode.waitTime = 1f;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.Wait;
        cutSceneNode.waitTime = 0.4f;
        cutSceneNodes.Add(cutSceneNode);


        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowCharacterRightSide;
        cutSceneNode.valueNum = 0;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.Wait;
        cutSceneNode.waitTime = 2f;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowCharacterLeftSide;
        cutSceneNode.valueNum = 1;

        cutSceneNodes.Add(cutSceneNode);
        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.Wait;
        cutSceneNode.waitTime = 0.2f;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.HighLightCharacterRightSide;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.Wait;
        cutSceneNode.waitTime = 0.3f;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowText;
        cutSceneNode.text = "오, 마법사님. 무슨 일이신가요?";
        cutSceneNode.name = "브로디";
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.HighLightCharacterLeftSide;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowText;
        cutSceneNode.text = "문제가 좀 생겨서 말이야. 상담하고 싶은게 있는데...";
        cutSceneNode.name = "이드";
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.HighLightCharacterRightSide;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowText;
        cutSceneNode.text = "저한테 상담이라니... 이거 정말 귀한 일이군요.";
        cutSceneNode.name = "브로디";
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowText;
        cutSceneNode.text = "무슨 일이라도 있었던 겁니까?";
        cutSceneNode.name = "브로디";
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.HideText;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.Wait;
        cutSceneNode.waitTime = 0.3f;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.FadeOutScreen;
        cutSceneNode.waitTime = 0.8f;
        cutSceneNodes.Add(cutSceneNode);
    
        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.Wait;
        cutSceneNode.waitTime = 1.5f;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.FadeInScreen;
        cutSceneNode.waitTime = 0.8f;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowText;
        cutSceneNode.text = "음.. 그런 일이...";
        cutSceneNode.name = "브로디";
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.HighLightCharacterLeftSide;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowText;
        cutSceneNode.text = "혹시 이럴 때 도움이 될 만한 카드가 있을까?";
        cutSceneNode.name = "샐리온";
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.HighLightCharacterRightSide;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowText;
        cutSceneNode.text = "제 친구들 중에 도와줄만한 녀석이 하나 있긴 합니다만...";
        cutSceneNode.name = "브로디";
        cutSceneNodes.Add(cutSceneNode);


        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.HighLightCharacterLeftSide;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowText;
        cutSceneNode.text = "그럼 혹시 그 카드를 빌려줄 수는 없나?";
        cutSceneNode.name = "이드";
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.HighLightCharacterRightSide;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowText;
        cutSceneNode.text = "아쉽지만 그건 좀 어렵습니다. 정말 힘들게 만난 귀한 친구거든요.";
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.HideCharacterLeftSide;
        cutSceneNodes.Add(cutSceneNode);
    }
    void Update()
    {
        
    }


}
