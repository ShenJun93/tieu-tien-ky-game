using TieuTienKy.Input;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Builds the entire P0A greybox arena procedurally at runtime from Unity
    /// primitives (player, dummy target, water zone, hazard obstacle). This
    /// keeps the .unity scene file itself trivial (camera, light, and this
    /// single bootstrap object) instead of hand-authoring a large serialized
    /// scene graph outside the Editor.
    /// </summary>
    public sealed class GreyboxSceneBootstrapper : MonoBehaviour
    {
        static readonly Color GroundColor = new Color(0.55f, 0.55f, 0.55f);
        static readonly Color PlayerColor = new Color(0.9f, 0.75f, 0.2f);
        static readonly Color DummyColor = new Color(0.8f, 0.3f, 0.3f);
        static readonly Color WaterZoneColor = new Color(0.2f, 0.5f, 0.9f, 0.6f);
        static readonly Color HazardColor = new Color(0.3f, 0.3f, 0.3f);

        void Awake()
        {
            BuildGround();
            GameObject player = BuildPlayer(new Vector3(0f, 1f, 0f));
            GameObject dummy = BuildDummyTarget(new Vector3(3f, 1f, 0f));
            BuildWaterZone(new Vector3(3f, 0.5f, 0f), new Vector3(3f, 1f, 3f));
            BuildHazardObstacle(new Vector3(5.5f, 1f, 0f));

            player.transform.LookAt(dummy.transform.position);
        }

        static void BuildGround()
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(2f, 1f, 2f);
            Tint(ground, GroundColor);
        }

        static GameObject BuildPlayer(Vector3 position)
        {
            GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.transform.position = position;
            Tint(player, PlayerColor);

            ReplaceWithCharacterController(player);

            player.AddComponent<KnockbackReceiver>();
            player.AddComponent<TouchInputReader>();
            player.AddComponent<PlayerController>();
            player.AddComponent<BasicAttack>();

            return player;
        }

        static GameObject BuildDummyTarget(Vector3 position)
        {
            GameObject dummy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            dummy.name = "DummyTarget";
            dummy.transform.position = position;
            Tint(dummy, DummyColor);

            ReplaceWithCharacterController(dummy);

            dummy.AddComponent<KnockbackReceiver>();
            dummy.AddComponent<DummyTarget>();

            return dummy;
        }

        static void BuildWaterZone(Vector3 position, Vector3 size)
        {
            GameObject zone = GameObject.CreatePrimitive(PrimitiveType.Cube);
            zone.name = "WaterZone";
            zone.transform.position = position;
            zone.transform.localScale = size;
            Tint(zone, WaterZoneColor);

            var boxCollider = zone.GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            zone.AddComponent<WaterZone>();
        }

        static void BuildHazardObstacle(Vector3 position)
        {
            GameObject hazard = GameObject.CreatePrimitive(PrimitiveType.Cube);
            hazard.name = "HazardObstacle";
            hazard.transform.position = position;
            hazard.transform.localScale = new Vector3(1f, 2f, 3f);
            Tint(hazard, HazardColor);

            hazard.AddComponent<HazardObstacle>();
        }

        static void ReplaceWithCharacterController(GameObject go)
        {
            var existingCollider = go.GetComponent<CapsuleCollider>();
            if (existingCollider != null)
            {
                Destroy(existingCollider);
            }

            var controller = go.AddComponent<CharacterController>();
            controller.center = Vector3.zero;
            controller.height = 2f;
            controller.radius = 0.5f;
        }

        static void Tint(GameObject go, Color color)
        {
            var renderer = go.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = color;
            }
        }
    }
}
