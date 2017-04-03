using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AtlasManager))]
[RequireComponent(typeof(AnimationManager))]
[RequireComponent(typeof(PanelManager))]
public class MasterManager : MonoBehaviour
{
    private List<IManager> _managerList = new List<IManager>();

    public static AtlasManager atlasManager { get; private set; }
    public static AnimationManager animationManager { get; private set; }
    public static PanelManager panelManager { get; private set; }

    private void Awake()
    {
        atlasManager = GetComponent<AtlasManager>();
        animationManager = GetComponent<AnimationManager>();
        panelManager = GetComponent<PanelManager>();

        _managerList.Add(atlasManager);
        _managerList.Add(animationManager);
        _managerList.Add(panelManager);

        StartCoroutine(BootAllManagers());
    }

    private IEnumerator BootAllManagers()
    {
        foreach (IManager manager in _managerList)
        {
            manager.BootSequence();
        }
        yield return null;
    }

    
}
