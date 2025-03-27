using System.Collections.Generic;

public class SelectionNode
{
    protected ESelectionType selectionType;
    protected ERequireType requireType;
    private string selectionTitle;
    private string selectionNodeNum;
    private List<string> selectionText;
    private int requireGold;
    private int requireHealth;
    private CardData requireCard;
    private Item requireItem;
    private SelectionNode firstSelection;
    private SelectionNode secondSelection;
    private SelectionNode thirdSelection;

    public SelectionNode()
    {}

    

    public ERequireType GetRequireType()
    {return requireType;}
    public void SetRequireType(ERequireType requireType)
    {this.requireType = requireType;}
    public List<string> GetSelectionText()
    {return selectionText;}
    public void SetSelectionText(List<string> selectionText)
    {this.selectionText = selectionText;}
    public string GetSelectionTitle()
    {return selectionTitle;}
    public void SetSelectionTitle(string selectionTitle)
    {this.selectionTitle = selectionTitle;}
    public SelectionNode GetFirstSelection()
    {return firstSelection;}
    public void SetFirstSelection(SelectionNode firstSelection)
    {this.firstSelection = firstSelection;}
    public SelectionNode GetSecondSelection()
    {return secondSelection;}
    public void SetSecondSelection(SelectionNode secondSelection)
    {this.secondSelection =  secondSelection;}
    public SelectionNode GetThirdSelection()
    {return thirdSelection;}
    public void SetThirdSelection(SelectionNode thirdSelection)
    {this.thirdSelection =  thirdSelection;}
    public Item GetRequireItem()
    {return requireItem;}
    public void SetRequireCard(CardData requireCard)
    {this.requireCard = requireCard;}
    public int GetRequireGold()
    {return requireGold;}
    public void SetRequireGold(int requireGold)
    {this.requireGold = requireGold;}

    public int GetRequireHealth()
    {return requireHealth;}

    public CardData GetRequireCard()
    {return requireCard;}
}
public enum ESelectionType
{None, Random, ConditionChange}