using UnityEngine;

namespace TieuTienKy.Brawl
{
    /// <summary>
    /// Presentation of one fighter: places the model at the interpolated simulation position,
    /// turns it smoothly toward its facing, and drives the locomotion blend. It never changes the simulation.
    /// </summary>
    public sealed class FighterView : MonoBehaviour
    {
        static readonly int SpeedParam = Animator.StringToHash("Speed");

        [SerializeField] Animator animator;
        [SerializeField] float turnSpeedDegrees = 900f;

        public void Present(Vector2 position, Vector2 facing, float normalizedSpeed)
        {
            var t = transform;
            t.position = new Vector3(position.x, t.position.y, position.y);
            if (facing.sqrMagnitude > 1e-4f)
            {
                var target = Quaternion.LookRotation(new Vector3(facing.x, 0f, facing.y), Vector3.up);
                t.rotation = Quaternion.RotateTowards(t.rotation, target, turnSpeedDegrees * Time.deltaTime);
            }
            if (animator != null)
                animator.SetFloat(SpeedParam, normalizedSpeed, 0.08f, Time.deltaTime);
        }

        public void Bind(Animator target) => animator = target;
    }
}
