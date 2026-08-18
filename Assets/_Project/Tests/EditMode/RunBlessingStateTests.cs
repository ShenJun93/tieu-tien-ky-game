using NUnit.Framework;

namespace TieuTienKy.Gameplay.Tests
{
    public class RunBlessingStateTests
    {
        const float Tolerance = 0.001f;

        [Test]
        public void NoBlessings_ModifiersAreBaseValues()
        {
            var state = new RunBlessingState();

            RunCombatModifiers modifiers = state.CurrentModifiers;

            Assert.AreEqual(2.5f, modifiers.ConductiveMultiplier, Tolerance);
            Assert.AreEqual(1f, modifiers.MoveSpeedMultiplier, Tolerance);
            Assert.AreEqual(1f, modifiers.AttackRecoveryMultiplier, Tolerance);
            Assert.AreEqual(0, modifiers.MaxHealthBonus);
        }

        [Test]
        public void ThunderSword_OneStack_AddsConductiveBonus()
        {
            var state = new RunBlessingState();
            state.Apply(BlessingId.ThunderSword);

            Assert.AreEqual(3.25f, state.CurrentModifiers.ConductiveMultiplier, Tolerance);
        }

        [Test]
        public void ThunderSword_ThreeStacks_StacksAdditively()
        {
            var state = new RunBlessingState();
            state.Apply(BlessingId.ThunderSword);
            state.Apply(BlessingId.ThunderSword);
            state.Apply(BlessingId.ThunderSword);

            Assert.AreEqual(2.5f + 0.75f * 3f, state.CurrentModifiers.ConductiveMultiplier, Tolerance);
        }

        [Test]
        public void ThunderSword_FourthApply_DoesNotExceedThreeStacks()
        {
            var state = new RunBlessingState();
            state.Apply(BlessingId.ThunderSword);
            state.Apply(BlessingId.ThunderSword);
            state.Apply(BlessingId.ThunderSword);
            state.Apply(BlessingId.ThunderSword);

            Assert.AreEqual(3, state.StackCount(BlessingId.ThunderSword));
            Assert.AreEqual(2.5f + 0.75f * 3f, state.CurrentModifiers.ConductiveMultiplier, Tolerance);
        }

        [Test]
        public void WindStride_OneStack_MultipliesMoveSpeedAndRecovery()
        {
            var state = new RunBlessingState();
            state.Apply(BlessingId.WindStride);

            Assert.AreEqual(1.12f, state.CurrentModifiers.MoveSpeedMultiplier, Tolerance);
            Assert.AreEqual(0.92f, state.CurrentModifiers.AttackRecoveryMultiplier, Tolerance);
        }

        [Test]
        public void WindStride_TwoStacks_CompoundsMultiplicatively()
        {
            var state = new RunBlessingState();
            state.Apply(BlessingId.WindStride);
            state.Apply(BlessingId.WindStride);

            Assert.AreEqual(1.12f * 1.12f, state.CurrentModifiers.MoveSpeedMultiplier, Tolerance);
            Assert.AreEqual(0.92f * 0.92f, state.CurrentModifiers.AttackRecoveryMultiplier, Tolerance);
        }

        [Test]
        public void BodyWard_OneStack_AddsTwoMaxHealth()
        {
            var state = new RunBlessingState();
            state.Apply(BlessingId.BodyWard);

            Assert.AreEqual(2, state.CurrentModifiers.MaxHealthBonus);
        }

        [Test]
        public void WindStride_OneStack_ReducesPhongBoCooldownMultiplier()
        {
            var state = new RunBlessingState();
            state.Apply(BlessingId.WindStride);

            Assert.AreEqual(0.85f, state.CurrentModifiers.PhongBoCooldownMultiplier, Tolerance);
        }

        [Test]
        public void WindStride_NoStacks_PhongBoCooldownMultiplierIsOne()
        {
            var state = new RunBlessingState();
            Assert.AreEqual(1f, state.CurrentModifiers.PhongBoCooldownMultiplier, Tolerance);
        }

        [Test]
        public void BodyWard_OneStack_AddsHoTheWindowBonus()
        {
            var state = new RunBlessingState();
            state.Apply(BlessingId.BodyWard);

            Assert.AreEqual(0.15f, state.CurrentModifiers.HoTheWindowBonusSeconds, Tolerance);
        }

        [Test]
        public void BodyWard_ThreeStacks_AddsHoTheWindowBonusLinearly()
        {
            var state = new RunBlessingState();
            state.Apply(BlessingId.BodyWard);
            state.Apply(BlessingId.BodyWard);
            state.Apply(BlessingId.BodyWard);

            Assert.AreEqual(0.45f, state.CurrentModifiers.HoTheWindowBonusSeconds, Tolerance);
        }

        [Test]
        public void BodyWard_ThreeStacks_AddsSixMaxHealth()
        {
            var state = new RunBlessingState();
            state.Apply(BlessingId.BodyWard);
            state.Apply(BlessingId.BodyWard);
            state.Apply(BlessingId.BodyWard);

            Assert.AreEqual(6, state.CurrentModifiers.MaxHealthBonus);
        }

        [Test]
        public void Reset_ReturnsAllModifiersToBase()
        {
            var state = new RunBlessingState();
            state.Apply(BlessingId.ThunderSword);
            state.Apply(BlessingId.WindStride);
            state.Apply(BlessingId.BodyWard);

            state.Reset();

            RunCombatModifiers modifiers = state.CurrentModifiers;
            Assert.AreEqual(2.5f, modifiers.ConductiveMultiplier, Tolerance);
            Assert.AreEqual(1f, modifiers.MoveSpeedMultiplier, Tolerance);
            Assert.AreEqual(1f, modifiers.AttackRecoveryMultiplier, Tolerance);
            Assert.AreEqual(0, modifiers.MaxHealthBonus);
            Assert.AreEqual(0, state.StackCount(BlessingId.ThunderSword));
        }
    }
}
