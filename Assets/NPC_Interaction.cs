using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Interaction : MonoBehaviour {

    NPCdata currentNPC;

    public void SetCurrentNPC(NPCdata newCurrent)
    {
        currentNPC = newCurrent;
    }

    public void Trade()
    {
        currentNPC.CreateMerchantInventory();
    }

    public void Talk()
    {
        currentNPC.Engage_Dialogue();
    }

    public void Exit()
    {

    }
}
