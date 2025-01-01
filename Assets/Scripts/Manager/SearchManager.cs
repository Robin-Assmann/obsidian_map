using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchManager : MonoBehaviour
{
    [SerializeField] TMP_InputField searchInput;
    [SerializeField] Button searchButton;

    string _searchText;

    private void Awake()
    {
        searchInput.onValueChanged.AddListener(text => _searchText = text);
        searchInput.onSubmit.AddListener(_ => Search());
        searchInput.onSelect.AddListener(_ => ModeManager.SetInputFieldSelection(true));
        searchInput.onDeselect.AddListener(_ => ModeManager.SetInputFieldSelection(false));

        searchButton.onClick.AddListener(() => Search());
    }

    void Search()
    {
        DisplayManager.SelectBestFitDisplay(_searchText);
    }
}
