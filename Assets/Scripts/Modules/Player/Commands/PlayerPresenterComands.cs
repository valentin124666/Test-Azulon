using UnityEngine;

namespace Modules.Player.Commands
{
    public class BasePlayerPresenterDataCommands
    {
    }

    public class PPDCGetCameraAnchor : BasePlayerPresenterDataCommands
    {
    }  
    public class PPDRGetEquipmentSlot : BasePlayerPresenterDataCommands
    {
    }

    public class BasePlayerPresenterDataRequests
    {
    }

    public class PPDREquipmentSlot : BasePlayerPresenterDataRequests
    {
        public IEquipmentSlot equipmentSlot;

        public PPDREquipmentSlot(IEquipmentSlot equipmentSlot)
        {
            this.equipmentSlot = equipmentSlot;
        }
    }

    public class PPDRCameraAnchor : BasePlayerPresenterDataRequests
    {
        public Transform followCamera;
        public Transform lookAtCamera;

        public PPDRCameraAnchor(Transform followCamera, Transform lookAtCamera)
        {
            this.followCamera = followCamera;
            this.lookAtCamera = lookAtCamera;
        }
    }
}