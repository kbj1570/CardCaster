using TMPro;
public class NoticeBoardWindow : Window
{

	public TMP_Text pageNumberText;
	int currentPage;

	public void UpdatePageNum()
	{pageNumberText.text = currentPage + "/ 2";}

	public void SetCurrentPage(int value)
	{currentPage = value;}

	public int GetCurrentPage()
	{return currentPage;}
}