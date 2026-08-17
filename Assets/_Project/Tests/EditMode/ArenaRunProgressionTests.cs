using NUnit.Framework;

namespace TieuTienKy.Gameplay.Tests
{
    public class ArenaRunProgressionTests
    {
        [Test]
        public void NewProgression_StartsAtWave1()
        {
            var progression = new ArenaRunProgression();

            Assert.AreEqual(ArenaRunStage.Wave1, progression.Stage);
        }

        [Test]
        public void FullLegalSequence_AdvancesInExactOrder()
        {
            var progression = new ArenaRunProgression();

            Assert.IsTrue(progression.AdvanceAfterCombatClear());
            Assert.AreEqual(ArenaRunStage.Blessing1, progression.Stage);

            Assert.IsTrue(progression.AdvanceAfterBlessingChoice());
            Assert.AreEqual(ArenaRunStage.Wave2, progression.Stage);

            Assert.IsTrue(progression.AdvanceAfterCombatClear());
            Assert.AreEqual(ArenaRunStage.Blessing2, progression.Stage);

            Assert.IsTrue(progression.AdvanceAfterBlessingChoice());
            Assert.AreEqual(ArenaRunStage.EliteWave, progression.Stage);

            Assert.IsTrue(progression.AdvanceAfterCombatClear());
            Assert.AreEqual(ArenaRunStage.Blessing3, progression.Stage);

            Assert.IsTrue(progression.AdvanceAfterBlessingChoice());
            Assert.AreEqual(ArenaRunStage.Boss, progression.Stage);

            Assert.IsTrue(progression.AdvanceAfterCombatClear());
            Assert.AreEqual(ArenaRunStage.Victory, progression.Stage);
        }

        [Test]
        public void AdvanceAfterBlessingChoice_DuringCombatStage_DoesNotSkipStages()
        {
            var progression = new ArenaRunProgression();

            bool advanced = progression.AdvanceAfterBlessingChoice();

            Assert.IsFalse(advanced);
            Assert.AreEqual(ArenaRunStage.Wave1, progression.Stage);
        }

        [Test]
        public void AdvanceAfterCombatClear_DuringBlessingGate_DoesNotSkipStages()
        {
            var progression = new ArenaRunProgression();
            progression.AdvanceAfterCombatClear();

            bool advanced = progression.AdvanceAfterCombatClear();

            Assert.IsFalse(advanced);
            Assert.AreEqual(ArenaRunStage.Blessing1, progression.Stage);
        }

        [Test]
        public void AdvanceAfterCombatClear_AtVictory_DoesNotChangeStage()
        {
            var progression = new ArenaRunProgression();
            progression.AdvanceAfterCombatClear();
            progression.AdvanceAfterBlessingChoice();
            progression.AdvanceAfterCombatClear();
            progression.AdvanceAfterBlessingChoice();
            progression.AdvanceAfterCombatClear();
            progression.AdvanceAfterBlessingChoice();
            progression.AdvanceAfterCombatClear();

            bool advanced = progression.AdvanceAfterCombatClear();

            Assert.IsFalse(advanced);
            Assert.AreEqual(ArenaRunStage.Victory, progression.Stage);
        }

        [TestCase(ArenaRunStage.Wave1)]
        [TestCase(ArenaRunStage.Wave2)]
        [TestCase(ArenaRunStage.EliteWave)]
        [TestCase(ArenaRunStage.Boss)]
        public void MarkDefeat_FromAnyCombatStage_GoesToDefeat(ArenaRunStage combatStage)
        {
            var progression = new ArenaRunProgression();
            AdvanceTo(progression, combatStage);

            progression.MarkDefeat();

            Assert.AreEqual(ArenaRunStage.Defeat, progression.Stage);
        }

        [Test]
        public void MarkDefeat_AtVictory_DoesNotOverwriteVictory()
        {
            var progression = new ArenaRunProgression();
            AdvanceTo(progression, ArenaRunStage.Victory);

            progression.MarkDefeat();

            Assert.AreEqual(ArenaRunStage.Victory, progression.Stage);
        }

        [Test]
        public void Reset_FromAnyStage_ReturnsToWave1()
        {
            var progression = new ArenaRunProgression();
            AdvanceTo(progression, ArenaRunStage.EliteWave);

            progression.Reset();

            Assert.AreEqual(ArenaRunStage.Wave1, progression.Stage);
        }

        static void AdvanceTo(ArenaRunProgression progression, ArenaRunStage target)
        {
            while (progression.Stage != target)
            {
                if (!progression.AdvanceAfterCombatClear())
                {
                    progression.AdvanceAfterBlessingChoice();
                }
            }
        }
    }
}
