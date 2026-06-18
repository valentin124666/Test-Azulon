using Modules.Player.Modules;
using UnityEngine;

namespace Modules.Player.Commands
{
    public class BasePlayerDataCommands
    {
    }

    public class PDCGetPosition : BasePlayerDataCommands
    {
    }

    public class PDCGetCameraAnchor : BasePlayerDataCommands
    {
    }

    public class PDCGetStatsInterface : BasePlayerDataCommands
    {
    }

    public class PDCGetPlayerBackpack : BasePlayerDataCommands
    {
    }

    public class BasePlayerDataRequests
    {
    }

    public class PDRPlayerPosition : BasePlayerDataRequests
    {
        public Vector3 position;

        public PDRPlayerPosition(Vector3 position)
        {
            this.position = position;
        }
    }

    public class PDRPlayerBackpack : BasePlayerDataRequests
    {
        public PlayerBackpack PlayerBackpack;

        public PDRPlayerBackpack(PlayerBackpack playerBackpack)
        {
            PlayerBackpack = playerBackpack;
        }
    }

    public class PDRStatsInterface : BasePlayerDataRequests
    {
        public IPlayerModuleStats[] PlayerModuleStats;

        public PDRStatsInterface(IPlayerModuleStats[] playerModuleStats)
        {
            PlayerModuleStats = playerModuleStats;
        }
    }

    public class PDRCameraAnchor : BasePlayerDataRequests
    {
        public Transform followCamera;
        public Transform lookAtCamera;

        public PDRCameraAnchor(Transform followCamera, Transform lookAtCamera)
        {
            this.followCamera = followCamera;
            this.lookAtCamera = lookAtCamera;
        }

        public PDRCameraAnchor(PPDRCameraAnchor cameraAnchor)
        {
            this.followCamera = cameraAnchor.followCamera;
            this.lookAtCamera = cameraAnchor.lookAtCamera;
        }
    }
}