using UnityEngine;
using System.Collections.Generic;

public class SectionFactory : MonoBehaviour {

    #region Variables

    #region Public

    #endregion

    #region Private
    private List<Dictionary<int, Cell>> completedSections = new List<Dictionary<int, Cell>>(); // The list that holds all dictionaries of sections
    private ForestGenerator forestGen;
    #endregion

    #endregion

    
    #region Unity Events

    void Start () {
        forestGen = FindObjectOfType<ForestGenerator>();
        BuildSectionList();
    }

    #endregion

    #region Public Methods

    public void BuildSectionList () {
        forestGen.GenerateForest();
    }

    #endregion
}
