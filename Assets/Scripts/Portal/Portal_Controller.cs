using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_Controller
{
    public void Update()
    {
        DestroyPortal();
    }

    public void PortalEnter(GameObject alien)
    {
        if (Portal_MVC.Database.portalHP > 0)
        {
            Debug.Log("Alien entered! " + Portal_MVC.Database.portalHP);
            Portal_MVC.Database.portalHP--;
            alien.SetActive(false);
            ScoreSystem.Instance.onBuildingCollide?.Invoke();
        }
    }

    private void DestroyPortal()
    {
        if(Portal_MVC.Database.portalHP <= 0)
        {
            //Portal_MVC.Destroy(Portal_MVC.Instance);
            Portal_MVC.Database.PortalGO.SetActive(false);
        }
    }

    public bool IsPortalDestroyed()
    {
        if(Portal_MVC.Database.portalHP <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
