public class Intro : CutScenes
{

    public Intro()
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
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowCharacterLeftSide;
        cutSceneNode.valueNum = 1;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.Wait;
        cutSceneNode.waitTime = 2f;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.PlaySoundEffect;
        cutSceneNode.valueNum = 0;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowText;
        cutSceneNode.text = "!!";
        cutSceneNode.name = "이드";
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowText;
        cutSceneNode.text = "이봐! 형씨!";
        cutSceneNode.name = "???";
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowText;
        cutSceneNode.text = "문제가 생겼어. 잠깐만 나와봐!";
        cutSceneNode.name = "???";
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode.cutSceneCommand = ECutSceneCommand.FadeOutScreen;
        cutSceneNode.waitTime = 0.5f;
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode.cutSceneCommand = ECutSceneCommand.FadeInScreen;
        cutSceneNode.waitTime = 0.5f;
        cutSceneNodes.Add(cutSceneNode);


        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowText;
        cutSceneNode.text = "무슨 문제라도 있나?";
        cutSceneNode.name = "이드";
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowText;
        cutSceneNode.text = "아니 글쎄. 시간이 너무 늦어서 그런건지 몰라도 말들이 겁을 먹은 것 같아. 꿈쩍을 안해.";
        cutSceneNode.name = "마부";
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowText;
        cutSceneNode.text = ":그 말은.. 더이상 갈 수 없다는건가?";
        cutSceneNode.name = "이드";
        cutSceneNodes.Add(cutSceneNode);

        cutSceneNode = new();
        cutSceneNode.cutSceneCommand = ECutSceneCommand.ShowText;
        cutSceneNode.text = "그런 셈이지 뭐.";
        cutSceneNode.name = "마부";
        cutSceneNodes.Add(cutSceneNode);
    }

}