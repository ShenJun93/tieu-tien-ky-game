using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>Forwards PlayerController.NormalizedMoveSpeed into CharacterPresentation.SetMovement every frame - the one per-frame gameplay-to-presentation bridge, kept as its own tiny component rather than adding a presentation dependency to PlayerController itself.</summary>
    public sealed class PresentationMovementDriver : MonoBehaviour
    {
        PlayerController controller;
        CharacterPresentation presentation;

        public void Initialize(PlayerController playerController, CharacterPresentation characterPresentation)
        {
            controller = playerController;
            presentation = characterPresentation;
        }

        void Update()
        {
            if (controller != null && presentation != null)
            {
                presentation.SetMovement(controller.NormalizedMoveSpeed);
            }
        }
    }
}
